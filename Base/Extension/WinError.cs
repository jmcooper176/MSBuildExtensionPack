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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public enum WinError : uint
    {
        /// <summary>
        /// </summary>
        [Display(Name = "Access Denied Error", ShortName = "Access Denied")]
        [Description("")]
        ERROR_ACCESS_DENIED = 0x0000_0005,

        /// <summary>
        /// </summary>
        [Display(Name = "Arena Trashed Error", ShortName = "Arena Trashed")]
        [Description("")]
        ERROR_ARENA_TRASHED = 0x0000_0007,

        /// <summary>
        /// </summary>
        [Display(Name = "Arithmetic Overflow Error", ShortName = "Arithmetic Overflow")]
        [Description("")]
        ERROR_ARITHMETIC_OVERFLOW = 0x0000_0216,

        /// <summary>
        /// </summary>
        [Display(Name = "Assertion Failure Error", ShortName = "Assertion Failed")]
        [Description("")]
        ERROR_ASSERTION_FAILURE = 0x0000_029C,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Arguments Error", ShortName = "Invalid Arguments")]
        [Description("")]
        ERROR_BAD_ARGUMENTS = 0x0000_00A0,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Environment Error", ShortName = "Invalid Environment")]
        [Description("")]
        ERROR_BAD_ENVIRONMENT = 0x0000_000A,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Format Error", ShortName = "Bad Format")]
        [Description("")]
        ERROR_BAD_FORMAT = 0x0000_000B,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Path Name Error", ShortName = "Invalid Path Name")]
        [Description("")]
        ERROR_BAD_PATHNAME = 0x0000_00A1,

        /// <summary>
        /// </summary>
        [Display(Name = "Broken Pipe Error", ShortName = "Broken Pipe")]
        [Description("")]
        ERROR_BROKEN_PIPE = 0x0000_006D,

        /// <summary>
        /// </summary>
        [Display(Name = "Buffer Overflow Error", ShortName = "Buffer Overflow")]
        [Description("")]
        ERROR_BUFFER_OVERFLOW = 0x0000_006F,

        /// <summary>
        /// </summary>
        [Display(Name = "Call Not Implemented Error", ShortName = "Not Implemented")]
        [Description("")]
        ERROR_CALL_NOT_IMPLEMENTED = 0x0000_0078,

        /// <summary>
        /// </summary>
        [Display(Name = "Cancelled Error", ShortName = "Cancelled")]
        [Description("")]
        ERROR_CANCELLED = 0x0000_04C7,

        /// <summary>
        /// </summary>
        [Display(Name = "Connection Refused Error", ShortName = "Connection Refused")]
        [Description("")]
        ERROR_CONNECTION_REFUSED = 0x0000_04C9,

        /// <summary>
        /// </summary>
        [Display(Name = "Control-C Exit Error", ShortName = "Control-C Exit")]
        [Description("")]
        ERROR_CONTROL_C_EXIT = 0x0000_023C,

        /// <summary>
        /// </summary>
        [Display(Name = "Current Directory Error", ShortName = "Current Directory")]
        [Description("")]
        ERROR_CURRENT_DIRECTORY = 0x0000_0010,

        /// <summary>
        /// </summary>
        [Display(Name = "Directory Not Empty Error", ShortName = "Directory Not Empty")]
        [Description("")]
        ERROR_DIR_NOT_EMPTY = 0x0000_0091,

        /// <summary>
        /// </summary>
        [Display(Name = "Directory Error", ShortName = "Directory")]
        [Description("")]
        ERROR_DIRECTORY = 0x0000_010B,

        /// <summary>
        /// </summary>
        [Display(Name = "Fatal Application Exit Error", ShortName = "Application Exit")]
        [Description("")]
        ERROR_FATAL_APP_EXIT = 0x0000_02C9,

        /// <summary>
        /// </summary>
        [Display(Name = "File Exists Error", ShortName = "File Exists")]
        [Description("")]
        ERROR_FILE_EXISTS = 0x0000_0050,

        /// <summary>
        /// </summary>
        [Display(Name = "File Not Found Error", ShortName = "File Not Found")]
        [Description("")]
        ERROR_FILE_NOT_FOUND = 0x0000_0002,

        /// <summary>
        /// </summary>
        [Display(Name = "File Name Exceeded Range Error", ShortName = "File Name Exceeded Range")]
        [Description("")]
        ERROR_FILENAME_EXED_RANGE = 0x0000_00CE,

        /// <summary>
        /// </summary>
        [Display(Name = "Handle End Of File Error", ShortName = "End Of File")]
        [Description("")]
        ERROR_HANDLE_EOF = 0x0000_0026,

        /// <summary>
        /// </summary>
        [Display(Name = "Install Failure Error", ShortName = "Install Failed")]
        [Description("")]
        ERROR_INSTALL_FAILURE = 0x0000_0643,

        /// <summary>
        /// </summary>
        [Display(Name = "Install User Exit Error", ShortName = "User Exited Install")]
        [Description("")]
        ERROR_INSTALL_USEREXIT = 0x0000_0642,

        /// <summary>
        /// </summary>
        [Display(Name = "Internal Error", ShortName = "Internal")]
        [Description("")]
        ERROR_INTERNAL_ERROR = 0x0000_054F,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Access Error", ShortName = "Invalid Access")]
        [Description("")]
        ERROR_INVALID_ACCESS = 0x0000_000C,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Block Error", ShortName = "Invalid Block")]
        [Description("")]
        ERROR_INVALID_BLOCK = 0x0000_0009,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Data Error", ShortName = "Invalid Data")]
        [Description("")]
        ERROR_INVALID_DATA = 0x0000_000D,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Drive Error", ShortName = "Invalid Drive")]
        [Description("")]
        ERROR_INVALID_DRIVE = 0x0000_000F,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Function Error or False but Success", ShortName = "False OK")]
        [Description("")]
        ERROR_INVALID_FUNCTION = 0x0000_0001,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Handle Error", ShortName = "Invalid Handle")]
        [Description("")]
        ERROR_INVALID_HANDLE = 0x0000_0006,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Name Error", ShortName = "Invalid Name")]
        [Description("")]
        ERROR_INVALID_NAME = 0x0000_007B,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Parameter Error", ShortName = "Invalid Parameter")]
        [Description("")]
        ERROR_INVALID_PARAMETER = 0x0000_0057,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Password Error", ShortName = "Invalid Password")]
        [Description("")]
        ERROR_INVALID_PASSWORD = 0x0000_0056,

        /// <summary>
        /// </summary>
        [Display(Name = "Lock Violation Error", ShortName = "Lock Violation")]
        [Description("")]
        ERROR_LOCK_VIOLATION = 0x0000_0021,

        /// <summary>
        /// </summary>
        [Display(Name = "Logon Failure Error", ShortName = "Logon Failed")]
        [Description("")]
        ERROR_LOGON_FAILURE = 0x0000_052E,

        /// <summary>
        /// </summary>
        [Display(Name = "Network Access Denied Error", ShortName = "Network Access Denied")]
        [Description("")]
        ERROR_NETWORK_ACCESS_DENIED = 0x0000_0041,

        /// <summary>
        /// </summary>
        [Display(Name = "No Match Error", ShortName = "No Match")]
        [Description("")]
        ERROR_NO_MATCH = 0x0000_0491,

        /// <summary>
        /// </summary>
        [Display(Name = "No More Items Error", ShortName = "No More Items")]
        [Description("")]
        ERROR_NO_MORE_ITEMS = 0x0000_0103,

        /// <summary>
        /// </summary>
        [Display(Name = "No More Matches Error", ShortName = "No More Matches")]
        [Description("")]
        ERROR_NO_MORE_MATCHES = 0x0000_0272,

        /// <summary>
        /// </summary>
        [Display(Name = "Not Enough Memory Error", ShortName = "Not Enough Memory")]
        [Description("")]
        ERROR_NOT_ENOUGH_MEMORY = 0x0000_0008,

        /// <summary>
        /// </summary>
        [Display(Name = "Not Found Error", ShortName = "Not Found")]
        [Description("")]
        ERROR_NOT_FOUND = 0x0000_0490,

        /// <summary>
        /// </summary>
        [Display(Name = "Not Supported Error", ShortName = "Not Supported")]
        [Description("")]
        ERROR_NOT_SUPPORTED = 0x0000_0032,

        /// <summary>
        /// </summary>
        [Display(Name = "Open Failed Error", ShortName = "Open Failed")]
        [Description("")]
        ERROR_OPEN_FAILED = 0x0000_006E,

        /// <summary>
        /// </summary>
        [Display(Name = "Out of Memory Error", ShortName = "Out of Memory")]
        [Description("")]
        ERROR_OUTOFMEMORY = 0x0000_000E,

        /// <summary>
        /// </summary>
        [Display(Name = "Path Not Found Error", ShortName = "Path Not Found")]
        [Description("")]
        ERROR_PATH_NOT_FOUND = 0x0000_0003,

        /// <summary>
        /// </summary>
        [Display(Name = "Possible Deadlock Error", ShortName = "Possible Deadlock")]
        [Description("")]
        ERROR_POSSIBLE_DEADLOCK = 0x0000_046B,

        /// <summary>
        /// </summary>
        [Display(Name = "Read Fault Error", ShortName = "Read Fault")]
        [Description("")]
        ERROR_READ_FAULT = 0x0000_001E,

        /// <summary>
        /// </summary>
        [Display(Name = "Sharing Violation Error", ShortName = "Sharing Violation")]
        [Description("")]
        ERROR_SHARING_VIOLATION = 0x0000_0020,

        /// <summary>
        /// </summary>
        [Display(Name = "Stack Overflow Error", ShortName = "Stack Overflow")]
        [Description("")]
        ERROR_STACK_OVERFLOW = 0x0000_03E9,

        /// <summary>
        /// </summary>
        [Display(Name = "No Error", ShortName = "Success")]
        [Description("")]
        ERROR_SUCCESS = 0x0000_0000,

        /// <summary>
        /// </summary>
        [Display(Name = "System Shutdown Error", ShortName = "System Shutdown")]
        [Description("")]
        ERROR_SYSTEM_SHUTDOWN = 0x0000_0281,

        /// <summary>
        /// </summary>
        [Display(Name = "Timeout Error", ShortName = "Timeout")]
        [Description("")]
        ERROR_TIMEOUT = 0x0000_05B4,

        /// <summary>
        /// </summary>
        [Display(Name = "Too Many Open Files Error", ShortName = "Too Many Open Files")]
        [Description("")]
        ERROR_TOO_MANY_OPEN_FILES = 0x0000_0004,

        /// <summary>
        /// </summary>
        [Display(Name = "Unknown Error", ShortName = "Unknown")]
        [Description("")]
        ERROR_UNKNOWN_ERROR = 0x0000_FFFF,

        /// <summary>
        /// </summary>
        [Display(Name = "Write Fault Error", ShortName = "Write Fault")]
        [Description("")]
        ERROR_WRITE_FAULT = 0x0000_001D,
    }

    public static class WinErrorExtension
    {
        #region Public Methods

        public static int GetSeverity(this int statusCode)
        {
            return statusCode.GetFacilityCodeFromHResult();
        }

        /// <summary>
        /// </summary>
        /// <param name="hr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static WinError MakeWinError(this HResult hr)
        {
            const uint MAX_HR = 0x8081FFFF;

            ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)hr, MAX_HR, nameof(hr));

            return (WinError)Enum.ToObject(typeof(WinError), ((int)hr).ToWinErrorCode());
        }

        public static uint ToNtStatusCode(ulong severity, ulong facilityCode, WinError code)
        {
            return HResultExtension.ToHResultCode(severity, facilityCode, code);
        }

        public static uint ToNtStatusCode(this int hr)
        {
            return Convert.ToUInt32(hr & FacilityCode.FACILITY_NT_BIT);
        }

        public static uint ToWinErrorCode(this WinError code)
        {
            return Enum.IsDefined<WinError>(code) ? (uint)code : (uint)WinError.ERROR_UNKNOWN_ERROR;
        }

        public static uint ToWinErrorCode(this int hr)
        {
            return Convert.ToUInt32(hr & FacilityCode.FACILITY_MASK);
        }

        #endregion Public Methods
    }
}
