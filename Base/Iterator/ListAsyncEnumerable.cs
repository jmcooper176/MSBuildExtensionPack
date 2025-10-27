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

// Ignore Spelling: cyclonedx Cli

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace MSBuild.ExtensionPack.Base.Iterator
{
    /// <summary>
    /// Static class implementing the <see cref="IEnumerable{T}"/> items enumeration with <see langref="async"/><see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    public static class ListAsyncEnumerable
    {
        /// <summary>
        /// Static method implementing <see cref="IAsyncEnumerable{T}"/> for <see cref="IEnumerable{T}"/>..
        /// </summary>
        /// <typeparam name="T">Specifies the element <see cref="Type"/> of <paramref name="items"/>.</typeparam>
        /// <param name="items">            Specifies the <see cref="IEnumerable{T}"/> to enumerate.</param>
        /// <param name="cancellationToken">
        /// Specifies an optional <see cref="CancellationToken"/> allowing the <see cref="IAsyncEnumerable{T}"/> to be canceled.
        /// </param>
        /// <returns>Returns an item of <typeparamref name="T"/> for each enumeration of <paramref name="items"/>.</returns>
        /// <exception cref="OperationCanceledException">Thrown is <paramref name="cancellationToken"/> requests cancellation.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "IAsyncEnumerable API")]
        public static async IAsyncEnumerable<T> GetAsyncEnumerable<T>(IEnumerable<T> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Contract.Requires(!CliUtils.IsNullOrEmpty(items), $"Parameter {nameof(items)} cannot be null or empty");

            await TaskScheduler.Default.SwitchTo(false);

            foreach (var item in items!)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;

                await TaskScheduler.Default.SwitchTo(false);
            }
        }
    }
}
