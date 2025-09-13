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

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// Implements extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtension
    {
        #region Internal Methods

        /// <summary>
        /// Shows the default.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="message">  The message.</param>
        internal static void ShowDefault(StringBuilder buffer, Exception exception, IFormatProvider? provider, string? message)
        {
            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                thrown = DateTime.UtcNow;
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                source = exception.Source;
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                msg = exception.Message;
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : {3}",
                    thrown,
                    source,
                    exception.GetType().Name,
                    message ?? msg);
        }

        /// <summary>
        /// Shows the default.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="message">  The message.</param>
        internal static void ShowDefault(StringBuilder buffer, ArgumentException exception, IFormatProvider? provider, string? message)
        {
            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                thrown = DateTime.UtcNow;
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                source = exception.Source;
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                msg = exception.Message;
            }

            if (!TryGetValue(exception.Data, "ParamName", out object? paramName))
            {
                paramName = exception.ParamName;
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : {3} {4}",
                    thrown,
                    source,
                    exception.GetType().Name,
                    paramName,
                    message ?? msg);
        }

        /// <summary>
        /// Shows the default.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="message">  The message.</param>
        internal static void ShowDefault(StringBuilder buffer, ArgumentNullException exception, IFormatProvider? provider, string? message)
        {
            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                thrown = DateTime.UtcNow;
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                source = exception.Source;
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                msg = exception.Message;
            }

            if (!TryGetValue(exception.Data, "ParamName", out object? paramName))
            {
                paramName = exception.ParamName;
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : {3} {4}",
                    thrown,
                    source,
                    exception.GetType().Name,
                    paramName,
                    message ?? msg);
        }

        /// <summary>
        /// Shows the default.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="message">  The message.</param>
        internal static void ShowDefault(StringBuilder buffer, ArgumentOutOfRangeException exception, IFormatProvider? provider, string? message)
        {
            if (!TryGetValue(exception.Data, "ActualValue", out object? value))
            {
                value = exception.ActualValue;
            }

            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                thrown = DateTime.UtcNow;
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                source = exception.Source;
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                msg = exception.Message;
            }

            if (!TryGetValue(exception.Data, "ParamName", out object? paramName))
            {
                paramName = exception.ParamName;
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : {3} {4} {5}",
                    thrown,
                    source,
                    exception.GetType().Name,
                    paramName,
                    value,
                    message ?? msg);
        }

        /// <summary>
        /// Shows the default.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="message">  The message.</param>
        internal static void ShowDefault(StringBuilder buffer, FileNotFoundException exception, IFormatProvider? provider, string? message)
        {
            if (!TryGetValue(exception.Data, "FileName", out object? fileName))
            {
                fileName = exception.FileName;
            }

            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                thrown = DateTime.UtcNow;
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                source = exception.Source;
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                msg = exception.Message;
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : FileName => {3} : {4}",
                    thrown,
                    source,
                    exception.GetType().Name,
                    fileName,
                    message ?? msg);
        }

        /// <summary>
        /// Shows the detail.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="provider"> The provider.</param>
        internal static void ShowDetail(StringBuilder buffer, Exception exception, IFormatProvider? provider)
        {
            if (!TryGetValue(exception.Data, "HResult", out object? hr))
            {
                hr = exception.HResult;
            }

            if (!TryGetValue(exception.Data, "Cause", out object? innerException))
            {
                innerException = exception.InnerException;
            }

            buffer.AppendLine().AppendFormat(
                provider ?? CultureInfo.InvariantCulture,
                "Detail : HResult 0x{0:X8} : Cause {1}",
                hr is not null ? (int)hr : HResultExtension.ToHResultCode(FacilityCode.FACILITY_WIN32, WinError.ERROR_INTERNAL_ERROR),
                innerException is not null ? innerException.GetType().Name : "No inner exception");
        }

        /// <summary>
        /// Shows the stack trace.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="exception">The exception.</param>
        internal static void ShowStackTrace(StringBuilder buffer, Exception exception)
        {
            buffer.AppendLine().AppendLine("Stack Trace : ").Append(exception.StackTrace?.ReplaceLineEndings());
        }

        #endregion Internal Methods

        #region Public Fields

        /// <summary>
        /// </summary>
        public const int S_FALSE = 1;

        /// <summary>
        /// </summary>
        public const int S_OK = 0;

        #endregion Public Fields

        #region Public Methods

        public static string FormatMessage(
            IFormatProvider? provider,
            Exception exception,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            StringBuilder buffer = StringBuilderExtension.Create();

            ShowDefault(buffer, exception, null, message);

            if (showDetail)
            {
                ShowDetail(buffer, exception, null);
            }

            if (showStackTrace)
            {
                ShowStackTrace(buffer, exception);
            }

            buffer.AppendLine();

            return buffer.ToString();
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            FileNotFoundException exception,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            StringBuilder buffer = StringBuilderExtension.Create();

            ShowDefault(buffer, exception, null, message);

            if (showDetail)
            {
                ShowDetail(buffer, exception, null);
            }

            if (showStackTrace)
            {
                ShowStackTrace(buffer, exception);
            }

            buffer.AppendLine();

            return buffer.ToString();
        }

        public static string FormatMessage(
            IFormatProvider? provider,
            ArgumentOutOfRangeException exception,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            StringBuilder buffer = StringBuilderExtension.Create();

            ShowDefault(buffer, exception, null, message);

            if (showDetail && !showStackTrace)
            {
                ShowDetail(buffer, exception, null);
            }

            if (showStackTrace)
            {
                ShowStackTrace(buffer, exception);
            }

            buffer.AppendLine();

            return buffer.ToString();
        }

        public static string FormatMessage(
            ArgumentOutOfRangeException exception,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            return FormatMessage(null, exception, message, showDetail, showStackTrace, path, lineNumber, memberName);
        }

        public static string FormatMessage(
            Exception exception,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            return FormatMessage(null, exception, message, showDetail, showStackTrace, path, lineNumber, memberName);
        }

        public static string GetConstructorName(this Type constructedType)
        {
            return constructedType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(i => i.Name).FirstOrDefault() ?? ".ctor";
        }

        public static string GetDefaultConstructorName(this Type constructedType)
        {
            return constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.EmptyTypes)?.Name ?? ".ctor";
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception"> 
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.Data"/> for.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <returns></returns>
        public static Dictionary<string, object?> GetDefaultData<TException>(
            this TException exception,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            return exception.GetDefaultData(null, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception"> 
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.Data"/> for.
        /// </param>
        /// <param name="comparer">  Specifies the <see cref="IEqualityComparer{T}"/> to use for the default data dictionary.</param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <returns></returns>
        public static Dictionary<string, object?> GetDefaultData<TException>(
            this TException exception,
            IEqualityComparer<string>? comparer,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            return new(comparer ?? EqualityComparer<string>.Default)
            {
                { "Thrown", DateTime.UtcNow },
                { "Source", exception.Source },
                { "Name", exception.GetType().Name },
                { "Message", exception.Message },
                { "HResult", exception.HResult != HResultExtension.ToHResultCode(FacilityCode.FACILITY_WIN32, WinError.ERROR_SUCCESS) ? exception.HResult : (exception.InnerException?.HResult ?? HResultExtension.ToHResultCode(FacilityCode.FACILITY_WIN32, WinError.ERROR_INTERNAL_ERROR)) },
                { "Cause", exception.InnerException },
                { "TargetSite", exception.TargetSite },
                { "SourceFile", path },
                { "LineNumber", lineNumber },
                { "Member", memberName },
                { "FullName", exception.GetType().FullName },
                { "BaseException", exception.GetBaseException() },
                { "HelpUri", exception.HelpLink },
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="hr"></param>
        /// <returns></returns>
        public static bool HasFailed(int hr)
        {
            return hr < S_OK;
        }

        public static bool HasPublicConstructor(this Type constructedType)
        {
            return !constructedType.IsValueType && constructedType.GetConstructors().Length > 0;
        }

        public static bool HasPublicDefaultConstructor(this Type constructedType)
        {
            return constructedType.HasPublicConstructor() && constructedType.GetConstructor(Type.EmptyTypes) is not null;
        }

        /// <summary>
        /// </summary>
        /// <param name="hr"></param>
        /// <returns></returns>
        public static bool HasSucceeded(int hr)
        {
            return hr >= S_OK;
        }

        public static bool IsInRange(int index, int inclusiveStart, int exclusiveEnd)
        {
            return index >= inclusiveStart && index < exclusiveEnd;
        }

        public static bool IsInRange(int index, Range range)
        {
            return IsInRange(index, range.Start.Value, range.End.Value);
        }

        public static bool IsInRange(Index index, int inclusiveStart, int exclusiveEnd)
        {
            return IsInRange(index.Value, inclusiveStart..^exclusiveEnd);
        }

        public static bool IsInRange(Index index, Range range)
        {
            return IsInRange(index.Value, range);
        }

        public static bool IsInRange<TIndex>(TIndex index, Range range) where TIndex : IConvertible, IComparable<TIndex>
        {
            return (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(range.Start.Value) >= 0)
                && (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(range.End.Value) < 0);
        }

        public static bool IsInRange<TIndex>(TIndex index, int inclusiveStart, int exclusiveEnd) where TIndex : IConvertible, IComparable<TIndex>
        {
            return (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(inclusiveStart) >= 0)
                && (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(exclusiveEnd) < 0);
        }

        public static bool IsOutOfRange(int index, int inclusiveStart, int exclusiveEnd)
        {
            return index < inclusiveStart || index >= exclusiveEnd;
        }

        public static bool IsOutOfRange(int index, Range range)
        {
            return IsOutOfRange(index, range.Start.Value, range.End.Value);
        }

        public static bool IsOutOfRange(Index index, int inclusiveStart, int exclusiveEnd)
        {
            return IsOutOfRange(index.Value, inclusiveStart..^exclusiveEnd);
        }

        public static bool IsOutOfRange(Index index, Range range)
        {
            return IsOutOfRange(index.Value, range);
        }

        public static bool IsOutOfRange<TIndex>(TIndex index, Range range) where TIndex : IConvertible, IComparable<TIndex>
        {
            return (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(range.Start.Value) < 0)
                && (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(range.End.Value) >= 0);
        }

        public static bool IsOutOfRange<TIndex>(TIndex index, int inclusiveStart, int exclusiveEnd) where TIndex : IConvertible, IComparable<TIndex>
        {
            return (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(inclusiveStart) < 0)
                && (Convert.ToInt32(index, CultureInfo.InvariantCulture).CompareTo(exclusiveEnd) >= 0);
        }

        [DoesNotReturn]
        public static void LogAndRethrow<TException>(
            this TException exception,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            exception.LogAndRethrow<TException>(null, null, showDetail, showStackTrace, null, path, lineNumber, memberName);
        }

        [DoesNotReturn]
        public static void LogAndRethrow<TException>(
            this TException exception,
            string? paramName,
            object? actualValue,
            string? message,
            bool showDetail,
            bool showStackTrace,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [CallerArgumentExpression("paramName")] string? paramNameArgument = null,
            [CallerArgumentExpression("actualValue")] string? actualValueArgument = null,
            [CallerArgumentExpression("message")] string? messageArgument = null) where TException : Exception
        {
            Dictionary<string, object?> data = new()
            {
                { "ParamNameArgument", paramNameArgument },
                { "ActualValueArgument", actualValueArgument },
                { "Message", messageArgument }
            };

            Console.Error.WriteLine(FormatMessage(null, exception, message, showDetail, showStackTrace));

            TException? constructedException = null;

            if (typeof(TException).BaseType == typeof(ArgumentNullException))
            {
                constructedException = (TException?)Activator.CreateInstance(typeof(TException), paramName, message);
            }
            else if (typeof(TException).BaseType == typeof(ArgumentOutOfRangeException))
            {
                constructedException = (TException?)Activator.CreateInstance(typeof(TException), paramName, actualValue, message);
            }
            else if (typeof(TException).BaseType == typeof(ArgumentException))
            {
                constructedException = (TException?)Activator.CreateInstance(typeof(TException), message, paramName);
            }
            else
            {
                constructedException = (TException?)Activator.CreateInstance(typeof(TException), message, null);
            }

            if (constructedException is not null)
            {
                constructedException = constructedException.SetData(data, path, lineNumber, memberName);
                throw constructedException;
            }
            else
            {
                throw new MissingMethodException($"Unable to create an instance of the {typeof(TException).Name} exception.");
            }
        }

        [DoesNotReturn]
        public static void LogAndRethrow<TException>(
            this TException exception,
            string? message,
            Exception? innerException,
            bool showDetail,
            bool showStackTrace,
            IFormatProvider? provider,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [CallerArgumentExpression("message")] string? messageArgument = null,
            [CallerArgumentExpression("innerException")] string? innerExceptionArgument = null) where TException : Exception
        {
            Dictionary<string, object?> data = new()
            {
                { "MessageArgument", messageArgument },
                { "InnerExceptionArgument", innerExceptionArgument }
            };

            Console.Error.WriteLine(FormatMessage(provider ?? CultureInfo.InvariantCulture, exception, message, showDetail, showStackTrace));

            TException? constructedException = (TException?)Activator.CreateInstance(typeof(TException), message, innerException);

            if (constructedException is not null)
            {
                constructedException = constructedException.SetData(data, path, lineNumber, memberName);
                throw constructedException;
            }
            else
            {
                throw new MissingMethodException($"Unable to create an instance of the {typeof(TException).Name} exception.");
            }
        }

        [DoesNotReturn]
        public static void LogAndTrap<TException>(this TException exception) where TException : Exception
        {
            exception.LogAndTrap<TException>(null, null);
        }

        [DoesNotReturn]
        public static void LogAndTrap<TException>(this TException exception, string? message) where TException : Exception
        {
            exception.LogAndTrap<TException>(message, null);
        }

        [DoesNotReturn]
        public static void LogAndTrap<TException>(this TException exception, string? message, Exception? innerException) where TException : Exception
        {
            Console.Error.WriteLine(message ?? exception.ToString());
            innerException?.LogAndTrap(message ?? innerException.ToString());
        }

        public static DirectoryNotFoundException? NewDirectoryException(
            string? message,
            string? directoryName,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Contract.Requires(Directory.Exists(directoryName), message);
            return NewException<DirectoryNotFoundException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message]);
        }

        public static DirectoryNotFoundException? NewDirectoryException(
            string? message,
            DirectoryInfo path,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Contract.Requires(path.Exists, message);
            return NewException<DirectoryNotFoundException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message]);
        }

        public static TException? NewException<TException>(
                                                                                                                                                                                            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[]? arguments)
            where TException : Exception
        {
            TException? exception;

            if (arguments?.Length < 1 || arguments?.All(a => a is null || (a.GetType() == typeof(string) && string.IsNullOrWhiteSpace(a.ToString()))) == true)
            {
                exception = Activator.CreateInstance<TException>();
            }
            else
            {
                exception = (TException?)Activator.CreateInstance(typeof(TException), arguments);
            }

            Contract.Ensures(exception is not null, $"Exception instance of type {typeof(TException).FullName} could not be created.");

            return exception?.SetData(data, filePath, lineNumber, memberName);
        }

        public static TException? NewException<TException>(
            string? message,
            Exception? innerException,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
            where TException : Exception
        {
            return NewException<TException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message, innerException]);
        }

        public static ArgumentException? NewException(
            string? message,
            string? paramName,
            Exception? innerException,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                return NewException<ArgumentException>(message: message, innerException: innerException, data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName);
            }
            else
            {
                return NewException<ArgumentException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message, paramName, innerException]);
            }
        }

        public static ArgumentException? NewException(
            Func<bool> paramPredicate,
            string? message,
            string? paramName,
            Exception? innerException,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Contract.Requires(paramPredicate.Invoke(), message);
            return NewException<ArgumentException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message, paramName, innerException]);
        }

        public static FileNotFoundException? NewException(
            string? message,
            string? fileName,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Contract.Requires(File.Exists(fileName), message);
            return NewException<FileNotFoundException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message, fileName]);
        }

        public static FileNotFoundException? NewException(
            string? message,
            FileInfo path,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Contract.Requires(path.Exists, message);
            return NewException<FileNotFoundException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [message, path.FullName]);
        }

        public static ArgumentOutOfRangeException? NewException(
            [AllowNull] string paramName,
            object? actualValue,
            string? message,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(paramName, nameof(paramName));

            if (actualValue is null && string.IsNullOrWhiteSpace(message))
            {
                return NewException<ArgumentOutOfRangeException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [paramName, message]);
            }
            else if (actualValue is null)
            {
                return NewException<ArgumentOutOfRangeException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [paramName, message]);
            }
            else
            {
                return NewException<ArgumentOutOfRangeException>(data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [paramName, actualValue, message]);
            }
        }

        public static ArgumentOutOfRangeException? NewException(
            Func<bool> rangePredicate,
            [AllowNull] string paramName,
            object? actualValue,
            string? message,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(paramName, nameof(paramName));

            Contract.Requires(rangePredicate.Invoke(), message);

            return NewException(paramName: paramName, actualValue: actualValue, message: message, data: data, filePath: filePath, lineNumber: lineNumber, memberName: memberName);
        }

        public static ArgumentOutOfRangeException? NewException(
            string paramName,
            int actualIndex,
            string? message,
            IDictionary<string, object?>? data)
        {
            return NewException(paramName: paramName, actualValue: actualIndex, message: message, data: data);
        }

        public static ArgumentOutOfRangeException? NewException(string paramName, Index actualIndex, string? message, IDictionary<string, object?>? data)
        {
            return NewException(paramName: paramName, actualValue: actualIndex, message: message, data: data);
        }

        public static ArgumentOutOfRangeException? NewException(string paramName, Range halfInclusive, string? message, IDictionary<string, object?>? data)
        {
            return NewException(paramName: paramName, actualValue: halfInclusive, message: message, data: data);
        }

        public static ArgumentNullException? NewNullException(
                                                    [AllowNull] string paramName,
            string? message,
            IDictionary<string, object?>? data,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(paramName, nameof(paramName));

            if (string.IsNullOrWhiteSpace(message))
            {
                return NewException<ArgumentNullException>(data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [paramName]);
            }
            else
            {
                return NewException<ArgumentNullException>(data, filePath: filePath, lineNumber: lineNumber, memberName: memberName, arguments: [paramName, message]);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception"> 
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.Data"/> for.
        /// </param>
        /// <param name="first">     
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <returns></returns>
        public static TException SetData<TException>(
            this TException exception,
            IDictionary<string, object?>? first = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            return exception.SetData(null, first, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception"> 
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.Data"/> for.
        /// </param>
        /// <param name="first">     
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="comparer">  Specifies the <see cref="IEqualityComparer{T}"/> to use for the merged dictionaries.</param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <returns></returns>
        public static TException SetData<TException>(
            this TException exception,
            IEqualityComparer<string>? comparer,
            IDictionary<string, object?>? first = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            Dictionary<string, object?>? merged = null;
            Dictionary<string, object?> data = new(first ?? new Dictionary<string, object?>(), comparer ?? EqualityComparer<string>.Default);
            Dictionary<string, object?> second = exception.GetDefaultData(comparer ?? EqualityComparer<string>.Default, path, lineNumber, memberName);

            if (data.Count > 0)
            {
                merged = data.Union(second.Where(kvp => !data.ContainsKey(kvp.Key))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value, comparer ?? EqualityComparer<string>.Default);
            }

            foreach (var item in data.Count > 0 && merged is not null ? merged : second)
            {
                exception.Data.Add(item.Key, item.Value);
            }

            return exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception">
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.HelpLink"/> for.
        /// </param>
        /// <param name="helpLink"> Specifies the <see cref="Exception.HelpLink"/><see cref="Uri"/> as a string.</param>
        /// <returns></returns>
        public static TException SetHelpLink<TException>(this TException exception, string? helpLink) where TException : Exception
        {
            exception.HelpLink = helpLink;
            return exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception">
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.HelpLink"/> for.
        /// </param>
        /// <param name="uri">      Specifies the <see cref="Exception.HelpLink"/><see cref="Uri"/> as a <see cref="Uri"/>.</param>
        /// <returns></returns>
        public static TException SetHelpLink<TException>(this TException exception, Uri uri) where TException : Exception
        {
            return exception.SetHelpLink(uri.AbsoluteUri);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception">
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.HResult"/> for.
        /// </param>
        /// <param name="hr">       </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static TException SetHResult<TException>(this TException exception, int hr) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(hr.ToWinErrorCode(), WinError.ERROR_SUCCESS.ToWinErrorCode(), nameof(hr));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(hr.ToWinErrorCode(), WinError.ERROR_UNKNOWN_ERROR.ToWinErrorCode(), nameof(hr));

            exception.HResult = hr;
            return exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="exception"> 
        /// Specifies the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> to update the <see
        /// cref="Exception.Source"/> for.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <returns></returns>
        public static TException SetSource<TException>(this TException exception, [CallerFilePath] string? path = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string? memberName = null) where TException : Exception
        {
            exception.Source = $"{path}({lineNumber}) : {memberName}";
            return exception;
        }

        public static bool TryGetValue(IDictionary data, object key, out object? value)
        {
            value = data.Contains(key) ? data[key] : null;
            return value is null;
        }

        #endregion Public Methods
    }
}
