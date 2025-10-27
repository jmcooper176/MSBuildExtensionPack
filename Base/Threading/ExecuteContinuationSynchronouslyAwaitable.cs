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

namespace MSBuild.ExtensionPack.Base.Threading
{
    /// <summary>
    /// A Task awaitable that has affinity to executing callbacks synchronously on the completing call stack.
    /// </summary>
    /// <typeparam name="T">The type of value returned by the awaited <see cref="Task"/>.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public readonly struct ExecuteContinuationSynchronouslyAwaitable<T>
    {
        /// <summary>
        /// The task whose completion will execute the continuation.
        /// </summary>
        private readonly Task<T> antecedent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteContinuationSynchronouslyAwaitable{T}"/> struct.
        /// </summary>
        /// <param name="antecedent">The task whose completion will execute the continuation.</param>
        public ExecuteContinuationSynchronouslyAwaitable(Task<T> antecedent)
        {
            Requires.NotNull(antecedent, nameof(antecedent));
            this.antecedent = antecedent;
        }

        /// <summary>
        /// Gets the <see cref="ExecuteContinuationSynchronouslyAwaiter{T}"/>.
        /// </summary>
        /// <returns>The <see cref="ExecuteContinuationSynchronouslyAwaiter{T}"/></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ExecuteContinuationSynchronouslyAwaiter<T> GetAwaiter() => new(antecedent);
    }
}
