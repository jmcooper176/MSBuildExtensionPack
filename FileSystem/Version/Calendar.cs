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

    public class Calendar : IVersionMethod, IComparable<Calendar>, IEquatable<Calendar>, IEqualityComparer<Calendar>
    {
        public string Caption { get; set; }
        public string ComputerName { get; set; }

        public IEqualityComparer<Calendar> DefaultComparer => throw new NotImplementedException();

        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string FixComments { get; set; }
        public string HotFixId { get; set; }
        public DateTime InstallDate { get; set; }
        public string InstalledBy { get; set; }
        public string InstalledOn { get; set; }
        public string Name { get; set; }
        public string? ServicePackInEffect { get; set; }
        public QuickFixEngineeringStatus Status { get; set; }
        public Version Version { get; }
        public DateTime ZeroDay { get; set; }

        public object Clone() => throw new NotImplementedException();

        public int CompareTo(object? obj) => throw new NotImplementedException();

        public int CompareTo(Calendar? other) => throw new NotImplementedException();

        public override bool Equals(object? obj) => base.Equals(obj);

        public bool Equals(Calendar? other) => throw new NotImplementedException();

        public bool Equals(Calendar? x, Calendar? y) => throw new NotImplementedException();

        public override int GetHashCode() => base.GetHashCode();

        public int GetHashCode([DisallowNull] Calendar obj) => throw new NotImplementedException();

        public override string ToString() => throw new NotImplementedException();

        public string ToString(int fieldCount) => throw new NotImplementedException();

        public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
    }
}
