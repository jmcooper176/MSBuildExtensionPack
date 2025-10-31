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
namespace MSBuild.ExtensionPack.SdkProject
{
    using MSBuild.ExtensionPack.Base.Enumeration;

    public class AssemblyVersionSettings
    {
        public AssemblyVersionSettings()
        {
            Version = new System.Version(0, 0, 0, 0);
            MajorVersion = 0;
            MinorVersion = 0;
            BuildNumber = 0;
            Revision = 0;
            BuildNumberType = IncrementMethod.NoIncrement;
            RevisionType = IncrementMethod.NoIncrement;
            BuildNumberFormat = "{0}";
            RevisionFormat = "{0}";
            RevisionReset = false;
        }

        public int BuildNumber
        {
            get
            {
                return Version.Build < 0 ? 0 : Version.Build;
            }

            set
            {
                Version = new Version(MajorVersion, MinorVersion, value < 0 ? 0 : value, Revision);
            }
        }

        public string BuildNumberFormat { get; set; }
        public IncrementMethod BuildNumberType { get; set; }

        public int MajorVersion
        {
            get
            {
                return Version.Major < 0 ? 0 : Version.Major;
            }

            set
            {
                Version = new System.Version(value < 0 ? 0 : value, MinorVersion, BuildNumber, Revision);
            }
        }

        public int MinorVersion
        {
            get
            {
                return Version.Minor < 0 ? 0 : Version.Minor;
            }

            set
            {
                Version = new System.Version(MajorVersion, value < 0 ? 0 : value, BuildNumber, Revision);
            }
        }

        public int Revision
        {
            get
            {
                return Version.Revision < 0 ? 0 : Version.Revision;
            }

            set
            {
                Version = new System.Version(MajorVersion, MinorVersion, BuildNumber, value < 0 ? 0 : value);
            }
        }

        public string RevisionFormat { get; set; }
        public bool RevisionReset { get; set; }
        public IncrementMethod RevisionType { get; set; }

        public Version Version { get; set; }
    }
}
