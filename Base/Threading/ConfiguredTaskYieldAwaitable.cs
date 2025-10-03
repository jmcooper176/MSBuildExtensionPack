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
namespace MSBuild.ExtensionPack.Base.Threading
{
    /// <summary>
    /// An awaitable that will always lead the calling async method to yield, then immediately resume, possibly on the original <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <remarks>Initializes a new instance of the <see cref="ConfiguredTaskYieldAwaitable"/> struct.</remarks>
    /// <param name="continueOnCapturedContext">
    /// A value indicating whether the continuation should run on the captured <see cref="SynchronizationContext"/>, if any.
    /// </param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public readonly struct ConfiguredTaskYieldAwaitable(bool continueOnCapturedContext)
    {
        #region Private Fields

        /// <summary>
        /// A value indicating whether the continuation should run on the captured <see cref="SynchronizationContext"/>, if any.
        /// </summary>
        private readonly bool continueOnCapturedContext = continueOnCapturedContext;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets the <see cref="ConfiguredTaskYieldAwaiter"/>.
        /// </summary>
        /// <returns>The <see cref="ConfiguredTaskYieldAwaiter"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ConfiguredTaskYieldAwaiter GetAwaiter() => new(continueOnCapturedContext);

        #endregion Public Methods
    }
}
