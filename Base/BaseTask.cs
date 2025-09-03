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

namespace MSBuild.ExtensionPack.Base
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base.SystemAttribute;

    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides a common task for all the MSBuildExtensionPack Tasks
    /// </summary>
    public abstract class BaseTask : Task, IBaseTask
    {
        #region Protected Methods

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        protected bool Execute([CallerFilePath] string? file = null, [CallerLineNumber] int lineNumber = 0)
        {
            string code = $"{nameof(BaseTask)}MSG0001";
            string helpKeyWord = $"{this.HelpKeywordPrefix}{nameof(BaseTask)}HLP0001";
            file = Path.GetFullPath(file!);
            string taskAction = TaskAction ?? "none";

            try
            {
                this.Log.LogTaskMessage(
                    () => !SuppressTaskMessages,
                    MessageImportance.Low,
                    taskAction,
                    code,
                    helpKeyWord,
                    $"{0}({1}) : Execute : Task {nameof(BaseTask)} with Task Action {TaskAction}.",
                    file,
                    lineNumber,
                    file,
                    lineNumber,
                    nameof(BaseTask),
                    taskAction);

                switch (TaskAction)
                {
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Log.LogErrorFromException(ex, LogExceptionStackTrace, LogExceptionDetail, file);
            }

            return !this.Log.HasLoggedErrors;
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether set <see cref="TaskLoggingHelper.HasLoggedErrors"/> if the task has been deprecated;
        /// otherwise, a warning will be logged.
        /// </summary>
        public bool ErrorOnDeprecated
        {
            get
            {
                ObsoleteCustomAttribute obsolete = new();
                var inError = obsolete.GetObsoleteAttributeIsError<BaseTask>(obsolete.GetCustomAttributeForType<BaseTask>());
                this.Log.LogTaskError(
                    () => inError,
                    "Task {0} is marked obsolete and 'IsError' is '{1}'",
                    this.GetType().Name, inError);
                return inError;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a trapped <see cref="Exception"/> will log details.
        /// </summary>
        public bool LogExceptionDetail => Logging.GetLogExceptionDetail();

        /// <summary>
        /// Gets a value indicating whether a trapped <see cref="Exception"/> will log the full stack trace.
        /// </summary>
        public bool LogExceptionStackTrace => Logging.GetLogExceptionStackTrace();

        /// <summary>
        /// Gets a value indicating whether to suppress all Message logging by tasks; otherwise, all messages will be logged.
        /// </summary>
        /// <remarks>Errors and Warnings are never affected.</remarks>
        public bool SuppressTaskMessages => Logging.GetSuppressTaskMessages();

        /// <summary>
        /// Gets or sets a value indicating the task action string.
        /// </summary>
        public virtual string? TaskAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <inheritdoc/>
        public override bool Execute()
        {
            return this.Execute(null);
        }

        #endregion Public Methods
    }
}
