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
namespace MSBuild.ExtensionPack.Communication.Extended
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Static class containing all PInvoke methods for WinInet API
    /// </summary>
    public static partial class NativeMethods
    {
        #region Private Fields

        private const uint FormatMessageFromSystem = 4096;
        private const uint FormatMessageIgnoreInserts = 512;

        #endregion Private Fields

        #region Internal Methods

        [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [MarshalAsAttribute(UnmanagedType.LPTStr)] StringBuilder lpBuffer, uint nSize, IntPtr arguments);

        [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial int FreeLibrary(IntPtr hModule);

        [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial IntPtr LoadLibrary([In][MarshalAsAttribute(UnmanagedType.LPTStr)] string lpLibFileName);

        internal static string TranslateInternetError(uint errorCode)
        {
            IntPtr hModule = IntPtr.Zero;

            try
            {
                StringBuilder buf = new(255);
                hModule = LoadLibrary("wininet.dll");

                if (FormatMessage(FormatMessageFromSystem | FormatMessageIgnoreInserts, hModule, errorCode, 0U, buf, (uint)buf.Capacity + 1, IntPtr.Zero) != 0)
                {
                    return buf.ToString();
                }
                else
                {
                    Debug.WriteLine("Error:: {0}", Marshal.GetLastWin32Error());
                    return string.Empty;
                }
            }
            finally
            {
                FreeLibrary(hModule);
            }
        }

        #endregion Internal Methods

        #region Public Fields

        public const int ErrorInternetExtendedError = (InternetErrorBase + 3);
        public const int ErrorNoMoreFiles = 18;
        public const int FileAttributeDirectory = 16;
        public const int FileAttributeNormal = 128;
        public const int FtpTransferTypeAscii = 0x00000001;
        public const int FtpTransferTypeBinary = 0x00000002;
        public const int FtpTransferTypeUnknown = 0x00000000;
        public const int InternetDefaultFtpPort = 21;
        public const int InternetErrorBase = 12000;
        public const int InternetFlagAsync = 0x10000000;
        public const int InternetFlagFromCache = 0x01000000;
        public const int InternetFlagHyperlink = 0x00000400;
        public const int InternetFlagNeedFile = 0x00000010;
        public const int InternetFlagNoCacheWrite = 0x04000000;
        public const int InternetFlagOffline = 0x01000000;
        public const int InternetFlagPassive = 0x08000000;
        public const int InternetFlagReload = 8;
        public const int InternetFlagResynchronize = 0x00000800;
        public const int InternetFlagSync = 0x00000004;
        public const int InternetNoCallback = 0;
        public const int InternetOpenTypeDirect = 1;
        public const int InternetOpenTypePreconfig = 0;
        public const int InternetServiceFtp = 1;
        public const int MaxPath = 260;
        public const int NoError = 0;

        #endregion Public Fields

        #region Public Methods

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpCommand(
            [In] IntPtr hConnect,
            [In] bool fExpectResponse,
            [In] int dwFlags,
            [In] string command,
            [In] IntPtr dwContext,
            [In][Out] ref IntPtr ftpCmd);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpCreateDirectory(
            [In] IntPtr hConnect,
            [In] string directory);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpDeleteFile(
            [In] IntPtr hConnect,
            [In] string fileName);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr FtpFindFirstFile(
            [In] IntPtr hConnect,
            [In] string searchFile,
            [In][Out] ref NativeMethods.WIN32_FIND_DATA findFileData,
            [In] int dwFlags,
            [In] IntPtr dwContext);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpGetCurrentDirectory(
            [In] IntPtr hConnect,
            [In][Out] StringBuilder currentDirectory,
            [In][Out] ref int dwCurrentDirectory);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpGetFile(
            [In] IntPtr hConnect,
            [In] string remoteFile,
            [In] string newFile,
            [In] bool failIfExists,
            [In] int dwFlagsAndAttributes,
            [In] int dwFlags,
            [In] IntPtr dwContext);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpGetFileSize(
            [In] IntPtr hConnect,
            [In][Out] ref int dwFileSizeHigh);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpOpenFile(
            [In] IntPtr hConnect,
            [In] string fileName,
            [In] int dwAccess,
            [In] int dwFlags,
            [In] IntPtr dwContext);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpPutFile(
            [In] IntPtr hConnect,
            [In] string localFile,
            [In] string newRemoteFile,
            [In] int dwFlags,
            [In] IntPtr dwContext);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpRemoveDirectory(
            [In] IntPtr hConnect,
            [In] string directory);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpRenameFile(
            [In] IntPtr hConnect,
            [In] string existingName,
            [In] string newName);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int FtpSetCurrentDirectory(
            [In] IntPtr hConnect,
            [In] string directory);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int InternetCloseHandle(
            [In] IntPtr hInternet);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr InternetConnect(
            [In] IntPtr hInternet,
            [In] string serverName,
            [In] int serverPort,
            [In] string userName,
            [In] string password,
            [In] int dwService,
            [In] int dwFlags,
            [In] IntPtr dwContext);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int InternetFindNextFile(
            [In] IntPtr hInternet,
            [In][Out] ref NativeMethods.WIN32_FIND_DATA findData);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int InternetGetLastResponseInfo(
            [In][Out] ref int dwError,
            [MarshalAs(UnmanagedType.LPTStr)][Out] StringBuilder buffer,
            [In][Out] ref int bufferLength);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial IntPtr InternetOpen(
            [In] string agent,
            [In] int dwAccessType,
            [In] string proxyName,
            [In] string proxyBypass,
            [In] int dwFlags);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int InternetReadFile(
            [In] IntPtr hConnect,
            [MarshalAs(UnmanagedType.LPTStr)][In][Out] StringBuilder buffer,
            [In] int buffCount,
            [In][Out] ref int bytesRead);

        [LibraryImport("wininet.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int InternetReadFileEx(
            [In] IntPtr hFile,
            [In][Out] ref NativeMethods.INTERNET_BUFFERS lpBuffersOut,
            [In] int dwFlags,
            [In][Out] int dwContext);

        #endregion Public Methods

        #region Public Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct FILETIME
        {
            public int dwHighDateTime;
            public int dwLowDateTime;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct INTERNET_BUFFERS
        {
            public int dwBufferLength;
            public int dwBufferTotal;
            public int dwHeadersLength;
            public int dwHeadersTotal;
            public int dwOffsetHigh;
            public int dwOffsetLow;
            public int dwStructSize;
            public IntPtr lpvBuffer;
            public IntPtr Next;
            public string Header;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WIN32_FIND_DATA
        {
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public int dfFileAttributes;
            public int dwReserved0;
            public int dwReserved1;
            public int nFileSizeHigh;
            public int nFileSizeLow;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxPath)]
            public char[] fileName;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] alternateFileName;
        }

        #endregion Public Structs
    }
}
