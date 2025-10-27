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
namespace MSBuild.ExtensionPack.Base.Cause
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

        protected FtpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

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

        public int ErrorCode { get; }

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
