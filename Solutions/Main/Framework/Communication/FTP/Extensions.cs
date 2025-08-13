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

using MSBuild.ExtensionPack.Communication.Extended;

namespace MSBuild.ExtensionPack.Communication.Ftp.Ftp
{
    using System;

    /// <summary>
    /// Helper class used to convert FILETIME to DateTime
    /// </summary>
    internal static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Converts given datetime in FILETIME struct format and convert it to .Net DateTime.
        /// </summary>
        /// <param name="time">The given time in FileTime structure format</param>
        /// <returns>The DateTime equivalent of the given fileTime</returns>
        public static DateTime? ToDateTime(this NativeMethods.FILETIME time)
        {
            if (time.dwHighDateTime == 0 && time.dwLowDateTime == 0)
            {
                return null;
            }

            unchecked
            {
                uint low = (uint)time.dwLowDateTime;
                long ft = (long)time.dwHighDateTime << 32 | low;
                return DateTime.FromFileTimeUtc(ft);
            }
        }

        #endregion Public Methods
    }
}
