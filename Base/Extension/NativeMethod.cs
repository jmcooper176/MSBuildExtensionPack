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
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Microsoft.VisualStudio.Threading;
using Microsoft.Win32.SafeHandles;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Implements <c>P/INVOKE</c> methods to support <c>vs-threading</c> (with modifications).
    /// </summary>
    internal static partial class NativeMethod
    {
        #region Internal Fields

        /// <summary>
        /// Constant indicating that the lifetime of the registration must not be tied to the lifetime of the thread issuing the
        /// <see cref="RegNotifyChangeKeyValue(SafeRegistryHandle, bool, RegistryChangeNotificationFilters, SafeWaitHandle, bool)"/> call.
        /// </summary>
        [SupportedOSPlatform("Windows10.0")]
        internal const RegistryChangeNotificationFilters REG_NOTIFY_THREAD_AGNOSTIC = (RegistryChangeNotificationFilters)0x10000000L;

        #endregion Internal Fields

        #region Internal Methods

        /// <summary>
        /// Register to receive notifications of changes to a registry key.
        /// </summary>
        /// <param name="hKey">        Specifies the handle to the registry key to watch.</param>
        /// <param name="watchSubtree">
        /// If <see langref="true"/>, watch the descendant keys as well; otherwise, <see langref="false"/> to watch only this key.
        /// </param>
        /// <param name="notifyFilter">Specifies the types of changes to watch for.</param>
        /// <param name="hEvent">      Specifies the handle to the event to set when a change occurs.</param>
        /// <param name="asynchronous">
        /// If this parameter is <see langref="true"/>, the function returns immediately and reports changes by signaling the
        /// specified event. If this parameter is <see langref="false"/>, the function does not return until a change has occurred.
        /// </param>
        /// <returns>A win32 error code. ERROR_SUCCESS (0) if successful.</returns>
        [SupportedOSPlatform("Windows")]
        [LibraryImport("advapi32.dll", SetLastError = true)]
        internal static partial int RegNotifyChangeKeyValue(
            SafeRegistryHandle hKey,
            [MarshalAs(UnmanagedType.Bool)] bool watchSubtree,
            RegistryChangeNotificationFilters notifyFilter,
            SafeWaitHandle hEvent,
            [MarshalAs(UnmanagedType.Bool)] bool asynchronous);

        [SupportedOSPlatform("Windows")]
        [LibraryImport("user32.dll", SetLastError = true)]
        internal static partial int SendMessageTimeout(int hWnd, int Msg, int wParam, string lParam, int fuFlags, int uTimeout, int lpdwResult);

        /// <summary>
        /// Waits for multiple objects.
        /// </summary>
        /// <param name="handleCount">        Specifies he number of handles in the <paramref name="waitHandles"/> array.</param>
        /// <param name="waitHandles">        Specifies the handles to wait for.</param>
        /// <param name="waitAll">            Specifies a flag indicating whether all handles must be signaled before returning.</param>
        /// <param name="millisecondsTimeout">Specifies the timeout that will cause this method to return.</param>
        /// <remarks>
        /// Raw <see cref="IntPtr"/> have to be used, because the marshaller does not support arrays of <see cref="SafeHandle"/>,
        /// only singletons.
        /// </remarks>
        [SupportedOSPlatform("Windows")]
        [LibraryImport("kernel32.dll", SetLastError = true)]
        internal static partial int WaitForMultipleObjects(uint handleCount, [In] IntPtr[] waitHandles, [MarshalAs(UnmanagedType.Bool)] bool waitAll, uint millisecondsTimeout);

        #endregion Internal Methods

        #region Internal Fields

        internal const int HWND_BROADCAST = 0xffff;
        internal const int SENDMESSAGE_TIMEOUT = 10000;
        internal const int SMTO_ABORTIFHUNG = 0x0002;
        internal const int WM_SETTINGCHANGE = 0x001A;

        #endregion Internal Fields
    }
}
