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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Enumeration of all Apple operating systems, the Safari web browser, and the <c>XCode</c><c>macOS</c> IDE.
    /// </summary>
    public enum AppleOs
    {
        /// <summary>
        /// An unknown Apple operating system or product.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Unknown = 0,

        /// <summary>
        /// The <c>macOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MacOS,

        /// <summary>
        /// The <c>iOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IOs,

        /// <summary>
        /// The <c>iPadOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IPadOs,

        /// <summary>
        /// The <c>watchOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        WatchOs,

        /// <summary>
        /// The <c>tvOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TvOs,

        /// <summary>
        /// The <c>visionOS</c> Apple operating system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        VisionOs,

        /// <summary>
        /// The Safari Apple web browser.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Safari,

        /// <summary>
        /// The <c>XCode</c> Apple integrated development environment (IDE) for <c>macOS</c>.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        XCode,
    }
}
