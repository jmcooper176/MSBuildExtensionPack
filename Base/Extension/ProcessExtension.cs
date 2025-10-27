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
using System.Diagnostics;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Static class implementing <see cref="Process"/> extensions for threading support.
    /// </summary>
    public static class ProcessExtension
    {
        /// <summary>
        /// Returns a task that completes when the process exits and provides the exit code of that process.
        /// </summary>
        /// <param name="process">          The process to wait for exit.</param>
        /// <param name="cancellationToken">
        /// A token whose cancellation will cause the returned Task to complete before the process exits in a faulted state with an
        /// <see cref="OperationCanceledException"/>. This token has no effect on the <paramref name="process"/> itself.
        /// </param>
        /// <returns>A task whose result is the <see cref="Process.ExitCode"/> of the <paramref name="process"/>.</returns>
        public static async Task<int> WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<int>();

            void exitHandler(object? sender, EventArgs e) => tcs.TrySetResult(process.ExitCode);

            try
            {
                process.EnableRaisingEvents = true;
                process.Exited += exitHandler;

                if (process.HasExited)
                {
                    // Allow for the race condition that the process has already exited.
                    tcs.TrySetResult(process.ExitCode);
                }

                using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
                {
                    return await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                process.Exited -= exitHandler;
            }
        }
    }
}
