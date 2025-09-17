// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

using Microsoft.Win32;

using System.Security.AccessControl;

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
        /// Closes the specified <see cref="RegistryKey"/> if it is not <see langref="null"/> and then set it to <see langref="null"/>.
        /// </summary>
        /// <param name="hKey">Specifies the <see cref="RegistryKey"/> to close.</param>
        public static void Close(this RegistryHive? hKey)
        {
            if (hKey is not null)
            {
                hKey.Close();
                hKey = null;
            }
        }

        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name)
        {
            return OpenSubKey(hKey, view, name, RegistryKeyPermissionCheck.Default);
        }

        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name, RegistryKeyPermissionCheck permissionCheck)
        {
            return RegistryKey.OpenBaseKey(hKey, view).OpenSubKey(name, permissionCheck);
        }

        public static RegistryKey? OpenSubKey(RegistryHive hKey, RegistryView view, string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            return RegistryKey.OpenBaseKey(hKey, view).OpenSubKey(name, permissionCheck, rights);
        }

        #endregion Public Methods
    }
}
