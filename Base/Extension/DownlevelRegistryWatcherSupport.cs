// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sub-license, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// SPDX-License-Identifier: MIT
using Microsoft;

using MSBuild.ExtensionPack.Base.Extension;
using MSBuild.ExtensionPack.Base.Threading;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Provides a dedicated thread for requesting registry change notifications.
    /// </summary>
    /// <remarks>
    /// For versions of Windows prior to Windows 8, requesting registry change notifications required that the thread that made the
    /// request remain alive or else the watcher would simply signal the event and stop watching for changes. This class provides a
    /// single, dedicated thread for requesting such notifications so that they don't get canceled when a thread happens to exit.
    /// The dedicated thread is released when no one is watching the registry any more.
    /// </remarks>
    internal static class DownlevelRegistryWatcherSupport
    {
        /// <summary>
        /// The size of the stack allocated for a thread that expects to stay within just a few methods in depth.
        /// </summary>
        /// <remarks>The default stack size for a thread is 1MB.</remarks>
        private const int SmallThreadStackSize = 100 * 1024;

        /// <summary>
        /// A queue of actions the dedicated thread should take.
        /// </summary>
        private static readonly Queue<Tuple<Action, TaskCompletionSource<EmptyStruct>>> PendingWork = new();

        /// <summary>
        /// The object to lock when accessing any fields. This is also the object that is waited on by the dedicated thread, and may
        /// be pulsed by others to wake the dedicated thread to do some work.
        /// </summary>
        private static readonly object SyncObject = new();

        /// <summary>
        /// The number of callers that still have an interest in the survival of the dedicated thread. The dedicated thread will
        /// exit when this value reaches 0.
        /// </summary>
        private static int keepAliveCount;

        /// <summary>
        /// The thread that should stay alive and be dequeuing <see cref="PendingWork"/>.
        /// </summary>
        private static Thread? liveThread;

        /// <summary>
        /// Decrements the count of interested parties in the live thread, and helps it to terminate if necessary.
        /// </summary>
        private static void ReleaseRefOnDedicatedThread()
        {
            lock (SyncObject)
            {
                if (--keepAliveCount == 0)
                {
                    liveThread = null;

                    // Wake up any obsolete thread(s) so they can go to exit.
                    Monitor.PulseAll(SyncObject);
                }
            }
        }

        /// <summary>
        /// Executes thread-affinity work from a queue until both the queue is empty and any lingering interest in the survival of
        /// the dedicated thread has been released.
        /// </summary>
        /// <remarks>This method serves as the <see cref="ThreadStart"/> for our dedicated thread.</remarks>
        private static void Worker()
        {
            while (true)
            {
                Tuple<Action, TaskCompletionSource<EmptyStruct>>? work = null;

                lock (SyncObject)
                {
                    if (Thread.CurrentThread != liveThread)
                    {
                        // Regardless of our PendingWork and keepAliveCount, it isn't meant for this thread any more. This happens
                        // when keepAliveCount (at least temporarily) hits 0, so this thread must be assumed to be on its exit path,
                        // and another thread will be spawned to process new requests.
                        Assumes.True(liveThread is not null || keepAliveCount == 0 && PendingWork.Count == 0);
                        return;
                    }
                    else if (PendingWork.Count > 0)
                    {
                        work = PendingWork.Dequeue();
                    }
                    else if (keepAliveCount == 0)
                    {
                        // No work, and no reason to stay alive. Exit the thread.
                        return;
                    }
                    else
                    {
                        // Sleep until another thread wants to wake us up with a Pulse.
                        Monitor.Wait(SyncObject);
                    }
                }

                try
                {
                    work?.Item1.Invoke();
                    work?.Item2.SetResult(EmptyStruct.Instance);
                }
                catch (ObjectDisposedException odex)
                {
                    Cause ex = new(odex);
                    ex.Trap(odex);
                    work?.Item2.SetException(odex);
                }
                catch (ArgumentNullException anex)
                {
                    Cause ex = new(anex);
                    ex.Trap(anex);
                    work?.Item2.SetException(anex);
                }
                catch (InvalidOperationException ioex)
                {
                    Cause ex = new(ioex);
                    ex.Trap(ioex);
                    work?.Item2.SetException(ioex);
                }
            }
        }

        /// <summary>
        /// Decrements the dedicated thread use counter by at most one upon disposal.
        /// </summary>
        private class ThreadHandleRelease : IDisposable
        {
            /// <summary>
            /// A value indicating whether this instance has already been disposed.
            /// </summary>
            private bool disposed;

            /// <summary>
            /// Release the keep alive count reserved by this instance.
            /// </summary>
            public void Dispose()
            {
                lock (SyncObject)
                {
                    if (!disposed)
                    {
                        disposed = true;
                        ReleaseRefOnDedicatedThread();
                    }
                }
            }
        }

        /// <summary>
        /// Executes some action on a long-lived thread.
        /// </summary>
        /// <param name="action">The delegate to execute.</param>
        /// <returns>
        /// A task that either faults with the exception thrown by <paramref name="action"/> or completes after successfully
        /// executing the delegate with a result that should be disposed when it is safe to terminate the long-lived thread.
        /// </returns>
        /// <remarks>
        /// This thread never posts to <see cref="SynchronizationContext.Current"/>, so it is safe to call this method and
        /// synchronously block on its result.
        /// </remarks>
        internal static async Task<IDisposable> ExecuteOnDedicatedThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<EmptyStruct>();
            var keepAliveCountIncremented = false;

            try
            {
                lock (SyncObject)
                {
                    PendingWork.Enqueue(Tuple.Create(action, tcs));

                    try
                    {
                        // This block intentionally left blank.
                    }
                    finally
                    {
                        // We make these two assignments within a finally block to guard against an untimely ThreadAbortException
                        // causing us to execute just one of them.
                        keepAliveCountIncremented = true;
                        ++keepAliveCount;
                    }

                    if (keepAliveCount == 1)
                    {
                        Assumes.Null(liveThread);

                        liveThread = new Thread(Worker, SmallThreadStackSize)
                        {
                            IsBackground = true,
                            Name = "Registry watcher",
                        };
                        liveThread.Start();
                    }
                    else
                    {
                        // There *could* temporarily be multiple threads in some race conditions. Pulse all of them so that the live
                        // one is sure to get the message.
                        Monitor.PulseAll(SyncObject);
                    }
                }

                await tcs.Task.ConfigureAwait(false);
                return new ThreadHandleRelease();
            }
            catch (Exception e)
            {
                Cause ex = new(e);
                await ex.TrapAsync(e).ConfigureAwait(false);

                if (keepAliveCountIncremented)
                {
                    // Our caller will never have a chance to release their claim on the dedicated thread, so do it for them.
                    ReleaseRefOnDedicatedThread();
                }

                throw;
            }
        }
    }
}
