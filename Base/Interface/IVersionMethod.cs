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
namespace MSBuild.ExtensionPack.Base.Interface
{
    using System;

    using MSBuild.ExtensionPack.Base.Enumeration;

    /// <summary>
    /// Interface for use by all versioning methods.
    /// </summary>
    /// <seealso cref="ICloneable"/>
    /// <seealso cref="IComparable"/>
    /// <seealso cref="IFormattable"/>
    /// <seealso cref="ISpanFormattable"/>
    /// <seealso cref="IUtf8SpanFormattable"/>
    public interface IVersionMethod : ICloneable, IComparable, IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        /// <summary>
        /// Gets or sets a value indicating the caption for this instance.
        /// </summary>
        /// <value>The <see cref="string"/> caption.</value>
        string Caption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of the computer associated with this instance.
        /// </summary>
        string ComputerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the description associated with the current instance.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the display name associated with the current instance.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating any comments associated with the current instance.
        /// </summary>
        string FixComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the unique identifier of the applied hot fix for this instance.
        /// </summary>
        string HotFixId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the date and time when the hot fix was installed for this instance.
        /// </summary>
        DateTime InstallDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name of the user or process that performed the installation for this instance.
        /// </summary>
        string InstalledBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the formatted string value of <see cref="InstallDate"/> for this instance.
        /// </summary>
        string InstalledOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name associated with the current instance.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the name or identifier of the service pack currently applied for this instance, if any.
        /// </summary>
        string? ServicePackInEffect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the current <see cref="QuickFixEngineeringStatus"/> status.
        /// </summary>
        QuickFixEngineeringStatus Status { get; set; }

        /// <summary>
        /// Gets a value indicating the internal <see cref="Version"/> value.
        /// </summary>
        /// <value>The <see cref="Version"/>.</value>
        Version Version { get; }

        /// <summary>
        /// Gets or sets the reference date used as the origin point for calculations or time measurements.
        /// </summary>
        DateTime ZeroDay { get; set; }

        /// <summary>
        /// Returns a string that represents this instance, formatted to include the specified number of version fields.
        /// </summary>
        /// <param name="fieldCount">Specifies the field count.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="fieldCount"/> is out of range.</exception>
        string ToString(int fieldCount);
    }
}
