// This file is part of CycloneDX CLI Tool
//
// Licensed under the Apache License, Version 2.0 (the “License”); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an “AS IS”
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language
// governing permissions and limitations under the License.
//
// SPDX-License-Identifier: Apache-2.0 Copyright (c) OWASP Foundation. All Rights Reserved. Ignore Spelling: cyclonedx Cli
using MSBuild.ExtensionPack.Communication.Extended;

namespace MSBuild.ExtensionPack.Communication.Ftp
{
    using System;

    /// <summary>
    /// Helper class used to convert FILETIME to DateTime
    /// </summary>
    internal static class Extensions
    {
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
    }
}
