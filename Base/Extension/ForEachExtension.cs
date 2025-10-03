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
using MSBuild.ExtensionPack.Base.Extension;

namespace MSBuild.ExtensionPack.Base.Extension
{
    using MSBuild.ExtensionPack.Base.Iterator;

    /// <summary>
    /// Static class implementing <see langref="async"/> for each methods for <see cref="Array"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class ForEachExtension
    {
        #region Public Methods

        /// <summary>
        /// Extension method that, for each element of <typeparamref name="T"/> in <paramref name="array"/>, perform <paramref
        /// name="task"/> asynchronously..
        /// </summary>
        /// <typeparam name="T">Specifies the type to cast <paramref name="array"/> to.</typeparam>
        /// <param name="array">Specifies <see cref="Array"/> to case, enumerate, and process.</param>
        /// <param name="task"> Specifies the <see cref="Task{T}"/> to process <paramref name="array"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public static async Task ForEachAsync<T>(this Array? array, Func<T, Task<T>> task)
        {
            if (!CliUtils.IsNullOrEmpty(array))
            {
                await foreach (var element in ArrayAsyncEnumerable.GetAsyncEnumerable<T>(array).ConfigureAwait(false))
                {
                    await task(element).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Extension method that, for each element of <typeparamref name="T"/> in <paramref name="array"/>, perform <paramref
        /// name="task"/> asynchronously..
        /// </summary>
        /// <typeparam name="T">Specifies the type to cast <paramref name="array"/> to.</typeparam>
        /// <param name="array">Specifies <see cref="Array"/> of <typeparamref name="T"/> elements to enumerate and process.</param>
        /// <param name="task"> Specifies the <see cref="Task{T}"/> to process <paramref name="array"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public static async Task ForEachAsync<T>(this T[]? array, Func<T, Task<T>> task)
        {
            if (!CliUtils.IsNullOrEmpty(collection: array))
            {
                await foreach (var element in ArrayAsyncEnumerable.GetAsyncEnumerable(array!).ConfigureAwait(false))
                {
                    await task(element).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Extension method that, for each element of <typeparamref name="T"/> in <paramref name="list"/>, perform <paramref
        /// name="task"/> asynchronously..
        /// </summary>
        /// <typeparam name="T">Specifies the type to cast <paramref name="list"/> to.</typeparam>
        /// <param name="list">Specifies <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> elements to enumerate and process.</param>
        /// <param name="task">Specifies the <see cref="Task{T}"/> to process <paramref name="array"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T>? list, Func<T, Task<T>> task)
        {
            if (!CliUtils.IsNullOrEmpty(list))
            {
                await foreach (var element in ListAsyncEnumerable.GetAsyncEnumerable(list!).ConfigureAwait(false))
                {
                    await task(element).ConfigureAwait(false);
                }
            }
        }

        #endregion Public Methods
    }
}
