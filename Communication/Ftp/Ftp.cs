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
namespace Communication.Ftp
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Framework;

    using MSBuild.ExtensionPack.Base;
    using MSBuild.ExtensionPack.Base.Cause;
    using MSBuild.ExtensionPack.ErrorMessage.Message;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>UploadFiles</i> ( <b>Required:</b> Host, FileNames <b>Optional:</b> UserName, UserPassword, WorkingDirectory,
    /// RemoteDirectoryName, Port)
    /// </para>
    /// <para>
    /// <i>DownloadFiles</i> ( <b>Required:</b> Host <b>Optional:</b> FileNames, UserName, UserPassword, WorkingDirectory,
    /// RemoteDirectoryName, Port)
    /// </para>
    /// <para>
    /// <i>DeleteFiles</i> ( <b>Required:</b> Host, FileNames <b>Optional:</b> UserName, UserPassword, WorkingDirectory,
    /// RemoteDirectoryName, Port)
    /// </para>
    /// <para>
    /// <i>DeleteDirectory</i> ( <b>Required:</b> Host <b>Optional:</b> UserName, UserPassword, WorkingDirectory,
    /// RemoteDirectoryName, Port)
    /// </para>
    /// <para>
    /// <i>CreateDirectory</i> ( <b>Required:</b> Host <b>Optional:</b> UserName, UserPassword, WorkingDirectory,
    /// RemoteDirectoryName, Port)
    /// </para>
    /// <para><b>Remote Execution Support:</b> NA</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" DefaultTargets="Default" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///<ftpHost>localhost</ftpHost>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<Target Name="Default">
    ///<ItemGroup>
    ///<!-- Specify FilesToUpload -->
    ///<FilesToUpload Include="C:\demo.txt" />
    ///<FilesToUpload Include="C:\demo2.txt" />
    ///</ItemGroup>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="UploadFiles" Host="$(ftpHost)" FileNames="@(FilesToUpload)"/>
    ///<ItemGroup>
    ///<!-- Specify the files to Download-->
    ///<FilesToDownload Include="demo2.txt" />
    ///<FilesToDownload Include="demo.txt" />
    ///</ItemGroup>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="DownloadFiles" Host="$(ftpHost)" FileNames="@(FilesToDownload)" WorkingDirectory="C:\FtpWorkingFolder"/>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="CreateDirectory" Host="$(ftpHost)" RemoteDirectoryName="NewFolder1"/>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="CreateDirectory" Host="$(ftpHost)" RemoteDirectoryName="NewFolder2"/>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="DeleteDirectory" Host="$(ftpHost)" RemoteDirectoryName="NewFolder1"/>
    ///<MSBuild.ExtensionPack.Communication.Ftp TaskAction="DeleteFiles" Host="$(ftpHost)" FileNames="@(FilesToDownload)" />
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class Ftp : BaseTask
    {
        private const string CreateDirectoryTaskAction = "CreateDirectory";
        private const string DeleteDirectoryTaskAction = "DeleteDirectory";
        private const string DeleteFilesTaskAction = "DeleteFiles";
        private const string DownloadFilesTaskAction = "DownloadFiles";
        private const string UploadFilesTaskAction = "UploadFiles";

        /// <summary>
        /// Creates a new Ftp directory on the ftp server.
        /// </summary>
        private void CreateDirectory()
        {
            if (string.IsNullOrEmpty(RemoteDirectoryName))
            {
                this.Log.LogError("The required RemoteDirectoryName attribute has not been set for FTP.");
                return;
            }

            using FtpConnection ftpConnection = CreateFtpConnection();
            ftpConnection.LogOn();
            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Creating Directory: {0}", RemoteDirectoryName));
            try
            {
                ftpConnection.CreateDirectory(RemoteDirectoryName);
            }
            catch (FtpException ex)
            {
                if (ex.Message.Contains("550"))
                {
                    return;
                }

                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "There was an error creating ftp directory: {0}. The Error Details are \"{1}\" and error code is {2} ", RemoteDirectoryName, ex.Message, ex.ErrorCode));
            }
        }

        /// <summary>
        /// Creates an FTP Connection object
        /// </summary>
        /// <returns>An initialised FTP Connection</returns>
        private FtpConnection CreateFtpConnection()
        {
            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Connecting to FTP Host: {0}", Host));

            if (!string.IsNullOrEmpty(UserName))
            {
                return Port != 0 ? new FtpConnection(Host, Port, UserName, UserPassword) : new FtpConnection(Host, UserName, UserPassword);
            }

            return Port != 0 ? new FtpConnection(Host, Port) : new FtpConnection(Host);
        }

        /// <summary>
        /// Deletes an Ftp directory on the ftp server.
        /// </summary>
        private void DeleteDirectory()
        {
            if (string.IsNullOrEmpty(RemoteDirectoryName))
            {
                this.Log.LogTaskError("The required RemoteDirectoryName attribute has not been set for FTP.");
                return;
            }

            using FtpConnection ftpConnection = CreateFtpConnection();
            ftpConnection.LogOn();
            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Deleting Directory: {0}", RemoteDirectoryName));
            try
            {
                ftpConnection.DeleteDirectory(RemoteDirectoryName);
            }
            catch (FtpException ex)
            {
                if (ex.Message.Contains("550"))
                {
                    return;
                }

                this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "There was an error deleting ftp directory: {0}. The Error Details are \"{1}\" and error code is {2} ", RemoteDirectoryName, ex.Message, ex.ErrorCode));
            }
        }

        /// <summary>
        /// Delete given files from the FTP Directory
        /// </summary>
        private void DeleteFiles()
        {
            if (FileNames is null)
            {
                this.Log.LogTaskError("The required FileNames attribute has not been set for FTP.");
                return;
            }

            using FtpConnection ftpConnection = CreateFtpConnection();
            ftpConnection.LogOn();
            this.Log.LogTaskMessage("Deleting Files");
            if (!string.IsNullOrEmpty(RemoteDirectoryName))
            {
                this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Setting Current Directory: {0}", RemoteDirectoryName));
                ftpConnection.SetCurrentDirectory(RemoteDirectoryName);
            }

            foreach (string fileName in FileNames.Select(item => item.ItemSpec))
            {
                try
                {
                    this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Deleting: {0}", fileName));
                    ftpConnection.DeleteFile(fileName);
                }
                catch (FtpException ex)
                {
                    if (ex.Message.Contains("550"))
                    {
                        continue;
                    }

                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "There was an error in deleting file: {0}. The Error Details are \"{1}\" and error code is {2} ", fileName, ex.Message, ex.ErrorCode));
                }
            }
        }

        /// <summary>
        /// Download Files
        /// </summary>
        private void DownloadFiles()
        {
            using FtpConnection ftpConnection = CreateFtpConnection();
            if (!string.IsNullOrEmpty(WorkingDirectory))
            {
                if (!Directory.Exists(WorkingDirectory))
                {
                    Directory.CreateDirectory(WorkingDirectory);
                }

                this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Setting Local Directory: {0}", WorkingDirectory));

                FtpConnection.SetLocalDirectory(WorkingDirectory);
            }

            ftpConnection.LogOn();

            if (!string.IsNullOrEmpty(RemoteDirectoryName))
            {
                ftpConnection.SetCurrentDirectory(RemoteDirectoryName);
            }

            this.Log.LogTaskMessage("Downloading Files");
            if (FileNames is null)
            {
                FtpFileInfo[] filesToDownload = ftpConnection.GetFiles();
                foreach (FtpFileInfo fileToDownload in filesToDownload)
                {
                    this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Downloading: {0}", fileToDownload));
                    ftpConnection.GetFile(fileToDownload.Name, false);
                }
            }
            else
            {
                foreach (string fileName in FileNames.Select(item => item.ItemSpec.Trim()))
                {
                    try
                    {
                        this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Downloading: {0}", fileName));
                        ftpConnection.GetFile(fileName, false);
                    }
                    catch (FtpException ex)
                    {
                        Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "There was an error downloading file: {0}. The Error Details are \"{1}\" and error code is {2} ", fileName, ex.Message, ex.ErrorCode));
                    }
                }
            }
        }

        /// <summary>
        /// Upload Files
        /// </summary>
        private void UploadFiles()
        {
            if (FileNames is null)
            {
                this.Log.LogTaskError("The required fileNames attribute has not been set for FTP.");
                return;
            }

            using FtpConnection ftpConnection = CreateFtpConnection();
            this.Log.LogTaskMessage("Uploading Files");
            if (!string.IsNullOrEmpty(WorkingDirectory))
            {
                this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Setting Local Directory: {0}", WorkingDirectory));
                FtpConnection.SetLocalDirectory(WorkingDirectory);
            }

            ftpConnection.LogOn();

            if (!string.IsNullOrEmpty(RemoteDirectoryName))
            {
                this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Setting Current Directory: {0}", RemoteDirectoryName));
                ftpConnection.SetCurrentDirectory(RemoteDirectoryName);
            }

            var overwrite = true;
            var files = new List<FtpFileInfo>();
            if (!string.IsNullOrEmpty(Overwrite))
            {
                if (!bool.TryParse(Overwrite, out overwrite))
                {
                    overwrite = true;
                }
            }

            if (!overwrite)
            {
                files.AddRange(ftpConnection.GetFiles());
            }

            foreach (string fileName in FileNames.Select(item => item.ItemSpec))
            {
                try
                {
                    if (File.Exists(fileName))
                    {
                        if (!overwrite && files.Any(fi => fi.Name == fileName))
                        {
                            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Skipped: {0}", fileName));
                            continue;
                        }

                        this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Uploading: {0}", fileName));
                        ftpConnection.PutFile(fileName);
                    }
                }
                catch (FtpException ex)
                {
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "There was an error uploading file: {0}. The Error Details are \"{1}\" and error code is {2} ", fileName, ex.Message, ex.ErrorCode));
                }
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected void InternalExecute()
        {
            if (string.IsNullOrEmpty(Host))
            {
                this.Log.LogTaskError("The required host attribute has not been set for FTP.");
                return;
            }

            switch (TaskAction)
            {
                case CreateDirectoryTaskAction:
                    CreateDirectory();
                    break;

                case DeleteDirectoryTaskAction:
                    DeleteDirectory();
                    break;

                case DeleteFilesTaskAction:
                    DeleteFiles();
                    break;

                case DownloadFilesTaskAction:
                    DownloadFiles();
                    break;

                case UploadFilesTaskAction:
                    UploadFiles();
                    break;

                default:
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "Invalid Task Action passed: {0}", TaskAction));
                    return;
            }
        }

        /// <summary>
        /// The list of files that needs to be transfered over FTP
        /// </summary>
        public IEnumerable<ITaskItem> FileNames { get; set; }

        /// <summary>
        /// Sets the Host of the FTP Site.
        /// </summary>
        [Required]
        public string Host { get; set; }

        /// <summary>
        /// Sets if the upload action will overwrite existing files
        /// </summary>
        public string Overwrite { get; set; }

        /// <summary>
        /// The port used to connect to the ftp server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Sets the Remote Path to connect to the FTP Site
        /// </summary>
        public string RemoteDirectoryName { get; set; }

        /// <summary>
        /// Sets the working directory on the local machine
        /// </summary>
        public string WorkingDirectory { get; set; }
    }
}
