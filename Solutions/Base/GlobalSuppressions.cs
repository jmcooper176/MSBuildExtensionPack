// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.Empty~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.Reverse(System.Text.StringBuilder)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.Where(System.Text.StringBuilder,System.Func{System.Char,System.Boolean})~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.TakeWhile(System.Text.StringBuilder,System.Func{System.Char,System.Int32,System.Boolean})~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.TakeWhile(System.Text.StringBuilder,System.Func{System.Char,System.Boolean})~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.Where(System.Text.StringBuilder,System.Func{System.Char,System.Int32,System.Boolean})~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithDirectories(System.IO.DirectoryInfo,System.String,System.IO.EnumerationOptions)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithDirectories(System.IO.DirectoryInfo,System.String,System.IO.SearchOption)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithDirectories(System.IO.DirectoryInfo)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithFiles(System.IO.DirectoryInfo,System.String,System.IO.EnumerationOptions)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithFiles(System.IO.DirectoryInfo)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.CreateWithFiles(System.IO.DirectoryInfo,System.String,System.IO.SearchOption)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.DefaultIfEmpty(System.Text.StringBuilder)~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.PrependJoin(System.Text.StringBuilder,System.String,System.Collections.Generic.IEnumerable{Microsoft.Build.Framework.ITaskItem})~System.Text.StringBuilder")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "False positive as capacity is not be formatted; it is being passed.", Scope = "member", Target = "~M:MSBuild.ExtensionPack.Base.Extension.StringBuilderExtension.Create(System.Collections.Generic.ICollection{System.Char},System.Int32)~System.Text.StringBuilder")]
