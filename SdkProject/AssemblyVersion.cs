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
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implements a sub-class of <see cref="Version"/> for <see cref="Assembly"/> versions.
    /// </summary>
    /// <seealso cref="IEquatable{T}"/>
    /// <seealso cref="IComparable{T}"/>
    public partial class AssemblyVersion : IEquatable<AssemblyVersion>, IComparable<AssemblyVersion>, IEqualityComparer<AssemblyVersion>
    {
        /// <summary>
        /// Regular expression <see cref="Regex"/> to validate and parse strings into <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"(?<majorVersion>(\d+))(\.(?<minorVersion>(\d+)))(\.(?<buildNumber>(\d+|\*)))?(\.(?<revision>(\d+|\*)))?", RegexOptions.Compiled)]
        private static partial Regex AssemblyVersionRegex();

        /// <summary>
        /// Validates the <see cref="AssemblyVersion"/> build part string.
        /// </summary>
        /// <param name="build">Specifies the build part of the <see cref="AssemblyVersion"/>.</param>
        /// <returns>A string parseable as an <see cref="AssemblyVersion"/>.</returns>
        private static string ValidateAssemblyVersionBuild(string build)
        {
            return string.IsNullOrEmpty(build) || build == "*" ? "0" : build;
        }

        protected IEqualityComparer<AssemblyVersion> DefaultComparer { get; }

        /// <summary>
        /// Gets a value indicating the <see cref="Version"/> representing the parsed <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <value>Specifies the <see cref="Version"/> representing the parsed <see cref="AssemblyVersion"/>.</value>
        protected Version TheVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion"/> class.
        /// </summary>
        public AssemblyVersion()
            : this("1.0.0.0", false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion"/> class.
        /// </summary>
        /// <param name="theVersion">Specifies the <see cref="Version"/>.</param>
        public AssemblyVersion(Version theVersion) => this.TheVersion = theVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion"/> class.
        /// </summary>
        /// <param name="version">Specifies the version string.</param>
        public AssemblyVersion(string version)
            : this(version, false)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="AssemblyVersion"/> class. </summary> <param
        /// name="version">Specifies the version string.</param> <param name="isAssemblyVersion">If set to <see langref="true"/>,
        /// parse <paramref name="version"/> as an <see cref="AssemblyVersion"/>; otherwise, parse as a <see
        /// cref="Version"/>.</param> <exception cref="ArgumentException">The specified string \"{version}\" is not a valid
        /// AssemblyVersion number.
        public AssemblyVersion(string version, bool isAssemblyVersion)
        {
            this.TheVersion = TryParse(version, isAssemblyVersion, out AssemblyVersion? assemblyVersion) ? assemblyVersion!.TheVersion : new Version(1, 0, 0, 0);
            this.DefaultComparer = EqualityComparer<AssemblyVersion>.Create((l, r) => l is not null ? l.Equals(r) : r is null, v => v.GetHashCode());
        }

        /// <summary>
        /// Gets a value indicating the build number part of the <see cref="TheVersion"/>.
        /// </summary>
        /// <value>Specifies the build number [0, 65534].</value>
        public int BuildNumber => this.TheVersion.Build < 0 ? 0 : this.TheVersion.Build;

        /// <summary>
        /// Gets a value indicating the major version part of the <see cref="TheVersion"/>.
        /// </summary>
        /// <value>Specifies the major version [0, 65534].</value>
        public int MajorVersion => this.TheVersion.Major < 0 ? 0 : this.TheVersion.Major;

        /// <summary>
        /// Gets a value indicating the minor version part of the <see cref="TheVersion"/>.
        /// </summary>
        /// <value>Specifies the minor version [0, 65534].</value>
        public int MinorVersion => this.TheVersion.Minor < 0 ? 0 : this.TheVersion.Minor;

        /// <summary>
        /// Gets a value indicating the revision part of the <see cref="TheVersion"/>.
        /// </summary>
        /// <value>Specifies the revision [0, 65534].</value>
        public int Revision => this.TheVersion.Revision < 0 ? 0 : this.TheVersion.Revision;

        /// <summary>
        /// Implements the operator != for <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <param name="left"> Specifies the left <see cref="AssemblyVersion"/>.</param>
        /// <param name="right">Specifies the right <see cref="AssemblyVersion"/>.</param>
        /// <returns>The <see cref="bool"/> result of the operator.</returns>
        public static bool operator !=(AssemblyVersion? left, AssemblyVersion? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator != for <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <param name="left"> Specifies the left <see cref="AssemblyVersion"/>.</param>
        /// <param name="right">Specifies the right string.</param>
        /// <returns>The <see cref="bool"/> result of the operator.</returns>
        public static bool operator !=(AssemblyVersion? left, string? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator != for <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <param name="left"> Specifies the left string.</param>
        /// <param name="right">Specifies the right <see cref="AssemblyVersion"/>.</param>
        /// <returns>The <see cref="bool"/> result of the operator.</returns>
        public static bool operator !=(string? left, AssemblyVersion? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator less than operator for <see cref="AssemblyVersion"/>.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(AssemblyVersion? left, AssemblyVersion? right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <(AssemblyVersion? left, string? right)
        {
            return left is null ? !string.IsNullOrEmpty(right) : left.CompareTo(new AssemblyVersion(right!)) < 0;
        }

        public static bool operator <(string? left, AssemblyVersion? right)
        {
            return string.IsNullOrEmpty(left) ? right is not null : (new AssemblyVersion(left)).CompareTo(right) < 0;
        }

        public static bool operator <=(AssemblyVersion? left, AssemblyVersion? right)
        {
            return !(left > right);
        }

        public static bool operator <=(AssemblyVersion? left, string? right)
        {
            return !(left > right);
        }

        public static bool operator <=(string? left, AssemblyVersion? right)
        {
            return !(left > right);
        }

        public static bool operator ==(AssemblyVersion? left, AssemblyVersion? right)
        {
            return left is null ? right is null : left.Equals(right);
        }

        public static bool operator ==(AssemblyVersion? left, string? right)
        {
            return left is null ? string.IsNullOrEmpty(right) : left.Equals(new AssemblyVersion(right!));
        }

        public static bool operator ==(string? left, AssemblyVersion? right)
        {
            return string.IsNullOrEmpty(left) ? right is null : (new AssemblyVersion(left)).Equals(right);
        }

        public static bool operator >(AssemblyVersion? left, AssemblyVersion? right)
        {
            return left is not null ? right is null : left?.CompareTo(right) > 0;
        }

        public static bool operator >(AssemblyVersion? left, string? right)
        {
            return left is not null ? string.IsNullOrEmpty(right) : left?.CompareTo(new AssemblyVersion(right!)) > 0;
        }

        public static bool operator >(string? left, AssemblyVersion? right)
        {
            return !string.IsNullOrEmpty(left) ? right is null : (new AssemblyVersion(left!)).CompareTo(right) > 0;
        }

        public static bool operator >=(AssemblyVersion? left, AssemblyVersion? right)
        {
            return !(left < right);
        }

        public static bool operator >=(AssemblyVersion? left, string? right)
        {
            return !(left < right);
        }

        public static bool operator >=(string? left, AssemblyVersion? right)
        {
            return !(left < right);
        }

        public int CompareTo(AssemblyVersion? other)
        {
            return other is null ? 1 : this.TheVersion.CompareTo(other.TheVersion);
        }

        public override bool Equals(object? obj)
        {
            return obj is AssemblyVersion other ? this.TheVersion.Equals(other.TheVersion) : false;
        }

        public bool Equals(AssemblyVersion? other)
        {
            return other is null ? false : this.TheVersion.Equals(other.TheVersion);
        }

        public bool Equals(AssemblyVersion? x, AssemblyVersion? y) => DefaultComparer.Equals(x, y);

        public override int GetHashCode()
        {
            return HashCode.Combine(this.TheVersion, MajorVersion, MinorVersion, BuildNumber, Revision, AssemblyVersionRegex);
        }

        public int GetHashCode([DisallowNull] AssemblyVersion obj) => obj.GetHashCode();

        public AssemblyVersion Parse(string version, bool isAssemblyVersion)
        {
            if (!AssemblyVersionRegex().IsMatch(version))
            {
                throw new ArgumentException($"The specified string \"{version}\" is not a valid AssemblyVersion number", nameof(version));
            }
            else if (isAssemblyVersion)
            {
                MatchCollection matches = AssemblyVersionRegex().Matches(version);
                int majorVersion = int.TryParse(matches[0].Groups["majorVersion"].Value, out int major) ? major : 0;
                int minorVersion = int.TryParse(matches[0].Groups["minorVersion"].Value, out int minor) ? minor : 0;
                string buildValue = ValidateAssemblyVersionBuild(matches[0].Groups["buildNumber"].Value);
                int buildNumber = int.TryParse(buildValue, out int build) ? build : 0;
                int revisionNumber = int.TryParse(matches[0].Groups["revision"].Value, out int revision) ? revision : 0;
                return new AssemblyVersion(new Version(majorVersion, minorVersion, buildNumber, revisionNumber));
            }
            else
            {
                return Version.TryParse(version, out Version? parsedVersion) ? new AssemblyVersion(parsedVersion) : new AssemblyVersion("1.0.0.0");
            }
        }

        public override string ToString()
        {
            return ToString(4);
        }

        public string ToString(int fieldCount)
        {
            return this.TheVersion.ToString(fieldCount);
        }

        public bool TryParse(string version, bool isAssemblyVersion, out AssemblyVersion? assemblyVersion)
        {
            try
            {
                assemblyVersion = Parse(version, isAssemblyVersion);
                return true;
            }
            catch
            {
                assemblyVersion = null;
                return false;
            }
        }
    }
}
