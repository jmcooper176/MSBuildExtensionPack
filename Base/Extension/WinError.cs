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
        /// The operation completed successfully.
        /// </summary>
        [Display(Name = "No Error", ShortName = "Success")]
        [Description("")]
        ERROR_SUCCESS = 0x0000_0000,

        /// <summary>
        /// Incorrect function.
        /// </summary>
        [Display(Name = "Invalid Function Error or False but Success", ShortName = "False OK")]
        [Description("")]
        ERROR_INVALID_FUNCTION = 0x0000_0001,

        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        [Display(Name = "File Not Found Error", ShortName = "File Not Found")]
        [Description("")]
        ERROR_FILE_NOT_FOUND = 0x0000_0002,

        /// <summary>
        /// The system cannot find the path specified.
        /// </summary>
        [Display(Name = "Path Not Found Error", ShortName = "Path Not Found")]
        [Description("")]
        ERROR_PATH_NOT_FOUND = 0x0000_0003,

        /// <summary>
        /// The system cannot open the file.
        /// </summary>
        [Display(Name = "Too Many Open Files Error", ShortName = "Too Many Open Files")]
        [Description("")]
        ERROR_TOO_MANY_OPEN_FILES = 0x0000_0004,

        /// <summary>
        /// Access is denied.
        /// </summary>
        [Display(Name = "Access Denied Error", ShortName = "Access Denied")]
        [Description("")]
        ERROR_ACCESS_DENIED = 0x0000_0005,

        /// <summary>
        /// The handle is invalid.
        /// </summary>
        [Display(Name = "Invalid Handle Error", ShortName = "Invalid Handle")]
        [Description("")]
        ERROR_INVALID_HANDLE = 0x0000_0006,

        /// <summary>
        /// The storage control blocks were destroyed.
        /// </summary>
        [Display(Name = "Arena Trashed Error", ShortName = "Arena Trashed")]
        [Description("")]
        ERROR_ARENA_TRASHED = 0x0000_0007,

        /// <summary>
        /// Not enough memory resources are available to process this command.
        /// </summary>
        [Display(Name = "Not Enough Memory Error", ShortName = "Not Enough Memory")]
        [Description("")]
        ERROR_NOT_ENOUGH_MEMORY = 0x0000_0008,

        /// <summary>
        /// The storage control block address is invalid.
        /// </summary>
        [Display(Name = "Invalid Block Error", ShortName = "Invalid Block")]
        [Description("")]
        ERROR_INVALID_BLOCK = 0x0000_0009,

        /// <summary>
        /// The environment is incorrect.
        /// </summary>
        [Display(Name = "Bad Environment Error", ShortName = "Invalid Environment")]
        [Description("")]
        ERROR_BAD_ENVIRONMENT = 0x0000_000A,

        /// <summary>
        /// An attempt was made to load a program with an incorrect.
        /// </summary>
        [Display(Name = "Bad Format Error", ShortName = "Bad Format")]
        [Description("")]
        ERROR_BAD_FORMAT = 0x0000_000B,

        /// <summary>
        /// The data is invalid.
        /// </summary>
        [Display(Name = "Invalid Access Error", ShortName = "Invalid Access")]
        [Description("")]
        ERROR_INVALID_ACCESS = 0x0000_000C,

        /// <summary>
        /// </summary>
        [Display(Name = "Invalid Data Error", ShortName = "Invalid Data")]
        [Description("")]
        ERROR_INVALID_DATA = 0x0000_000D,

        /// <summary>
        /// Not enough storage is available to complete this operation.
        /// </summary>
        [Display(Name = "Out of Memory Error", ShortName = "Out of Memory")]
        [Description("")]
        ERROR_OUTOFMEMORY = 0x0000_000E,

        /// <summary>
        /// The system cannot find the drive specified.
        /// </summary>
        [Display(Name = "Invalid Drive Error", ShortName = "Invalid Drive")]
        [Description("")]
        ERROR_INVALID_DRIVE = 0x0000_000F,

        /// <summary>
        /// The directory cannot be removed.
        /// </summary>
        [Display(Name = "Current Directory Error", ShortName = "Current Directory")]
        [Description("")]
        ERROR_CURRENT_DIRECTORY = 0x0000_0010,

        /// <summary>
        /// The system cannot move the file to a different disk drive.
        /// </summary>
        [Display(Name = "Not Same Device Error", ShortName = "Not Same Device")]
        [Description("")]
        ERROR_NOT_SAME_DEVICE = 0x0000_0011,

        /// <summary>
        /// There are no more files.
        /// </summary>
        [Display(Name = "No More Files Error", ShortName = "No More Files")]
        [Description("")]
        ERROR_NO_MORE_FILES = 0x0000_0012,

        /// <summary>
        /// The media is write protected.
        /// </summary>
        [Display(Name = "Write Protect Error", ShortName = "Write Protect")]
        [Description("")]
        ERROR_WRITE_PROTECT = 0x0000_0013,

        /// <summary>
        /// The system cannot find the device specified.
        /// </summary>
        [Display(Name = "Bad Unit Error", ShortName = "Bad Unit")]
        [Description("")]
        ERROR_BAD_UNIT = 0x0000_0014,

        /// <summary>
        /// The device is not ready.
        /// </summary>
        [Display(Name = "Not Ready Error", ShortName = "Not Ready")]
        [Description("")]
        ERROR_NOT_READY = 0x0000_0015,

        /// <summary>
        /// The device does not recognize the command.
        /// </summary>
        [Display(Name = "Bad Command Error", ShortName = "Bad Command")]
        [Description("")]
        ERROR_BAD_COMMAND = 0x0000_0016,

        /// <summary>
        /// Data error (cyclic redundancy check).
        /// </summary>
        [Display(Name = "CRC Error", ShortName = "CRC")]
        [Description("")]
        ERROR_CRC = 0x0000_0017,

        /// <summary>
        /// The program issued a command but the command length is incorrect.
        /// </summary>
        [Display(Name = "Bad Length Error", ShortName = "Bad Length")]
        [Description("")]
        ERROR_BAD_LENGTH = 0x0000_0018,

        /// <summary>
        /// The drive cannot locate a specific area or track on the disk.
        /// </summary>
        [Display(Name = "Seek Error", ShortName = "Seek")]
        [Description("")]
        ERROR_SEEK = 0x0000_0019,

        /// <summary>
        /// The specified disk or diskette cannot be accessed.
        /// </summary>
        [Display(Name = "Not DOS Disk Error", ShortName = "Not DOS Disk")]
        [Description("")]
        ERROR_NOT_DOS_DISK = 0x0000_001A,

        /// <summary>
        /// The drive cannot find the sector requested.
        /// </summary>
        [Display(Name = "Sector not Found Error", ShortName = "Sector not Found")]
        [Description("")]
        ERROR_SECTOR_NOT_FOUND = 0x0000_001B,

        /// <summary>
        /// The printer is out of paper.
        /// </summary>
        [Display(Name = "Out of Paper Error", ShortName = "Out of Paper")]
        [Description("")]
        ERROR_OUT_OF_PAPER = 0x0000_001C,

        /// <summary>
        /// The system cannot write to the specified device.
        /// </summary>
        [Display(Name = "Write Fault Error", ShortName = "Write Fault")]
        [Description("")]
        ERROR_WRITE_FAULT = 0x0000_001D,

        /// <summary>
        /// The system cannot read from the specified device.
        /// </summary>
        [Display(Name = "Read Fault Error", ShortName = "Read Fault")]
        [Description("")]
        ERROR_READ_FAULT = 0x0000_001E,

        /// <summary>
        /// The device attached to the system is not functioning.
        /// </summary>
        [Display(Name = "General Failure Error", ShortName = "General Failure")]
        [Description("")]
        ERROR_GEN_FAILURE = 0x0000_001F,

        /// <summary>
        /// The process cannot access the file because it is being used by another process.
        /// </summary>
        [Display(Name = "Sharing Violation Error", ShortName = "Sharing Violation")]
        [Description("")]
        ERROR_SHARING_VIOLATION = 0x0000_0020,

        /// <summary>
        /// </summary>
        [Display(Name = "Lock Violation Error", ShortName = "Lock Violation")]
        [Description("")]
        ERROR_LOCK_VIOLATION = 0x0000_0021,

        /// <summary>
        /// The wrong diskette is in the drive. Insert %2 (Volume Serial Number: %3) into drive %1.
        /// </summary>
        [Display(Name = "Wrong Disk Error", ShortName = "Wrong Disk")]
        [Description("")]
        ERROR_WRONG_DISK = 0x0000_0022,

        /// <summary>
        /// Too many files opened for sharing.
        /// </summary>
        [Display(Name = "Sharing Buffer Exceeded Error", ShortName = "Sharing Buffer Exceeded")]
        [Description("")]
        ERROR_SHARING_BUFFER_EXCEEDED = 0x0000_0024,

        /// <summary>
        /// Reached the end of the file.
        /// </summary>
        [Display(Name = "Handle End Of File Error", ShortName = "End Of File")]
        [Description("")]
        ERROR_HANDLE_EOF = 0x0000_0026,

        /// <summary>
        /// The disk is full.
        /// </summary>
        [Display(Name = "Disk Full Error", ShortName = "Disk Full")]
        [Description("")]
        ERROR_HANDLE_DISK_FULL = 0x0000_0027,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        [Display(Name = "Not Supported Error", ShortName = "Not Supported")]
        [Description("")]
        ERROR_NOT_SUPPORTED = 0x0000_0032,

        /// <summary>
        /// Windows cannot find the network path. Verify that the network path is correct and the destination computer is not busy
        /// or turned off. If Windows still cannot find the network path, contact your network administrator.
        /// </summary>
        [Display(Name = "Remote Not Listed Error", ShortName = "Remote Not Listed")]
        [Description("")]
        ERROR_REM_NOT_LIST = 0x0000_0033,

        /// <summary>
        /// You were not connected because a duplicate name exists on the network. If joining a domain, go to System in Control
        /// Panel to change the computer name and try again. If joining a workgroup, choose another workgroup name.
        /// </summary>
        [Display(Name = "Duplicate Name Error", ShortName = "Duplicate Name")]
        [Description("")]
        ERROR_DUP_NAME = 0x0000_0034,

        /// <summary>
        /// The network path was not found.
        /// </summary>
        [Display(Name = "Bad Network Path Error", ShortName = "Bad Network Path")]
        [Description("")]
        ERROR_BAD_NETPATH = 0x0000_0035,

        /// <summary>
        /// The network is busy.
        /// </summary>
        [Display(Name = "Network Busy Error", ShortName = "Network Busy")]
        [Description("")]
        ERROR_NETWORK_BUSY = 0x0000_0036,

        /// <summary>
        /// The specified network resource or device is no longer available.
        /// </summary>
        [Display(Name = "Device Not Exist Error", ShortName = "Device Not Exist")]
        [Description("")]
        ERROR_DEV_NOT_EXIST = 0x0000_0037,

        /// <summary>
        /// The network BIOS command limit has been reached.
        /// </summary>
        [Display(Name = "Too Many Commands Error", ShortName = "Too Many Commands")]
        [Description("")]
        ERROR_TOO_MANY_CMDS = 0x0000_0038,

        /// <summary>
        /// The network adapter hardware error occurred.
        /// </summary>
        [Display(Name = "Adapter Hardware Error", ShortName = "Adapter Hardware")]
        [Description("")]
        ERROR_ADAP_HDW_ERR = 0x0000_0039,

        /// <summary>
        /// The specified server cannot perform the requested operation.
        /// </summary>
        [Display(Name = "Bad Network Response Error", ShortName = "Bad Network Response")]
        [Description("")]
        ERROR_BAD_NET_RESP = 0x0000_003A,

        /// <summary>
        /// An unexpected network error occurred.
        /// </summary>
        [Display(Name = "Unexpected Network Error", ShortName = "Unexpected Network")]
        [Description("")]
        ERROR_UNEXP_NET_ERR = 0x0000_003B,

        /// <summary>
        /// The remote adapter is not compatible.
        /// </summary>
        [Display(Name = "Bad Remote Adapter Error", ShortName = "Bad Remote Adapter")]
        [Description("")]
        ERROR_BAD_REM_ADAP = 0x0000_003C,

        /// <summary>
        /// The printer queue is full.
        /// </summary>
        [Display(Name = "Printer Queue Full Error", ShortName = "Printer Queue Full")]
        [Description("")]
        ERROR_PRINTQ_FULL = 0x0000_003D,

        /// <summary>
        /// Space to the store the file waiting to be printed is not available on the server.
        /// </summary>
        [Display(Name = "No Spool Space Error", ShortName = "No Spool Space")]
        [Description("")]
        ERROR_NO_SPOOL_SPACE = 0x0000_003E,

        /// <summary>
        /// Your file waiting to be printed was deleted.
        /// </summary>
        [Display(Name = "Print Cancelled Error", ShortName = "Print Cancelled")]
        [Description("")]
        ERROR_PRINT_CANCELLED = 0x0000_003F,

        /// <summary>
        /// The specified network name is no longer available.
        /// </summary>
        [Display(Name = "Network Name Deleted Error", ShortName = "Network Name Deleted")]
        [Description("")]
        ERROR_NETNAME_DELETED = 0x0000_0040,

        /// <summary>
        /// Network access is denied.
        /// </summary>
        [Display(Name = "Network Access Denied Error", ShortName = "Network Access Denied")]
        [Description("")]
        ERROR_NETWORK_ACCESS_DENIED = 0x0000_0041,

        /// <summary>
        /// The network resource <see cref="Type"/> is not correct.
        /// </summary>
        [Display(Name = "Bad Device Type Error", ShortName = "Bad Device Type")]
        [Description("")]
        ERROR_BAD_DEV_TYPE = 0x0000_0042,

        /// <summary>
        /// The network name cannot be found.
        /// </summary>
        [Display(Name = "Bad Network Name Error", ShortName = "Bad Network Name")]
        [Description("")]
        ERROR_BAD_NET_NAME = 0x0000_0043,

        /// <summary>
        /// The name limit for the local computer network adapter card was exceeded.
        /// </summary>
        [Display(Name = "Too Many Names Error", ShortName = "Too Many Names")]
        [Description("")]
        ERROR_TOO_MANY_NAMES = 0x0000_0044,

        /// <summary>
        /// The network BIOS session limit was exceeded.
        /// </summary>
        [Display(Name = "Too Many Sessions Error", ShortName = "Too Many Sessions")]
        [Description("")]
        ERROR_TOO_MANY_SESS = 0x0000_0045,

        /// <summary>
        /// The remote server has been paused or is in the process of being started.
        /// </summary>
        [Display(Name = "Sharing Paused Error", ShortName = "Sharing Paused")]
        [Description("")]
        ERROR_SHARING_PAUSED = 0x0000_0046,

        /// <summary>
        /// No more connections can be made to this remote computer at this time because there are already as many connections as
        /// the computer can accept.
        /// </summary>
        [Display(Name = "Request Not Accepted Error", ShortName = "Request Not Accepted")]
        [Description("")]
        ERROR_REQ_NOT_ACCEP = 0x0000_0047,

        /// <summary>
        /// The specified printer or disk device has been paused.
        /// </summary>
        [Display(Name = "Re-Direct Paused Error", ShortName = "Re-Direct Paused")]
        [Description("")]
        ERROR_REDIR_PAUSED = 0x0000_0048,

        /// <summary>
        /// The file exists.
        /// </summary>
        [Display(Name = "File Exists Error", ShortName = "File Exists")]
        [Description("")]
        ERROR_FILE_EXISTS = 0x0000_0050,

        /// <summary>
        /// The directory or file cannot be created.
        /// </summary>
        [Display(Name = "Cannot Make Error", ShortName = "Cannot Make")]
        [Description("")]
        ERROR_CANNOT_MAKE = 0x0000_0052,

        /// <summary>
        /// Failure on Interrupt 24.
        /// </summary>
        [Display(Name = "Fail Interrupt 24 Error", ShortName = "Fail Interrupt 24")]
        [Description("")]
        ERROR_FAIL_I24 = 0x0000_0053,

        /// <summary>
        /// Storage to process this request is not available.
        /// </summary>
        [Display(Name = "Out of Structures Error", ShortName = "Out of Structures")]
        [Description("")]
        ERROR_OUT_OF_STRUCTURES = 0x0000_0054,

        /// <summary>
        /// The local device name is already in use.
        /// </summary>
        [Display(Name = "Already Assigned Error", ShortName = "Already Assigned")]
        [Description("")]
        ERROR_ALREADY_ASSIGNED = 0x0000_0055,

        /// <summary>
        /// The specified network password is not correct.
        /// </summary>
        [Display(Name = "Invalid Password Error", ShortName = "Invalid Password")]
        [Description("")]
        ERROR_INVALID_PASSWORD = 0x0000_0056,

        /// <summary>
        /// The parameter is incorrect.
        /// </summary>
        [Display(Name = "Invalid Parameter Error", ShortName = "Invalid Parameter")]
        [Description("")]
        ERROR_INVALID_PARAMETER = 0x0000_0057,

        /// <summary>
        /// A write fault occurred on the network.
        /// </summary>
        [Display(Name = "Network Write Fault Error", ShortName = "Network Write Fault")]
        [Description("")]
        ERROR_NET_WRITE_FAULT = 0x0000_0058,

        /// <summary>
        /// The system cannot start another process at this time.
        /// </summary>
        [Display(Name = "No Process Slots Error", ShortName = "No Process Slots")]
        [Description("")]
        ERROR_NO_PROC_SLOTS = 0x0000_0059,

        /// <summary>
        /// Cannot create another system semaphore.
        /// </summary>
        [Display(Name = "Too Many Semaphores Error", ShortName = "Too Many Semaphores")]
        [Description("")]
        ERROR_TOO_MANY_SEMAPHORES = 0x0000_0064,

        /// <summary>
        /// The exclusive semaphore is owned by another process.
        /// </summary>
        [Display(Name = "Exclusive Semaphore Already Owned Error", ShortName = "Exclusive Semaphore Already Owned")]
        [Description("")]
        ERROR_EXCL_SEM_ALREADY_OWNED = 0x0000_0065,

        /// <summary>
        /// The semaphore is set and cannot be closed.
        /// </summary>
        [Display(Name = "Semaphore is Set Error", ShortName = "Semaphore is Set")]
        [Description("")]
        ERROR_SEM_IS_SET = 0x0000_0066,

        /// <summary>
        /// </summary>
        [Display(Name = "Broken Pipe Error", ShortName = "Broken Pipe")]
        [Description("")]
        ERROR_BROKEN_PIPE = 0x0000_006D,

        /// <summary>
        /// </summary>
        [Display(Name = "Open Failed Error", ShortName = "Open Failed")]
        [Description("")]
        ERROR_OPEN_FAILED = 0x0000_006E,

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
        [Display(Name = "Invalid Name Error", ShortName = "Invalid Name")]
        [Description("")]
        ERROR_INVALID_NAME = 0x0000_007B,

        /// <summary>
        /// </summary>
        [Display(Name = "Directory Not Empty Error", ShortName = "Directory Not Empty")]
        [Description("")]
        ERROR_DIR_NOT_EMPTY = 0x0000_0091,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Arguments Error", ShortName = "Invalid Arguments")]
        [Description("")]
        ERROR_BAD_ARGUMENTS = 0x0000_00A0,

        /// <summary>
        /// </summary>
        [Display(Name = "Bad Path Name Error", ShortName = "Invalid Path Name")]
        [Description("")]
        ERROR_BAD_PATHNAME = 0x0000_00A1,

        /// <summary>
        /// </summary>
        [Display(Name = "File Name Exceeded Range Error", ShortName = "File Name Exceeded Range")]
        [Description("")]
        ERROR_FILENAME_EXED_RANGE = 0x0000_00CE,

        /// <summary>
        /// </summary>
        [Display(Name = "No More Items Error", ShortName = "No More Items")]
        [Description("")]
        ERROR_NO_MORE_ITEMS = 0x0000_0103,

        /// <summary>
        /// </summary>
        [Display(Name = "Directory Error", ShortName = "Directory")]
        [Description("")]
        ERROR_DIRECTORY = 0x0000_010B,

        /// <summary>
        /// </summary>
        [Display(Name = "Arithmetic Overflow Error", ShortName = "Arithmetic Overflow")]
        [Description("")]
        ERROR_ARITHMETIC_OVERFLOW = 0x0000_0216,

        /// <summary>
        /// </summary>
        [Display(Name = "Control-C Exit Error", ShortName = "Control-C Exit")]
        [Description("")]
        ERROR_CONTROL_C_EXIT = 0x0000_023C,

        /// <summary>
        /// </summary>
        [Display(Name = "No More Matches Error", ShortName = "No More Matches")]
        [Description("")]
        ERROR_NO_MORE_MATCHES = 0x0000_0272,

        /// <summary>
        /// </summary>
        [Display(Name = "System Shutdown Error", ShortName = "System Shutdown")]
        [Description("")]
        ERROR_SYSTEM_SHUTDOWN = 0x0000_0281,

        /// <summary>
        /// </summary>
        [Display(Name = "Assertion Failure Error", ShortName = "Assertion Failed")]
        [Description("")]
        ERROR_ASSERTION_FAILURE = 0x0000_029C,

        /// <summary>
        /// </summary>
        [Display(Name = "Fatal Application Exit Error", ShortName = "Application Exit")]
        [Description("")]
        ERROR_FATAL_APP_EXIT = 0x0000_02C9,

        /// <summary>
        /// </summary>
        [Display(Name = "Stack Overflow Error", ShortName = "Stack Overflow")]
        [Description("")]
        ERROR_STACK_OVERFLOW = 0x0000_03E9,

        /// <summary>
        /// </summary>
        [Display(Name = "Possible Deadlock Error", ShortName = "Possible Deadlock")]
        [Description("")]
        ERROR_POSSIBLE_DEADLOCK = 0x0000_046B,

        /// <summary>
        /// </summary>
        [Display(Name = "Not Found Error", ShortName = "Not Found")]
        [Description("")]
        ERROR_NOT_FOUND = 0x0000_0490,

        /// <summary>
        /// </summary>
        [Display(Name = "No Match Error", ShortName = "No Match")]
        [Description("")]
        ERROR_NO_MATCH = 0x0000_0491,

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
        [Display(Name = "Logon Failure Error", ShortName = "Logon Failed")]
        [Description("")]
        ERROR_LOGON_FAILURE = 0x0000_052E,

        /// <summary>
        /// </summary>
        [Display(Name = "Internal Error", ShortName = "Internal")]
        [Description("")]
        ERROR_INTERNAL_ERROR = 0x0000_054F,

        /// <summary>
        /// </summary>
        [Display(Name = "Timeout Error", ShortName = "Timeout")]
        [Description("")]
        ERROR_TIMEOUT = 0x0000_05B4,

        /// <summary>
        /// </summary>
        [Display(Name = "Install User Exit Error", ShortName = "User Exited Install")]
        [Description("")]
        ERROR_INSTALL_USEREXIT = 0x0000_0642,

        /// <summary>
        /// </summary>
        [Display(Name = "Install Failure Error", ShortName = "Install Failed")]
        [Description("")]
        ERROR_INSTALL_FAILURE = 0x0000_0643,

        /// <summary>
        /// Undefined WinError code.
        /// </summary>
        [Display(Name = "Unknown Error", ShortName = "Unknown")]
        [Description("")]
        ERROR_UNKNOWN_ERROR = 0x0000_FFFF,
    }

    public static class WinErrorExtension
    {
        #region Public Methods

        /// <summary>
        /// Gets the severity bit.
        /// </summary>
        /// <param name="statusCode">Specifies the NT Status Code.</param>
        /// <returns>A <see cref="int"/> representing the Severity Bit.</returns>
        public static int GetSeverity(this int statusCode)
        {
            return statusCode.GetFacilityCodeFromHResult();
        }

        /// <summary>
        /// Create a <see cref="WinError"/> from <paramref name="hr"/>.
        /// </summary>
        /// <param name="hr">Specifies the <see cref="HResult"/> to convert.</param>
        /// <returns>A <see cref="WinError"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="hr"/> is greater than <c>MAX_HR</c>.</exception>
        public static WinError MakeWinError(this HResult hr)
        {
            const uint MAX_HR = 0x8081FFFF;

            ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)hr, MAX_HR, nameof(hr));

            return (WinError)Enum.ToObject(typeof(WinError), ((int)hr).ToWinErrorCode());
        }

        /// <summary>
        /// Converts to <paramref name="severity"/>, <paramref name="facilityCode"/>, and <see cref="code"/> to an NT Status Code.
        /// </summary>
        /// <param name="severity">    Specifies the severity bit. A value of '1' specifies an error; '0' specifies other conditions.</param>
        /// <param name="facilityCode">Specifies the Windows facility code.</param>
        /// <param name="code">        Specifies the <see cref="WinError"/> code.</param>
        /// <returns>A <see cref="uint"/> representing an NT Status Code.</returns>
        public static uint ToNtStatusCode(ulong severity, ulong facilityCode, WinError code)
        {
            return HResultExtension.ToHResultCode(severity, facilityCode, code);
        }

        /// <summary>
        /// Converts to <paramref name="hr"/> to an NT Status Code.
        /// </summary>
        /// <param name="hr">Specifies the <see cref="HResult"/>.</param>
        /// <returns>A <see cref="uint"/> representing an NT Status Code.</returns>
        public static uint ToNtStatusCode(this int hr)
        {
            return Convert.ToUInt32(hr & FacilityCode.FACILITY_NT_BIT);
        }

        /// <summary>
        /// Converts <paramref name="code"/> to a <see cref="uint"/> representing the <see cref="WinError"/>.
        /// </summary>
        /// <param name="code">Specifies the <see cref="WinError"/> code.</param>
        /// <returns>A <see cref="uint"/> representing the <see cref="WinError"/>.</returns>
        public static uint ToWinErrorCode(this WinError code)
        {
            return Enum.IsDefined<WinError>(code) ? (uint)code : (uint)WinError.ERROR_UNKNOWN_ERROR;
        }

        /// <summary>
        /// Converts <paramref name="hr"/> to a <see cref="uint"/> representing a <see cref="WinError"/>.
        /// </summary>
        /// <param name="hr">Specifies the <see cref="HResult"/>.</param>
        /// <returns>A <see cref="uint"/> representing the <see cref="WinError"/>.</returns>
        public static uint ToWinErrorCode(this int hr)
        {
            return Convert.ToUInt32(hr & FacilityCode.FACILITY_MASK);
        }

        #endregion Public Methods
    }
}
