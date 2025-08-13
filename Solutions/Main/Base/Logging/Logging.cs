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

namespace MSBuild.ExtensionPack.Base.Logging
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public static class Logging
    {
        #region Public Methods

        public static void LogTaskCriticalMessage(
                    this TaskLoggingHelper log,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object[] arguments)
        {
            log.LogTaskCriticalMessage(subcategory, code, helpKeyWord, message, file, lineNumber, 0, 0, 0, arguments);
        }

        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string helpKeyWord,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments)
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
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
            params object[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskCriticalMessage(
                subcategory,
                code,
                helpKeyWord,
                message,
                origin,
                file,
                CultureInfo.CurrentCulture,
                arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            params object[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogTaskError(
                    "message",
                    "error",
                    "unknown",
                    message,
                    null,
                    0,
                    arguments);
            }
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string message,
            IFormatProvider? provider = null,
            params object[] arguments)
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
            log.LogError(msg);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            IFormatProvider? provider = null,
            params object[] arguments)
        {
            if (predicate.Invoke())
            {
                var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
                log.LogError(msg);
            }
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            params object[] arguments
            )
        {
            log.LogTaskError(subcategory, errorCode, helpKeyWord, message, file, lineNumber, 0, 0, 0, arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments
            )
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
            log.LogError(subcategory, errorCode, helpKeyWord, file, origin.Item1, origin.Item2, origin.Item3, origin.Item4, msg);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                message: message,
                origin: origin,
                file: file,
                provider: CultureInfo.CurrentCulture,
                arguments: arguments);
        }

        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string helpKeyWord,
            string helpLink,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments
            )
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
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
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                helpLink: helpLink,
                message: message,
                origin: origin,
                file: file,
                provider: CultureInfo.CurrentCulture,
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
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskError(
                subcategory: subcategory,
                errorCode: errorCode,
                helpKeyWord: helpKeyWord,
                helpLink: helpLink,
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
            if (data is not null && data.Count > 0)
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
            params object[] arguments)
        {
            log.LogTaskMessage(
                predicate,
                messageImportance,
                "message",
                "text",
                "unknown",
                message,
                null,
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
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments)
        {
            if (predicate.Invoke())
            {
                var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
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
            params object[] arguments)
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
            params object[] arguments)
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskMessage(predicate, messageImportance, subcategory, code, helpKeyWord, message, origin, file, arguments);
        }

        public static void LogTaskWarning(this TaskLoggingHelper log, string message, params object[] arguments)
        {
            log.LogTaskWarning(
                "message",
                "warning",
                "unknown",
                message,
                null,
                0,
                arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments
            )
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
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
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, 0, 0, 0);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, message, origin, file, arguments);
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
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, message, origin, file, arguments);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string helpKeyWord,
            string helpLink,
            string message,
            Tuple<int, int, int, int> origin,
            [CallerFilePath] string? file = null,
            IFormatProvider? provider = null,
            params object[] arguments
            )
        {
            var msg = string.Format(provider ?? CultureInfo.CurrentCulture, message, arguments);
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
            params object[] arguments
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
            params object[] arguments
            )
        {
            var origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
            log.LogTaskWarning(subcategory, warningCode, helpKeyWord, helpLink, message, origin, file, arguments);
        }

        public static void LogTaskWarning(this TaskLoggingHelper log, Exception exception, bool showStackTrace = false)
        {
            log.LogWarningFromException(exception, showStackTrace);
        }

        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception,
            IDictionary<object, object?>? data,
            bool showStackTrace = false)
        {
            if (data is not null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    exception.Data.Add(item.Key, item.Value);
                }
            }

            log.LogWarningFromException(exception, showStackTrace);
        }

        #endregion Public Methods
    }
}
