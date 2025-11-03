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

// Ignore Spelling: unformatted

namespace MSBuild.ExtensionPack.ErrorMessage.Message
{
    using System.Resources;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Implements extension methods for <see cref="TaskLoggingHelper"/> that process <see cref="ResourceManager"/> strings.
    /// </summary>
    public static class Info
    {
        /// <summary>
        /// Extracts the message code from <paramref name="message"/> returning the message code and outputting the stripped message
        /// to <paramref name="messageWithoutCodePrefix"/>.
        /// </summary>
        /// <param name="log">                     Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="message">                 Specifies the message to strip.</param>
        /// <param name="messageWithoutCodePrefix">Specifies the <paramref name="message"/> without code prefix.</param>
        /// <returns>A <see cref="string"/> representing the message code.</returns>
        public static string ExtractMessageCode(this TaskLoggingHelper log, string message, out string? messageWithoutCodePrefix)
        {
            return log.ExtractMessageCode(message, out messageWithoutCodePrefix);
        }

        /// <summary>
        /// Formats the specified <paramref name="unformatted"/> string with <paramref name="arguments"/>.
        /// </summary>
        /// <param name="log">        Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="unformatted">Specifies the unformatted string to format.</param>
        /// <param name="arguments">  Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="unformatted"/>.</param>
        /// <returns>The formatted <see cref="string"/>.</returns>
        public static string Format(this TaskLoggingHelper log, string unformatted, params object?[] arguments)
        {
            return log.FormatString(unformatted, arguments);
        }

        /// <summary>
        /// Formats the specified <paramref name="unformatted"/> string with <paramref name="arguments"/>.
        /// </summary>
        /// <param name="log">         Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="resourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">   Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.</param>
        /// <returns>The formatted <see cref="string"/>.</returns>
        public static string FormatResource(this TaskLoggingHelper log, string resourceName, params object?[] arguments)
        {
            return log.FormatResourceString(resourceName, arguments);
        }

        /// <summary>
        /// Gets the <see cref="TaskLoggingHelper.HelpKeywordPrefix"/> help keyword prefix.
        /// </summary>
        /// <param name="log">Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <returns>The <see cref="TaskLoggingHelper.HelpKeywordPrefix"/>.</returns>
        public static string GetHelpKeywordPrefix(this TaskLoggingHelper log)
        {
            return log.HelpKeywordPrefix;
        }

        /// <summary>
        /// Gets the message string obtained from <see cref="ResourceManager"/> using the <paramref name="resourceName"/>.
        /// </summary>
        /// <param name="log">         Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="resourceName">Specifies the name of the resource.</param>
        /// <returns>A message string associated with <paramref name="resourceName"/>.</returns>
        public static string GetMessage(this TaskLoggingHelper log, string resourceName)
        {
            return log.GetResourceMessage(resourceName);
        }

        /// <summary>
        /// Gets the <see cref="TaskLoggingHelper.TaskResources"/>.
        /// </summary>
        /// <param name="log">Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <returns>A <see cref="ResourceManager"/>.</returns>
        public static ResourceManager GetResourceManager(this TaskLoggingHelper log)
        {
            return log.TaskResources;
        }

        /// <summary>
        /// Logs the task error using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskError(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogErrorFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="subcategory">        
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="errorCode">          Specifies the error code which is usually the resource name for a multi-lingual string.</param>
        /// <param name="helpKeyword">        Specifies the help keyword to be appended to <see cref="TaskLoggingHelper.HelpKeywordPrefix"/>.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="filePath">           Specifies the source file path for the task error.</param>
        /// <param name="lineNumber">         
        /// Specifies the source beginning line number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="columnNumber">       
        /// Specifies the source beginning column number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="endLineNumber">      
        /// Specifies the source end line number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="endColumnNumber">    
        /// Specifies the source end column number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string? helpKeyword,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.SetHelpKeywordPrefix(helpKeyword ?? "MSBuild");
            log.LogErrorFromResources(subcategory, errorCode, log.GetHelpKeywordPrefix(), filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error using the <paramref name="messageResourceName"/> resource name from the <see
        /// cref="ResourceManager"/> with the error code.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskErrorWithCode(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogErrorWithCodeFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error using the <paramref name="messageResourceName"/> resource name from the <see
        /// cref="ResourceManager"/> with the error code.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="subcategory">        
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="filePath">           Specifies the source file path for the task error.</param>
        /// <param name="lineNumber">         
        /// Specifies the source beginning line number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="columnNumber">       
        /// Specifies the source beginning column number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="endLineNumber">      
        /// Specifies the source end line number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="endColumnNumber">    
        /// Specifies the source end column number in <paramref name="filePath"/> for the task error.
        /// </param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskErrorWithCode(
            this TaskLoggingHelper log,
            string subcategory,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogErrorWithCodeFromResources(subcategory, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task message using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskMessage(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            Misc.LogTaskMessage(log, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task message using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="importance">         The importance.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskMessage(this TaskLoggingHelper log, MessageImportance importance, string messageResourceName, params object?[] arguments)
        {
            log.LogMessageFromResources(importance, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskWarning(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogWarningFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning using the <paramref name="messageResourceName"/> resource name from the <see cref="ResourceManager"/>.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="subcategory">        
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">        
        /// Specifies the warning code which is usually the resource name for a multi-lingual string.
        /// </param>
        /// <param name="helpKeyword">        Specifies the help keyword to be appended to <see cref="TaskLoggingHelper.HelpKeywordPrefix"/>.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="filePath">           Specifies the source file path for the task error.</param>
        /// <param name="lineNumber">         
        /// Specifies the source beginning line number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="columnNumber">       
        /// Specifies the source beginning column number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="endLineNumber">      
        /// Specifies the source end line number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="endColumnNumber">    
        /// Specifies the source end column number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string? helpKeyword,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.SetHelpKeywordPrefix(helpKeyword ?? "MSBuild");
            log.LogWarningFromResources(subcategory, warningCode, log.GetHelpKeywordPrefix(), filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning using the <paramref name="messageResourceName"/> resource name from the <see
        /// cref="ResourceManager"/> with warning code.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskWarningWithCode(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogWarningWithCodeFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning using the <paramref name="messageResourceName"/> resource name from the <see
        /// cref="ResourceManager"/> with warning code.
        /// </summary>
        /// <param name="log">                Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="subcategory">        
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="messageResourceName">Specifies the name of the resource.</param>
        /// <param name="filePath">           Specifies the source file path for the task error.</param>
        /// <param name="lineNumber">         
        /// Specifies the source beginning line number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="columnNumber">       
        /// Specifies the source beginning column number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="endLineNumber">      
        /// Specifies the source end line number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="endColumnNumber">    
        /// Specifies the source end column number in <paramref name="filePath"/> for the task warning.
        /// </param>
        /// <param name="arguments">          
        /// Specifies an <see cref="Array"/> of zero or more arguments to instantiate the resource string.
        /// </param>
        public static void LogTaskWarningWithCode(
            this TaskLoggingHelper log,
            string subcategory,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogWarningWithCodeFromResources(subcategory, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Sets the <see cref="TaskLoggingHelper.HelpKeywordPrefix"/> property.
        /// </summary>
        /// <param name="log">              Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="helpKeywordPrefix">Specifies the <see cref="TaskLoggingHelper.HelpKeywordPrefix"/> help keyword prefix.</param>
        public static void SetHelpKeywordPrefix(this TaskLoggingHelper log, string? helpKeywordPrefix)
        {
            log.HelpKeywordPrefix = helpKeywordPrefix ?? "MSBuild";
        }

        /// <summary>
        /// Sets the <see cref="TaskLoggingHelper.TaskResources"/> property.
        /// </summary>
        /// <param name="log">         Specifies the <see cref="TaskLoggingHelper"/> to use.</param>
        /// <param name="taskResource">Specifies the task <see cref="ResourceManager"/> resource.</param>
        public static void SetResourceManager(this TaskLoggingHelper log, ResourceManager taskResource)
        {
            log.TaskResources = taskResource;
        }
    }
}
