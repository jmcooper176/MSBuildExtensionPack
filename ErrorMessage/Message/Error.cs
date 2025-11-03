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
namespace MSBuild.ExtensionPack.ErrorMessage.Message
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// Static class to provide extension methods for <see cref="BaseTask"/> and <see cref="BaseToolTask"/> error logging.
    /// </summary>
    public static class Error
    {
        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskError(
            this TaskLoggingHelper log,
            string message,
            params object?[] arguments)
    {
        log.LogError(message, arguments);
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="subcategory">
    /// Specifies the error subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
    /// usually the same as <see cref="IBaseTask.TaskAction"/>.
    /// </param>
    /// <param name="errorCode">
    /// Specifies the error code to log which is usually the same as the resource identifier for a multi-lingual string..
    /// </param>
    /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
    /// <param name="message">        Specifies the message to log.</param>
    /// <param name="filePath">       Specifies the source file path where the error to be logged occurred.</param>
    /// <param name="lineNumber">
    /// Specifies the source beginning line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="columnNumber">
    /// Specifies the source beginning column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endLineNumber">
    /// Specifies the source ending line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endColumnNumber">
    /// Specifies the source ending column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="arguments">
    /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
    /// </param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        string subcategory,
        string errorCode,
        [AllowNull] string? helpKeyword,
        string message,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        int columnNumber = 0,
        int endLineNumber = 0,
        int endColumnNumber = 0,
        params object?[] arguments)
    {
        log.LogError(subcategory, errorCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="subcategory">
    /// Specifies the error subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
    /// usually the same as <see cref="IBaseTask.TaskAction"/>.
    /// </param>
    /// <param name="errorCode">
    /// Specifies the error code to log which is usually the same as the resource identifier for a multi-lingual string.
    /// </param>
    /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
    /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
    /// <param name="message">        Specifies the message to log.</param>
    /// <param name="filePath">       Specifies the source file path where the error to be logged occurred.</param>
    /// <param name="lineNumber">
    /// Specifies the source beginning line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="columnNumber">
    /// Specifies the source beginning column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endLineNumber">
    /// Specifies the source ending line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endColumnNumber">
    /// Specifies the source ending column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="arguments">
    /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
    /// </param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        string subcategory,
        string errorCode,
        [AllowNull] string helpKeyword,
        string? helpLink,
        string message,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        int columnNumber = 0,
        int endLineNumber = 0,
        int endColumnNumber = 0,
        params object?[] arguments)
    {
        log.LogError(subcategory, errorCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="predicate">
    /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the error should be logged.
    /// </param>
    /// <param name="message">  Specifies the message to log.</param>
    /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> predicate,
        string message,
        params object?[] arguments)
    {
        Contract.Requires(!predicate.Invoke(), string.Format(CultureInfo.CurrentCulture, message, arguments));

        if (!predicate.Invoke())
        {
            log.LogError(message, arguments);
        }
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="predicate">
    /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the error should be logged.
    /// </param>
    /// <param name="subcategory">
    /// Specifies the error subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
    /// usually the same as <see cref="IBaseTask.TaskAction"/>.
    /// </param>
    /// <param name="errorCode">
    /// Specifies the error code to log which is usually the same as the resource identifier for a multi-lingual string..
    /// </param>
    /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
    /// <param name="message">        Specifies the message to log.</param>
    /// <param name="filePath">       Specifies the source file path where the error to be logged occurred.</param>
    /// <param name="lineNumber">
    /// Specifies the source beginning line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="columnNumber">
    /// Specifies the source beginning column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endLineNumber">
    /// Specifies the source ending line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endColumnNumber">
    /// Specifies the source ending column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="arguments">
    /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
    /// </param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> predicate,
        string subcategory,
        string errorCode,
        [AllowNull] string? helpKeyword,
        string message,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        int columnNumber = 0,
        int endLineNumber = 0,
        int endColumnNumber = 0,
        params object?[] arguments)
    {
        Contract.Requires(!predicate.Invoke(), string.Format(CultureInfo.CurrentCulture, message, arguments));

        if (!predicate.Invoke())
        {
            log.LogError(subcategory, errorCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="predicate">
    /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the error should be logged.
    /// </param>
    /// <param name="subcategory">
    /// Specifies the error subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
    /// usually the same as <see cref="IBaseTask.TaskAction"/>.
    /// </param>
    /// <param name="errorCode">
    /// Specifies the error code to log which is usually the same as the resource identifier for a multi-lingual string.
    /// </param>
    /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
    /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
    /// <param name="message">        Specifies the message to log.</param>
    /// <param name="filePath">       Specifies the source file path where the error to be logged occurred.</param>
    /// <param name="lineNumber">
    /// Specifies the source beginning line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="columnNumber">
    /// Specifies the source beginning column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endLineNumber">
    /// Specifies the source ending line number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="endColumnNumber">
    /// Specifies the source ending column number in <paramref name="filePath"/> where the error to be logged occurred.
    /// </param>
    /// <param name="arguments">
    /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
    /// </param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> predicate,
        string subcategory,
        string errorCode,
        [AllowNull] string helpKeyword,
        string? helpLink,
        string message,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0,
        int columnNumber = 0,
        int endLineNumber = 0,
        int endColumnNumber = 0,
        params object?[] arguments)
    {
        Contract.Requires(!predicate.Invoke(), string.Format(CultureInfo.CurrentCulture, message, arguments));

        if (!predicate.Invoke())
        {
            log.LogError(subcategory, errorCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="exception">Specifies the <see cref="Exception"/> to log as an error.</param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Exception exception)
    {
        log.LogErrorFromException(exception);
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an error.</param>
    /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Exception exception,
        bool showStackTrace)
    {
        log.LogErrorFromException(exception, showStackTrace);
    }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an error.</param>
        /// <param name="data">          Specifies key/value pairs to be appended to <see cref="Exception.Data"/>.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <param name="showDetail">    If <see langref="true"/>, details of <paramref name="exception"/> will be logged.</param>
        /// <param name="filePath">      Specifies the source file path where the error to be logged occurred.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskError(
        this TaskLoggingHelper log,
        Exception exception,
        IDictionary<object, object?>? data,
        bool showStackTrace,
        bool showDetail,
        [CallerFilePath] string? filePath = null)
    {
        if (data?.Count > 0)
        {
            foreach (var item in data)
            {
                exception.Data.Add(item.Key, item.Value);
            }
        }

        log.LogErrorFromException(exception, showStackTrace, showDetail, filePath);
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">          Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="postPredicate">Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
    /// <param name="exception">    Specifies the <see cref="Exception"/> to log as an error.</param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> postPredicate,
        Exception exception)
    {
        log.LogErrorFromException(exception);
        Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
    }

    /// <summary>
    /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
    /// </summary>
    /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
    /// <param name="postPredicate"> Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
    /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an error.</param>
    /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
    /// <remarks>
    /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
    /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
    /// </remarks>
    public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> postPredicate,
        Exception exception,
        bool showStackTrace)
    {
        log.LogErrorFromException(exception, showStackTrace);
        Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
    }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task errors.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="postPredicate"> Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an error.</param>
        /// <param name="data">          Specifies key/value pairs to be appended to <see cref="Exception.Data"/>.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <param name="showDetail">    If <see langref="true"/>, details of <paramref name="exception"/> will be logged.</param>
        /// <param name="filePath">      Specifies the source file path where the error to be logged occurred.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskError(
        this TaskLoggingHelper log,
        Func<bool> postPredicate,
        Exception exception,
        IDictionary<object, object?>? data,
        bool showStackTrace,
        bool showDetail,
        [CallerFilePath] string? filePath = null)
    {
        if (data?.Count > 0)
        {
            foreach (var item in data)
            {
                exception.Data.Add(item.Key, item.Value);
            }
        }

        log.LogErrorFromException(exception, showStackTrace, showDetail, filePath);
        Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
    }
}
}
