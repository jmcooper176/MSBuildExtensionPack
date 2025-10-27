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
namespace FileSystem.Version
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class Windows : IVersionMethod, IComparable<Windows>, IEquatable<Windows>, IEqualityComparer<Windows>
    {
        /// <inheritdoc/>
        public string Caption { get; set; }

        /// <inheritdoc/>
        public string ComputerName { get; set; }

        public IEqualityComparer<Windows> DefaultComparer => throw new NotImplementedException();

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

        /// <inheritdoc/>
        public object Clone() => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(object? obj) => throw new NotImplementedException();

        /// <inheritdoc/>
        public int CompareTo(Windows? other) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override bool Equals(object? obj) => base.Equals(obj);

        /// <inheritdoc/>
        public bool Equals(Windows? other) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool Equals(Windows? x, Windows? y) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Windows obj) => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string ToString() => throw new NotImplementedException();

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
