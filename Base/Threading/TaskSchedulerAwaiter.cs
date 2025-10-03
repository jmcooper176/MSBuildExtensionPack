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
using System.Runtime.CompilerServices;

namespace MSBuild.ExtensionPack.Base.Threading
{
    /// <summary>
    /// Implements a <see langref="struct"/><c>TaskSchedulerAwaiter</c>.
    /// </summary>
    /// <remarks>Initializes a new instance of the <see cref="TaskSchedulerAwaiter"/> struct.</remarks>
    /// <param name="scheduler">  Specifies the <see cref="TaskScheduler"/> for continuations.</param>
    /// <param name="alwaysYield">
    /// A value indicating whether the caller should yield even if already executing on the desired <see cref="TaskScheduler"/>.
    /// </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public readonly struct TaskSchedulerAwaiter(TaskScheduler scheduler, bool alwaysYield = false) : ICriticalNotifyCompletion
    {
        #region Private Fields

        /// <summary>
        /// A value indicating whether <see cref="IsCompleted"/> should always return false.
        /// </summary>
        private readonly bool alwaysYield = alwaysYield;

        /// <summary>
        /// The scheduler for continuations.
        /// </summary>
        private readonly TaskScheduler scheduler = scheduler;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether no yield is necessary.
        /// </summary>
        /// <value><c>true</c> if the caller is already running on that TaskScheduler.</value>
        public bool IsCompleted
        {
            get
            {
                if (alwaysYield)
                {
                    return false;
                }

                // We special case the TaskScheduler.Default since that is semantically equivalent to being on a ThreadPool thread,
                // and there are various ways to get on those threads. TaskScheduler.Current is never null. Even if no scheduler is
                // really active and the current thread is not a thread pool thread, TaskScheduler.Current == TaskScheduler.Default,
                // so we have to protect against that case too.
                var isThreadPoolThread = Thread.CurrentThread.IsThreadPoolThread;
                return scheduler == TaskScheduler.Default && isThreadPoolThread
                    || scheduler == TaskScheduler.Current && TaskScheduler.Current != TaskScheduler.Default;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Does nothing.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void GetResult()
        {
        }

        /// <summary>
        /// Schedules a continuation to execute using the specified task scheduler.
        /// </summary>
        /// <param name="continuation">The delegate to invoke.</param>
        public void OnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
            {
                ThreadPool.QueueUserWorkItem(state => ((Action)state!)(), continuation);
            }
            else
            {
                _ = Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
            }
        }

        /// <summary>
        /// Schedules a continuation to execute using the specified task scheduler without capturing the ExecutionContext.
        /// </summary>
        /// <param name="continuation">The action.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            if (scheduler == TaskScheduler.Default)
            {
                ThreadPool.UnsafeQueueUserWorkItem(state => ((Action)state!)(), continuation);
            }
            else
            {
                // There is no API for scheduling a Task without capturing the ExecutionContext.
                _ = Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, scheduler);
            }
        }

        #endregion Public Methods
    }
}
