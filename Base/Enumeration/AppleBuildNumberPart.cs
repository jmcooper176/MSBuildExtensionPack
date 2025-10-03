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
namespace MSBuild.ExtensionPack.Base.Enumeration
{
    using System;

    [Flags]
    public enum AppleBuildNumberPart : int
    {
        /// <summary>
        /// No increment of any part of the Apple build number will be attempted.
        /// </summary>
        None = 0,

        /// <summary>
        /// The build major part of the Apple build number will be incremented.
        /// </summary>
        Major = 1,

        /// <summary>
        /// The release character part of the Apple build number will be incremented.
        /// </summary>
        ReleaseChar = 2,

        /// <summary>
        /// The build revision part of the Apple build number will be incremented.
        /// </summary>
        Revision = 4,

        /// <summary>
        /// All parts of the Apple build number will be incremented.
        /// </summary>
        All = Major | ReleaseChar | Revision
    }
}
