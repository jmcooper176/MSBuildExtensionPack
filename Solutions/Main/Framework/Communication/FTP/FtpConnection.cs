//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpConnection.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
using MSBuild.ExtensionPack.Communication.Extended;

namespace MSBuild.ExtensionPack.Communication.FTP
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
    public class FtpConnection : IDisposable
    {   
        private readonly string ftpHost;        
        private readonly int ftpPort;
        private readonly string ftpUserName;
        private readonly string ftpPassword;

        private nint connectionHandle;
        private nint internetHandle;

        #region Constructors
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
        /// <param name="host">The host of the ftp site where the connection would be made.</param>
        /// <param name="userName">The userName used to make the FTP connection</param>
        /// <param name="password">The password for the user connecting to the ftp site</param>
        public FtpConnection(string host, string userName, string password) : this(host, NativeMethods.InternetDefaultFtpPort, userName, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FtpConnection class.
        /// </summary>
        /// <param name="host">The host of the ftp site where the connection would be made.</param>
        /// <param name="port">The port to make the connection on</param>
        /// <param name="userName">The userName used to make the FTP connection</param>
        /// <param name="password">The Password used to make the FTP connection</param>
        public FtpConnection(string host, int port, string userName, string password)
        {
            ftpHost = host;
            ftpPort = port;
            ftpUserName = userName;
            ftpPassword = password;
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Finalizes an instance of the FtpConnection class. 
        /// Disposable types with unmanaged resources need to implement a finalizer.
        /// </summary>
        ~FtpConnection()
        {
            Dispose(false);
        }                                                                                                    
        #endregion

        #region Properties
        public int Port => ftpPort;

        public string FtpHost => ftpHost;

        #endregion

        /// <summary>
        /// Sets the directory on the local machine used to upload / download files.
        /// </summary>
        /// <param name="directory">The directory file path.</param>
        public static void SetLocalDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Environment.CurrentDirectory = directory;
            }
            else
            {
                throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "{0} is not a directory!", directory));
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
        /// Sets the current directory on FTP site to the given directory path
        /// </summary>
        /// <param name="directory">The directory path to set on the FTP site.</param>
        public void SetCurrentDirectory(string directory)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }
                
            if (NativeMethods.FtpSetCurrentDirectory(connectionHandle, directory) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Returns the directory currently set on the Ftp site in current session.
        /// </summary>
        /// <returns>The path of the current directory</returns>
        public string GetCurrentDirectory()
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            int buffLength = NativeMethods.MaxPath + 1;
            StringBuilder str = new StringBuilder(buffLength);
            if (NativeMethods.FtpGetCurrentDirectory(connectionHandle, str, ref buffLength) == 0)
            {
                Error();
                return null;
            }

            return str.ToString();
        }

        /// <summary>
        /// Returns Directory information of the the currently selected directory.
        /// </summary>
        /// <returns>A FtpDirectoryInfo object containing information of the current directory.</returns>
        public FtpDirectoryInfo GetCurrentDirectoryInfo()
        {
            return new FtpDirectoryInfo(this, GetCurrentDirectory());
        }

        /// <summary>
        /// Download a file from the current remote directory ftp directory to the current local selected directory
        /// </summary>
        /// <param name="remoteFile">The name of the file to be downloaded.</param>
        /// <param name="failIfExists">Flag to indicate whether to overwrite the file if it exists already in local directory.</param>
        public void GetFile(string remoteFile, bool failIfExists)
        {
            GetFile(remoteFile, remoteFile, failIfExists);
        }

        /// <summary>
        /// Download a file from the current remote directory ftp directory to the current local selected directory
        /// </summary>
        /// <param name="remoteFile">The name of the file to be downloaded.</param>
        /// <param name="localFile">The name of the file to be save locally as.</param>
        /// <param name="failIfExists">Flag to indicate whether to overwrite the file if it exists already in local directory.</param>
        public void GetFile(string remoteFile, string localFile, bool failIfExists)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpGetFile(connectionHandle, remoteFile, localFile, failIfExists, NativeMethods.FileAttributeNormal, NativeMethods.FtpTransferTypeBinary, nint.Zero) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Upload a file from the current local directory to the ftp directory currently selected
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded.</param>
        public void PutFile(string fileName)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            PutFile(fileName, Path.GetFileName(fileName));
        }

        /// <summary>
        /// Upload a file from the current local directory to the ftp directory currently selected
        /// </summary>
        /// <param name="localFile">The name of the file to be uploaded.</param>
        /// <param name="remoteFile">The remote name of the file.</param>
        public void PutFile(string localFile, string remoteFile)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpPutFile(connectionHandle, localFile, remoteFile, NativeMethods.FtpTransferTypeBinary, nint.Zero) == 0)
            {
                Error();
            }
        }

        /// <summary>
        /// Rename a file on the remote FTP directory
        /// </summary>
        /// <param name="fileName">The name of the file to be renamed.</param>
        /// <param name="newFileName">The name the file needs to be renamed to.</param>
        public void RenameFile(string fileName, string newFileName)
        {
            if (connectionHandle == nint.Zero)
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
        /// Deletes a file in the Ftp remote directory
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>        
        public void DeleteFile(string fileName)
        {
            if (connectionHandle == nint.Zero)
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
        /// Deletes a file in the Ftp remote directory
        /// </summary>
        /// <param name="directory">The name of the file to be deleted.</param>        
        public void DeleteDirectory(string directory)
        {
            if (connectionHandle == nint.Zero)
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
        /// Retrieves the list of all files in the ftp directory currently selected
        /// </summary> 
        /// <returns>Returns the list of files present in the current ftp directory.</returns>
        public FtpFileInfo[] GetFiles()
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            return GetFiles(GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves the list of all files in the ftp directory currently selected whose name matches the fileName mask
        /// </summary>
        /// <param name="mask">The search criteria to return files.</param>        
        /// <returns>Returns the list of files present in the current ftp directory.</returns>        
        public FtpFileInfo[] GetFiles(string mask)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();

            nint fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, mask, ref findData, NativeMethods.InternetFlagNoCacheWrite, nint.Zero);
            try
            {
                List<FtpFileInfo> files = new List<FtpFileInfo>();
                if (fileHandle == nint.Zero)
                {
                    if (Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles)
                    {
                        return files.ToArray();
                    }
                    else
                    {
                        Error();
                        return files.ToArray();
                    }
                }

                if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) != NativeMethods.FileAttributeDirectory)
                {
                    FtpFileInfo file = new FtpFileInfo(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                    files.Add(file);
                }

                findData = new NativeMethods.WIN32_FIND_DATA();
                while (NativeMethods.InternetFindNextFile(fileHandle, ref findData) != 0)
                {
                    if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) != NativeMethods.FileAttributeDirectory)
                    {
                        FtpFileInfo file = new FtpFileInfo(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                        files.Add(file);
                    }

                    findData = new NativeMethods.WIN32_FIND_DATA();
                }

                if (Marshal.GetLastWin32Error() != NativeMethods.ErrorNoMoreFiles)
                {
                    Error();
                }

                return files.ToArray();
            }
            finally
            {
                if (fileHandle != nint.Zero)
                {
                    NativeMethods.InternetCloseHandle(fileHandle);
                }
            }
        }

        /// <summary>
        /// Retrieves the list of all directories in the ftp directory currently selected.
        /// </summary>
        /// <returns>Returns the list of diretories present in the current ftp directory.</returns>
        public FtpDirectoryInfo[] GetDirectories()
        {
            return GetDirectories(GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves the list of all directories in the given ftp directory 
        /// </summary>
        /// <param name="path">The remote ftp directory path.</param>        
        /// <returns>Returns the list of diretories present in the given ftp directory.</returns>
        public FtpDirectoryInfo[] GetDirectories(string path)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();

            nint fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, nint.Zero);
            try
            {
                List<FtpDirectoryInfo> directories = new List<FtpDirectoryInfo>();

                if (fileHandle == nint.Zero)
                {
                    if (Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles)
                    {
                        return directories.ToArray();
                    }
                    else
                    {
                        Error();
                        return directories.ToArray();
                    }
                }

                if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) == NativeMethods.FileAttributeDirectory)
                {
                    FtpDirectoryInfo dir = new FtpDirectoryInfo(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                    directories.Add(dir);
                }

                findData = new NativeMethods.WIN32_FIND_DATA();

                while (NativeMethods.InternetFindNextFile(fileHandle, ref findData) != 0)
                {
                    if ((findData.dfFileAttributes & NativeMethods.FileAttributeDirectory) == NativeMethods.FileAttributeDirectory)
                    {
                        FtpDirectoryInfo dir = new FtpDirectoryInfo(this, new string(findData.fileName).TrimEnd('\0')) { LastAccessTime = findData.ftLastAccessTime.ToDateTime(), LastWriteTime = findData.ftLastWriteTime.ToDateTime(), CreationTime = findData.ftCreationTime.ToDateTime(), Attributes = (FileAttributes)findData.dfFileAttributes };
                        directories.Add(dir);
                    }

                    findData = new NativeMethods.WIN32_FIND_DATA();
                }

                if (Marshal.GetLastWin32Error() != NativeMethods.ErrorNoMoreFiles)
                {
                    Error();
                }

                return directories.ToArray();
            }
            finally
            {
                if (fileHandle != nint.Zero)
                {
                    NativeMethods.InternetCloseHandle(fileHandle);
                }
            }
        }

        /// <summary>
        /// Creates a directory in the remote ftp directory
        /// </summary>
        /// <param name="path">The path of the ftp directory.</param>        
        public void CreateDirectory(string path)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            if (NativeMethods.FtpCreateDirectory(connectionHandle, path) == 0)
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
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();
            nint fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, nint.Zero);
            return fileHandle != nint.Zero || Marshal.GetLastWin32Error() == NativeMethods.ErrorNoMoreFiles;            
        }

        /// <summary>
        /// Checks whether the given file exists on the remote ftp server or not
        /// </summary>
        /// <param name="path">The path of the file to check whether it exists or not.</param>
        /// <returns>True if the file exists, false otherwise</returns>
        public bool FileExists(string path)
        {
            if (connectionHandle == nint.Zero)
            {
                throw new FtpException("The user is not connected to the FTP server. Please connect and try again.");
            }

            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();
            nint fileHandle = NativeMethods.FtpFindFirstFile(connectionHandle, path, ref findData, NativeMethods.InternetFlagNoCacheWrite, nint.Zero);
            return fileHandle != nint.Zero;            
        }

        /// <summary>
        /// Sends a command line command to the remote ftp server.
        /// </summary>
        /// <param name="cmd">The command to execute remotely on the remote ftp server.</param>
        /// <returns>Result from the command execution on remote server.</returns>
        public string SendCommand(string cmd)
        {
            int result;
            nint dataSocket = new nint();
            switch (cmd)
            {
                case "PASV":
                    result = NativeMethods.FtpCommand(connectionHandle, false, NativeMethods.FtpTransferTypeAscii, cmd, nint.Zero, ref dataSocket);
                    break;
                default:
                    result = NativeMethods.FtpCommand(connectionHandle, false, NativeMethods.FtpTransferTypeAscii, cmd, nint.Zero, ref dataSocket);
                    break;
            }

            const int BUFFER_SIZE = 8192;

            if (result == 0)
            {
                Error();
            }
            else if (dataSocket != nint.Zero)
            {
                StringBuilder buffer = new StringBuilder(BUFFER_SIZE);
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
        /// Close connection to FTP server and all relevant sessions.
        /// </summary>
        public void Close()
        {
            NativeMethods.InternetCloseHandle(connectionHandle);
            connectionHandle = nint.Zero;

            NativeMethods.InternetCloseHandle(internetHandle);
            internetHandle = nint.Zero;
        }
        
        #region IDisposable
        /// <summary>
        /// The overrided Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resource
            }

            if (connectionHandle != nint.Zero)
            {
                NativeMethods.InternetCloseHandle(connectionHandle);
            }

            if (internetHandle != nint.Zero)
            {
                NativeMethods.InternetCloseHandle(internetHandle);
            }
        }
        #endregion

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
            if (internetHandle == nint.Zero)
            {
                Open();
            }

            // Connect to the Internet using Ftp Credentials
            connectionHandle = NativeMethods.InternetConnect(internetHandle, ftpHost, ftpPort, userName, password, NativeMethods.InternetServiceFtp, NativeMethods.InternetFlagPassive, nint.Zero);
            if (connectionHandle == nint.Zero)
            {
                Error();
            }
        }

        /// <summary>
        /// Returns full description of the 
        /// </summary>
        /// <param name="code">The error code whose details would be returned</param>
        /// <returns>The description of the error code passed</returns>
        private static string InternetLastResponseInfo(ref int code)
        {
            int buffersize = 8192;
            StringBuilder buff = new StringBuilder(buffersize);
            NativeMethods.InternetGetLastResponseInfo(ref code, buff, ref buffersize);
            return buff.ToString();
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
        /// The method is opens a connection to the Internet and should be invoked before 
        /// any attempt to connect to the FTP Site.
        /// </summary>
        private void Open()
        {
            if (string.IsNullOrEmpty(ftpHost))
            {
                throw new ArgumentNullException(ftpHost);
            }

            internetHandle = NativeMethods.InternetOpen(Environment.UserName, NativeMethods.InternetOpenTypePreconfig, null, null, NativeMethods.InternetFlagSync);
            if (internetHandle == nint.Zero)
            {
                Error();
            }
        }
    }
}