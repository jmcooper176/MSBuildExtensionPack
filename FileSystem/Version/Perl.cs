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
namespace MSBuild.ExtensionPack.FileSystem.Version
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implements versioning methods using the Perl versioning scheme.
    /// </summary>
    /// <seealso cref="IVersionMethod"/>
    public partial class Perl : IVersionMethod, IComparable<Perl>, IEquatable<Perl>, IEqualityComparer<Perl>
    {
        /// <summary>
        /// Defines the <see cref="Perl"/> version regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Perl"/> regular expression.</returns>
        [GeneratedRegex(@"^((?:<major>0|[1-9]\d{0,4})\.(?:<fractional>0|[1-9]\d*))$", RegexOptions.Compiled)]
        private static partial Regex VersionNumberRegex();

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        protected Perl()
        {
            this.Caption = "Perl Version Number";
            this.ComputerName = Environment.MachineName;
            this.DefaultComparer = EqualityComparer<Perl>.Create((x, y) => x is not null ? x.Equals(y) : y is null, this.GetHashCode);
            this.Description = "Perl Version Number";
            this.DisplayName = "Perl Version";
            this.FixComments = string.Empty;
            this.HotFixId = "HotFix_0";
            this.InstallDate = DateTime.UtcNow;
            this.InstalledBy = Environment.UserName;
            this.InstalledOn = InstallDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Name = "Perl Version Number";
            this.PerlVersion = 0.0;
            this.ServicePackInEffect = null;
            this.Status = QuickFixEngineeringStatus.Ok;
            this.Version = new Version();
            this.ZeroDay = new(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// The maximum major version number for <see cref="Perl"/>.
        /// </summary>
        public const int MAX_MAJOR = 65534;

        /// <summary>
        /// The maximum minor version number for <see cref="Perl"/>.
        /// </summary>
        public const int MAX_MINOR = 999;

        /// <summary>
        /// The maximum revision version number for <see cref="Perl"/>.
        /// </summary>
        public const int MAX_REVISION = 999;

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="version">Specifies the <see cref="double"/><see cref="Perl"/> version.</param>
        public Perl(double version)
            : this()
        {
            this.PerlVersion = version;
            this.Version = new(GetMajor(version), GetMinor(version), GetRevision(version));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="version">Specifies the version string parsable as a <see cref="double"/>.</param>
        /// <exception cref="ArgumentException">Version string is not in the correct format.</exception>
        public Perl(string version)
            : this()
        {
            if (!VersionNumberRegex().IsMatch(version))
            {
                throw new ArgumentException("Version string is not in the correct format", nameof(version));
            }

            this.PerlVersion = double.TryParse(version, out double result) ? result : 0.0;
            this.Version = new(GetMajor(this.PerlVersion), GetMinor(this.PerlVersion), GetRevision(this.PerlVersion));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="major">   The major.</param>
        /// <param name="minor">   The minor.</param>
        /// <param name="revision">The revision.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="major"/>, <paramref name="minor"/>, or <paramref name="revision"/> are out of range.
        /// </exception>
        public Perl(int major, int minor, int revision)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(revision, nameof(revision));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(revision, MAX_REVISION, nameof(revision));

            this.PerlVersion = major + (minor / 100.0) + (revision / 10000.0);
            this.Version = new(major, minor, revision);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        /// <param name="minor">Specifies the minor version number.</param>
        public Perl(int major, int minor)
            : this(major, minor, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        public Perl(int major)
            : this(major, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="version">Specifies the <see cref="Version"/>.</param>
        public Perl(Version version)
            : this(version.Major, version.Minor, version.Build)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Perl"/> class.
        /// </summary>
        /// <param name="version">Specifies the <see cref="SemanticVersion"/>.</param>
        public Perl(SemanticVersion version)
            : this(version.Major, version.Minor, version.Patch)
        {
        }

        /// <inheritdoc/>
        public string Caption { get; set; }

        /// <inheritdoc/>
        public string ComputerName { get; set; }

        /// <inheritdoc/>
        public EqualityComparer<Perl> DefaultComparer { get; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public string DisplayName { get; set; }

        /// <inheritdoc/>
        public string FixComments { get; set; }

        /// <inheritdoc/>
        public string HotFixId { get; set; }

        /// <inheritdoc/>
        public DateTime InstallDate { get; set; }

        /// <inheritdoc/>
        public string InstalledBy { get; set; }

        /// <inheritdoc/>
        public string InstalledOn { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating the <see cref="Perl"/> version as a <see cref="double"/>.
        /// </summary>
        /// <value>A <see cref="double"/> representing the <see cref="Perl"/> version.</value>
        public double PerlVersion { get; }

        /// <inheritdoc/>
        public string? ServicePackInEffect { get; set; }

        /// <inheritdoc/>
        public QuickFixEngineeringStatus Status { get; set; }

        /// <inheritdoc/>
        public Version Version { get; }

        /// <inheritdoc/>
        public DateTime ZeroDay { get; set; }

        /// <summary>
        /// Gets the fractional part of a double.
        /// </summary>
        /// <param name="version">Specifies the <see cref="double"/> version.</param>
        /// <returns>A <see cref="double"/> representing the factional portion after the <see cref="int"/> portion has been stripped.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="version"/> is negative.</exception>
        /// "
        public static double GetFractional(double version)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(version, nameof(version));

            return version - Math.Truncate(version);
        }

        /// <summary>
        /// Gets the major version number from <paramref name="version"/>.
        /// </summary>
        /// <param name="version">Specifies the <see cref="double"/> version.</param>
        /// <returns>An <see cref="int"/> representing the major version number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="version"/> is negative.</exception>
        /// "
        public static int GetMajor(double version)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(version, nameof(version));

            return version.IsNearlyEqual(0.0) ? 0 : Convert.ToInt32(Math.Truncate(version)) % MAX_MAJOR;
        }

        /// <summary>
        /// Gets the minor version number from <paramref name="version"/>.
        /// </summary>
        /// <param name="version">Specifies the <see cref="double"/> version.</param>
        /// <returns>An <see cref="int"/> representing the minor version number.</returns>
        public static int GetMinor(double version)
        {
            return version.IsNearlyEqual(0.0) ? 0 : Convert.ToInt32(Math.Round(GetFractional(version) * 100.0, MidpointRounding.AwayFromZero)) % MAX_MINOR;
        }

        /// <summary>
        /// Gets the revision version number from <paramref name="version"/>.
        /// </summary>
        /// <param name="version">Specifies the <see cref="double"/> version.</param>
        /// <returns>An <see cref="int"/> representing the revision version number.</returns>
        public static int GetRevision(double version)
        {
            return version.IsNearlyEqual(0.0)
                ? 0
                : Convert.ToInt32(Math.Round(GetFractional(GetFractional(version) * 100.0) * 100.0, MidpointRounding.AwayFromZero)) % MAX_REVISION;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Perl left, Perl right) => !left.Equals(right);

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Perl left, Perl right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Perl left, Perl right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Perl left, Perl right) => left.Equals(right);

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Perl left, Perl right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Perl left, Perl right) => left.CompareTo(right) >= 0;

        /// <inheritdoc/>
        public object Clone() => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(object? obj) => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(Perl? other) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Perl other && this.Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(Perl? other) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool Equals(Perl? x, Perl? y) => this.DefaultComparer.Equals(x, y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">Specifies the <see cref="Perl"/> object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode([DisallowNull] Perl obj) => obj.GetHashCode();

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.PerlVersion, this.Version, this.HotFixId, this.InstallDate, this.ServicePackInEffect, this.Status, this.ZeroDay);
        }

        /// <inheritdoc/>
        public string ToString(int fieldCount) => throw new NotImplementedException();

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
    }
}
