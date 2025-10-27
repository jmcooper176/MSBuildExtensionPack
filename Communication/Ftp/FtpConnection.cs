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
using MSBuild.ExtensionPack.Communication.Extended;

namespace Communication.Ftp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The <c>FtpConnection</c> class provides the ability to connect and perform operations on FTP servers.
    /// </summary>
    /// <remarks>Initializes a new instance of the FtpConnection class.</remarks>
    /// <param name="host">    The host of the ftp site where the connection would be made.</param>
    /// <param name="port">    The port to make the connection on</param>
    /// <param name="userName">The userName used to make the FTP connection</param>
    /// <param name="password">The Password used to make the FTP connection</param>
    /// <seealso cref="IDisposable"/>
    public class FtpConnection(string host, int port, string userName, string password) : IDisposable
    {
        private readonly string ftpHost = host;
        private readonly string ftpPassword = password;
        private readonly int ftpPort = port;
        private readonly string ftpUserName = userName;
        private IntPtr connectionHandle;
        private IntPtr internetHandle;

        /// <summary>
        /// Finalizes an instance of the FtpConnection class. Disposable types with unmanaged resources need to implement a finalizer.
        /// </summary>
        ~FtpConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// The private helper method to raise exception based on the error occured in native calls
        /// </summary>
        private static void Error()
        {
            int code = Marshal.GetLastWin32Error();

            if (code == NativeMethods.ErrorInternetExtendedError)
            {
                string errorText = InternetLastResponseInfo(ref code);
                throw new FtpException(code, errorText);
            }

            throw new Win32Exception(code, "Error code: " + code + ". Please see: http://support.microsoft.com/kb/193625");
        }

        /// <summary>
        /// Returns full description of the
        /// </summary>
        /// <param name="code">The error code whose details would be returned</param>
        /// <returns>The description of the error code passed</returns>
        private static string InternetLastResponseInfo(ref int code)
        {
            const int buffersize = 8192;
            StringBuilder buff = new(buffersize);
            NativeMethods.InternetGetLastResponseInfo(ref code, buff, ref buffersize);
            return buff.ToString();
        }

        /// <summary>
        /// The method is opens a connection to the Internet and should be invoked before any attempt to connect to the FTP Site.
        /// </summary>
        private void Open()
        {
            if (string.IsNullOrEmpty(ftpHost))
            {
                throw new ArgumentNullException(ftpHost);
            }

            internetHandle = NativeMethods.InternetOpen(Environment.UserName, NativeMethods.InternetOpenTypePreconfig, null, null, NativeMethods.InternetFlagSync);
            if (internetHandle == IntPtr.Zero)
            {
                Error();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resource
            }

            if (connectionHandle != IntPtr.Zero)
            {
                NativeMethods.InternetCloseHandle(connectionHandle);
            }

            if (internetHandle != IntPtr.Zero)
            {
                NativeMethods.InternetCloseHandle(internetHandle);
            }
        }

        /// <summary>
        /// LogOn to the given FTP host using the given ftp UserName and Password
        /// </summary>
        /// <param name="userName">The userName used to LogOn</param>
        /// <param name="password">The password of the user used to LogOn</param>
        protected void LogOn(string userName, string password)
        {
            // If no UserName is given, try connecting with anonymous user
            if (string.IsNullOrEmpty(userName))
            {
                userName = null;
            }

            // If no password is given, try connecting with anonymous user
            if (string.IsNullOrEmpty(password))
            {
                password = null;
            }

            // If there is no connection open, Open a new connection
            if (internetHandle == IntPtr.Zero)
            {
                Open();
            }

            // Connect to the Internet using Ftp Credentials
            connectionHandle = NativeMethods.InternetConnect(internetHandle, ftpHost, ftpPort, userName, password, NativeMethods.InternetServiceFtp, NativeMethods.InternetFlagPassive, IntPtr.Zero);
            if (connectionHandle == IntPtr.Zero)
            {
                Error();
            }
        }

        /// <summary>
        /// Initializes a new instance of the FtpConnection class.
        /// </summary>
        /// <param name="host">The host name of the ftp site where the connection would be made.</param>
        public FtpConnection(string host) : this(host, NativeMethods.InternetDefaultFtpPort, string.Empty, string.Empty)
        {
            ftpHost = host;
        }

        /// <summary>
        /// Initializes a new instance of the FtpConnection class.
        /// </summary>
        /// <param name="host">The host of the ftp site where the connection would be made.</param>
        /// <param name="port">The port to connect to.</param>
        public FtpConnection(string host, int port) : this(host, port, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FtpConnection class.
        /// </summary>
        /// <param name="host">    The host of the ftp site where the connection would be made.</param>
        /// <param name="userName">The userName used to make the FTP connection</param>
        /// <param name="password">The password for the user connecting to the ftp site</param>
        public FtpConnection(string host, string userName, string password) : this(host, NativeMethods.InternetDefaultFtpPort, userName, password)
        {
        }

        public string FtpHost { get; }
        public int Port { get; }

        /// <summary>
        /// Sets the directory on the local machine used to upload / download files.
        /// </summary>
        /// <param name="directory">The directory file path.</param>
        public static void SetLocalDirectory(string directory)
        {
            Environment.CurrentDirectory = Directory.Exists(directory)
                ? directory
                : throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "{0} is not a directory!", directory));
        }

        /// <summary>
        /// Close connection to FTP server and all relevant sessions.
        /// </summary>
        public void Close()
        {
            NativeMethods.InternetCloseHandle(connectionHandle);
            connectionHandle = IntPtr.Zero;

            NativeMethods.InternetCloseHandle(internetHandle);
            internetHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Creates a directory in the remote ftp directory
        /// </summary>
        /// <param name="path">The path of the ftp directory.</param>
        public void CreateDirectory(string path)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpCreateDirectory(connectionHandle, path) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Deletes a file in the Ftp remote directory
        /// </summary>
        /// <param name="directory">The name of the file to be deleted.</param>
        public void DeleteDirectory(string directory)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            int ret = NativeMethods.FtpRemoveDirectory(connectionHandle, directory);
            if (ret == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Deletes a file in the Ftp remote directory
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        public void DeleteFile(string fileName)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            int ret = NativeMethods.FtpDeleteFile(connectionHandle, fileName);
            if (ret == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Checks whether the given directory exists on the remote ftp server or not
        /// </summary>
        /// <param name="path">The path of the directory to check whether it exists or not.</param>
        /// <returns>True if the directory exists, false otherwise</returns>
        public bool DirectoryExists(string path)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new();
            IntPtr fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, IntPtr.Zero);
            return fileHandle != IntPtr.Zero || Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles;
        }

        /// <summary>
        /// The overrided Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Checks whether the given file exists on the remote ftp server or not
        /// </summary>
        /// <param name="path">The path of the file to check whether it exists or not.</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public bool FileExists(string path)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new();
            IntPtr fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, IntPtr.Zero);
            return fileHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Returns the directory currently set on the Ftp site in current session.
        /// </summary>
        /// <returns>The path of the current directory</returns>
        public string GetCurrentDirectory()
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            int buffLength = NativeMethods.MaxPath + 1;
            StringBuilder str = new(buffLength);
            if (NativeMethods.FtpGetCurrentDirectory(connectionHandle, str, ref buffLength) == 0)
            {
                Error();
                return null;
            }

            return str.ToString();
        }

        /// <summary>
        /// Returns Directory information of the currently selected directory.
        /// </summary>
        /// <returns>A FtpDirectoryInfo object containing information of the current directory.</returns>
        public FtpDirectoryInfo GetCurrentDirectoryInfo()
        {
            return new FtpDirectoryInfo(this, GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves the list of all directories in the ftp directory currently selected.
        /// </summary>
        /// <returns>Returns the list of diretories present in the current ftp directory.</returns>
        public IEnumerable<FtpDirectoryInfo> GetDirectories()
        {
            return GetDirectories(GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves the list of all directories in the given ftp directory
        /// </summary>
        /// <param name="path">The remote ftp directory path.</param>
        /// <returns>Returns the list of diretories present in the given ftp directory.</returns>
        public IEnumerable<FtpDirectoryInfo> GetDirectories(string path)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new();

            IntPtr fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, IntPtr.Zero);
            try
            {
                List<FtpDirectoryInfo> directories = [];

                if (fileHandle == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles)
                    {
                        return [.. directories];
                    }
                    else
                    {
                        Error();
                        return [.. directories];
                    }
                }

                if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) == NativeMethods.FileAttributeDirectory)
                {
                    FtpDirectoryInfo dir = new(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                    directories.Add(dir);
                }

                findData = new NativeMethods.WIN32_FIND_DATA();

                while (NativeMethods.InternetFindNextFile(fileHandle, ref findData) != 0)
                {
                    if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) == NativeMethods.FileAttributeDirectory)
                    {
                        FtpDirectoryInfo dir = new(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                        directories.Add(dir);
                    }

                    findData = new NativeMethods.WIN32_FIND_DATA();
                }

                if (Marshal.GetLastWin32Error() != NativeMethods.ErrorNoMoreFiles)
                {
                    Error();
                }

                return [.. directories];
            }
            finally
            {
                if (fileHandle != IntPtr.Zero)
                {
                    NativeMethods.InternetCloseHandle(fileHandle);
                }
            }
        }

        /// <summary>
        /// Download a file from the current remote directory ftp directory to the current local selected directory
        /// </summary>
        /// <param name="remoteFile">  The name of the file to be downloaded.</param>
        /// <param name="failIfExists">Flag to indicate whether to overwrite the file if it exists already in local directory.</param>
        public void GetFile(string remoteFile, bool failIfExists)
        {
            GetFile(remoteFile, remoteFile, failIfExists);
        }

        /// <summary>
        /// Download a file from the current remote directory ftp directory to the current local selected directory
        /// </summary>
        /// <param name="remoteFile">  The name of the file to be downloaded.</param>
        /// <param name="localFile">   The name of the file to be save locally as.</param>
        /// <param name="failIfExists">Flag to indicate whether to overwrite the file if it exists already in local directory.</param>
        public void GetFile(string remoteFile, string localFile, bool failIfExists)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpGetFile(connectionHandle, remoteFile, localFile, failIfExists, NativeMethods.FileAttributeNormal, NativeMethods.FtpTransferTypeBinary, IntPtr.Zero) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Retrieves the list of all files in the ftp directory currently selected
        /// </summary>
        /// <returns>Returns the list of files present in the current ftp directory.</returns>
        public IEnumerable<FtpFileInfo> GetFiles()
        {
            return connectionHandle == IntPtr.Zero
                ? throw new FtpException("The user is not connected to the FTP server. Please connect and try again.")
                : GetFiles(GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves the list of all files in the ftp directory currently selected whose name matches the fileName mask
        /// </summary>
        /// <param name="mask">The search criteria to return files.</param>
        /// <returns>Returns the list of files present in the current ftp directory.</returns>
        public IEnumerable<FtpFileInfo> GetFiles(string mask)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new();

            IntPtr fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, mask, ref findData, NativeMethods.InternetFlagNoCacheWrite, IntPtr.Zero);
            try
            {
                List<FtpFileInfo> files = [];
                if (fileHandle == IntPtr.Zero)
                {
                    if (Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles)
                    {
                        return [.. files];
                    }
                    else
                    {
                        Error();
                        return [.. files];
                    }
                }

                if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) != NativeMethods.FileAttributeDirectory)
                {
                    FtpFileInfo file = new(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                    files.Add(file);
                }

                findData = new NativeMethods.WIN32_FIND_DATA();
                while (NativeMethods.InternetFindNextFile(fileHandle, ref findData) != 0)
                {
                    if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) != NativeMethods.FileAttributeDirectory)
                    {
                        FtpFileInfo file = new(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                        files.Add(file);
                    }

                    findData = new NativeMethods.WIN32_FIND_DATA();
                }

                if (Marshal.GetLastWin32Error() != NativeMethods.ErrorNoMoreFiles)
                {
                    Error();
                }

                return [.. files];
            }
            finally
            {
                if (fileHandle != IntPtr.Zero)
                {
                    NativeMethods.InternetCloseHandle(fileHandle);
                }
            }
        }

        /// <summary>
        /// LogOn to the given FTP host using the given ftp UserName and Password
        /// </summary>
        public void LogOn()
        {
            LogOn(ftpUserName, ftpPassword);
        }

        /// <summary>
        /// Upload a file from the current local directory to the ftp directory currently selected
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded.</param>
        public void PutFile(string fileName)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            PutFile(fileName, Path.GetFileName(fileName));
        }

        /// <summary>
        /// Upload a file from the current local directory to the ftp directory currently selected
        /// </summary>
        /// <param name="localFile"> The name of the file to be uploaded.</param>
        /// <param name="remoteFile">The remote name of the file.</param>
        public void PutFile(string localFile, string remoteFile)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpPutFile(connectionHandle, localFile, remoteFile, NativeMethods.FtpTransferTypeBinary, IntPtr.Zero) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Rename a file on the remote FTP directory
        /// </summary>
        /// <param name="fileName">   The name of the file to be renamed.</param>
        /// <param name="newFileName">The name the file needs to be renamed to.</param>
        public void RenameFile(string fileName, string newFileName)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            int ret = NativeMethods.FtpRenameFile(connectionHandle, fileName, newFileName);
            if (ret == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Sends a command line command to the remote ftp server.
        /// </summary>
        /// <param name="cmd">The command to execute remotely on the remote ftp server.</param>
        /// <returns>Result from the command execution on remote server.</returns>
        public string SendCommand(string cmd)
        {
            IntPtr dataSocket = new();
            var result = cmd switch
            {
                "PASV" => NativeMethods.FtpCommand(connectionHandle, false, NativeMethods.FtpTransferTypeAscii, cmd, IntPtr.Zero, ref dataSocket),
                _ => NativeMethods.FtpCommand(connectionHandle, false, NativeMethods.FtpTransferTypeAscii, cmd, IntPtr.Zero, ref dataSocket),
            };
            const int BUFFER_SIZE = 8192;

            if (result == 0)
            {
                Error();
            }
            else if (dataSocket != IntPtr.Zero)
            {
                StringBuilder buffer = new(BUFFER_SIZE);
                int bytesRead = 0;

                do
                {
                    result = NativeMethods.InternetReadFile(dataSocket, buffer, BUFFER_SIZE, ref bytesRead);
                }
                while (result == 1 && bytesRead > 1);

                return buffer.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets the current directory on FTP site to the given directory path
        /// </summary>
        /// <param name="directory">The directory path to set on the FTP site.</param>
        public void SetCurrentDirectory(string directory)
        {
            if (connectionHandle == IntPtr.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpSetCurrentDirectory(connectionHandle, directory) == 0)
            {
                Error();
            }
        }
    }
}
