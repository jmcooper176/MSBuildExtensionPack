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
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Utilities;

    public static class Warning
    {
        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string message,
            params object?[] arguments)
        {
            log.LogWarning(message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string..
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            [AllowNull] string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string.
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
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
            log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogWarning(message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">      
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string..
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string warningCode,
            [AllowNull] string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">      
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string.
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string warningCode,
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
            if (predicate.Invoke())
            {
                log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="exception">Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception)
        {
            log.LogWarningFromException(exception);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception,
            bool showStackTrace)
        {
            log.LogWarningFromException(exception, showStackTrace);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">          Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="postPredicate">Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
        /// <param name="exception">    Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> postPredicate,
            Exception exception)
        {
            log.LogWarningFromException(exception);
            Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="postPredicate"> Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> postPredicate,
            Exception exception,
            bool showStackTrace)
        {
            log.LogWarningFromException(exception, showStackTrace);
            Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
        }
    }
}
