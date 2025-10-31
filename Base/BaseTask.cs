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
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base.Interface;

    /// <summary>
    /// Provides a common task for all the MSBuildExtensionPack Tasks
    /// </summary>
    public abstract class BaseTask : Task, IBaseTask
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        protected bool Execute([CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            string errorCode = $"{nameof(BaseTask)}ERR0002";
            string helpKeyWord = $"{this.HelpKeywordPrefix}{nameof(BaseTask)}HLP0002";
            FileInfo taskFilePath = new(filePath!);

            if (ValidateTaskAction())
            {
                this.Log.LogTaskError(TaskAction, errorCode, helpKeyWord, CultureInfo.CurrentCulture, "Property TaskAction is set to an invalid task action '{0}'", taskFilePath.FullName, lineNumber, this.TaskAction);
                return !this.Log.HasLoggedErrors;
            }

            TaskActionRouter(taskFilePath.FullName, lineNumber);
            return !this.Log.HasLoggedErrors;
        }

        /// <summary>
        /// Gets a value indicating whether set <see cref="TaskLoggingHelper.HasLoggedErrors"/> if the task has been deprecated;
        /// otherwise, a warning will be logged.
        /// </summary>
        public bool ErrorOnDeprecated
        {
            get
            {
                var inError = (CustomAttribute.TryGetCustomAttribute<ObsoleteCustomAttribute>(this.GetType().GetTypeInfo(), inherit: false, out ObsoleteCustomAttribute? value) && value?.IsErrorAttribute() == true)
                    || (Attribute.IsDefined(this.GetType().GetTypeInfo(), typeof(ObsoleteAttribute), inherit: false) && ((ObsoleteAttribute?)Attribute.GetCustomAttribute(this.GetType().GetTypeInfo(), typeof(ObsoleteAttribute), inherit: false))?.IsError == true);

                this.Log.LogTaskError(
                    () => inError,
                    "Task {0} is marked obsolete and 'IsError' is '{1}'",
                    null,
                    0,
                    this.GetType().Name,
                    inError);

                return this.Log.HasLoggedErrors;
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
        /// Gets a value indicating whether to suppress all message logging by tasks; otherwise, all messages will be logged.
        /// </summary>
        /// <remarks>Errors and Warnings are never affected.</remarks>
        public bool SuppressTaskMessages { get; set; } = Logging.GetSuppressTaskMessages();

        /// <summary>
        /// Gets or sets a value indicating the sub-task action string for <see cref="BaseTask"/>.
        /// </summary>
        [Required]
        public virtual string TaskAction { get; set; } = "None";

        /// <inheritdoc/>
        public override bool Execute()
        {
            return this.Execute(null, 0);
        }

        /// <inheritdoc/>
        public virtual void TaskActionRouter([CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            string errorCode = $"{nameof(BaseTask)}ERR0001";
            string messageCode = $"{nameof(BaseTask)}MSG0001";
            string warningCode = $"{nameof(BaseTask)}WRN0001";
            string helpKeyWord = $"{this.HelpKeywordPrefix}{nameof(BaseTask)}HLP0001";

            this.Log.LogTaskMessage(
                    () => !SuppressTaskMessages,
                    MessageImportance.Low,
                    TaskAction,
                    messageCode,
                    helpKeyWord,
                    "{0}({1}) : Execute : Task {2} with Task Action {3}.",
                    filePath,
                    lineNumber,
                    this.GetType().Name,
                    TaskAction);

            try
            {
                switch (TaskAction.ToUpperInvariant())
                {
                    case "NONE":
                        this.Log.LogTaskWarning(TaskAction, warningCode, helpKeyWord, "Nothing to do.", filePath, lineNumber);
                        break;

                    default:
                        throw new InvalidOperationException($"Property TaskAction value '{TaskAction}' is invalid.");
                }
            }
            catch (Exception ex)
            {
                this.Log.LogTaskError(ex, LogExceptionStackTrace, LogExceptionDetail, filePath);
            }
        }

        /// <inheritdoc/>
        public bool ValidateTaskAction()
        {
            return ValidateTaskAction(this.TaskAction);
        }

        /// <inheritdoc/>
        public virtual bool ValidateTaskAction([System.Diagnostics.CodeAnalysis.AllowNull] string taskAction)
        {
            return string.IsNullOrWhiteSpace(taskAction) || taskAction.Equals("None", StringComparison.OrdinalIgnoreCase);
        }
    }
}
