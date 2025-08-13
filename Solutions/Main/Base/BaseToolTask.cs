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

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using MSBuild.ExtensionPack.Base.Logging;

using System.Globalization;

namespace MSBuild.ExtensionPack
{
    public abstract class BaseToolTask : ToolTask
    {
        #region Private Methods

        /// <summary>
        /// </summary>
        private void GetLogExceptionDetail()
        {
            string? logExceptionDetail = Environment.GetEnvironmentVariable("LogExceptionDetail", EnvironmentVariableTarget.Machine);

            this.LogExceptionDetail = string.IsNullOrEmpty(logExceptionDetail) ? false : Convert.ToBoolean(logExceptionDetail, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// </summary>
        private void GetLogExceptionStack()
        {
            string? logExceptionStack = Environment.GetEnvironmentVariable("LogExceptionStack", EnvironmentVariableTarget.Machine);

            this.LogExceptionStack = string.IsNullOrEmpty(logExceptionStack) ? false : Convert.ToBoolean(logExceptionStack, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// </summary>
        private void GetSuppressTaskMessages()
        {
            string? suppressTaskMessages = Environment.GetEnvironmentVariable("SuppressTaskMessages", EnvironmentVariableTarget.Machine);

            this.SuppressTaskMessages = string.IsNullOrEmpty(suppressTaskMessages) ? false : Convert.ToBoolean(suppressTaskMessages, CultureInfo.CurrentCulture);
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            GetSuppressTaskMessages();

            try
            {
                return this.InternalExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
            }
            catch (Exception ex)
            {
                GetLogExceptionStack();
                GetLogExceptionDetail();
                this.Log.LogErrorFromException(ex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, file: pathToTool);
                return !this.Log.HasLoggedErrors ? 0 : ex.HResult;
            }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        protected int ExecuteToolWithLogging(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            GetSuppressTaskMessages();

            try
            {
                return this.InternalExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
            }
            catch (Exception ex)
            {
                GetLogExceptionStack();
                GetLogExceptionDetail();
                this.Log.LogErrorFromException(ex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, file: pathToTool);
                return !this.Log.HasLoggedErrors ? 0 : ex.HResult;
            }
        }

        /// <summary>
        /// This is the main InternalExecuteTool method that all tool tasks should implement
        /// </summary>
        /// <param name="pathToTool">          </param>
        /// <param name="responseFileCommands"></param>
        /// <param name="commandLineCommands"> </param>
        /// <returns></returns>
        /// <remarks>LogError should be thrown in the event of errors</remarks>
        protected abstract int InternalExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands);

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Set to true to error if the task has been deprecated
        /// </summary>
        public bool ErrorOnDeprecated { get; set; }

        /// <summary>
        /// </summary>
        public bool LogExceptionDetail { get; set; }

        /// <summary>
        /// Set to true to log the full Exception Stack to the console.
        /// </summary>
        public bool LogExceptionStack { get; set; }

        /// <summary>
        /// Set to true to suppress all Message logging by tasks. Errors and Warnings are not affected.
        /// </summary>
        public bool SuppressTaskMessages { get; set; }

        /// <summary>
        /// Sets the TaskAction.
        /// </summary>
        public virtual string TaskAction { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Executes a tool, logs standard error and a nonzero exit code as errors, returns the output and optionally logs that as well.
        /// </summary>
        /// <param name="executable">the name of the executable</param>
        /// <param name="args">      the command line arguments</param>
        /// <param name="logOutput"> should we log the output in real time</param>
        /// <returns>the output of the tool</returns>
        public string ExecuteShellWithLogging(string executable, string args, bool logOutput)
        {
            ArgumentNullException.ThrowIfNull(Log);

            this.Log.LogCommandLine(MessageImportance.Low, $"{executable} {args}");

            ShellWrapper exec = new(executable, args);

            // stderr is logged as errors
            exec.ErrorDataReceived += (sender, e) => this.Log.LogTaskError(() => e.Data is not null, e.Data!);

            // stdout is logged normally if requested
            exec.OutputDataReceived += (sender, e) => this.Log.LogTaskMessage(() => logOutput && e.Data is not null, MessageImportance.Normal, e.Data!);

            // execute the process
            exec.Execute();

            // check the exit code
            this.Log.LogTaskError(
                predicate: () => exec.ExitCode != 0,
                message: "The tool {0} exited with error code {1}",
                arguments: [executable, exec.ExitCode]);

            return exec.StandardOutput;
        }

        #endregion Public Methods
    }
}
