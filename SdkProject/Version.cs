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
namespace SdkProject
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal class Version
    {
        private string versionString;

        private static string ValidateAssemblyVersionPart(string part)
        {
            return string.IsNullOrEmpty(part) || part == "*" ? "0" : part;
        }

        private void ParseAssemblyVersion(string version)
        {
            Regex versionPattern = new Regex(@"(?<majorVersion>(\d+))(\.(?<minorVersion>(\d+)))(\.(?<buildNumber>(\d+|\*)))?(\.(?<revision>(\d+|\*)))?", RegexOptions.Compiled);

            MatchCollection matches = versionPattern.Matches(version);
            if (matches.Count != 1)
            {
                throw new ArgumentException($"The specified string \"{version}\" is not a valid AssemblyVersion number", nameof(version));
            }

            MajorVersion = matches[0].Groups["majorVersion"].Value;
            MinorVersion = matches[0].Groups["minorVersion"].Value;
            BuildNumber = ValidateAssemblyVersionPart(matches[0].Groups["buildNumber"].Value);
            Revision = ValidateAssemblyVersionPart(matches[0].Groups["revision"].Value);
            versionString = version;
        }

        private void ParseVersion(string version)
        {
            Regex versionPattern = new Regex(@"(?<majorVersion>(\d+))(\.(?<minorVersion>(\d+)))(\.(?<buildNumber>(\d+)))(\.(?<revision>(\d+)))", RegexOptions.Compiled);

            MatchCollection matches = versionPattern.Matches(version);
            if (matches.Count != 1)
            {
                throw new ArgumentException($"The specified string \"{version}\" is not a valid version number", nameof(version));
            }

            MajorVersion = matches[0].Groups["majorVersion"].Value;
            MinorVersion = matches[0].Groups["minorVersion"].Value;
            BuildNumber = matches[0].Groups["buildNumber"].Value;
            Revision = matches[0].Groups["revision"].Value;
            versionString = version; // Very important that this is a little v, not big v, otherwise you get infinite recursion!
        }

        public Version()
        {
            MajorVersion = "1";
            MinorVersion = "0";
            BuildNumber = "0";
            Revision = "0";
        }

        public Version(string version) : this(version, false)
        {
        }

        public Version(string version, bool isAssemblyVersion)
        {
            if (isAssemblyVersion)
            {
                ParseAssemblyVersion(version);
            }
            else
            {
                ParseVersion(version);
            }
        }

        public string BuildNumber { get; set; }

        public string MajorVersion { get; set; }

        public string MinorVersion { get; set; }

        public string Revision { get; set; }

        public string VersionString
        {
            get => versionString;
            set => ParseVersion(value);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", MajorVersion, MinorVersion, BuildNumber, Revision);
        }
    }
}
