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
    using System.Management.Automation;
    using System.Text.RegularExpressions;

    using MSBuild.ExtensionPack.Base.Enumeration;
    using MSBuild.ExtensionPack.Base.Interface;

    public partial class Semantic : IVersionMethod, IComparable<Semantic>, IEquatable<Semantic>, IEqualityComparer<Semantic>
    {
        #region Private Methods

        /// <summary>
        /// Defines the <see cref="Semantic"/> build metadata label regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Semantic"/> build metadata label regular expression.</returns>
        [GeneratedRegex(@"^(?:<buildmetadata>[0-9A-Z\-]+(?:\.[0-9A-Z\-]+)*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex LabelUnitRegex();

        /// <summary>
        /// Defines the <see cref="Semantic"/> pre-release label regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Semantic"/> pre-release label regular expression.</returns>
        [GeneratedRegex(@"^(?:<prerelease>(?:0|[1-9]\d*|\d*[A-Z\-][0-9A-Z\-]*)(?:\.(?:0|[1-9]\d*|\d*[A-Z\-][0-9A-Z\-]*))*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex PreReleaseLabelRegex();

        /// <summary>
        /// Defines the <see cref="Semantic"/> version string regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Semantic"/> version string regular expression.</returns>
        [GeneratedRegex(@"^(?:<major>0|[1-9]\d*)\.(?:<minor>0|[1-9]\d*)\.(?:<patch>0|[1-9]\d*)(?:-(?:<prerelease>(?:0|[1-9]\d*|\d*[A-Z\-][0-9A-Z\-]*)(?:\.(?:0|[1-9]\d*|\d*[A-Z\-][0-9A-Z\-]*))*))?(?:\+(?:<buildmetadata>[0-9A-Z\-]+(?:\.[0-9A-Z\-]+)*))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex SemanticVersionRegex();

        /// <summary>
        /// Defines the <see cref="Semantic"/> version number part regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Semantic"/> version number part regular expression.</returns>
        [GeneratedRegex(@"^(?:<major>0|[1-9]\d*)\.(?:<minor>0|[1-9]\d*)\.(?:<patch>0|[1-9]\d*)$", RegexOptions.Compiled)]
        private static partial Regex VersionNumberRegex();

        #endregion Private Methods

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        protected Semantic()
        {
            this.Caption = "SemVer 2.0 Version Number";
            this.ComputerName = Environment.MachineName;
            this.DefaultComparer = EqualityComparer<Semantic>.Create((x, y) => x is not null ? x.Equals(y) : y is null, this.GetHashCode);
            this.Description = "PowerShell SemVer 2.0 Version Number";
            this.DisplayName = "SemVer";
            this.FixComments = string.Empty;
            this.HotFixId = "HotFix_0";
            this.InstallDate = DateTime.UtcNow;
            this.InstalledBy = Environment.UserName;
            this.InstalledOn = InstallDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Name = "Semantic Version Number";
            this.SemanticVersion = new(0, 0, 0);
            this.ServicePackInEffect = null;
            this.Status = QuickFixEngineeringStatus.Ok;
            this.Version = new Version();
            this.ZeroDay = new(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        #endregion Protected Constructors

        #region Public Fields

        /// <summary>
        /// The maximum major version number supported by <see cref="Semantic"/>.
        /// </summary>
        public const int MAX_MAJOR = 2147483647;

        /// <summary>
        /// The maximum minor version number supported by <see cref="Semantic"/>.
        /// </summary>
        public const int MAX_MINOR = 2147483647;

        /// <summary>
        /// The maximum patch version number supported by <see cref="Semantic"/>.
        /// </summary>
        public const int MAX_PATCH = 2147483647;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="major"/> is negative.</exception>
        public Semantic(int major)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));

            this.SemanticVersion = new(major);
            this.Version = new(major % QuickFixEngineering.MAX_MAJOR, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <exception cref="ArgumentException">
        /// Throws if <paramref name="version"/> is not a valid <see cref="SemanticVersion"/> string.
        /// </exception>
        public Semantic(string version)
            : this()
        {
            if (!(SemanticVersionRegex().IsMatch(version) || VersionNumberRegex().IsMatch(version)))
            {
                throw new ArgumentException($"Parameter {nameof(version)} is not a valid System.Management.Automation.SemanticVersion string.", nameof(version));
            }

            this.SemanticVersion = new(version);
            this.Version = new(this.SemanticVersion.Major % QuickFixEngineering.MAX_MAJOR, this.SemanticVersion.Minor % QuickFixEngineering.MAX_MINOR, this.SemanticVersion.Patch % QuickFixEngineering.MAX_BUILD);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="version">Specifies the <see cref="Version"/>.</param>
        public Semantic(Version version)
            : this()
        {
            this.SemanticVersion = new(version);
            this.Version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        /// <param name="minor">Specifies the minor version number.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="major"/> or <paramref name="minor"/> is negative.
        /// </exception>
        public Semantic(int major, int minor)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));

            this.SemanticVersion = new(major, minor);
            this.Version = new(major % QuickFixEngineering.MAX_MAJOR, minor % QuickFixEngineering.MAX_MINOR);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        /// <param name="minor">Specifies the minor version number.</param>
        /// <param name="patch">Specifies the patch version number.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="major"/>, <paramref name="minor"/> or <paramref name="patch"/> is negative.
        /// </exception>
        public Semantic(int major, int minor, int patch)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(patch, nameof(patch));

            this.SemanticVersion = new(major, minor, patch);
            this.Version = new(major % QuickFixEngineering.MAX_MAJOR, minor % QuickFixEngineering.MAX_MINOR, patch % QuickFixEngineering.MAX_BUILD);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="major">Specifies the major version number.</param>
        /// <param name="minor">Specifies the minor version number.</param>
        /// <param name="patch">Specifies the patch version number.</param>
        /// <param name="label">Specifies the build metadata label.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="major"/>, <paramref name="minor"/> or <paramref name="patch"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throws if <paramref name="label"/> is not a valid <see cref="SemanticVersion"/> build metadata label.
        /// </exception>
        public Semantic(int major, int minor, int patch, [System.Diagnostics.CodeAnalysis.AllowNull] string label)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(patch, nameof(patch));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(label, nameof(label));

            if (!LabelUnitRegex().IsMatch(label))
            {
                throw new ArgumentException($"Parameter {nameof(label)} is not a valid System.Management.Automation.SemanticVersion label");
            }

            this.SemanticVersion = new(major, minor, patch, label);
            this.Version = new(major % QuickFixEngineering.MAX_MAJOR, minor % QuickFixEngineering.MAX_MINOR, patch % QuickFixEngineering.MAX_BUILD);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semantic"/> class.
        /// </summary>
        /// <param name="major">          The major.</param>
        /// <param name="minor">          The minor.</param>
        /// <param name="patch">          The patch.</param>
        /// <param name="preReleaseLabel">The pre release label.</param>
        /// <param name="buildLabel">     The build label.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if <paramref name="major"/>, <paramref name="minor"/> or <paramref name="patch"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throws if <paramref name="preReleaseLabel"/> or <paramref name="buildLabel"/> contains invalid characters.
        /// </exception>
        public Semantic(int major, int minor, int patch, string preReleaseLabel, string buildLabel)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(patch, nameof(patch));

            if (!PreReleaseLabelRegex().IsMatch(preReleaseLabel))
            {
                throw new ArgumentException($"Parameter {nameof(preReleaseLabel)} contains invalid characters.", nameof(preReleaseLabel));
            }

            if (!LabelUnitRegex().IsMatch(buildLabel))
            {
                throw new ArgumentException($"Parameter {nameof(buildLabel)} contains invalid characters.", nameof(buildLabel));
            }

            this.SemanticVersion = new(major, minor, patch, preReleaseLabel, buildLabel);
            this.Version = new(major % QuickFixEngineering.MAX_MAJOR, minor % QuickFixEngineering.MAX_MINOR, patch % QuickFixEngineering.MAX_BUILD);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <inheritdoc/>
        public string Caption { get; set; }

        /// <inheritdoc/>
        public string ComputerName { get; set; }

        /// <inheritdoc/>
        public EqualityComparer<Semantic> DefaultComparer { get; set; }

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

        public SemanticVersion SemanticVersion { get; }

        /// <inheritdoc/>
        public string? ServicePackInEffect { get; set; }

        /// <inheritdoc/>
        public QuickFixEngineeringStatus Status { get; set; }

        /// <inheritdoc/>
        public Version Version { get; }

        /// <inheritdoc/>
        public DateTime ZeroDay { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static explicit operator Semantic(SemanticVersion semanticVersion) => new(semanticVersion.Major, semanticVersion.Minor, semanticVersion.Patch, semanticVersion.PreReleaseLabel ?? string.Empty, semanticVersion.BuildLabel ?? string.Empty);

        public static explicit operator Semantic(Version version) => new(version);

        public static explicit operator Semantic(string version) => new(version);

        public static implicit operator SemanticVersion(Semantic semantic) => semantic.SemanticVersion;

        public static implicit operator string(Semantic semantic) => semantic.SemanticVersion.ToString();

        public static implicit operator Version(Semantic semantic) => semantic.Version;

        public static Semantic operator --(Semantic semantic) => new(semantic.SemanticVersion.Major, semantic.SemanticVersion.Minor, Math.Max(0, semantic.SemanticVersion.Patch - 1), semantic.SemanticVersion.PreReleaseLabel ?? string.Empty, semantic.SemanticVersion.BuildLabel ?? string.Empty);

        public static bool operator !=(Semantic? left, Semantic? right) => !(left == right);

        public static Semantic operator ++(Semantic semantic) => new(semantic.SemanticVersion.Major, semantic.SemanticVersion.Minor, semantic.SemanticVersion.Patch + 1, semantic.SemanticVersion.PreReleaseLabel ?? string.Empty, semantic.SemanticVersion.BuildLabel ?? string.Empty);

        public static bool operator <(Semantic? left, Semantic? right) => left is null ? right is not null : left.CompareTo(right) < 0;

        public static bool operator <=(Semantic? left, Semantic? right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator ==(Semantic? left, Semantic? right) => left is not null ? left.Equals(right) : right is null;

        public static bool operator >(Semantic? left, Semantic? right) => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(Semantic? left, Semantic? right) => left is null ? right is null : left.CompareTo(right) >= 0;

        public Semantic ClearLabels() => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch);

        /// <inheritdoc/>
        public object Clone() => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(object? obj) => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(Semantic? other)
        {
            if (other is null)
            {
                return 1;
            }
            else if (ReferenceEquals(this, other))
            {
                return 0;
            }
            else if (this.ZeroDay != other.ZeroDay)
            {
                return this.ZeroDay.CompareTo(other.ZeroDay);
            }
            else if (this.SemanticVersion == other.SemanticVersion && this.Version == other.Version)
            {
                return 0;
            }
            else if (this.SemanticVersion != other.SemanticVersion)
            {
                return this.SemanticVersion.CompareTo(other.SemanticVersion);
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

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Semantic other && this.Equals(other);

        /// <inheritdoc/>
        public bool Equals(Semantic? other) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool Equals(Semantic? x, Semantic? y) => this.DefaultComparer.Equals(x, y);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.SemanticVersion, this.Version, this.HotFixId, this.InstallDate, this.ServicePackInEffect, this.Status, this.ZeroDay);

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Semantic obj) => obj.GetHashCode();

        public Semantic NextMajor() => new(this.SemanticVersion.Major + 1, 0, 0);

        public Semantic NextMinor() => new(this.SemanticVersion.Major, this.SemanticVersion.Minor + 1, 0);

        public Semantic NextPatch() => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch + 1);

        /// <inheritdoc/>
        public string ToString(int fieldCount) => throw new NotImplementedException();

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();

        public override string ToString() => this.SemanticVersion.ToString();

        /// <inheritdoc/>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        public Semantic WithBuildMetadata(string buildMetadata) => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch, this.SemanticVersion.PreReleaseLabel ?? string.Empty, buildMetadata);

        public Semantic WithoutBuildMetadata() => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch, this.SemanticVersion.PreReleaseLabel ?? string.Empty, string.Empty);

        public Semantic WithoutPreReleaseLabel() => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch, string.Empty, this.SemanticVersion.BuildLabel ?? string.Empty);

        public Semantic WithPreReleaseLabel(string preReleaseLabel) => new(this.SemanticVersion.Major, this.SemanticVersion.Minor, this.SemanticVersion.Patch, preReleaseLabel, this.SemanticVersion.BuildLabel ?? string.Empty);

        #endregion Public Methods
    }
}
