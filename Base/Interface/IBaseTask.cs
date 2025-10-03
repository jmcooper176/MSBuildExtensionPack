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
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interface for see <see cref="BaseTask"/> and <see cref="BaseToolTask"/>.
    /// </summary>
    public interface IBaseTask
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether both <see cref="ObsoleteAttribute"/> has deprecated the task and <see
        /// cref="ObsoleteAttribute.IsError"/> is set to <see langref="true"/>; otherwise, if <see cref="ObsoleteAttribute"/>
        /// deprecates the task, a warning will be logged.
        /// </summary>
        bool ErrorOnDeprecated { get; }

        /// <summary>
        /// Gets a value indicating whether a trapped <see cref="Exception"/> will log details.
        /// </summary>
        bool LogExceptionDetail { get; }

        /// <summary>
        /// Gets a value indicating whether a trapped <see cref="Exception"/> will log the full stack trace.
        /// </summary>
        bool LogExceptionStackTrace { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to suppress all Message logging by tasks; otherwise, all messages will be logged.
        /// </summary>
        /// <remarks>Errors and Warnings are never affected.</remarks>
        bool SuppressTaskMessages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the task action string.
        /// </summary>
        string TaskAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Executes a task for sub-task <see cref="TaskAction"/>.
        /// </summary>
        /// <returns><see langref="true"/> if execution of the task is successful; otherwise, <see langref="false"/>.</returns>
        /// <remarks>Must be implemented in any derived <see cref="BaseTask"/> or <see cref="BaseToolTask"/>.</remarks>
        bool Execute();

        /// <summary>
        /// When overridden in a task implementing <see cref="BaseTask"/>, the task action router calls the appropriate sub-task
        /// based on <see cref="TaskAction"/>.
        /// </summary>
        /// <param name="filePath">  Specifies the source file path to task implementing <see cref="BaseTask"/>.</param>
        /// <param name="lineNumber">
        /// Specifies the source line number in <paramref name="filePath"/> to the task implementing <see cref="BaseTask"/>.
        /// </param>
        /// <exception cref="InvalidOperationException">Throws when property TaskAction value is invalid.</exception>
        /// <exception cref="Exception">Throws when a sub-task method throws an exception.</exception>
        void TaskActionRouter([CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Validates values associated with <see cref="TaskAction"/>.
        /// </summary>
        /// <returns>
        /// <see langref="true"/> if <see cref="TaskAction"/> is valid; otherwise, if it is null, empty, all whitespace, or invalid,
        /// <see langref="false"/>.
        /// </returns>
        /// <remarks><see cref="ValidateTaskAction(string)"/> will be called.</remarks>
        bool ValidateTaskAction();

        /// <summary>
        /// When overridden in a task implementing <see cref="BaseTask"/>, validates the value of <paramref name="taskAction"/> for
        /// a particular implementation of <see cref="IBaseTask"/>.
        /// </summary>
        /// <param name="taskAction">Specifies the <see cref="TaskAction"/> to validate.</param>
        /// <returns>
        /// <see langref="true"/> if <see cref="TaskAction"/> is valid; otherwise, if it is null, empty, all whitespace, or invalid,
        /// <see langref="false"/>.
        /// </returns>
        /// <remarks>Must be implemented in any derived <see cref="BaseTask"/> or <see cref="BaseToolTask"/>.</remarks>
        bool ValidateTaskAction([AllowNull] string taskAction);

        #endregion Public Methods
    }
}
