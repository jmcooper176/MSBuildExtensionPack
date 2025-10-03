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
namespace MSBuild.ExtensionPack.Base.Extension
{
    using MSBuild.ExtensionPack.Base.Threading;

    /// <summary>
    /// Static class implementing extensions for <see cref="TaskScheduler"/> to support threading.
    /// </summary>
    public static class TaskSchedulerExtension
    {
        #region Public Methods

        /// <summary>
        /// Gets a <see cref="TaskSchedulerAwaiter"/> that schedules continuations on the specified scheduler.
        /// </summary>
        /// <param name="scheduler">Specifies the task scheduler used to execute continuations.</param>
        /// <returns>A <see cref="TaskSchedulerAwaiter"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="scheduler"/> is null.</exception>
        public static TaskSchedulerAwaiter GetAwaiter(this TaskScheduler scheduler)
        {
            return new TaskSchedulerAwaiter(scheduler);
        }

        /// <summary>
        /// Gets an <see cref="TaskSchedulerAwaitable"/> that schedules continuations on the specified scheduler.
        /// </summary>
        /// <param name="scheduler">  Specifies the <see cref="TaskScheduler"/> task scheduler used to execute continuations.</param>
        /// <param name="alwaysYield">
        /// Specifies a value indicating whether the caller should yield even if already executing on the desired task scheduler.
        /// </param>
        /// <returns>A <see cref="TaskSchedulerAwaitable"/>.</returns>
        public static TaskSchedulerAwaitable SwitchTo(this TaskScheduler scheduler, bool alwaysYield = false)
        {
            return new TaskSchedulerAwaitable(scheduler, alwaysYield);
        }

        #endregion Public Methods
    }
}
