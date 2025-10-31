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

    using MSBuild.ExtensionPack.Base.Interface;

    public partial class Apple : IVersionMethod, IComparable<Apple>, IEquatable<Apple>, IEqualityComparer<Apple>
    {
        /// <summary>
        /// Defines the <see cref="Apple"/> build number regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Apple"/> build number regular expression.</returns>
        [GeneratedRegex(@"^(?:<buildmajor>0|[1-9]\d*)(?:<releasechar>[A-Z]{1})(?:<revision>0|[1-9]\d*)$", RegexOptions.Compiled)]
        private static partial Regex BuildNumberRegex();

        /// <summary>
        /// Defines the <see cref="Semantic"/> version number part regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> representing the <see cref="Semantic"/> version number part regular expression.</returns>
        [GeneratedRegex(@"^(?:<major>0|[1-9]\d*)\.(?:<minor>0|[1-9]\d*)\.(?:<patch>0|[1-9]\d*)(\s+(?:<buildnumber>0|[1-9]\d*[A-Z]{1}0|[1-9]\d*))?$", RegexOptions.Compiled)]
        private static partial Regex VersionNumberRegex();

        protected Apple()
        {
            this.BuildNumber ??= CreateBuildNumber(0, "A", 0);
            this.Caption = "Apple Version Number";
            this.ComputerName = Environment.MachineName;
            this.DefaultComparer = EqualityComparer<Apple>.Create((x, y) => x is not null ? x.Equals(y) : y is null, this.GetHashCode);
            this.Description = "Apple Operating System or Product Version Number"; ;
            this.DisplayName = "Version";
            this.FixComments = string.Empty;
            this.HotFixId = "HotFix_0";
            this.InstallDate = DateTime.UtcNow;
            this.InstalledBy = Environment.UserName;
            this.InstalledOn = InstallDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Name = "Version Number";
            this.OperatingSystem = AppleOs.MacOS;
            this.ServicePackInEffect = null;
            this.Status = QuickFixEngineeringStatus.Ok;
            this.Version = new Version(0, 0, 0);
            this.ZeroDay = new(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// The maximum major version number supported by <see cref="Apple"/>.
        /// </summary>
        public const int MAX_MAJOR = 65534;

        /// <summary>
        /// The maximum minor version number supported by <see cref="Apple"/>.
        /// </summary>
        public const int MAX_MINOR = 65534;

        /// <summary>
        /// The maximum patch version number supported by <see cref="Apple"/>.
        /// </summary>
        public const int MAX_PATCH = 65534;

        public Apple(int major)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));

            this.Version = new(major, 0, 0);
            this.BuildNumber = IncrementBuildNumber(this.BuildNumber, AppleBuildNumberPart.Major);
        }

        public Apple(int major, int minor)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));

            this.Version = new(major, minor, 0);
            this.BuildNumber = IncrementBuildNumber(this.BuildNumber, AppleBuildNumberPart.Major | AppleBuildNumberPart.ReleaseChar);
        }

        public Apple(int major, int minor, int patch)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(patch, nameof(patch));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(patch, MAX_PATCH, nameof(patch));

            this.Version = new(major, minor, patch);
            this.BuildNumber = IncrementBuildNumber(this.BuildNumber, AppleBuildNumberPart.All);
        }

        public Apple(int major, int minor, int patch, [AllowNull] string buildNumber)
            : this()
        {
            ArgumentOutOfRangeException.ThrowIfNegative(major, nameof(major));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(major, MAX_MAJOR, nameof(major));
            ArgumentOutOfRangeException.ThrowIfNegative(minor, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(minor, MAX_MINOR, nameof(minor));
            ArgumentOutOfRangeException.ThrowIfNegative(patch, nameof(patch));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(patch, MAX_PATCH, nameof(patch));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(buildNumber, nameof(buildNumber));

            if (!BuildNumberRegex().IsMatch(buildNumber))
            {
                throw new ArgumentException($"Parameter {nameof(buildNumber)} is not a valid Apple build number.");
            }

            this.Version = new(major, minor, patch);
            this.BuildNumber = IncrementBuildNumber(buildNumber);
        }

        public Apple([AllowNull] string version)
            : this()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(version, nameof(version));

            if (!VersionNumberRegex().IsMatch(version))
            {
                throw new ArgumentException($"Parameter {nameof(version)} is not a valid Apple version number.");
            }

            if (TryParse(version, out Apple? result))
            {
                ArgumentNullException.ThrowIfNull(result, nameof(version));

                this.Version = new Version(result.Version.Major, result.Version.Minor, result.Version.Build);
                this.BuildNumber = IncrementBuildNumber(result.BuildNumber ?? "0A0");
            }
        }

        public Apple([AllowNull] Version version)
            : this()
        {
            ArgumentNullException.ThrowIfNull(version);

            this.Version = version;
            this.BuildNumber = IncrementBuildNumber(this.BuildNumber, AppleBuildNumberPart.All);
        }

        public string BuildNumber { get; set; }

        public string Caption { get; set; }

        public string ComputerName { get; set; }

        public IEqualityComparer<Apple> DefaultComparer { get; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string FixComments { get; set; }

        public string HotFixId { get; set; }

        public DateTime InstallDate { get; set; }

        public string InstalledBy { get; set; }

        public string InstalledOn { get; set; }

        public string Name { get; set; }

        public AppleOs OperatingSystem { get; set; }

        public string? ServicePackInEffect { get; set; }

        public QuickFixEngineeringStatus Status { get; set; }

        public Version Version { get; }

        public DateTime ZeroDay { get; set; }

        public static string CreateBuildNumber(int buildMajor, string releaseChar, int revision)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(buildMajor, nameof(buildMajor));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(buildMajor, MAX_MAJOR, nameof(buildMajor));
            ArgumentOutOfRangeException.ThrowIfNegative(revision, nameof(revision));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(revision, MAX_PATCH, nameof(revision));

            if (releaseChar.Length != 1 || !char.IsAsciiLetterUpper(releaseChar[0]))
            {
                throw new ArgumentException($"Parameter {nameof(releaseChar)} must be a single ASCII upper case letter A-Z.");
            }

            return $"{buildMajor}{releaseChar}{revision}";
        }

        public static string IncrementBuildNumber(string? buildNumber, AppleBuildNumberPart incrementMask = AppleBuildNumberPart.Revision)
        {
            if (!TryParseBuildNumber(buildNumber, out int buildMajor, out string releaseChar, out int revision))
            {
                throw new ArgumentException($"Parameter {nameof(buildNumber)} is not a valid Apple build number.");
            }

            if (incrementMask.HasFlag(AppleBuildNumberPart.Revision) && revision < MAX_PATCH)
            {
                revision++;
            }

            if (incrementMask.HasFlag(AppleBuildNumberPart.ReleaseChar) && string.CompareOrdinal(releaseChar, "Z") < 0)
            {
                revision = 0;
                releaseChar = ((char)(releaseChar[0] + 1)).ToString();
            }

            if (incrementMask.HasFlag(AppleBuildNumberPart.Major))
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(buildMajor, MAX_MAJOR, nameof(buildMajor));
                releaseChar = "A";
                buildMajor++;
            }

            return CreateBuildNumber(buildMajor, releaseChar, revision);
        }

        public static bool TryParse(string version, out Apple? result)
        {
            result = null;

            Match match = VersionNumberRegex().Match(version.Trim());

            if (!match.Success)
            {
                return false;
            }

            if (!int.TryParse(match.Groups["major"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int major))
            {
                return false;
            }

            if (!int.TryParse(match.Groups["minor"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int minor))
            {
                return false;
            }

            if (!int.TryParse(match.Groups["patch"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int patch))
            {
                return false;
            }

            string? buildNumber = match.Groups["buildnumber"].Success ? match.Groups["buildnumber"].Value : null;

            result = string.IsNullOrEmpty(buildNumber) ? new Apple(major, minor, patch) : new Apple(major, minor, patch, buildNumber);

            return true;
        }

        public static bool TryParseBuildNumber(string? buildNumber, out int buildMajor, out string releaseChar, out int revision)
        {
            buildMajor = 0;
            releaseChar = "A";
            revision = 0;

            if (string.IsNullOrWhiteSpace(buildNumber))
            {
                return false;
            }

            Match match = BuildNumberRegex().Match(buildNumber.Trim());

            if (!match.Success)
            {
                return false;
            }

            if (!int.TryParse(match.Groups["buildmajor"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out buildMajor))
            {
                return false;
            }

            releaseChar = match.Groups["releasechar"].Value;

            if (!int.TryParse(match.Groups["revision"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out revision))
            {
                return false;
            }

            return true;
        }

        public object Clone() => throw new NotImplementedException();

        public int CompareTo(object? obj) => throw new NotImplementedException();

        public int CompareTo(Apple? other) => throw new NotImplementedException();

        public override bool Equals(object? obj) => throw new NotImplementedException();

        public bool Equals(Apple? other) => throw new NotImplementedException();

        public bool Equals(Apple? x, Apple? y) => throw new NotImplementedException();

        public override int GetHashCode() => base.GetHashCode();

        public int GetHashCode([DisallowNull] Apple obj) => throw new NotImplementedException();

        public override string ToString() => ToString(4);

        public string ToString(int fieldCount)
        {
            switch (fieldCount)
            {
                case 0:
                    return string.Empty;

                case 3:
                    return $"{this.Version.ToString(3)}";

                case 4:
                    return $"{this.Version.ToString(3)} ({this.BuildNumber})";

                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldCount), fieldCount, $"Parameter {nameof(fieldCount)} must be between 0, 3, or 4.");
            }
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            const string NO_SERVICE_PACK_APPLIED = "No Service Pack Applied";

            return string.IsNullOrWhiteSpace(format)
                ? string.Empty
                : format.Trim().ToUpperInvariant() switch
                {
                    "B" => this.ToString(4),
                    "H" => string.Format(formatProvider ?? CultureInfo.CurrentCulture, "{0} [{1}]", this.ToString(4), this.HotFixId),
                    "S" => string.Format(formatProvider ?? CultureInfo.CurrentCulture, "{0} << {1} >>", this.ToString(4), this.ServicePackInEffect ?? NO_SERVICE_PACK_APPLIED),
                    "V" => this.Version.ToString(3),
                    _ => string.Empty,
                };
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
    }
}
