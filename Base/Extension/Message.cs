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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

using MSBuild.ExtensionPack.Base.Enumeration;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public static class Message
    {
        #region Public Methods

        public static string FormatDebug(
            IFormatProvider? provider,
            [AllowNull] string message,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: "DEBUG",
                message: string.Format(
                    provider ?? CultureInfo.CurrentCulture,
                    "{0} Details => No Details",
                    message));
        }

        public static string FormatDebug(
            IFormatProvider? provider,
            [AllowNull] string message,
            [AllowNull] string detailedMessage,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(detailedMessage, nameof(detailedMessage));

            string detailedMsg = string.Format(
                provider ?? CultureInfo.CurrentCulture,
                detailedMessage,
                arguments);
            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: "DEBUG",
                message: string.Format(
                    provider ?? CultureInfo.CurrentCulture,
                    "{0} Details => {1}",
                    message,
                    detailedMsg));
        }

        public static string FormatError(
                            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            WinError code)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : ERROR {1} 0x{2:X8} : {3}", origin, code, (int)code, message);
        }

        public static string FormatError(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            HResult code)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : ERROR {1} 0x{2:X8} : {3}", origin, code, (int)code, message);
        }

        public static string FormatError(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            int exitCode)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : ERROR EXIT {1}|0x{1:X8} : {2}", origin, exitCode, message);
        }

        public static string FormatError<TException>(
            IFormatProvider? provider,
            [AllowNull] string origin,
            string? message,
            TException exception) where TException : Exception
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : EXCEPTION {1} {2}|0x{2:X8} : {3}", origin, exception.GetType().Name, exception.HResult, message ?? exception.Message);
        }

        public static string FormatFatal(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            WinError code)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : FATAL ERROR {1} 0x{2:X8} : {3}", origin, code, (int)code, message);
        }

        public static string FormatFatal(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            HResult code)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : FATAL ERROR {1} 0x{2:X8} : {3}", origin, code, (int)code, message);
        }

        public static string FormatFatal(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            int exitCode)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : FATAL EXIT {1}|0x{1:X8} : {2}", origin, exitCode, message);
        }

        public static string FormatFatal<TException>(
            IFormatProvider? provider,
            [AllowNull] string origin,
            string? message,
            TException exception) where TException : Exception
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : FATAL EXCEPTION {1} {2}|0x{2:X8} : {3}", origin, exception.GetType().Name, exception.HResult, message ?? exception.Message);
        }

        public static string FormatMessage(
                                                                                            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string metadata,
            [AllowNull] string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(metadata, nameof(metadata));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : {1} : {2}", origin, metadata, message);
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : {1}", origin, message);
        }

        public static string FormatMetadata(
            IFormatProvider? provider,
            [AllowNull] string format,
            params object?[] arguments)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(format, nameof(format));

            return string.Format(
                provider ?? CultureInfo.CurrentCulture,
                format,
                arguments);
        }

        public static string FormatOrigin(
                    [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            int CountOrigin()
            {
                int count = 0;

                if (lineNumber > 0)
                {
                    count++;
                }

                if (columnNumber > 0)
                {
                    count++;
                }

                if (endLineNumber > 0)
                {
                    count++;
                }

                if (endColumnNumber > 0)
                {
                    count++;
                }

                return count;
            }

            StringBuilder buffer = StringBuilderExtension.Create();

            buffer.Append('[').Append(DateTime.UtcNow.ToString("s")).Append(']').Append(' ').Append(filePath);

            switch (CountOrigin())
            {
                case 0:
                    buffer.Append('(').Append(')').Append(" => ").Append(memberName).Append(" : ");
                    return buffer.ToString();

                case 1:
                    buffer.Append(filePath).Append('(').Append(lineNumber).Append(')');
                    buffer.Append(" => ").Append(memberName).Append(" : ");
                    return buffer.ToString();

                case 2:
                    if (columnNumber > 0)
                    {
                        buffer.Append('(').Append(lineNumber).Append(", ").Append(columnNumber).Append(')');
                    }
                    else if (endLineNumber > 0)
                    {
                        buffer.Append('(').Append(lineNumber).Append('-').Append(endLineNumber).Append(')');
                    }
                    else
                    {
                        if (lineNumber == 0 && columnNumber == 0)
                        {
                            throw new ArgumentException($"No message (line, column) coordinates passed for message.", nameof(lineNumber));
                        }
                        else if (lineNumber == 0 && endLineNumber == 0)
                        {
                            throw new ArgumentException($"No message (line-endLine) coordinates passed for message.", nameof(endLineNumber));
                        }
                        else
                        {
                            throw new NotSupportedException($"Unsupported two-argument message (f, s) origin.");
                        }
                    }

                    buffer.Append(" => ").Append(memberName).Append(" : ");
                    return buffer.ToString();

                case 3:
                    if (lineNumber > 0 && columnNumber > 0 && endColumnNumber > 0)
                    {
                        buffer.Append('(').Append(lineNumber).Append(", ").Append(columnNumber).Append('-').Append(endColumnNumber).Append(')');
                        buffer.Append(" => ").Append(memberName).Append(" : ");
                        return buffer.ToString();
                    }
                    else
                    {
                        throw new NotSupportedException($"Unsupported three-argument message (f, s-t) origin.");
                    }

                default:
                    buffer.Append('(').Append(lineNumber).Append(", ").Append(columnNumber).Append(", ").Append(endLineNumber).Append(", ").Append(endColumnNumber).Append(')');
                    buffer.Append(" => ").Append(memberName).Append(" : ");
                    return buffer.ToString();
            }
        }

        public static string FormatVerbose(
                    IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            return string.Format(provider ?? CultureInfo.CurrentCulture, "{0} : VERBOSE : {1}", origin, message);
        }

        public static string FormatWarning(
            IFormatProvider? provider,
            [AllowNull] string warningMessage,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(warningMessage, nameof(warningMessage));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: "WARNING",
                message: warningMessage);
        }

        public static string FormatWarning(
            IFormatProvider? provider,
            [AllowNull] string warningMessage,
            WinError code,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(warningMessage, nameof(warningMessage));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: string.Format(provider ?? CultureInfo.CurrentCulture, "WARNING {0}", code),
                message: warningMessage);
        }

        public static string FormatWarning(
            IFormatProvider? provider,
            [AllowNull] string warningMessage,
            HResult code,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(warningMessage, nameof(warningMessage));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: FormatMetadata(provider, "WARNING {0} 0x{1:X8}", code, ),
                message: warningMessage);
        }

        public static string FormatWarning(
            IFormatProvider? provider,
            [AllowNull] string warningMessage,
            int exitCode,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(warningMessage, nameof(warningMessage));
            ArgumentOutOfRangeException.ThrowIfZero(exitCode, nameof(exitCode));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: FormatMetadata(provider, "WARNING EXIT {0}|{0:X8}", exitCode),
                message: warningMessage);
        }

        public static string FormatWarning<TException>(
            IFormatProvider? provider,
            [AllowNull] string origin,
            [AllowNull] string message,
            [AllowNull] TException exception,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerFilePath] string? filePath = null,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(origin, nameof(origin));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message, nameof(message));
            ArgumentNullException.ThrowIfNull(exception, nameof(exception));

            return FormatMessage(
                provider: provider,
                origin: FormatOrigin(lineNumber, columnNumber, endLineNumber, endColumnNumber, filePath, memberName),
                metadata: FormatMetadata(
                    provider: provider,
                    format: "TRAPPED EXCEPTION {0} {1}|0x{1:X8}",
                    exception.GetType().Name,
                    exception.HResult),
                message: message ?? exception.Message);
        }

        #endregion Public Methods
    }
}
