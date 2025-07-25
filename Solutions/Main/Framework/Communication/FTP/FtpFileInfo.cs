//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpFileInfo.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Communication.FTP
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>FtpFileInfo</c> class encapsulates a remote FTP directory.
    /// </summary>
    [Serializable]
    public sealed class FtpFileInfo : FileSystemInfo
    {
        private readonly string fileName; 
        private readonly FtpConnection ftpConnection;

        private DateTime? lastAccessTime;
        private DateTime? lastWriteTime;
        private DateTime? creationTime;

        public FtpFileInfo(FtpConnection ftp, string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            OriginalPath = filePath;
            FullPath = filePath;
            ftpConnection = ftp;
            fileName = Path.GetFileName(filePath);
        }

        private FtpFileInfo(SerializationInfo info, StreamingContext context) : base(info, context)
        {         
        }

        public FtpConnection FtpConnection => ftpConnection;

        public new DateTime? LastAccessTime
        {
            get { return lastAccessTime.HasValue ? lastAccessTime.Value : null; }
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

        public new FileAttributes Attributes { get; internal set; }

        public override string Name => fileName;

        public override bool Exists => FtpConnection.FileExists(FullName);

        public override void Delete()
        {
            FtpConnection.DeleteDirectory(FullName);
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