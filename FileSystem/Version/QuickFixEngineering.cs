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

// Ignore Spelling: utf

namespace MSBuild.ExtensionPack.FileSystem.Version
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class implementing the Quick Fix Engineering (QFE) versioning scheme.
    /// </summary>
    /// <seealso cref="IVersionMethod"/>
    public partial class QuickFixEngineering : IVersionMethod, IComparable<QuickFixEngineering>, IEquatable<QuickFixEngineering>, IEqualityComparer<QuickFixEngineering>
    {
        /// <summary>
        /// Defines the <see cref="QuickFixEngineering"/> version regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="QuickFixEngineering"/> regular expression.</returns>
        [GeneratedRegex(@"^(?:<major>([0-9]\d{0,4})(?:<quads>(\.([0-9]\d{0,4})){0,3}))$", RegexOptions.Compiled)]
        private static partial Regex VersionNumberRegex();

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        protected QuickFixEngineering()
        {
            this.Caption = "QFE Version Number";
            this.ComputerName = Environment.MachineName;
            this.DefaultComparer = EqualityComparer<QuickFixEngineering>.Create((x, y) => x is not null ? x.Equals(y) : y is null, this.GetHashCode);
            this.Description = "Quick Fix Engineering Version Number";
            this.DisplayName = "Version";
            this.FixComments = string.Empty;
            this.HotFixId = "HotFix_0";
            this.InstallDate = DateTime.UtcNow;
            this.InstalledBy = Environment.UserName;
            this.InstalledOn = InstallDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Name = "Version Number";
            this.ServicePackInEffect = null;
            this.Status = QuickFixEngineeringStatus.Ok;
            this.Version = new Version();
            this.ZeroDay = new(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// The maximum build number value.
        /// </summary>
        public const int MAX_BUILD = 32767;

        /// <summary>
        /// The maximum major number value.
        /// </summary>
        public const int MAX_MAJOR = 65534;

        /// <summary>
        /// The maximum minor number value.
        /// </summary>
        public const int MAX_MINOR = 65534;

        /// <summary>
        /// The maximum revision number value.
        /// </summary>
        public const int MAX_REVISION = 65534;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="major">Specifies the major number.</param>
        /// <param name="minor">Specifies the minor number.</param>
        /// <param name="build">Specifies the build number.</param>
        /// <remarks><see cref="CreateRevision"/> is called to set the revision build number.</remarks>
        public QuickFixEngineering(int major, int minor, int build)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(build, nameof(build));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(build, MAX_BUILD, nameof(build));

            this.Version = new(major, minor, build, CreateRevision());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="version">Specifies the version number string.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="version"/> does not match <see cref="VersionNumberRegex()"/>.</exception>
        public QuickFixEngineering(string version)
            : this()
        {
            if (!VersionNumberRegex().IsMatch(version))
            {
                throw new ArgumentException($"Parameter {nameof(version)} is not a valid System.Version string", nameof(version));
            }

            this.Version = new(version);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="version">Specifies the <see cref="Version"/> number.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="version"/> is <see langref="null"/>.</exception>
        /// <remarks>
        /// <see cref="CreateBuild()"/> is called if the builder number is out of range in <paramref name="version"/>. <see
        /// cref="CreateRevision()"/> is called if the revision number is out of range in <paramref name="version"/>.
        /// </remarks>
        public QuickFixEngineering([System.Diagnostics.CodeAnalysis.AllowNull] Version version)
            : this()
        {
            ArgumentNullException.ThrowIfNull(version, nameof(version));

            this.Version = new(
                version.Major,
                version.Minor,
                version.Build >= 0 && version.Build <= MAX_BUILD ? version.Build : CreateBuild(),
                version.Revision >= 0 && version.Revision <= MAX_REVISION ? version.Revision : CreateRevision());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="major">Specifies the major number.</param>
        /// <remarks>
        /// <see cref="CreateBuild"/> and <see cref="CreateRevision"/> are called to set the build and revision build numbers.
        /// </remarks>
        public QuickFixEngineering(int major)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));

            this.Version = new(major, 0, CreateBuild(), CreateRevision());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="major">Specifies the major number.</param>
        /// <param name="minor">Specifies the minor number.</param>
        /// <remarks>
        /// <see cref="CreateBuild"/> and <see cref="CreateRevision"/> are called to set the build and revision build numbers.
        /// </remarks>
        public QuickFixEngineering(int major, int minor)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));

            this.Version = new(major, minor, CreateBuild(), CreateRevision());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFixEngineering"/> class.
        /// </summary>
        /// <param name="major">   Specifies the major number.</param>
        /// <param name="minor">   Specifies the minor number.</param>
        /// <param name="build">   Specifies the build number.</param>
        /// <param name="revision">Specifies the revision number.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the parameters is out of range.</exception>
        /// "
        public QuickFixEngineering(int major, int minor, int build, int revision)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(build, nameof(build));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(build, MAX_BUILD, nameof(build));
            ArgumentOutOfRangeException.ThrowIfNegative(revision, nameof(revision));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(revision, MAX_REVISION, nameof(revision));

            this.Version = new(major, minor, build, revision);
        }

        /// <inheritdoc/>
        public string Caption { get; set; }

        /// <inheritdoc/>
        public string ComputerName { get; set; }

        /// <summary>
        /// Gets a value indicating the default <see cref="IEqualityComparer{T}"/> comparer.
        /// </summary>
        /// <value>The default <see cref="IEqualityComparer{T}"/> comparer.</value>
        public EqualityComparer<QuickFixEngineering> DefaultComparer { get; }

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

        /// <inheritdoc/>
        public string? ServicePackInEffect { get; set; }

        /// <inheritdoc/>
        public QuickFixEngineeringStatus Status { get; set; }

        /// <inheritdoc/>
        public Version Version { get; }

        /// <inheritdoc/>
        public DateTime ZeroDay { get; set; }

        /// <summary>
        /// Generates a revision number based on the current UTC time, suitable for use as a short-lived identifier within a single day.
        /// </summary>
        /// <remarks>
        /// The returned revision number increases throughout the day and wraps around after reaching <c>MAX_REVISION</c>. This
        /// method is intended for scenarios where a frequently changing, time-based revision identifier is needed, but is not
        /// suitable for use as a globally unique or persistent identifier.
        /// </remarks>
        /// <returns>
        /// A 16-bit unsigned integer representing the current revision number, calculated from the number of seconds elapsed since
        /// midnight UTC divided by two and modulo <c>MAX_REVISION</c>.
        /// </returns>
        public static ushort CreateRevision() => Convert.ToUInt16((DateTime.UtcNow - DateTime.UtcNow.Date).TotalSeconds / 2.0 % MAX_REVISION);

        /// <summary>
        /// Converts a <see cref="Version"/> instance to a <see cref="QuickFixEngineering"/> value by copying its major, minor,
        /// build, and revision components.
        /// </summary>
        /// <remarks>
        /// This operator enables explicit casting from <see cref="Version"/> to <see cref="QuickFixEngineering"/>. All version
        /// components are mapped directly. If any component in <paramref name="v"/> is undefined, its value will be set to -1 in
        /// the resulting <see cref="QuickFixEngineering"/> instance.
        /// </remarks>
        /// <param name="v">The <see cref="Version"/> instance to convert. Cannot be null.</param>
        public static explicit operator QuickFixEngineering(Version v) => new(v.Major, v.Minor, v.Build, v.Revision);

        /// <summary>
        /// Defines an explicit conversion from a string to a <see cref="QuickFixEngineering"/> instance.
        /// </summary>
        /// <param name="v">The string value to convert to a <see cref="QuickFixEngineering"/> instance. Cannot be null.</param>
        public static explicit operator QuickFixEngineering(string v) => new(v);

        /// <summary>
        /// Converts the specified <see cref="QuickFixEngineering"/> instance to its version string representation.
        /// </summary>
        /// <param name="v">Specifies the <see cref="QuickFixEngineering"/> instance to convert to a string.</param>
        public static explicit operator string(QuickFixEngineering v) => v.Version.ToString();

        /// <summary>
        /// Defines an implicit conversion from a <see cref="QuickFixEngineering"/> instance to a Version instance.
        /// </summary>
        /// <remarks>
        /// This operator enables seamless use of <see cref="QuickFixEngineering"/> objects where a Version is expected, by
        /// automatically returning the associated Version instance.
        /// </remarks>
        /// <param name="v">Specifies the <see cref="QuickFixEngineering"/> instance to convert to a <see cref="Version"/>.</param>
        public static implicit operator Version(QuickFixEngineering v) => v.Version;

        /// <summary>
        /// Creates a new version by incrementing the major component of the specified version and resetting all lower components to zero.
        /// </summary>
        /// <remarks>Use this method to advance to the next major version, discarding minor, build, and revision information.</remarks>
        /// <param name="version">The version to increment. Cannot be null.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the major version incremented by one and all other components set
        /// to zero.
        /// </returns>
        public static QuickFixEngineering IncrementMajor(QuickFixEngineering version) => new(version.Version.Major + 1, 0, 0, 0);

        /// <summary>
        /// Creates a new version with the minor component incremented by one and the build and revision components reset to zero.
        /// </summary>
        /// <param name="version">The version to increment. Must not be null.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the minor version increased by one, and the build and revision set
        /// to zero.
        /// </returns>
        public static QuickFixEngineering IncrementMinor(QuickFixEngineering version) => new(version.Version.Major, version.Version.Minor + 1, 0, 0);

        /// <summary>
        /// Increments the revision component of the specified quick-fix engineering version.
        /// </summary>
        /// <param name="version">The quick-fix engineering version to increment. Must not be null.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the revision component incremented by one. If the revision exceeds
        /// the maximum allowed value, it wraps to zero.
        /// </returns>
        public static QuickFixEngineering IncrementRevision(QuickFixEngineering version) => new(version.Version.Major, version.Version.Minor, version.Version.Build, version.Version.Revision <= MAX_REVISION ? version.Version.Revision + 1 : 0);

        /// <summary>
        /// Subtracts the version components of one <see cref="QuickFixEngineering"/> instance from another, producing a new
        /// instance with non-negative values for each component.
        /// </summary>
        /// <remarks>
        /// Each version component (Major, Minor, Build, Revision) is subtracted individually. If the result of any component is
        /// negative, it is set to zero in the resulting instance.
        /// </remarks>
        /// <param name="v1">The <see cref="QuickFixEngineering"/> instance to subtract from.</param>
        /// <param name="v2">The <see cref="QuickFixEngineering"/> instance whose version components are subtracted.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance representing the non-negative difference between the corresponding
        /// version components of v1 and v2.
        /// </returns>
        public static QuickFixEngineering operator -(QuickFixEngineering v1, QuickFixEngineering v2) => new(Math.Max(0, v1.Version.Major - v2.Version.Major), Math.Max(0, v1.Version.Minor - v2.Version.Minor), Math.Max(0, v1.Version.Build - v2.Version.Build), Math.Max(0, v1.Version.Revision - v2.Version.Revision));

        /// <summary>
        /// Decrements the revision component of the specified <see cref="QuickFixEngineering"/> version by one.
        /// </summary>
        /// <remarks>
        /// If the revision component is already zero, the returned instance will also have a revision of zero. Other version
        /// components remain unchanged.
        /// </remarks>
        /// <param name="v">The <see cref="QuickFixEngineering"/> instance whose revision component is to be decremented.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the same major, minor, and build components as the input, and the
        /// revision component decreased by one, or zero if the revision is already zero.
        /// </returns>
        public static QuickFixEngineering operator --(QuickFixEngineering v) => new(v.Version.Major, v.Version.Minor, v.Version.Build, Math.Max(0, v.Version.Revision - 1));

        /// <summary>
        /// Determines whether two <see cref="QuickFixEngineering"/> instances are not equal.
        /// </summary>
        /// <remarks>
        /// This operator performs a value-based comparison if both operands are non-null. If either operand is null, it returns
        /// true unless both are null.
        /// </remarks>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>true if the specified instances are not equal; otherwise, false.</returns>
        public static bool operator !=(QuickFixEngineering v1, QuickFixEngineering v2) => !v1.Equals(v2);

        /// <summary>
        /// Adds the version components of two <see cref="QuickFixEngineering"/> instances.
        /// </summary>
        /// <remarks>
        /// Each version component (Major, Minor, Build, Revision) is added separately. This operation does not modify the original instances.
        /// </remarks>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to add.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to add.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance whose version components are the sums of the corresponding components
        /// of the input instances.
        /// </returns>
        public static QuickFixEngineering operator +(QuickFixEngineering v1, QuickFixEngineering v2) => new(v1.Version.Major + v2.Version.Major, v1.Version.Minor + v2.Version.Minor, v1.Version.Build + v2.Version.Build, v1.Version.Revision + v2.Version.Revision);

        /// <summary>
        /// Increments the revision of the specified <see cref="QuickFixEngineering"/> instance.
        /// </summary>
        /// <remarks>
        /// This operator does not modify the original instance; it returns a new instance with the updated revision value.
        /// </remarks>
        /// <param name="v">The <see cref="QuickFixEngineering"/> instance to increment.</param>
        /// <returns>A new <see cref="QuickFixEngineering"/> instance with the revision incremented by one.</returns>
        public static QuickFixEngineering operator ++(QuickFixEngineering v) => IncrementRevision(v);

        /// <summary>
        /// Determines whether one <see cref="QuickFixEngineering"/> instance is less than another based on their version numbers.
        /// </summary>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>
        /// true if v1 is less than v2; otherwise, false. If either value is null, null is considered less than any non-null value.
        /// </returns>
        public static bool operator <(QuickFixEngineering v1, QuickFixEngineering v2) => v1.CompareTo(v2) < 0;

        /// <summary>
        /// Determines whether one <see cref="QuickFixEngineering"/> instance is less than or equal to another, based on their
        /// version values.
        /// </summary>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>
        /// true if the version of v1 is less than or equal to the version of v2; otherwise, false. If both values are null, returns
        /// true. If only one value is null, a null value is considered less than a non-null value.
        /// </returns>
        public static bool operator <=(QuickFixEngineering v1, QuickFixEngineering v2) => v1.CompareTo(v2) <= 0;

        /// <summary>
        /// Determines whether two specified <see cref="QuickFixEngineering"/> instances are equal.
        /// </summary>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>true if the two instances are equal or both are null; otherwise, false.</returns>
        public static bool operator ==(QuickFixEngineering v1, QuickFixEngineering v2) => v1.Equals(v2);

        /// <summary>
        /// Determines whether one <see cref="QuickFixEngineering"/> instance is greater than another based on their version numbers.
        /// </summary>
        /// <remarks>If either v1 or v2 is null, the comparison treats null as less than any non-null instance.</remarks>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>true if the version of v1 is greater than the version of v2; otherwise, false.</returns>
        public static bool operator >(QuickFixEngineering v1, QuickFixEngineering v2) => v1.CompareTo(v2) > 0;

        /// <summary>
        /// Determines whether one <see cref="QuickFixEngineering"/> instance is greater than or equal to another based on their
        /// version values.
        /// </summary>
        /// <remarks>If either v1 or v2 is null, a null instance is considered less than any non-null instance.</remarks>
        /// <param name="v1">The first <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <param name="v2">The second <see cref="QuickFixEngineering"/> instance to compare, or null.</param>
        /// <returns>true if the version of v1 is greater than or equal to the version of v2; otherwise, false.</returns>
        public static bool operator >=(QuickFixEngineering v1, QuickFixEngineering v2) => v1.CompareTo(v2) >= 0;

        /// <summary>
        /// Parses the specified string and returns a new instance of the <see cref="QuickFixEngineering"/> class that represents
        /// the parsed value.
        /// </summary>
        /// <param name="input">
        /// A string containing the value to parse. The string must be in a format recognized by the Version.Parse method.
        /// </param>
        /// <returns>A <see cref="QuickFixEngineering"/> instance that represents the value specified by input.</returns>
        public static QuickFixEngineering Parse(string input) => new(Version.Parse(input));

        /// <summary>
        /// Creates a new <see cref="QuickFixEngineering"/> instance with the specified build number, preserving the major and minor
        /// version components from the given version.
        /// </summary>
        /// <param name="version">
        /// The <see cref="QuickFixEngineering"/> instance whose major and minor version components will be used for the new instance.
        /// </param>
        /// <param name="build">  The build number to assign to the new <see cref="QuickFixEngineering"/> instance.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the specified build number and the same major and minor version
        /// components as the provided version.
        /// </returns>
        public static QuickFixEngineering SetBuild(QuickFixEngineering version, int build) => new(version.Version.Major, version.Version.Minor, build, 0);

        /// <summary>
        /// Creates a new <see cref="QuickFixEngineering"/> instance with the build number from <see cref="CreateBuild()"/>,
        /// preserving the major and minor version components from the given version.
        /// </summary>
        /// <param name="version">
        /// The <see cref="QuickFixEngineering"/> instance whose major and minor version components will be used for the new instance.
        /// </param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the build number from <see cref="CreateBuild()"/> and the same
        /// major and minor version components as the provided version.
        /// </returns>
        public static QuickFixEngineering SetBuild(QuickFixEngineering version) => SetBuild(version, version.CreateBuild());

        /// <summary>
        /// Creates a new <see cref="QuickFixEngineering"/> instance with the specified revision number, preserving the major,
        /// minor, and build version components from the given version.
        /// </summary>
        /// <param name="version"> 
        /// The <see cref="QuickFixEngineering"/> instance whose major, minor, and build version components will be used for the new instance.
        /// </param>
        /// <param name="revision">The revision number to assign to the new <see cref="QuickFixEngineering"/> instance.</param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the specified revision number and the same major, minor, and build
        /// version components as the provided version.
        /// </returns>
        public static QuickFixEngineering SetRevision(QuickFixEngineering version, int revision) => new(version.Version.Major, version.Version.Minor, version.Version.Build, revision);

        /// <summary>
        /// Creates a new <see cref="QuickFixEngineering"/> instance with the revision number from <see cref="CreateRevision()"/>,
        /// preserving the major, minor, and build version components from the given version.
        /// </summary>
        /// <param name="version">
        /// The <see cref="QuickFixEngineering"/> instance whose major, minor, and build version components will be used for the new instance.
        /// </param>
        /// <returns>
        /// A new <see cref="QuickFixEngineering"/> instance with the specified revision number and the same major, minor, and build
        /// version components as the provided version.
        /// </returns>
        public static QuickFixEngineering SetRevision(QuickFixEngineering version) => SetRevision(version, CreateRevision());

        /// <summary>
        /// Attempts to parse the specified string representation of a version into a <see cref="QuickFixEngineering"/> instance.
        /// </summary>
        /// <param name="input"> 
        /// The string containing the version information to parse. The string should be in a format recognized by Version.TryParse.
        /// Can be null.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="QuickFixEngineering"/> value equivalent to the version information
        /// contained in input, if the parse succeeded, or the default value if the parse failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the input string was successfully parsed; otherwise, false.</returns>
        public static bool TryParse(string? input, out QuickFixEngineering result)
        {
            bool success = Version.TryParse(input, out Version? version);
            result = success && version is not null ? new QuickFixEngineering(version.Major, version.Minor, version.Build, version.Revision) : new QuickFixEngineering();
            return success;
        }

        /// <summary>
        /// Attempts to parse the specified <see cref="ReadOnlySpan{T}"/> representation of a version into a <see
        /// cref="QuickFixEngineering"/> instance.
        /// </summary>
        /// <param name="input"> 
        /// The <see cref="RenamedEventHandler"/> containing the version information to parse. The string should be in a format
        /// recognized by Version.TryParse.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="QuickFixEngineering"/> value equivalent to the version information
        /// contained in input, if the parse succeeded, or the default value if the parse failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the input <see cref="ReadOnlySpan{T}"/> was successfully parsed; otherwise, false.</returns>
        public static bool TryParse(ReadOnlySpan<char> input, out QuickFixEngineering result)
        {
            bool success = Version.TryParse(input, out Version? version);
            result = success && version is not null ? new QuickFixEngineering(version.Major, version.Minor, version.Build, version.Revision) : new QuickFixEngineering();
            return success;
        }

        /// <summary>
        /// Attempts to parse the specified <see cref="ReadOnlySpan{T}"/> of <see cref="Encoding.UTF8"/><see cref="byte"/>
        /// representation of a version into a <see cref="QuickFixEngineering"/> instance.
        /// </summary>
        /// <param name="utf8Input">
        /// The <see cref="RenamedEventHandler"/> containing the version information to parse. The string should be in a format
        /// recognized by Version.TryParse.
        /// </param>
        /// <param name="result">   
        /// When this method returns, contains the <see cref="QuickFixEngineering"/> value equivalent to the version information
        /// contained in input, if the parse succeeded, or the default value if the parse failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the input <see cref="ReadOnlySpan{T}"/> was successfully parsed; otherwise, false.</returns>
        public static bool TryParse(ReadOnlySpan<byte> utf8Input, out QuickFixEngineering result)
        {
            return TryParse(Encoding.UTF8.GetString(utf8Input), out result);
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new QuickFixEngineering(this.Version)
            {
                Caption = this.Caption,
                ComputerName = this.ComputerName,
                Description = this.Description,
                DisplayName = this.DisplayName,
                FixComments = this.FixComments,
                HotFixId = this.HotFixId,
                InstallDate = this.InstallDate,
                InstalledBy = this.InstalledBy,
                InstalledOn = this.InstalledOn,
                Name = this.Name,
                ServicePackInEffect = this.ServicePackInEffect,
                Status = this.Status,
                ZeroDay = this.ZeroDay
            };
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return 0;
            }
            else if (obj is null)
            {
                return 1;
            }
            else if (obj is QuickFixEngineering other)
            {
                return this.CompareTo(other);
            }
            else if (obj is DateTime zeroDay)
            {
                return this.ZeroDay.CompareTo(zeroDay);
            }
            else if (obj is Version version)
            {
                return this.Version.CompareTo(version);
            }
            else if (obj is QuickFixEngineeringStatus status)
            {
                return this.Status.CompareTo(status);
            }
            else if (obj is string text)
            {
                var resultHotFixId = string.Compare(text, this.HotFixId, StringComparison.OrdinalIgnoreCase);

                if (resultHotFixId != 0)
                {
                    return resultHotFixId;
                }

                var resultServicePackInEffect = string.Compare(text, this.ServicePackInEffect, StringComparison.OrdinalIgnoreCase);

                if (resultServicePackInEffect != 0)
                {
                    return resultServicePackInEffect;
                }

                var versionString = this.ToString();

                if (!string.Equals(text, versionString, StringComparison.Ordinal))
                {
                    return string.CompareOrdinal(text, versionString);
                }
            }
            else
            {
                throw new ArgumentException($"Parameter {nameof(obj)} is not a {this.GetType().Name} instance.", nameof(obj));
            }

            return 0;
        }

        /// <inheritdoc/>
        public int CompareTo(QuickFixEngineering? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            else if (other is null)
            {
                return 1;
            }
            if (this.ZeroDay != other.ZeroDay)
            {
                return this.ZeroDay.CompareTo(other.ZeroDay);
            }
            else if (this.Version != other.Version)
            {
                return this.Version.CompareTo(other.Version);
            }
            else if (this.HotFixId != other.HotFixId)
            {
                return string.Compare(this.HotFixId, other.HotFixId, StringComparison.OrdinalIgnoreCase);
            }
            else if (this.ServicePackInEffect != other.ServicePackInEffect)
            {
                return string.Compare(this.ServicePackInEffect, other.ServicePackInEffect, StringComparison.OrdinalIgnoreCase);
            }
            else if (this.Status != other.Status)
            {
                return this.Status.CompareTo(other.Status);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Creates the build.
        /// </summary>
        /// <returns></returns>
        public short CreateBuild() => Convert.ToInt16(DateTime.UtcNow.Subtract(this.ZeroDay).TotalDays);

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is QuickFixEngineering other && this.Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(QuickFixEngineering? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            else if (other is null)
            {
                return false;
            }
            else if (this.HotFixId != other.HotFixId)
            {
                return false;
            }
            else if (this.InstallDate != other.InstallDate)
            {
                return false;
            }
            else if (this.ServicePackInEffect != other.ServicePackInEffect)
            {
                return false;
            }
            else if (this.Status != other.Status)
            {
                return false;
            }
            else if (this.ZeroDay != other.ZeroDay)
            {
                return false;
            }
            else
            {
                return this.Version == other.Version;
            }
        }

        /// <inheritdoc/>
        public bool Equals(QuickFixEngineering? x, QuickFixEngineering? y)
        {
            return DefaultComparer.Equals(x, y);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.Version, this.HotFixId, this.InstallDate, this.ServicePackInEffect, this.Status, this.ZeroDay);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">Specifies the <see cref="QuickFixEngineering"/> object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Required by IEqualityCompare{T} in this signature.")]
        public int GetHashCode([DisallowNull] QuickFixEngineering obj) => obj.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => this.ToString(4);

        /// <inheritdoc/>
        public string ToString(int fieldCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(fieldCount, nameof(fieldCount));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(fieldCount, 4, nameof(fieldCount));

            return this.Version.ToString(fieldCount);
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            const string NO_SERVICE_PACK_APPLIED = "No Service Pack Applied";

#pragma warning disable CS8509 // FALSE POSITIVE:  The switch expression DOES handle all possible values of its input type.
            return string.IsNullOrWhiteSpace(format)
                ? string.Empty
                : format.Trim().ToUpperInvariant() switch
                {
                    "B" => this.ToString(3),
                    "F" => this.ToString(4),
                    "G" => this.ToString(4),
                    "H" => string.Format(formatProvider ?? CultureInfo.CurrentCulture, "{0} [{1}]", this.ToString(4), this.HotFixId),
                    "M" => this.ToString(1),
                    "N" => this.ToString(2),
                    "R" => this.ToString(4),
                    "S" => string.Format(formatProvider ?? CultureInfo.CurrentCulture, "{0} << {1} >>", this.ToString(4), this.ServicePackInEffect ?? NO_SERVICE_PACK_APPLIED),
                    _ => throw new FormatException($"Parameter {nameof(format)} with value '{format}' is not supported."),
                };
#pragma warning restore CS8509 // FALSE POSITIVE:  The switch expression DOES handle all possible values of its input type.
        }

        /// <inheritdoc/>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => ((ISpanFormattable)this.Version).TryFormat(destination, out charsWritten, format, provider);

        /// <inheritdoc/>
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => ((IUtf8SpanFormattable)this.Version).TryFormat(utf8Destination, out bytesWritten, format, provider);
    }
}
