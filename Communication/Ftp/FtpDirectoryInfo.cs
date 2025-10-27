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
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>FtpDirectoryInfo</c> class encapsulates a remote FTP directory.
    /// </summary>
    /// <seealso cref="FileSystemInfo"/>
    [Serializable]
    public class FtpDirectoryInfo : FileSystemInfo
    {
        private DateTime? creationTime;
        private DateTime? lastAccessTime;
        private DateTime? lastWriteTime;

        protected FtpDirectoryInfo(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FtpDirectoryInfo(FtpConnection ftp, string path)
        {
            FtpConnection = ftp;
            FullPath = path;
        }

        public new FileAttributes Attributes { get; set; }

        public new DateTime? CreationTime
        {
            get => creationTime ?? null;
            internal set => creationTime = value;
        }

        public new DateTime? CreationTimeUtc => creationTime?.ToUniversalTime();
        public override bool Exists => FtpConnection.DirectoryExists(FullName);
        public FtpConnection FtpConnection { get; internal set; }

        public new DateTime? LastAccessTime
        {
            get => lastAccessTime ?? null;
            internal set => lastAccessTime = value;
        }

        public new DateTime? LastAccessTimeUtc => lastAccessTime?.ToUniversalTime();

        public new DateTime? LastWriteTime
        {
            get => lastWriteTime ?? null;
            internal set => lastWriteTime = value;
        }

        public new DateTime? LastWriteTimeUtc => lastWriteTime?.ToUniversalTime();
        public override string Name => Path.GetFileName(FullPath);

        public override void Delete()
        {
            FtpConnection.DeleteDirectory(Name);
        }

        public IEnumerable<FtpDirectoryInfo> GetDirectories()
        {
            return FtpConnection.GetDirectories(FullPath);
        }

        public IEnumerable<FtpDirectoryInfo> GetDirectories(string path)
        {
            path = Path.Combine(FullPath, path);
            return FtpConnection.GetDirectories(path);
        }

        public IEnumerable<FtpFileInfo> GetFiles()
        {
            return GetFiles(FtpConnection.GetCurrentDirectory());
        }

        public IEnumerable<FtpFileInfo> GetFiles(string mask)
        {
            return FtpConnection.GetFiles(mask);
        }

        /// <summary>
        /// No specific impelementation is needed of the GetObjectData to serialize this object because all attributes are redefined.
        /// </summary>
        /// <param name="info">   The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
