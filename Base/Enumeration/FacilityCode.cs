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

    using MSBuild.ExtensionPack.Base.Extension;
    using MSBuild.ExtensionPack.Base.SystemAttribute;

    /// <summary>
    /// Enumeration of the facility codes for HRESULT values.
    /// </summary>
    public enum FacilityCode : int
    {
        /// <summary>
        /// The default facility code.
        /// </summary>
        FACILITY_NULL = 0,

        /// <summary>
        /// The source of the error code is an RPC subsystem.
        /// </summary>
        FACILITY_RPC = 1,

        /// <summary>
        /// The source of the error code is the COM subsystem.
        /// </summary>
        FACILITY_DISPATCH = 2,

        /// <summary>
        /// The source of the error code is OLE storage.
        /// </summary>
        FACILITY_STORAGE = 3,

        /// <summary>
        /// The source of the error code is COM/OLE interface management.
        /// </summary>
        FACILITY_ITF = 4,

        /// <summary>
        /// This region is reserved to map undecorated error codes into HRESULTs.
        /// </summary>
        FACILITY_WIN32 = 7,

        /// <summary>
        /// The source of the error code is the Windows subsystem.
        /// </summary>
        FACILITY_WINDOWS = 8,

        /// <summary>
        /// The source of the error code is the Security API layer.
        /// </summary>
        FACILITY_SECURITY = 9,

        /// <summary>
        /// The source of the error code is the Security API layer.
        /// </summary>
        FACILITY_SSPI = FACILITY_SECURITY,

        /// <summary>
        /// The source of the error code is the control mechanism.
        /// </summary>
        FACILITY_CONTROL = 10,

        /// <summary>
        /// The source of the error code is a certificate client or server.
        /// </summary>
        FACILITY_CERT = 11,

        /// <summary>
        /// The source of the error code is <c>Wininet</c> related.
        /// </summary>
        FACILITY_INTERNET = 12,

        /// <summary>
        /// The source of the error code is the Windows Media Server.
        /// </summary>
        FACILITY_MEDIASERVER = 13,

        /// <summary>
        /// The source of the error code is the Microsoft Message Queue (MSMQ).
        /// </summary>
        FACILITY_MSMQ = 14,

        /// <summary>
        /// The source of the error code is the Setup API.
        /// </summary>
        FACILITY_SETUPAPI = 15,

        /// <summary>
        /// The source of the error code is the Smart-card subsystem.
        /// </summary>
        FACILITY_SCARD = 16,

        /// <summary>
        /// The source of the error code is the COM+.
        /// </summary>
        FACILITY_COMPLUS = 17,

        /// <summary>
        /// The source of the error code is the Microsoft agent.
        /// </summary>
        FACILITY_AAF = 18,

        /// <summary>
        /// The source of the error code is the .NET Common Language Runtime (CLR) and related systems.
        /// </summary>
        FACILITY_URT = 19,

        /// <summary>
        /// The source of the error code is the audit collection service.
        /// </summary>
        FACILITY_ACS = 20,

        /// <summary>
        /// The source of the error code is Direct Play.
        /// </summary>
        FACILITY_DPLAY = 21,

        /// <summary>
        /// The source of the error code is the ubiquitous memory introspection (UMI) service.
        /// </summary>
        FACILITY_UMI = 22,

        /// <summary>
        /// The source of the error code is side-by-side (SxS) servicing.
        /// </summary>
        FACILITY_SXS = 23,

        /// <summary>
        /// The source of the error code is specific to Windows CE.
        /// </summary>
        FACILITY_WINDOWS_CE = 24,

        /// <summary>
        /// The source of the error code is HTTP support.
        /// </summary>
        FACILITY_HTTP = 25,

        /// <summary>
        /// The source of the error code is common logging support.
        /// </summary>
        FACILITY_USERMODE_COMMONLOG = 26,

        /// <summary>
        /// The source of the error code is user mode filter manager.
        /// </summary>
        FACILITY_USERMODE_FILTER_MANAGER = 31,

        /// <summary>
        /// The source of the error code is the background copy service (also known as BITS).
        /// </summary>
        FACILITY_BACKGROUNDCOPY = 32,

        /// <summary>
        /// The source of the error code is configuration services.
        /// </summary>
        FACILITY_CONFIGURATION = 33,

        /// <summary>
        /// The source of the error code is state management services.
        /// </summary>
        FACILITY_STATE_MANAGEMENT = 34,

        /// <summary>
        /// The source of the error code is the Microsoft Identity Integration Server (MIIS) also known as Metadirectory Services.
        /// </summary>
        FACILITY_METADIRECTORY = 35,

        /// <summary>
        /// The source of the error code is a Windows Update.
        /// </summary>
        FACILITY_WINDOWSUPDATE = 36,

        /// <summary>
        /// The source of the error code is Active Directory or Entra.
        /// </summary>
        FACILITY_DIRECTORYSERVICE = 37,

        /// <summary>
        /// The source of the error code is graphics drivers.
        /// </summary>
        FACILITY_GRAPHICS = 38,

        /// <summary>
        /// The source of the error code is the user Shell.
        /// </summary>
        FACILITY_SHELL = 39,

        /// <summary>
        /// The source of the error code is the Trusted Platform Module services.
        /// </summary>
        FACILITY_TPM_SERVICES = 40,

        /// <summary>
        /// The source of the error code is the Trusted Platform Module applications.
        /// </summary>
        FACILITY_TPM_SOFTWARE = 41,

        /// <summary>
        /// The source of the error code is Performance Logs and Alerts.
        /// </summary>
        FACILITY_PLA = 48,

        /// <summary>
        /// The source of the error code is Full volume encryption.
        /// </summary>
        FACILITY_FVE = 49,

        /// <summary>
        /// The source of the error code is the Firewall Platform.
        /// </summary>
        FACILITY_FWP = 50,

        /// <summary>
        /// The source of the error code is Windows Resource Manager.
        /// </summary>
        FACILITY_WINRM = 51,

        /// <summary>
        /// The source of the error code is the Network Driver Interface Specification (NDIS).
        /// </summary>
        FACILITY_NDIS = 52,

        /// <summary>
        /// The source of the error code is the user mode Hypervisor components.
        /// </summary>
        FACILITY_USERMODE_HYPERVISOR = 53,

        /// <summary>
        /// The source of the error code is the Configuration Management Infrastructure (CMI) provider.
        /// </summary>
        FACILITY_CMI = 54,

        /// <summary>
        /// The source of the error code is the user mode virtualization system.
        /// </summary>
        FACILITY_USERMODE_VIRTUALIZATION = 55,

        /// <summary>
        /// The source of the error code is the user mode volume manager.
        /// </summary>
        FACILITY_USERMODE_VOLMGR = 56,

        /// <summary>
        /// The source of the error code is the Boot Configuration Database (BCD).
        /// </summary>
        FACILITY_BCD = 57,

        /// <summary>
        /// The source of the error code is the user mode virtual hard disk (VHD) support.
        /// </summary>
        FACILITY_USERMODE_VHD = 58,

        /// <summary>
        /// The source of the error code is System Diagnostics.
        /// </summary>
        FACILITY_SDIAG = 60,

        /// <summary>
        /// The source of the error code is Web Services.
        /// </summary>
        FACILITY_WEBSERVICES = 61,

        /// <summary>
        /// The source of the error code is a Windows Defender component.
        /// </summary>
        FACILITY_WINDOWS_DEFENDER = 80,

        /// <summary>
        /// The source of the error code is the open connectivity (OPC) service.
        /// </summary>
        FACILITY_OPC = 81,

        /// <summary>
        /// Mask to isolate the Windows error code or status code from an HRESULT value.
        /// </summary>
        HRESULT_MASK = 0x0000_1FFF,

        /// <summary>
        /// Mask to remove the facility code and facility Windows NT Bit from an HRESULT value.
        /// </summary>
        FACILITY_MASK = 0x0000_FFFF,

        /// <summary>
        /// Mask to isolate the Windows NT Bit from an HRESULT value.
        /// </summary>
        FACILITY_NT_BIT = 0x1000_0000,
    }

    public static class WindowsFacilityCodeExtension
    {
        #region Public Methods

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateField"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateField"/>.</returns>
        public static bool? GetAutoGenerateField(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateField;
        }

        /// <summary>
        /// Extension method to determine whether the <see cref="DisplayAttribute.AutoGenerateFilter"/> is set for the <see
        /// cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="bool"/> or <see langref="null"/> representing the state of the <see cref="DisplayAttribute.AutoGenerateFilter"/>.</returns>
        public static bool? GetAutoGenerateFilter(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.AutoGenerateFilter;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string? GetDescription(this FacilityCode value, bool inherit = false)
        {
            return value.GetDescriptionAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetDescription2(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DescriptionAttribute"/> or <see langref="null"/> if no <see cref="DescriptionAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DescriptionAttribute? GetDescriptionAttribute(this FacilityCode value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DescriptionAttribute, FacilityCode>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the <see cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DisplayAttribute"/> or <see langref="null"/> if no <see cref="DisplayAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DisplayAttribute? GetDisplayAttribute(this FacilityCode value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DisplayAttribute, FacilityCode>(value, inherit);
        }

        public static int GetFacilityCodeFromHResult(this int hr)
        {
            return hr >> 16 & (int)EnumExtension.GetValueAsUnderlyingType(FacilityCode.HRESULT_MASK);
        }

        public static int GetFacilityCodeFromStatusCode(this int code)
        {
            return code.GetFacilityCodeFromHResult();
        }

        /// <summary>
        /// Extension method to recover the group name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the group name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetGroupName(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.GroupName;
        }

        /// <summary>
        /// Extension method to recover the name string from the <see cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetName(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>An <see cref="int"/> or <see langref="null"/> representing the order property of the <see cref="DisplayAttribute"/>.</returns>
        public static int? GetOrder(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Order;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the prompt for the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetPrompt(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Prompt;
        }

        /// <summary>
        /// Extension method to recover the resource <see cref="Type"/> from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="Type"/> or <see langref="null"/> representing the resource <see cref="Type"/> of the <see cref="DisplayAttribute"/>.</returns>
        public static Type? GetResourceType(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ResourceType;
        }

        public static bool IsHResultFromFacilityCode(this int hr, FacilityCode facilityCode)
        {
            return hr.GetFacilityCodeFromHResult() == (int)facilityCode;
        }

        public static bool IsHResultFromFacilityCode(this int hr, int facilityCode)
        {
            return hr.GetFacilityCodeFromHResult() == facilityCode;
        }

        public static bool IsHResultFromFacilityCode(this int hr, string facilityCodeName)
        {
            if (string.IsNullOrWhiteSpace(facilityCodeName))
            {
                return false;
            }
            if (!Enum.TryParse<FacilityCode>(facilityCodeName, true, out var facilityCode))
            {
                return false;
            }
            return hr.GetFacilityCodeFromHResult() == (int)facilityCode;
        }

        public static bool IsHResultFromFacilityCode(this int hr, FacilityCode? facilityCode)
        {
            if (facilityCode == null)
            {
                return false;
            }
            return hr.GetFacilityCodeFromHResult() == (int)facilityCode;
        }

        public static bool IsHResultFromFacilityCode(this int hr, Enum? facilityCode)
        {
            if (facilityCode == null)
            {
                return false;
            }
            if (facilityCode.GetType() != typeof(FacilityCode))
            {
                return false;
            }
            return hr.GetFacilityCodeFromHResult() == Convert.ToInt32(facilityCode);
        }

        public static bool IsStatusCodeFromFacilityCode(this int code, int facilityCode)
        {
            return code.GetFacilityCodeFromHResult() == facilityCode;
        }

        public static bool IsStatusCodeFromFacilityCode(this int code, string facilityCodeName)
        {
            if (string.IsNullOrWhiteSpace(facilityCodeName))
            {
                return false;
            }
            if (!Enum.TryParse<FacilityCode>(facilityCodeName, true, out var facilityCode))
            {
                return false;
            }
            return code.GetFacilityCodeFromHResult() == (int)facilityCode;
        }

        public static bool IsStatusCodeFromFacilityCode(this int code, FacilityCode facilityCode)
        {
            return code.GetFacilityCodeFromHResult() == facilityCode.ToInt32();
        }

        /// <summary>
        /// Extension method to recover the short name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FacilityCode"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FacilityCode"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the short name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? ShortName(this FacilityCode value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ShortName;
        }

        public static FacilityCode ToFacilityCode(this int hr)
        {
            return (FacilityCode)(hr.GetFacilityCodeFromHResult());
        }

        public static FacilityCode ToFacilityCodeFromStatusCode(this int code)
        {
            return (FacilityCode)(code.GetFacilityCodeFromHResult());
        }

        public static int ToHResult(this FacilityCode facilityCode, int code)
        {
            return (int)facilityCode << 16 | (code & FacilityCode.HRESULT_MASK.ToInt32());
        }

        public static int ToInt32(this FacilityCode facilityCode) => (int)EnumExtension.ToUnderlyingType<FacilityCode, int>(facilityCode);

        public static int ToStatusCode(this FacilityCode facilityCode, int code)
        {
            return facilityCode.ToHResult(code);
        }

        #endregion Public Methods
    }
}
