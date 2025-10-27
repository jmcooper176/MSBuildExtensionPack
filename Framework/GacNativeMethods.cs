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
namespace MSBuild.ExtensionPack
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// GacNativeMethods
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
        internal interface IAssemblyCache
        {
            [PreserveSig]
            int UninstallAssembly(int flags, [MarshalAs(UnmanagedType.LPWStr)] string assemblyName, IntPtr reserved, out int disposition);

            [PreserveSig]
            int QueryAssemblyInfo(int flags, [MarshalAs(UnmanagedType.LPWStr)] string assemblyName, IntPtr assemblyInfo);

            [PreserveSig]
            int CreateAssemblyCacheItem(int flags, IntPtr reserved, out IntPtr assemblyItem, [MarshalAs(UnmanagedType.LPWStr)] string assemblyName);

            [PreserveSig]
            int CreateAssemblyScavenger(out object assemblyScavenger);

            [PreserveSig]
            int InstallAssembly(int flags, [MarshalAs(UnmanagedType.LPWStr)] string manifestFilePath, IntPtr reserved);
        }

        [DllImport("fusion.dll")]
        internal static extern int CreateAssemblyCache(out IAssemblyCache assemblyCache, int reserved);
    }
}
