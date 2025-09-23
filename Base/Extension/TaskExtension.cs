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

using static Microsoft.VisualStudio.Threading.AwaitExtensions;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Static class implementing <see cref="Task"/> extensions to support threading.
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// Gets an awaitable that schedules the continuation with a preference to executing synchronously on the call stack that
        /// completed the <see cref="Task"/>, without regard to thread ID or any <see cref="SynchronizationContext"/> that may be
        /// applied when the continuation is scheduled or when the antecedent completes.
        /// </summary>
        /// <param name="antecedent">The task to await on.</param>
        /// <returns>An awaitable.</returns>
        /// <remarks>
        /// If there is not enough stack space remaining on the thread that is completing the <paramref name="antecedent"/><see
        /// cref="Task"/>, the continuation may be scheduled on the thread pool.
        /// </remarks>
        public static ExecuteContinuationSynchronouslyAwaitable ConfigureAwaitRunInline(this Task antecedent)
        {
            Requires.NotNull(antecedent, nameof(antecedent));

            return new ExecuteContinuationSynchronouslyAwaitable(antecedent);
        }

        /// <summary>
        /// Gets an awaitable that schedules the continuation with a preference to executing synchronously on the call stack that
        /// completed the <see cref="Task"/>, without regard to thread ID or any <see cref="SynchronizationContext"/> that may be
        /// applied when the continuation is scheduled or when the antecedent completes.
        /// </summary>
        /// <typeparam name="T">The type of value returned by the awaited <see cref="Task"/>.</typeparam>
        /// <param name="antecedent">The task to await on.</param>
        /// <returns>An awaitable.</returns>
        /// <remarks>
        /// If there is not enough stack space remaining on the thread that is completing the <paramref name="antecedent"/><see
        /// cref="Task"/>, the continuation may be scheduled on the thread pool.
        /// </remarks>
        public static ExecuteContinuationSynchronouslyAwaitable<T> ConfigureAwaitRunInline<T>(this Task<T> antecedent)
        {
            Requires.NotNull(antecedent, nameof(antecedent));

            return new ExecuteContinuationSynchronouslyAwaitable<T>(antecedent);
        }
    }
}
