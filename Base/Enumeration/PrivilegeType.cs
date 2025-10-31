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

    public enum PrivilegeType
    {
        /// <summary>
        /// SeInteractiveLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeInteractiveLogonRight,

        /// <summary>
        /// SeNetworkLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeNetworkLogonRight,

        /// <summary>
        /// SeBatchLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeBatchLogonRight,

        /// <summary>
        /// SeServiceLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeServiceLogonRight,

        /// <summary>
        /// SeDenyInteractiveLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeDenyInteractiveLogonRight,

        /// <summary>
        /// SeDenyNetworkLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeDenyNetworkLogonRight,

        /// <summary>
        /// SeDenyBatchLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeDenyBatchLogonRight,

        /// <summary>
        /// SeDenyServiceLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeDenyServiceLogonRight,

        /// <summary>
        /// SeRemoteInteractiveLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeRemoteInteractiveLogonRight,

        /// <summary>
        /// SeDenyRemoteInteractiveLogonRight
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeDenyRemoteInteractiveLogonRight,

        /// <summary>
        /// SeIncreaseQuotaPrivilege
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeIncreaseQuotaPrivilege,

        /// <summary>
        /// SeAuditPrivilege
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeAuditPrivilege,

        /// <summary>
        /// SeAssignPrimaryTokenPrivilege
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SeAssignPrimaryTokenPrivilege
    }
}
