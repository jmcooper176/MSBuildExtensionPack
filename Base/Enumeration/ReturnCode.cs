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

    using MSBuild.ExtensionPack.ErrorMessage.Code;

    public enum ReturnCode : int
    {
        /// <summary>
        /// Success
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Success = WinError.ERROR_SUCCESS,

        /// <summary>
        /// AccessDenied
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AccessDenied = WinError.ERROR_ACCESS_DENIED,

        /// <summary>
        /// UnknownFailure
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UnknownFailure = WinError.ERROR_UNKNOWN_ERROR,

        /// <summary>
        /// InvalidName
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidName = WinError.ERROR_INVALID_NAME,

        /// <summary>
        /// InvalidLevel
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidLevel = WinError.ERROR_INVALID_LEVEL,

        /// <summary>
        /// InvalidParameter
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter = WinError.ERROR_INVALID_PARAMETER,

        /// <summary>
        /// ShareAlreadyExists
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ShareAlreadyExists = WinError.ERROR_SHARING_VIOLATION,

        /// <summary>
        /// RedirectedPath
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RedirectedPath = WinError.ERROR_PATH_NOT_FOUND,

        /// <summary>
        /// UnknownDeviceOrDirectory
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UnknownDeviceOrDirectory = WinError.ERROR_DEVICE_NOT_CONNECTED,

        /// <summary>
        /// NetNameNotFound
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NetNameNotFound = WinError.ERROR_BAD_NET_NAME,
    }
}
