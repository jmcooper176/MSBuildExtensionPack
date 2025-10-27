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
namespace Security
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Static class containing all PInvoke methods for WinInet API
    /// </summary>
    [CompilerGeneratedAttribute]
    internal static class NativeMethods
    {
        [DllImport("crypt32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CryptAcquireCertificatePrivateKey(IntPtr pCert, uint dwFlags, IntPtr pvReserved, ref IntPtr phCryptProv, ref int pdwKeySpec, ref bool pfCallerFreeProv);

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CryptGetProvParam(IntPtr hCryptProv, CryptGetProvParamType dwParam, IntPtr pvData, ref int pcbData, uint dwFlags);

        [DllImport("advapi32", SetLastError = true)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);
    }
}
