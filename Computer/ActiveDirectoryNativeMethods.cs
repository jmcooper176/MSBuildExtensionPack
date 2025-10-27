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
namespace Computer
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    [StructLayout(LayoutKind.Sequential)]
    internal struct LSA_OBJECT_ATTRIBUTES
    {
        internal uint Length;
        internal IntPtr RootDirectory;
        internal LSA_UNICODE_STRING ObjectName;
        internal uint Attributes;
        internal IntPtr SecurityDescriptor;
        internal IntPtr SecurityQualityOfService;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LSA_UNICODE_STRING
    {
        internal ushort Length;
        internal ushort MaximumLength;
        internal IntPtr Buffer;
    }

    /// <summary>
    /// RegistryNativeMethods
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class ActiveDirectoryNativeMethods
    {
        internal const int POLICY_CREATE_SECRET = 20;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int LookupAccountName([In, MarshalAs(UnmanagedType.LPTStr)] string systemName, [In, MarshalAs(UnmanagedType.LPTStr)] string accountName, IntPtr Sid, ref int cbSid, StringBuilder domainName, ref int cbDomainName, ref int use);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint LsaAddAccountRights(IntPtr PolicyHandle, IntPtr AccountSid, ref LSA_UNICODE_STRING UserRights, uint CountOfRights);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern uint LsaClose(IntPtr ObjectHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern uint LsaOpenPolicy(ref LSA_UNICODE_STRING SystemName, ref LSA_OBJECT_ATTRIBUTES ObjectAttributes, int DesiredAccess, out IntPtr PolicyHandle);
    }
}
