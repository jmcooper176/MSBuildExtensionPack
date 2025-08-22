// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

namespace MSBuild.ExtensionPack.Base.Validator
{
    /// <summary>
    /// Enumeration of pre-defined <see cref="int"/> value ranges.
    /// </summary>
    public enum IntValidatorRange
    {
        /// <summary>
        /// All values of <see cref="int"/> are valid.
        /// </summary>
        All,

        /// <summary>
        /// Only positive and non-zero values of <see cref="int"/> are valid.
        /// </summary>
        Positive,

        /// <summary>
        /// Only <see cref="Positive"/> and zero values of <see cref="int"/> are valid.
        /// </summary>
        NonNegative,

        /// <summary>
        /// Only negative values of <see cref="int"/> are valid.
        /// </summary>
        Negative,

        /// <summary>
        /// Only non-zero <see cref="int"/> values of <see cref="int"/> are valid.
        /// </summary>
        NonZero,

        /// <summary>
        /// Only zero <see cref="int"/> values of <see cref="int"/> are valid.
        /// </summary>
        Zero,

        /// <summary>
        /// Only values of <see cref="int"/> between a minimum and maximum range are valid.
        /// </summary>
        Inclusive,

        /// <summary>
        /// Only values of <see cref="int"/> between, but not equal to, a minimum and maximum range are valid.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Only values of <see cref="int"/> between a minimum and a maximum, but not equal to, range are valid.
        /// </summary>
        HalfInclusive,

        /// <summary>
        /// Only values of <see cref="int"/> between a minimum, but not equal to, value and a maximum value range are valid.
        /// </summary>
        HalfExclusive,

        /// <summary>
        /// All values of <see cref="int"/> are valid.
        /// </summary>
        None,
    }
}
