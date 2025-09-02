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

using Microsoft.Data.SqlClient;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public static class ExceptionExtension
    {
        #region Public Fields

        /// <summary>
        /// </summary>
        public const int ERROR_ACCESS_DENIED = 0x00000005;

        /// <summary>
        /// </summary>
        public const int ERROR_ARENA_TRASHED = 0x00000007;

        /// <summary>
        /// </summary>
        public const int ERROR_ARITHMETIC_OVERFLOW = 0x00000216;

        /// <summary>
        /// </summary>
        public const int ERROR_ASSERTION_FAILURE = 0x0000029C;

        /// <summary>
        /// </summary>
        public const int ERROR_BAD_ARGUMENTS = 0x000000A0;

        /// <summary>
        /// </summary>
        public const int ERROR_BAD_ENVIRONMENT = 0x0000000A;

        /// <summary>
        /// </summary>
        public const int ERROR_BAD_FORMAT = 0x0000000B;

        /// <summary>
        /// </summary>
        public const int ERROR_BAD_PATHNAME = 0x000000A1;

        /// <summary>
        /// </summary>
        public const int ERROR_BROKEN_PIPE = 0x0000006D;

        /// <summary>
        /// </summary>
        public const int ERROR_BUFFER_OVERFLOW = 0x0000006F;

        /// <summary>
        /// </summary>
        public const int ERROR_CALL_NOT_IMPLEMENTED = 0x00000078;

        /// <summary>
        /// </summary>
        public const int ERROR_CANCELLED = 0x000004C7;

        /// <summary>
        /// </summary>
        public const int ERROR_CONNECTION_REFUSED = 0x000004C9;

        /// <summary>
        /// </summary>
        public const int ERROR_CONTROL_C_EXIT = 0x0000023C;

        /// <summary>
        /// </summary>
        public const int ERROR_CURRENT_DIRECTORY = 0x00000010;

        /// <summary>
        /// </summary>
        public const int ERROR_DIR_NOT_EMPTY = 0x00000091;

        /// <summary>
        /// </summary>
        public const int ERROR_DIRECTORY = 0x0000010B;

        /// <summary>
        /// </summary>
        public const int ERROR_FATAL_APP_EXIT = 0x000002C9;

        /// <summary>
        /// </summary>
        public const int ERROR_FILE_EXISTS = 0x00000050;

        /// <summary>
        /// </summary>
        public const int ERROR_FILE_NOT_FOUND = 0x00000002;

        /// <summary>
        /// </summary>
        public const int ERROR_FILENAME_EXED_RANGE = 0x000000CE;

        /// <summary>
        /// </summary>
        public const int ERROR_HANDLE_EOF = 0x00000026;

        /// <summary>
        /// </summary>
        public const int ERROR_INSTALL_FAILURE = 0x00000643;

        /// <summary>
        /// </summary>
        public const int ERROR_INSTALL_USEREXIT = 0x00000642;

        /// <summary>
        /// </summary>
        public const int ERROR_INTERNAL_ERROR = 0x0000054F;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_ACCESS = 0x0000000C;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_BLOCK = 0x00000009;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_DATA = 0x0000000D;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_DRIVE = 0x0000000F;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_FUNCTION = 0x00000001;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_HANDLE = 0x00000006;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_NAME = 0x0000007B;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_PARAMETER = 0x00000057;

        /// <summary>
        /// </summary>
        public const int ERROR_INVALID_PASSWORD = 0x00000056;

        /// <summary>
        /// </summary>
        public const int ERROR_LOCK_VIOLATION = 0x00000021;

        /// <summary>
        /// </summary>
        public const int ERROR_LOGON_FAILURE = 0x0000052E;

        /// <summary>
        /// </summary>
        public const int ERROR_NETWORK_ACCESS_DENIED = 0x00000041;

        /// <summary>
        /// </summary>
        public const int ERROR_NO_MATCH = 0x00000491;

        /// <summary>
        /// </summary>
        public const int ERROR_NO_MORE_ITEMS = 0x00000103;

        /// <summary>
        /// </summary>
        public const int ERROR_NO_MORE_MATCHES = 0x00000272;

        /// <summary>
        /// </summary>
        public const int ERROR_NOT_ENOUGH_MEMORY = 0x00000008;

        /// <summary>
        /// </summary>
        public const int ERROR_NOT_FOUND = 0x00000490;

        /// <summary>
        /// </summary>
        public const int ERROR_NOT_SUPPORTED = 0x00000032;

        /// <summary>
        /// </summary>
        public const int ERROR_OPEN_FAILED = 0x0000006E;

        /// <summary>
        /// </summary>
        public const int ERROR_OUTOFMEMORY = 0x0000000E;

        /// <summary>
        /// </summary>
        public const int ERROR_PATH_NOT_FOUND = 0x00000003;

        /// <summary>
        /// </summary>
        public const int ERROR_POSSIBLE_DEADLOCK = 0x0000046B;

        /// <summary>
        /// </summary>
        public const int ERROR_READ_FAULT = 0x0000001E;

        /// <summary>
        /// </summary>
        public const int ERROR_SHARING_VIOLATION = 0x00000020;

        /// <summary>
        /// </summary>
        public const int ERROR_STACK_OVERFLOW = 0x000003E9;

        /// <summary>
        /// </summary>
        public const int ERROR_SUCCESS = 0x00000000;

        /// <summary>
        /// </summary>
        public const int ERROR_SYSTEM_SHUTDOWN = 0x00000281;

        /// <summary>
        /// </summary>
        public const int ERROR_TIMEOUT = 0x000005B4;

        /// <summary>
        /// </summary>
        public const int ERROR_TOO_MANY_OPEN_FILES = 0x00000004;

        /// <summary>
        /// </summary>
        public const int ERROR_UNKNOWN_ERROR = 0xFFFF;

        /// <summary>
        /// </summary>
        public const int ERROR_WRITE_FAULT = 0x0000001D;

        /// <summary>
        /// </summary>
        public const int FACILITY_MASK = 0x0000FFFF;

        /// <summary>
        /// </summary>
        public const int FACILITY_NULL = 0;

        /// <summary>
        /// </summary>
        public const int FACILITY_OPC = 81;

        /// <summary>
        /// </summary>
        public const int FACILITY_WIN32 = 7;

        /// <summary>
        /// </summary>
        public const int S_FALSE = 1;

        /// <summary>
        /// </summary>
        public const int S_OK = 0;

        #endregion Public Fields

        #region Public Methods

        public static string FormatMessage(IFormatProvider? provider, string? message, Exception exception, bool showDetail = false, bool showStackTrace = false)
        {
            StringBuilder buffer = StringBuilderExtension.Create();

            if (!TryGetValue(exception.Data, "Thrown", out object? thrown))
            {
                ThrowException<KeyNotFoundException>(first: $"Key 'Thrown' not found in {exception.GetType().Name}.Data", data: null);
            }

            if (!TryGetValue(exception.Data, "Source", out object? source))
            {
                ThrowException<KeyNotFoundException>(first: $"Key 'Source' not found in {exception.GetType().Name}.Data", data: null);
            }

            if (!TryGetValue(exception.Data, "Message", out object? msg))
            {
                ThrowException<KeyNotFoundException>(first: $"Key 'Message' not found in {exception.GetType().Name}.Data", data: null);
            }

            buffer.AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "[{0}] {1} {2} : {3}",
                    thrown ?? DateTime.UtcNow,
                    source ?? exception.Source,
                    exception.GetType().Name,
                    message ?? msg ?? exception.Message);

            if (showDetail && !showStackTrace)
            {
                if (!TryGetValue(exception.Data, "HResult", out object? hr))
                {
                    ThrowException<KeyNotFoundException>(first: $"Key 'HResult' not found in {exception.GetType().Name}.Data", data: null);
                }

                if (!TryGetValue(exception.Data, "Cause", out object? innerException))
                {
                    ThrowException<KeyNotFoundException>(first: $"Key 'Cause' not found in {exception.GetType().Name}.Data", data: null);
                }

                buffer.AppendLine().AppendFormat(
                    provider ?? CultureInfo.InvariantCulture,
                    "Detail : HResult 0x{0:X8} : Cause {1}",
                    hr is not null ? (int)hr : ERROR_INTERNAL_ERROR,
                    innerException is not null ? innerException.GetType().Name : "No inner exception");
            }

            if (showStackTrace)
            {
                buffer.AppendLine().AppendLine("Stack Trace : ").Append(exception.StackTrace);
            }

            buffer.AppendLine();

            return buffer.ToString();
        }

        public static string GetConstructorName(this Type constructedType)
        {
            return constructedType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(i => i.Name).FirstOrDefault() ?? "ctor";
        }

        public static string GetDefaultConstructorName(this Type constructedType)
        {
            return constructedType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.EmptyTypes)?.Name ?? "ctor";
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
                { "HResult", exception.HResult != ERROR_SUCCESS ? exception.HResult : (exception.InnerException?.HResult ?? ToHResult(ERROR_INTERNAL_ERROR)) },
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
            ArgumentOutOfRangeException.ThrowIfLessThan(ToWin32ErrorCode(hr), ERROR_SUCCESS, nameof(hr));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(ToWin32ErrorCode(hr), ERROR_UNKNOWN_ERROR, nameof(hr));

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

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> unconditionally.
        /// </exception>
        /// <exception cref="MissingMethodException">Throws when typeof(TException) represents an abstract class.</exception>
        [DoesNotReturn]
        public static void ThrowException<TException>(
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            TException? exception = null;

            if (!typeof(TException).HasPublicConstructor())
            {
                throw new MissingMethodException(typeof(TException).FullName, typeof(TException).GetConstructorName());
            }

            exception = Activator.CreateInstance<TException>();
            exception = exception.SetSource(path, lineNumber, memberName);
            throw exception.SetData(data, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="arguments"> </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> unconditionally.
        /// </exception>
        /// <exception cref="ArgumentNullException">Throws if typeof(TException) is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Throws if typeof(TException) is not a RuntimeType.</exception>
        /// <exception cref="NotSupportedException">Throws if typeof(TException) is a <see cref="TypeBuilder"/>.</exception>
        /// <exception cref="TargetInvocationException">Throws if the constructor for typeof(TException) throws an <see cref="Exception"/>.</exception>
        /// <exception cref="MethodAccessException">
        /// Throws if the caller does not have permission to call the typeof(TException) constructor. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="MemberAccessException">Throws if typeof(TException) represents an abstract class.</exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws if the COM tyhpe was not obtained through either <see cref="Type.GetTypeFromProgID(string)"/> or <see cref="Type.GetTypeFromCLSID(Guid)"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws if no matching public constructor for typeof(TException) can be found. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="COMException">
        /// Throws if typeof(TException) is a COM object but the class identifier used to obtain the type is invalid or not registered.
        /// </exception>
        /// <exception cref="TypeLoadException">Throws if typeof(TException) is not a valid <see cref="Type"/>.</exception>
        [DoesNotReturn]
        public static void ThrowException<TException>(
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[]? arguments) where TException : Exception
        {
            TException? exception = null;

            if (!typeof(TException).HasPublicConstructor())
            {
                throw new MissingMethodException(typeof(TException).FullName, typeof(TException).GetConstructorName());
            }

            try
            {
                exception = (TException?)Activator.CreateInstance(typeof(TException), arguments) ?? Activator.CreateInstance<TException>();
                exception = exception.SetSource(path, lineNumber, memberName);
                exception = exception.SetData(data, path, lineNumber, memberName);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex1) when (ex1 is NotSupportedException || ex1 is TargetInvocationException || ex1 is MethodAccessException || ex1 is MemberAccessException || ex1 is MissingMethodException)
            {
                Console.Error.WriteLine(ex1.ToString());
                throw;
            }
            catch (Exception ex2) when (ex2 is InvalidComObjectException || ex2 is COMException)
            {
                Console.Error.WriteLine(ex2.ToString());
                throw;
            }

            throw exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="first">     </param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <paramref name="exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <paramref name="exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> unconditionally.
        /// </exception>
        /// <exception cref="ArgumentNullException">Throws if typeof(TException) is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Throws if typeof(TException) is not a RuntimeType.</exception>
        /// <exception cref="NotSupportedException">Throws if typeof(TException) is a <see cref="TypeBuilder"/>.</exception>
        /// <exception cref="TargetInvocationException">Throws if the constructor for typeof(TException) throws an <see cref="Exception"/>.</exception>
        /// <exception cref="MethodAccessException">
        /// Throws if the caller does not have permission to call the typeof(TException) constructor. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="MemberAccessException">Throws if typeof(TException) represents an abstract class.</exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws if the COM type was not obtained through either <see cref="Type.GetTypeFromProgID(string)"/> or <see cref="Type.GetTypeFromCLSID(Guid)"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws if no matching public constructor for typeof(TException) can be found. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="COMException">
        /// Throws if typeof(TException) is a COM object but the class identifier used to obtain the type is invalid or not registered.
        /// </exception>
        /// <exception cref="TypeLoadException">Throws if typeof(TException) is not a valid <see cref="Type"/>.</exception>
        [DoesNotReturn]
        public static void ThrowException<TException>(
            object? first,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            TException? exception = null;

            if (!typeof(TException).HasPublicConstructor())
            {
                throw new MissingMethodException(typeof(TException).FullName, typeof(TException).GetConstructorName());
            }

            try
            {
                exception = (TException?)Activator.CreateInstance(typeof(TException), first) ?? Activator.CreateInstance<TException>();
                exception = exception.SetSource(path, lineNumber, memberName);
                exception = exception.SetData(data, path, lineNumber, memberName);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex1) when (ex1 is NotSupportedException || ex1 is TargetInvocationException || ex1 is MethodAccessException || ex1 is MemberAccessException || ex1 is MissingMethodException)
            {
                Console.Error.WriteLine(ex1.ToString());
                throw;
            }
            catch (Exception ex2) when (ex2 is InvalidComObjectException || ex2 is COMException)
            {
                Console.Error.WriteLine(ex2.ToString());
                throw;
            }

            throw exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="first">     </param>
        /// <param name="second">    </param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> unconditionally.
        /// </exception>
        /// <exception cref="ArgumentNullException">Throws if typeof(TException) is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Throws if typeof(TException) is not a RuntimeType.</exception>
        /// <exception cref="NotSupportedException">Throws if typeof(TException) is a <see cref="TypeBuilder"/>.</exception>
        /// <exception cref="TargetInvocationException">Throws if the constructor for typeof(TException) throws an <see cref="Exception"/>.</exception>
        /// <exception cref="MethodAccessException">
        /// Throws if the caller does not have permission to call the typeof(TException) constructor. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="MemberAccessException">Throws if typeof(TException) represents an abstract class.</exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws if the COM tyhpe was not obtained through either <see cref="Type.GetTypeFromProgID(string)"/> or <see cref="Type.GetTypeFromCLSID(Guid)"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws if no matching public constructor for typeof(TException) can be found. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="COMException">
        /// Throws if typeof(TException) is a COM object but the class identifier used to obtain the type is invalid or not registered.
        /// </exception>
        /// <exception cref="TypeLoadException">Throws if typeof(TException) is not a valid <see cref="Type"/>.</exception>
        [DoesNotReturn]
        public static void ThrowException<TException>(
            object? first,
            object? second,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            TException? exception = null;

            if (!typeof(TException).HasPublicConstructor())
            {
                throw new MissingMethodException(typeof(TException).FullName, typeof(TException).GetConstructorName());
            }

            try
            {
                exception = (TException?)Activator.CreateInstance(typeof(TException), first, second) ?? Activator.CreateInstance<TException>();
                exception = exception.SetSource(path, lineNumber, memberName);
                exception = exception.SetData(data, path, lineNumber, memberName);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex1) when (ex1 is NotSupportedException || ex1 is TargetInvocationException || ex1 is MethodAccessException || ex1 is MemberAccessException || ex1 is MissingMethodException)
            {
                Console.Error.WriteLine(ex1.ToString());
                throw;
            }
            catch (Exception ex2) when (ex2 is InvalidComObjectException || ex2 is COMException)
            {
                Console.Error.WriteLine(ex2.ToString());
                throw;
            }

            throw exception;
        }

        public static void ThrowException(
            string? message,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentException>(message, paramName, data, path, lineNumber, memberName);
        }

        public static void ThrowException(
            string? message,
            string? paramName,
            Exception? innerException,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentException>(message, paramName, innerException, data, path, lineNumber, memberName);
        }

        public static void ThrowException(
            string? message,
            FileInfo? filePath,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<FileNotFoundException>(message, filePath?.FullName, data, path, lineNumber, memberName);
        }

        public static void ThrowException(
            string? message,
            FileInfo? filePath,
            Exception? innerException,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<FileNotFoundException>(message, filePath?.FullName, innerException, data, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="first">     </param>
        /// <param name="second">    </param>
        /// <param name="third">     </param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> unconditionally.
        /// </exception>
        /// <exception cref="ArgumentNullException">Throws if typeof(TException) is <see langref="null"/>.</exception>
        /// <exception cref="ArgumentException">Throws if typeof(TException) is not a RuntimeType.</exception>
        /// <exception cref="NotSupportedException">Throws if typeof(TException) is a <see cref="TypeBuilder"/>.</exception>
        /// <exception cref="TargetInvocationException">Throws if the constructor for typeof(TException) throws an <see cref="Exception"/>.</exception>
        /// <exception cref="MethodAccessException">
        /// Throws if the caller does not have permission to call the typeof(TException) constructor. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="MemberAccessException">Throws if typeof(TException) represents an abstract class.</exception>
        /// <exception cref="InvalidComObjectException">
        /// Throws if the COM tyhpe was not obtained through either <see cref="Type.GetTypeFromProgID(string)"/> or <see cref="Type.GetTypeFromCLSID(Guid)"/>.
        /// </exception>
        /// <exception cref="MissingMethodException">
        /// Throws if no matching public constructor for typeof(TException) can be found. For example, <see cref="SqlException"/>.
        /// </exception>
        /// <exception cref="COMException">
        /// Throws if typeof(TException) is a COM object but the class identifier used to obtain the type is invalid or not registered.
        /// </exception>
        /// <exception cref="TypeLoadException">Throws if typeof(TException) is not a valid <see cref="Type"/>.</exception>
        [DoesNotReturn]
        public static void ThrowException<TException>(
            object? first,
            object? second,
            object? third,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            TException? exception = null;

            if (!typeof(TException).HasPublicConstructor())
            {
                throw new MissingMethodException(typeof(TException).FullName, typeof(TException).GetConstructorName());
            }

            try
            {
                exception = (TException?)Activator.CreateInstance(typeof(TException), first, second, third) ?? Activator.CreateInstance<TException>();
                exception = exception.SetSource(path, lineNumber, memberName);
                exception = exception.SetData(data, path, lineNumber, memberName);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex1) when (ex1 is NotSupportedException || ex1 is TargetInvocationException || ex1 is MethodAccessException || ex1 is MemberAccessException || ex1 is MissingMethodException)
            {
                Console.Error.WriteLine(ex1.ToString());
                throw;
            }
            catch (Exception ex2) when (ex2 is InvalidComObjectException || ex2 is COMException)
            {
                Console.Error.WriteLine(ex2.ToString());
                throw;
            }

            throw exception;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfContains"></param>
        /// <param name="paramName">      Specifies the method parameter name that is the source of <see cref="ArgumentOutOfRangeException"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws an <see cref="ArgumentOutOfRangeException"/> conditionally.</exception>
        public static void ThrowIfContains<TIndex>(
            this TIndex index,
            ICollection<TIndex> throwIfContains,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TIndex : struct
        {
            index.ThrowIfContains<TIndex>(throwIfContains, paramName, null, data, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfContains"></param>
        /// <param name="paramName">      Specifies the method parameter name that is the source of <see cref="ArgumentOutOfRangeException"/>.</param>
        /// <param name="message">        Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">           
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">           Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">     Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">     Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws an <see cref="ArgumentOutOfRangeException"/> conditionally.</exception>
        public static void ThrowIfContains<TIndex>(
            this TIndex index,
            ICollection<TIndex> throwIfContains,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TIndex : struct
        {
            if (throwIfContains.Contains(index))
            {
                ThrowOutOfRangeException(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty<TSource, TElement>(
            this TSource? source,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TSource : ICollection<TElement>
        {
            source.ThrowIfEmpty<TSource, TElement>(paramName, null, data, path, lineNumber, memberName);
        }

        public static void ThrowIfEmpty<TSource, TElement>(
            this TSource? source,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TSource : ICollection<TElement>
        {
            if (source?.Count < 1)
            {
                ThrowNullException(paramName, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty<TElement>(
            this TElement[]? source,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (source?.Length < 1)
            {
                source.ThrowIfEmpty(paramName, null, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="ArgumentNullException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty<TElement>(
            this TElement[]? source,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (source?.Length < 1)
            {
                ThrowNullException(paramName, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty(
            this string? source,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (source?.Length < 1)
            {
                ThrowNullException(paramName, null, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="Exception">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty(
            this StringBuilder? source,
            string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (source?.Length < 1)
            {
                ThrowNullException(paramName, null, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="ArgumentNullException"/>.</param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> conditionally.</exception>
        public static void ThrowIfEmpty(
            this StringBuilder? source,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (source?.Length < 1)
            {
                ThrowNullException(paramName, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index">     </param>
        /// <param name="floor">     </param>
        /// <param name="paramName"> Specifies the method parameter name that is the source of <see cref="ArgumentOutOfRangeException"/>.</param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws an <see cref="ArgumentOutOfRangeException"/> conditionally.</exception>
        public static void ThrowIfGreaterThan(
            this int index,
            int floor,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            if (index > floor)
            {
                ThrowOutOfRangeException(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary> </summary> <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref <param name="index">
        /// </param> <param name="floor"> </param> <param name="paramName"> Specifies the method parameter name that is the source
        /// of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>. </param> <param name="message">
        /// Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param> <exception
        /// cref="Exception"> Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>
        /// conditionally. </exception>
        public static void ThrowIfGreaterThan<TException>(this Index index, int floor, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfGreaterThan<TException>(floor, paramName, index, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">         </param>
        /// <param name="floorInclusive"></param>
        /// <param name="paramName">     
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<TException>(this int index, int floorInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfGreaterThanOrEqual<TException>(floorInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">         </param>
        /// <param name="floorInclusive"></param>
        /// <param name="paramName">     
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">          
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">          Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">    Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">    Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<TException>(
            this int index,
            int floorInclusive,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index >= floorInclusive)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">         </param>
        /// <param name="floorInclusive"></param>
        /// <param name="paramName">     
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<TException>(this Index index, int floorInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfGreaterThanOrEqual<TException>(floorInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">         </param>
        /// <param name="floorInclusive"></param>
        /// <param name="paramName">     
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<TException>(this Index index, int floorInclusive, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfGreaterThanOrEqual<TException>(floorInclusive, paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="ceiling">  </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThan<TException>(this int index, int ceiling, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfLessThan<TException>(ceiling, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="ceiling">   </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThan<TException>(
            this int index,
            int ceiling,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index < ceiling)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="ceiling">  </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThan<TException>(this Index index, int ceiling, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfLessThan<TException>(ceiling, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="ceiling">  </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThan<TException>(this Index index, int ceiling, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfLessThan<TException>(ceiling, paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">           </param>
        /// <param name="ceilingInclusive"></param>
        /// <param name="paramName">       
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThanOrEqual<TException>(this int index, int ceilingInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfLessThanOrEqual<TException>(ceilingInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">           </param>
        /// <param name="ceilingInclusive"></param>
        /// <param name="paramName">       
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">         Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">            
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">            Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">      Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">      Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThanOrEqual<TException>(
            this int index,
            int ceilingInclusive,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index <= ceilingInclusive)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">           </param>
        /// <param name="ceilingInclusive"></param>
        /// <param name="paramName">       
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThanOrEqual<TException>(this Index index, int ceilingInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfLessThanOrEqual<TException>(ceilingInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">           </param>
        /// <param name="ceilingInclusive"></param>
        /// <param name="paramName">       
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">         Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfLessThanOrEqual<TException>(this Index index, int ceilingInclusive, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfLessThanOrEqual<TException>(ceilingInclusive, paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegative<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfNegative<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegative<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index < 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegative<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNegative<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegative<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNegative<TException>(paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegativeOrZero<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfNegativeOrZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegativeOrZero<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index <= 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegativeOrZero<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNegativeOrZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNegativeOrZero<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNegativeOrZero<TException>(paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNotZero<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfNotZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNotZero<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index != 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNotZero<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNotZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNotZero<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfNotZero<TException>(paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNull<TSource, TException>(this TSource? source, string? paramName) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source is null, paramName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <typeparamref name="TSource"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNull<TSource, TException>(this TSource? source, string? paramName, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source is null, paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <typeparamref name="TSource"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNull<TSource, TException>(this TSource? source, string? paramName, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source is null, paramName, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TSource, TElement, TException>(this TSource? source, string? paramName) where TSource : ICollection<TElement> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <typeparamref name="TSource"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TSource, TElement, TException>(this TSource? source, string? paramName, string? message) where TSource : ICollection<TElement> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1, paramName, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TSource, TElement, TException>(this TSource? source, string? paramName, string? message, Exception? innerException) where TSource : ICollection<TElement> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1, paramName, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TElement, TException>(this TElement[]? source, string? paramName) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TElement, TException>(this TElement[]? source, string? paramName, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TElement, TException>(this TElement[]? source, string? paramName, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this string? source, string? paramName) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrEmpty(source), paramName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this string? source, string? paramName, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrEmpty(source), paramName, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this string? source, string? paramName, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrEmpty(source), paramName, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this StringBuilder? source, string? paramName) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this StringBuilder? source, string? paramName, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrEmpty<TException>(this StringBuilder? source, string? paramName, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1, paramName, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TSource, TException>(this TSource? source, string? paramName) where TSource : ICollection<char> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><see cref="char"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TSource, TException>(this TSource? source, string? message) where TSource : ICollection<char> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><see cref="char"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TSource, TException>(this TSource? source, string? message, Exception? innerException) where TSource : ICollection<char> where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Count < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this char[]? source) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="char"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this char[]? source, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="char"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this char[]? source, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(source?.Length < 1 || (source?.All(c => char.IsWhiteSpace(c)) == true), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this string? source) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrWhiteSpace(source));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this string? source, string? message) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrWhiteSpace(source), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this string? source, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            ThrowOnTrue<TException>(string.IsNullOrWhiteSpace(source), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this StringBuilder? source) where TException : ArgumentNullException
        {
            source.ThrowIfNullOrWhiteSpace<TException>(null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source"> 
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this StringBuilder? source, string? message) where TException : ArgumentNullException
        {
            source.ThrowIfNullOrWhiteSpace<TException>(message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfNullOrWhiteSpace<TException>(this StringBuilder? source, string? message, Exception? innerException) where TException : ArgumentNullException
        {
            bool AllWhiteSpace()
            {
                for (int i = 0; i < source?.Length; i++)
                {
                    if (!char.IsWhiteSpace(source[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            ThrowOnTrue<TException>(source?.Length < 1 || AllWhiteSpace(), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">        </param>
        /// <param name="halfInclusive"></param>
        /// <param name="paramName">    
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfOutOfRange<TException>(this int index, Range halfInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfOutOfRange<TException>(halfInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">        </param>
        /// <param name="halfInclusive"></param>
        /// <param name="paramName">    
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">      Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">         
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">         Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">   Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">   Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfOutOfRange<TException>(
            this int index,
            Range halfInclusive,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index < halfInclusive.Start.Value || index >= halfInclusive.End.Value)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">        </param>
        /// <param name="halfInclusive"></param>
        /// <param name="paramName">    
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfOutOfRange<TException>(this Index index, Range halfInclusive, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfOutOfRange<TException>(halfInclusive, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">        </param>
        /// <param name="halfInclusive"></param>
        /// <param name="paramName">    
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">      Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfOutOfRange<TException>(this Index index, Range halfInclusive, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfOutOfRange<TException>(halfInclusive, paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfPositive<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfPositive<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfPositive<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index > 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfPositive<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfPositive<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfPositive<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfPositive<TException>(paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfSuperset"></param>
        /// <param name="paramName">      
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfProperSubsetOf<TIndex, TException>(this TIndex index, ISet<TIndex> throwIfSuperset, string? paramName) where TIndex : struct where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfSubsetOf<TIndex, TException>(throwIfSuperset, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfSuperset"></param>
        /// <param name="paramName">      
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">        Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfProperSubsetOf<TIndex, TException>(this TIndex index, ISet<TIndex> throwIfSuperset, string? paramName, string? message) where TIndex : struct where TException : ArgumentOutOfRangeException
        {
            HashSet<TIndex> singleton = [index];

            if (singleton.IsProperSubsetOf(throwIfSuperset))
            {
                throw (TException?)Activator.CreateInstance(typeof(TException), paramName, singleton, message) ?? Activator.CreateInstance<TException>();
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfSuperset"></param>
        /// <param name="paramName">      
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfSubsetOf<TIndex, TException>(this TIndex index, ISet<TIndex> throwIfSuperset, string? paramName) where TIndex : struct where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfSubsetOf<TIndex, TException>(throwIfSuperset, paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TIndex">Specifies the <see cref="Type"/> of <paramref name="index"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">          </param>
        /// <param name="throwIfSuperset"></param>
        /// <param name="paramName">      
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">        Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfSubsetOf<TIndex, TException>(this TIndex index, ISet<TIndex> throwIfSuperset, string? paramName, string? message) where TIndex : struct where TException : ArgumentOutOfRangeException
        {
            HashSet<TIndex> singleton = [index];

            if (singleton.IsSubsetOf(throwIfSuperset))
            {
                throw (TException?)Activator.CreateInstance(typeof(TException), paramName, singleton, message) ?? Activator.CreateInstance<TException>();
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfWhole<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfWhole<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfWhole<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index >= 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfWhole<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfWhole<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfWhole<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfPositive<TException>(paramName, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfZero<TException>(this int index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.ThrowIfZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">     </param>
        /// <param name="paramName"> 
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">   Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="data">      
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">      Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfZero<TException>(
            this int index,
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : ArgumentOutOfRangeException
        {
            if (index == 0)
            {
                ThrowException<TException>(paramName, index, message, data, path, lineNumber, memberName);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfZero<TException>(this Index index, string? paramName) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfZero<TException>(paramName, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="index">    </param>
        /// <param name="paramName">
        /// Specifies the method parameter name that is the source of <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowIfZero<TException>(this Index index, string? paramName, string? message) where TException : ArgumentOutOfRangeException
        {
            index.Value.ThrowIfZero<TException>(paramName, message);
        }

        public static void ThrowMissingMethodException(
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            string? className,
            string? methodName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<MissingMethodException>(className, methodName, data, path, lineNumber, memberName);
        }

        public static void ThrowNullException(
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentNullException>(paramName, data, path, lineNumber, memberName);
        }

        public static void ThrowNullException(
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentNullException>(paramName, message, data, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TSource, TElement, TException>(this TSource? source, Func<TElement, bool> predicate) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TSource, TElement, TException>(this TSource? source, Func<TElement, bool> predicate, string? message) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TSource, TElement, TException>(this TSource? source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TElement, TException>(this TElement[]? source, Func<TElement, bool> predicate) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TElement, TException>(this TElement[]? source, Func<TElement, bool> predicate, string? message) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TElement, TException>(this TElement[]? source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this string? source, Func<char, bool> predicate) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this string? source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this string? source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnTrue<TException>(source?.All(predicate) == true, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this StringBuilder? source, Func<char, bool> predicate) where TException : Exception
        {
            ThrowOnAll<TException>(source, predicate, null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            ThrowOnAll<TException>(source, predicate, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAll<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            bool AllPredicate()
            {
                for (int i = 0; i < (source?.Length ?? 0); i++)
                {
                    if (!predicate.Invoke(source?[i] ?? char.MinValue))
                    {
                        return false;
                    }
                }

                return true;
            }

            ThrowOnTrue<TException>(AllPredicate(), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Any() == true);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate, string? message) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Any() == true, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate, string? message, Exception? innerException) where TSource : ICollection<TElement> where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Any() == true, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Length > 0);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate, string? message) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Length > 0, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : source?.Length > 0, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this string? source, Func<char, bool>? predicate) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : !string.IsNullOrEmpty(source));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this string? source, Func<char, bool>? predicate, string? message) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : !string.IsNullOrEmpty(source), message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this string? source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnTrue<TException>(predicate is not null ? source?.Any(predicate) == true : !string.IsNullOrEmpty(source), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this StringBuilder? source, Func<char, bool>? predicate) where TException : Exception
        {
            ThrowOnAny<TException>(source, predicate, null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this StringBuilder? source, Func<char, bool>? predicate, string? message) where TException : Exception
        {
            ThrowOnAny<TException>(source, predicate, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnAny<TException>(this StringBuilder? source, Func<char, bool>? predicate, string? message, Exception? innerException) where TException : Exception
        {
            bool AnyPredicateOrNone()
            {
                if (predicate is null)
                {
                    return source?.Length > 0;
                }

                for (int i = 0; i < (source?.Length ?? 0); i++)
                {
                    if (predicate.Invoke(source?[i] ?? char.MinValue))
                    {
                        return true;
                    }
                }

                return false;
            }

            ThrowOnTrue<TException>(AnyPredicateOrNone(), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate, int trigger = 1) where TSource : ICollection<TElement> where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(trigger, 0, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Count() >= trigger);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate, string? message, int trigger) where TSource : ICollection<TElement> where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Count() >= trigger, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the
        /// source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <param name="trigger">       </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TSource, TElement, TException>(this TSource? source, Func<TElement, bool>? predicate, string? message, Exception? innerException, int trigger) where TSource : ICollection<TElement> where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Count() >= trigger, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate, string? message, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/> that is the source of the
        /// <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates
        /// to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <param name="trigger">       </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TElement, TException>(this TElement[]? source, Func<TElement, bool>? predicate, string? message, Exception? innerException, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this string? source, Func<char, bool>? predicate, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this string? source, Func<char, bool>? predicate, string? message, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger, message);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <param name="trigger">       </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this string? source, Func<char, bool> predicate, string? message, Exception? innerException, int trigger) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            ThrowOnTrue<TException>(predicate is not null ? source?.Count(predicate) >= trigger : source?.Length >= trigger, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this StringBuilder? source, Func<char, bool>? predicate, int trigger = 1) where TException : Exception
        {
            ThrowOnCount<TException>(source, predicate, null, null, trigger);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="trigger">  </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this StringBuilder? source, Func<char, bool>? predicate, string? message, int trigger = 1) where TException : Exception
        {
            ThrowOnCount<TException>(source, predicate, message, null, trigger);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <param name="trigger">       </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnCount<TException>(this StringBuilder? source, Func<char, bool>? predicate, string? message, Exception? innerException, int trigger = 1) where TException : Exception
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(trigger, nameof(trigger));

            int counter = 0;

            int CountPredicateOrNone()
            {
                if (predicate is null)
                {
                    return source?.Length ?? 0;
                }

                for (int i = 0; i < source?.Length; i++)
                {
                    if (predicate.Invoke(source?[i] ?? char.MinValue))
                    {
                        counter++;
                    }
                }

                return counter;
            }

            ThrowOnTrue<TException>(CountPredicateOrNone() >= trigger, message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFalse<TException>([DoesNotReturnIf(false)] bool condition) where TException : Exception
        {
            if (!condition)
            {
                throw Activator.CreateInstance<TException>();
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition">
        /// Specifies the condition, that, if <see langref="false"/>, will cause an <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> to be thrown.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFalse<TException>([DoesNotReturnIf(false)] bool condition, string? message) where TException : Exception
        {
            ThrowOnFalse<TException>(condition, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition">     </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFalse<TException>([DoesNotReturnIf(false)] bool condition, string? message, Exception? innerException) where TException : Exception
        {
            if (!condition)
            {
                throw (TException?)Activator.CreateInstance(typeof(TException), message, innerException) ?? Activator.CreateInstance<TException>();
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate, string? message) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate, string? message) where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><see cref="TElement"/> that is the source of the <see
        /// cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to
        /// <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.FirstOrDefault(predicate), default), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this string source, Func<char, bool> predicate) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.FirstOrDefault(predicate), char.MinValue));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this string source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.FirstOrDefault(predicate), char.MinValue), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this string source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.FirstOrDefault(predicate), char.MinValue), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this StringBuilder? source, Func<char, bool> predicate) where TException : Exception
        {
            source.ThrowOnFirst<TException>(predicate, null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            source.ThrowOnFirst<TException>(predicate, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/><see cref="Type"/> that is the source of the <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnFirst<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            char FirstOrDefault()
            {
                for (int i = 0; i < source?.Length; i++)
                {
                    if (predicate.Invoke(source?[i] ?? char.MinValue))
                    {
                        return source?[i] ?? char.MinValue;
                    }
                }

                return char.MinValue;
            }

            ThrowOnTrue<TException>(EqualityComparer<char>.Default.Equals(FirstOrDefault(), char.MinValue), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/><see
        /// cref="string"/> that is the source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>
        /// if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/><see
        /// cref="string"/> that is the source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>
        /// if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate, string? message) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TSource">Specifies the <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TElement">Specifies the element <see cref="Type"/> of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="ICollection{T}"/> of element <see cref="Type"/><typeparamref name="TElement"/><see
        /// cref="string"/> that is the source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/>
        /// if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TSource, TElement, TException>(this TSource source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TSource : ICollection<TElement> where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/><see cref="string"/> that
        /// is the source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/><see cref="string"/> that
        /// is thesource of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate, string? message) where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="Array"/> of element <see cref="Type"/><typeparamref name="TElement"/><see cref="string"/> that
        /// is thesource of the <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> if <paramref
        /// name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TElement, TException>(this TElement[] source, Func<TElement, bool> predicate, string? message, Exception? innerException) where TElement : IEqualityComparer<TElement> where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<TElement>.Default.Equals(source.LastOrDefault(predicate), default), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this string source, Func<char, bool> predicate) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.LastOrDefault(predicate), char.MinValue));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="string"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this string source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.LastOrDefault(predicate), char.MinValue), message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="string"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this string source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            ThrowOnFalse<TException>(EqualityComparer<char>.Default.Equals(source.LastOrDefault(predicate), char.MinValue), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this StringBuilder? source, Func<char, bool> predicate) where TException : Exception
        {
            source.ThrowOnLast<TException>(predicate, null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">   
        /// Specifies the <see cref="StringBuilder"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message) where TException : Exception
        {
            source.ThrowOnLast<TException>(predicate, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="source">        
        /// Specifies the <see cref="StringBuilder"/> source of the <see cref="Exception"/> of <see cref="Type"/><typeparamref
        /// name="TException"/> if <paramref name="predicate"/> evaluates to <see langref="true"/>.
        /// </param>
        /// <param name="predicate">     
        /// Specifies the functor <see cref="Func{T, TResult}"/> against which elements of <paramref name="source"/> are evaluated against.
        /// </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnLast<TException>(this StringBuilder? source, Func<char, bool> predicate, string? message, Exception? innerException) where TException : Exception
        {
            char LastOrDefault()
            {
                for (int i = (source?.Length ?? 0); i >= 0; i++)
                {
                    if (predicate.Invoke(source?[i] ?? char.MinValue))
                    {
                        return source?[i] ?? char.MinValue;
                    }
                }

                return char.MinValue;
            }

            ThrowOnTrue<TException>(EqualityComparer<char>.Default.Equals(LastOrDefault(), char.MinValue), message, innerException);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition"></param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnTrue<TException>([DoesNotReturnIf(true)] bool condition) where TException : Exception
        {
            ThrowOnTrue<TException>(condition, null, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition">
        /// Specifies the condition, that, if <see langref="true"/>, will cause an <see cref="Exception"/> of <see
        /// cref="Type"/><typeparamref name="TException"/> to be thrown.
        /// </param>
        /// <param name="message">  Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnTrue<TException>([DoesNotReturnIf(true)] bool condition, string? message) where TException : Exception
        {
            ThrowOnTrue<TException>(condition, message, null);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TException">Specifies the <see cref="Type"/> of the <see cref="Exception"/> to throw on condition.</typeparam>
        /// <param name="condition">     </param>
        /// <param name="message">       Specifies the message text used to override the localized <see cref="Exception.Message"/>.</param>
        /// <param name="innerException">Specifies the inner <see cref="Exception"/> that is the cause of this <see cref="Exception"/>.</param>
        /// <param name="data">          
        /// Specifies additional <see cref="IDictionary{TKey, TValue}"/> key/value pairs to append to <see cref="Exception.Data"/>.
        /// </param>
        /// <param name="path">          Specifies the source file path of the <see cref="Exception"/>.</param>
        /// <param name="lineNumber">    Specifies the line number in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <param name="memberName">    Specifies the member name in <paramref name="path"/> of the <see cref="Exception"/>.</param>
        /// <exception cref="Exception">
        /// Throws an <see cref="Exception"/> of <see cref="Type"/><typeparamref name="TException"/> conditionally.
        /// </exception>
        public static void ThrowOnTrue<TException>(
            [DoesNotReturnIf(true)] bool condition,
            string? message,
            Exception? innerException,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TException : Exception
        {
            if (condition)
            {
                ThrowException<TException>(message, innerException, data, path, lineNumber, memberName);
            }
        }

        public static void ThrowOutOfRangeException(
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    string? paramName,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentOutOfRangeException>(paramName, data, path, lineNumber, memberName);
        }

        public static void ThrowOutOfRangeException(
            string? paramName,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentOutOfRangeException>(paramName, message, data, path, lineNumber, memberName);
        }

        public static void ThrowOutOfRangeException(
            string? paramName,
            object? actualValue,
            string? message,
            IDictionary<string, object?>? data = null,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            ThrowException<ArgumentOutOfRangeException>(paramName, actualValue, message, data, path, lineNumber, memberName);
        }

        /// <summary>
        /// </summary>
        /// <param name="errorCode">   </param>
        /// <param name="facilityCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ToHResult(int errorCode, int facilityCode = FACILITY_WIN32)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(errorCode, ERROR_SUCCESS, nameof(errorCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(errorCode, ERROR_UNKNOWN_ERROR, nameof(errorCode));
            ArgumentOutOfRangeException.ThrowIfLessThan(facilityCode, FACILITY_NULL, nameof(facilityCode));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(facilityCode, FACILITY_OPC, nameof(facilityCode));

            return (int)(errorCode <= 0 ? errorCode : (ToWin32ErrorCode(errorCode) | (facilityCode << 16) | 0x80000000));
        }

        /// <summary>
        /// </summary>
        /// <param name="hr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ToWin32ErrorCode(int hr)
        {
            const uint MAX_HR = 0x8081FFFF;

            ArgumentOutOfRangeException.ThrowIfGreaterThan((uint)hr, MAX_HR, nameof(hr));

            return hr & FACILITY_MASK;
        }

        public static bool TryGetValue(IDictionary data, object key, out object? value)
        {
            value = data.Contains(key) ? data[key] : null;
            return value is null;
        }

        #endregion Public Methods
    }
}
