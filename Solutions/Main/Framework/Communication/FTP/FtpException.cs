﻿//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpException.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Communication.FTP
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>FtpException</c> class encapsulates an FTP exception.
    /// </summary>
    [Serializable]
    public class FtpException : Exception
    {
        private readonly int ftpError;

        public FtpException()
        {
            ftpError = 0;            
        }

        public FtpException(string message) : this(-1, message)
        {
        }
        
        public FtpException(int error, string message) : base(message)
        {
            ftpError = error;
        }

        public FtpException(string message, Exception innerException) : base(message, innerException)
        {         
        }

        protected FtpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {         
        }

        public int ErrorCode => ftpError;

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