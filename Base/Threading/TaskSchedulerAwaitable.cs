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

// Ignore Spelling: Awaiter

namespace MSBuild.ExtensionPack.Base.Threading
{
    /// <summary>
    /// An awaitable that executes continuations on the specified task scheduler.
    /// </summary>
    /// <remarks>Initializes a new instance of the <see cref="TaskSchedulerAwaitable"/> struct.</remarks>
    /// <param name="taskScheduler">The task scheduler used to execute continuations.</param>
    /// <param name="alwaysYield">  
    /// A value indicating whether the caller should yield even if already executing on the desired task scheduler.
    /// </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public readonly struct TaskSchedulerAwaitable(TaskScheduler taskScheduler, bool alwaysYield = false)
    {
        #region Private Fields

        /// <summary>
        /// A value indicating whether the awaitable will always call the caller to yield.
        /// </summary>
        private readonly bool alwaysYield = alwaysYield;

        /// <summary>
        /// The scheduler for continuations.
        /// </summary>
        private readonly TaskScheduler taskScheduler = taskScheduler;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets an awaitable that schedules continuations on the specified scheduler.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public TaskSchedulerAwaiter GetAwaiter()
        {
            return new TaskSchedulerAwaiter(taskScheduler, alwaysYield);
        }

        #endregion Public Methods
    }
}
