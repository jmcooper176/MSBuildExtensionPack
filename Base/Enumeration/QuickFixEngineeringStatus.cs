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
    /// Enumeration of the <c>CIM_ManagedSystemElement</c> 'Status' property values."/&gt;
    /// </summary>
    public enum QuickFixEngineeringStatus
    {
        /// <summary>
        /// Operational status hot fix or service pack 'OK'.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Ok = 0,

        /// <summary>
        /// Non-operational status hot fix or service pack 'Error'.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Error,

        /// <summary>
        /// Operational status hot fix or service pack 'Degraded'.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Degraded,

        /// <summary>
        /// Unknown status of hot fix or service pack.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Unknown,

        /// <summary>
        /// Operational status hot fix or service pack 'OK' but 'Predicted Failure' usually of an underlying device.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PredictFailure,

        /// <summary>
        /// Non-operational status hot fix or service pack 'Starting'.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Starting,

        /// <summary>
        /// Non-operational status hot fix or service pack 'Stopping'.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Stopping,

        /// <summary>
        /// Non-operational status hot fix or service pack 'Service' usually of an underlying device, reloading of a user permission
        /// list, or other administrative work.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Service,

        /// <summary>
        /// Stressed status of hot fix or service pack.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Stressed,

        /// <summary>
        /// Non-recoverable error status of hot fix or service pack.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NonRecoverable,

        /// <summary>
        /// No contact status of hot fix or service pack.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoContact,

        /// <summary>
        /// Lost communication status of hot fix or service pack.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LostCommunication,
    }
}
