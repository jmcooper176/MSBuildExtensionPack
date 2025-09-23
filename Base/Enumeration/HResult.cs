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
namespace MSBuild.ExtensionPack.Base.Enumeration
{
    public enum HResult : uint
    {
        E_OUTOFMEMORY,
    }

    public static class HResultExtension
    {
        #region Public Fields

        public const int SEVERITY_BIT = 0x0000_0001;
        public const ulong SEVERITY_ERROR = 1UL;
        public const uint SEVERITY_MASK = 0x8000_0000;

        #endregion Public Fields

        #region Public Methods

        public static bool Failed(this int hr)
        {
            return hr < WinError.ERROR_SUCCESS.ToWinErrorCode() || hr.IsError();
        }

        public static int GetSeverity(this int hr)
        {
            return hr >> 31 & SEVERITY_BIT;
        }

        public static bool IsError(this int hr)
        {
            return (ulong)hr >> 31 == SEVERITY_ERROR;
        }

        public static HResult MakeHResult(ulong severity, ulong facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, (ulong)FacilityCode.FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, (ulong)FacilityCode.FACILITY_OPC, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));

            if (severity != 0UL && severity != 1UL)
            {
                throw new ArgumentException($"Parameter {nameof(severity)} with value '{severity}' is invalid.", nameof(severity));
            }

            return (HResult)Enum.ToObject(typeof(HResult), ToHResultCode(severity, facilityCode, code));
        }

        public static bool Succeeded(this int hr)
        {
            return hr >= WinError.ERROR_SUCCESS.ToWinErrorCode() && !hr.IsError();
        }

        public static uint ToHResultCode(ulong severity, ulong facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, (ulong)FacilityCode.FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, (ulong)FacilityCode.FACILITY_OPC, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));

            if (severity != 0UL && severity != 1UL)
            {
                throw new ArgumentException($"Parameter {nameof(severity)} with value '{severity}' is invalid.", nameof(severity));
            }

            return Convert.ToUInt32(severity << 31 | facilityCode << 16 | code.ToWinErrorCode());
        }

        /// <summary>
        /// </summary>
        /// <param name="facilityCode"></param>
        /// <param name="code">        </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ToHResultCode(int facilityCode, WinError code)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(code.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(code.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(code));
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, FacilityCode.FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, FacilityCode.FACILITY_OPC, nameof(facilityCode));

            return Convert.ToInt32(code <= WinError.ERROR_SUCCESS ? code.ToWinErrorCode() : code.ToWinErrorCode() | (uint)facilityCode << 16 | SEVERITY_MASK);
        }

        public static int ToHResultCode(this int ntStatus)
        {
            return ntStatus | FacilityCode.FACILITY_NT_BIT;
        }

        #endregion Public Methods
    }
}
