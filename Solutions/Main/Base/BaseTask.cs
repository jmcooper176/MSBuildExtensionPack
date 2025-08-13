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
    using Microsoft.Build.Utilities;

    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides a common task for all the MSBuildExtensionPack Tasks
    /// </summary>
    public abstract class BaseTask : Task
    {
        #region Private Methods

        private void GetExceptionLevel()
        {
            this.LogExceptionStackTrace = TestEnvironmentValue("LogExceptionStackTrace", EnvironmentVariableTarget.Machine);
            this.LogExceptionDetail = TestEnvironmentValue("LogExceptionDetail", EnvironmentVariableTarget.Machine);
        }

        private void GetSuppressTaskMessages()
        {
            this.SuppressTaskMessages = TestEnvironmentValue("SuppressTaskMessages", EnvironmentVariableTarget.Machine);
        }

        private bool TestEnvironmentValue(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            string? value = Environment.GetEnvironmentVariable(variable, target);

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            else
            {
                try
                {
                    return Convert.ToBoolean(value, CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        protected virtual bool Execute([CallerFilePath] string? file = null)
        {
            try
            {
                this.InternalExecute();
            }
            catch (Exception ex)
            {
                this.Log.LogErrorFromException(ex, this.LogExceptionStackTrace, this.LogExceptionDetail, file);
            }

            return !this.Log.HasLoggedErrors;
        }

        /// <summary>
        /// This is the main InternalExecute method that all tasks should implement
        /// </summary>
        /// <remarks>LogError should be thrown in the event of errors</remarks>
        protected abstract void InternalExecute();

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Set to true to error if the task has been deprecated
        /// </summary>
        public bool ErrorOnDeprecated { get; set; }

        public bool LogExceptionDetail { get; set; }

        /// <summary>
        /// Set to true to log the full Exception Stack to the console.
        /// </summary>
        public bool LogExceptionStackTrace { get; set; }

        /// <summary>
        /// Set to true to suppress all Message logging by tasks. Errors and Warnings are not affected.
        /// </summary>
        public bool SuppressTaskMessages { get; set; }

        /// <summary>
        /// Sets the TaskAction.
        /// </summary>
        public virtual string? TaskAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Execute()
        {
            this.GetSuppressTaskMessages();
            this.GetExceptionLevel();

            return this.Execute(null);
        }

        #endregion Public Methods
    }
}
