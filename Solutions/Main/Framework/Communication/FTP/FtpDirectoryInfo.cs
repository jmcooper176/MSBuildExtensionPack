﻿//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpDirectoryInfo.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Communication.FTP
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>FtpDirectoryInfo</c> class encapsulates a remote FTP directory.
    /// </summary>
    [Serializable]    
    public class FtpDirectoryInfo : FileSystemInfo
    {
        private DateTime? creationTime;
        private DateTime? lastAccessTime;
        private DateTime? lastWriteTime;
        
        public FtpDirectoryInfo(FtpConnection ftp, string path)
        {
            FtpConnection = ftp;
            FullPath = path;
        }
        
        protected FtpDirectoryInfo(SerializationInfo info, StreamingContext context) : base(info, context)
        {         
        }

        public FtpConnection FtpConnection { get; internal set; }

        public new DateTime? LastAccessTime
        {
            get => lastAccessTime.HasValue ? lastAccessTime.Value : null;
            internal set => lastAccessTime = value;
        }

        public new DateTime? CreationTime
        {
            get => creationTime.HasValue ? creationTime.Value : null;
            internal set => creationTime = value;
        }

        public new DateTime? LastWriteTime
        {
            get => lastWriteTime.HasValue ? lastWriteTime.Value : null;
            internal set => lastWriteTime = value;
        }

        public new DateTime? LastAccessTimeUtc => lastAccessTime?.ToUniversalTime();

        public new DateTime? CreationTimeUtc => creationTime?.ToUniversalTime();

        public new DateTime? LastWriteTimeUtc => lastWriteTime?.ToUniversalTime();

        public new FileAttributes Attributes { get; set; }

        public override bool Exists => FtpConnection.DirectoryExists(FullName);

        public override string Name => Path.GetFileName(FullPath);

        public override void Delete()
        {
            FtpConnection.DeleteDirectory(Name);
        }

        public FtpDirectoryInfo[] GetDirectories()
        {
            return FtpConnection.GetDirectories(FullPath);
        }

        public FtpDirectoryInfo[] GetDirectories(string path)
        {
            path = Path.Combine(FullPath, path);
            return FtpConnection.GetDirectories(path);
        }

        public FtpFileInfo[] GetFiles()
        {
            return GetFiles(FtpConnection.GetCurrentDirectory());
        }

        public FtpFileInfo[] GetFiles(string mask)
        {
            return FtpConnection.GetFiles(mask);
        }

        /// <summary>
        /// No specific impelementation is needed of the GetObjectData to serialize this object
        /// because all attributes are redefined.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data. </param>
        /// <param name="context">The destination for this serialization. </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {   
            base.GetObjectData(info, context);
        }
    }
}