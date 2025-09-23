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
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Security.AccessControl;

using Microsoft.VisualStudio.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace MSBuild.ExtensionPack.Base.Extension
{
    internal static class RegistryKeyExtension
    {
        #region Public Properties

        /// <summary>
        /// Returns the 32 bit HKLM\SOFTWARE registry key. May return <see langref="null"/> if it doesn't exist.
        /// </summary>
        /// <remarks>Call must dispose of the <see cref="RegistryKey"/> return value.</remarks>
        public static RegistryKey? HklmSoftware32Bit
        {
            get
            {
                return Environment.Is64BitProcess ? HklmSoftwareNonnative : HklmSoftwareNative;
            }
        }

        /// <summary>
        /// Returns the 64 bit HKLM\SOFTWARE registry key. May return <see langref="null"/> if it doesn't exist.
        /// </summary>
        /// <remarks>Call must dispose of the <see cref="RegistryKey"/> return value.</remarks>
        public static RegistryKey? HklmSoftware64Bit
        {
            get
            {
                return Environment.Is64BitProcess ? HklmSoftwareNative : HklmSoftwareNonnative;
            }
        }

        /// <summary>
        /// For a 32 bit process, it returns the 32 bit HKLM\SOFTWARE registry key, otherwise the 64 bit one. May return <see
        /// langref="null"/> if it doesn't exist.
        /// </summary>
        /// <remarks>Call must dispose of the <see cref="RegistryKey"/> return value.</remarks>
        public static RegistryKey? HklmSoftwareNative
        {
            get
            {
                return OpenSubKey(RegistryHive.LocalMachine, RegistryView.Default, "SOFTWARE");
            }
        }

        /// <summary>
        /// For a 32 bit process, it returns the 64 bit HKLM\SOFTWARE registry key, otherwise the 32 bit one. May return <see
        /// langref="null"/> if it doesn't exist.
        /// </summary>
        /// <remarks>Call must dispose of the <see cref="RegistryKey"/> return value.</remarks>
        public static RegistryKey? HklmSoftwareNonnative
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    var view = Environment.Is64BitProcess ? RegistryView.Registry32 : RegistryView.Registry64;
                    return OpenSubKey(RegistryHive.LocalMachine, view, "SOFTWARE");
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a Task that completes when the specified registry key changes.
        /// </summary>
        /// <param name="registryKeyHandle">The handle to the open registry key to watch for changes.</param>
        /// <param name="watchSubtree">     
        /// <c>true</c> to watch the keys descendant keys as well; <c>false</c> to watch only this key without descendants.
        /// </param>
        /// <param name="change">           Indicates the kinds of changes to watch for.</param>
        /// <param name="cancellationToken">
        /// A token that may be canceled to release the resources from watching for changes and complete the returned Task as canceled.
        /// </param>
        /// <returns>A task that completes when the registry key changes, the handle is closed, or upon cancellation.</returns>
        [SupportedOSPlatform("Windows")]
        private static async Task WaitForRegistryChangeAsync(SafeRegistryHandle registryKeyHandle, bool watchSubtree, RegistryChangeNotificationFilters change, CancellationToken cancellationToken)
        {
            const int ERROR_SUCCESS = 0;

            using var evt = new ManualResetEventSlim();

            void registerAction()
            {
                var win32Error = NativeMethod.RegNotifyChangeKeyValue(
                    registryKeyHandle,
                    watchSubtree,
                    change,
                    evt.WaitHandle.SafeWaitHandle,
                    true);
                if (win32Error != ERROR_SUCCESS)
                {
                    throw new Win32Exception(win32Error);
                }
            }

            // Engage our down level support by using a single, dedicated thread to guarantee that we request notification on a
            // thread that will not be destroyed later. Although we *could* await this, we synchronously block because our caller
            // expects subscription to have begun before we return: for the async part to simply be notification. This async method
            // we're calling uses .ConfigureAwait(false) internally so this won't deadlock if we're called on a thread with a
            // single-thread SynchronizationContext.
#pragma warning disable VSTHRD103
            // underlying implementation of
            // <c>GetResult()</c>
            // makes this all but impossible to avoid.
            using var dedicatedThreadReleaser = DownlevelRegistryWatcherSupport.ExecuteOnDedicatedThreadAsync(registerAction).GetAwaiter().GetResult();
#pragma warning restore VSTHRD103

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0))
            {
                change |= NativeMethod.REG_NOTIFY_THREAD_AGNOSTIC;
                registerAction();
            }

            await evt.WaitHandle.ToTask(-1, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the specified <see cref="RegistryKey"/> if it is not <see langref="null"/> and then set it to <see langref="null"/>.
        /// </summary>
        /// <param name="hKey">Specifies the <see cref="RegistryKey"/> to close.</param>
        [SupportedOSPlatform("Windows")]
        public static void Close(this RegistryHive? hKey)
        {
            if (hKey is not null)
            {
                hKey.Close();
                hKey = null;
            }
        }

        [SupportedOSPlatform("Windows")]
        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name)
        {
            return OpenSubKey(hKey, view, name, RegistryKeyPermissionCheck.Default);
        }

        [SupportedOSPlatform("Windows")]
        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name, RegistryKeyPermissionCheck permissionCheck)
        {
            return RegistryKey.OpenBaseKey(hKey, view).OpenSubKey(name, permissionCheck);
        }

        [SupportedOSPlatform("Windows")]
        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            return RegistryKey.OpenBaseKey(hKey, view).OpenSubKey(name, permissionCheck, rights);
        }

        /// <summary>
        /// Returns a Task that completes when the specified registry key changes.
        /// </summary>
        /// <param name="registryKey">      The registry key to watch for changes.</param>
        /// <param name="watchSubtree">     
        /// <c>true</c> to watch the keys descendant keys as well; <c>false</c> to watch only this key without descendants.
        /// </param>
        /// <param name="change">           Indicates the kinds of changes to watch for.</param>
        /// <param name="cancellationToken">
        /// A token that may be canceled to release the resources from watching for changes and complete the returned Task as canceled.
        /// </param>
        /// <returns>A task that completes when the registry key changes, the handle is closed, or upon cancellation.</returns>
        [SupportedOSPlatform("Windows")]
        public static Task WaitForChangeAsync(this RegistryKey registryKey, bool watchSubtree = true, RegistryChangeNotificationFilters change = RegistryChangeNotificationFilters.Value | RegistryChangeNotificationFilters.Subkey, CancellationToken cancellationToken = default)
        {
            return WaitForRegistryChangeAsync(registryKey.Handle, watchSubtree, change, cancellationToken);
        }

        #endregion Public Methods
    }
}
