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
namespace MSBuild.ExtensionPack.Base
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public static class Logging
    {
        #region Public Methods

        public static string FormatMessage(
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            return FormatMessage(provider, message, origin, path, arguments);
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, 0, 0);
            return FormatMessage(provider, message, origin, path, arguments);
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, 0, endColumnNumber);
            return FormatMessage(provider, message, origin, path, arguments);
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            return FormatMessage(provider, message, origin, path, arguments);
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? path = null,
            params object?[] arguments)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(origin.Item1, 0, nameof(origin));
            var msg = string.Format(provider ?? CultureInfo.InvariantCulture, message, arguments) ?? message;

            if (origin.Item2 > 0 && origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}, {origin.Item3}, {origin.Item4}) : {msg}";
            }
            else if (origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}-{origin.Item4}) : {msg}";
            }
            else if (origin.Item2 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}) : {msg}";
            }
            else if (origin.Item3 > 0)
            {
                return $"{path}({origin.Item1}-{origin.Item3}) : {msg}";
            }
            else
            {
                return $"{path}({origin.Item1}) : {msg}";
            }
        }

        public static string FormatMessageError(
            IFormatProvider? provider,
            string subcategory,
            string errorCode,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? path = null,
            params object?[] arguments)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(origin.Item1, 0, nameof(origin));
            var msg = string.Format(provider ?? CultureInfo.InvariantCulture, message, arguments);

            if (origin.Item2 > 0 && origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}, {origin.Item3}, {origin.Item4}) : {errorCode} {subcategory} : {msg ?? "Error logged."}";
            }
            else if (origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}-{origin.Item4}) : {errorCode} {subcategory} : {msg ?? "Error logged."}";
            }
            else if (origin.Item2 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}) : {errorCode} {subcategory} : {msg ?? "Error logged."}";
            }
            else if (origin.Item3 > 0)
            {
                return $"{path}({origin.Item1}-{origin.Item3}) : {errorCode} {subcategory} : {msg ?? "Error logged."}";
            }
            else
            {
                return $"{path}({origin.Item1}) : {errorCode} {subcategory} : {msg ?? "Error logged."}";
            }
        }

        public static string FormatMessageError(
            IFormatProvider? provider,
            string subcategory,
            string errorCode,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            return FormatMessageError(provider, subcategory, errorCode, message, origin, path, arguments);
        }

        public static string FormatMessageError(
            IFormatProvider? provider,
            string subcategory,
            string errorCode,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, 0, 0);
            return FormatMessageError(provider, subcategory, errorCode, message, origin, path, arguments);
        }

        public static string FormatMessageError(
            IFormatProvider? provider,
            string subcategory,
            string errorCode,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, 0, endColumnNumber);
            return FormatMessageError(provider, subcategory, errorCode, message, origin, path, arguments);
        }

        public static string FormatMessageError(
            IFormatProvider? provider,
            string subcategory,
            string errorCode,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            return FormatMessageError(provider, subcategory, errorCode, message, origin, path, arguments);
        }

        public static string FormatMessageException<TException>(
            IFormatProvider? provider,
            string errorCode,
            string? message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments) where TException : Exception
        {
            var exception = Activator.CreateInstance<TException>();
            return FormatMessageException(provider, errorCode, exception, message, null, path, lineNumber, arguments);
        }

        public static string FormatMessageException(
            IFormatProvider? provider,
            string errorCode,
            Exception exception,
            string? message,
            string? source,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            var msg = string.Format(provider ?? CultureInfo.InvariantCulture, message ?? $"Exception of Type {exception.GetType().FullName} thrown", arguments) ?? exception.Message;

            return $"{path}({lineNumber}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
        }

        public static string FormatMessageException(
            IFormatProvider? provider,
            string errorCode,
            Exception exception,
            string? message,
            Tuple<int, int, int, int> origin,
            string? source,
            [CallerFilePath] string? path = null,
            params object?[] arguments)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(origin.Item1, 0, nameof(origin));
            var msg = string.Format(provider ?? CultureInfo.InvariantCulture, message ?? $"Exception of Type {exception.GetType().FullName} thrown", arguments) ?? exception.Message;

            if (origin.Item2 > 0 && origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}, {origin.Item3}, {origin.Item4}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
            }
            else if (origin.Item3 > 0 && origin.Item4 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}-{origin.Item4}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
            }
            else if (origin.Item2 > 0)
            {
                return $"{path}({origin.Item1}, {origin.Item2}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
            }
            else if (origin.Item3 > 0)
            {
                return $"{path}({origin.Item1}-{origin.Item3}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
            }
            else
            {
                return $"{path}({origin.Item1}) : {errorCode} 0x{exception.HResult: X8} {exception.GetType().Name} {source ?? exception.Source ?? exception.TargetSite?.ToString()} : {msg!}";
            }
        }

        public static string FormatMessageWarning(
                            IFormatProvider? provider,
            string subcategory,
            string warningCode,
            string message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            var msg = string.Format(provider ?? CultureInfo.InvariantCulture, message, arguments);
            return $"{path}({lineNumber}) : {warningCode} {subcategory} : {msg ?? "Warning logged."}";
        }

        public static bool GetLogExceptionDetail(bool defaultValue = false)
        {
            return TestEnvironmentValue("LogExceptionDetail", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }

        public static bool GetLogExceptionStackTrace(bool defaultValue = false)
        {
            return TestEnvironmentValue("LogExceptionStackTrace", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }

        public static int GetSourceColumnNumber(Exception exception, int compilerColumn, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? frame.GetFileColumnNumber() : compilerColumn;
        }

        public static string GetSourceFileName(Exception exception, string? compilerPath, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? (frame.GetFileName() ?? string.Empty) : (compilerPath ?? string.Empty);
        }

        public static int GetSourceLineNumber(Exception exception, int compilerLine, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasSource() == true ? frame.GetFileLineNumber() : compilerLine;
        }

        public static string GetSourceMethod(Exception exception, string? compilerMemberName, int index = 0)
        {
            StackTrace trace = new(exception, fNeedFileInfo: true);
            StackFrame? frame = trace.GetFrame(index);
            return frame?.HasMethod() == true ? (frame.GetFileName() ?? string.Empty) : (compilerMemberName ?? string.Empty);
        }

        public static bool GetSuppressTaskMessages(bool defaultValue = true)
        {
            return TestEnvironmentValue("SuppressTaskMessages", EnvironmentVariableTarget.Machine) ?? defaultValue;
        }

        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            log.LogTaskCriticalMessage(subcategory, code, helpKeyWord, message, file, lineNumber, 0, 0, 0, arguments);
        }

        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            params object?[] arguments)
        {
            var msg = FormatMessage(provider, message, file, origin.Item1, arguments);
            log.LogCriticalMessage(
                subcategory,
                code,
                helpKeyWord,
                file,
                origin.Item1,
                origin.Item2,
                origin.Item3,
                origin.Item4,
                msg);
        }

        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskCriticalMessage(
                subcategory,
                code,
                helpKeyWord,
                null,
                message,
                origin,
                file,
                arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogTaskError(
                    provider: null,
                    message: message,
                    file: file,
                    lineNumber: lineNumber,
                    arguments);
            }
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            const string subcategory = "generic";
            const string errorCode = "ERRXXXX";
            var msg = FormatMessageError(provider, subcategory, errorCode, message, file, lineNumber, arguments);

            log.LogError(msg);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                const string subcategory = "generic";
                const string errorCode = "ERRXXXX";

                var msg = FormatMessageError(provider, subcategory, errorCode, message, file, lineNumber, arguments);
                log.LogError(msg);
            }
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments
            )
        {
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                provider: null,
                message: message,
                file: file,
                lineNumber: lineNumber,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            params object?[] arguments
            )
        {
            var msg = FormatMessageError(provider, subcategory, errorCode, message, file, origin.Item1, arguments);
            log.LogError(subcategory, errorCode, helpKeyWord, file, origin.Item1, origin.Item2, origin.Item3, origin.Item4, msg);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                provider: provider,
                message: message,
                origin: origin,
                file: file,
                arguments: arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string helpLink,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments
            )
        {
            var msg = FormatMessageError(provider ?? CultureInfo.InvariantCulture, subcategory, errorCode, message, file, lineNumber, arguments);
            log.LogError(
                subcategory,
                errorCode,
                helpKeyWord,
                helpLink,
                file,
                origin.Item1,
                origin.Item2,
                origin.Item3,
                origin.Item4,
                msg);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string helpLink,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                helpLink: helpLink,
                provider: null,
                message: message,
                origin: origin,
                file: file,
                arguments: arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string helpLink,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                helpLink: helpLink,
                provider: null,
                message: message,
                origin: origin,
                file: file,
                arguments: arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Exception exception,
            bool showStackTrace = false,
            bool showDetail = false,
            [CallerFilePath] string? file = null)
        {
            log.LogErrorFromException(exception, showStackTrace, showDetail, file);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Exception exception,
            IDictionary<object, object?>? data,
            bool showStackTrace = false,
            bool showDetail = false,
            [CallerFilePath] string? file = null)
        {
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    exception.Data.Add(item.Key, item.Value);
                }
            }

            log.LogErrorFromException(exception, showStackTrace, showDetail, file);
        }

        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            MessageImportance messageImportance,
            string message,
            params object?[] arguments)
        {
            log.LogTaskMessage(
                predicate,
                messageImportance,
                "message",
                "text",
                "unknown",
                null,
                message,
                0,
                arguments);
        }

        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            MessageImportance messageImportance,
            string subcategory,
            string code,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                var msg = FormatMessage(provider ?? CultureInfo.InvariantCulture, message, file, origin.Item1, arguments);
                log.LogMessage(
                    subcategory,
                    code,
                    helpKeyWord,
                    file,
                    origin.Item1,
                    origin.Item2,
                    origin.Item3,
                    origin.Item4,
                    messageImportance,
                    msg);
            }
        }

        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            MessageImportance messageImportance,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskMessage(predicate, messageImportance, subcategory, code, helpKeyWord, message, origin, file, arguments);
        }

        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            MessageImportance messageImportance,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskMessage(predicate, messageImportance, subcategory, code, helpKeyWord, message, origin, file, arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments)
        {
            string subcategory = "generic";
            string warningCode = "WRNXXXX";
            string helpKeyWord = "warning";

            log.LogTaskWarning(
                subcategory,
                warningCode,
                helpKeyWord,
                message,
                file,
                lineNumber,
                arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            params object?[] arguments
            )
        {
            var msg = FormatMessageWarning(provider ?? CultureInfo.InvariantCulture, subcategory, warningCode, message, file, origin.Item1, arguments);
            log.LogWarning(subcategory, warningCode, helpKeyWord, file, origin.Item1, origin.Item2, origin.Item3, origin.Item4, msg);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, null, message, origin, file, arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, null, message, origin, file, arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string helpLink,
            IFormatProvider? provider,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            params object?[] arguments
            )
        {
            var msg = FormatMessageWarning(provider, subcategory, warningCode, message, file, origin.Item1, arguments);
            log.LogWarning(
                subcategory,
                warningCode,
                helpKeyWord,
                helpLink,
                file,
                origin.Item1,
                origin.Item2,
                origin.Item3,
                origin.Item4,
                msg);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string helpLink,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, helpLink, message, origin, file, arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string helpLink,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, helpLink, null, message, origin, file, arguments);
        }

        public static void LogTaskWarning(this TaskLoggingHelper log, Exception exception, bool showStackTrace = false)
        {
            log.LogWarningFromException(exception, showStackTrace);
        }

        public static void LogTaskWarning<TException>(
            this TaskLoggingHelper log,
            Exception exception,
            string warningCode,
            string? message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            bool showStackTrace = false,
            params object?[] arguments) where TException : Exception
        {
            var msg = FormatMessageException<TException>(null, warningCode, message, file, lineNumber, arguments);
            log.LogWarning(msg);
            log.LogWarningFromException(exception, showStackTrace);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception,
            string warningCode,
            string? message,
            IDictionary<object, object?>? data,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            bool showStackTrace = false,
            params object?[] arguments)
        {
            if (data is not null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    exception.Data.Add(item.Key, item.Value);
                }
            }

            var msg = FormatMessageException(null, warningCode, exception, message, exception.Source, file, lineNumber, arguments);
            log.LogWarning(msg);
            log.LogWarningFromException(exception, showStackTrace);
        }

        public static bool? TestEnvironmentValue(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            string? value = Environment.GetEnvironmentVariable(variable, target);

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else if (int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out int nonZero))
            {
                return nonZero >= 1 || nonZero < 0;
            }
            else if (bool.TryParse(value, out bool result) || (result = Convert.ToBoolean(value, CultureInfo.CurrentCulture)))
            {
                return result;
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods
    }
}
