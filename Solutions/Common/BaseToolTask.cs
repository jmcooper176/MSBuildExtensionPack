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

using MSBuild.ExtensionPack.Utility;

using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace MSBuild.ExtensionPack
{
    public abstract class BaseToolTask : ToolTask
    {
        #region Private Fields

        private AuthenticationLevel authenticationLevel = System.Management.AuthenticationLevel.Default;
        private string machineName;
        private SecureString? userPassword;

        #endregion Private Fields

        #region Private Methods

        private SecureString? ConvertToSecureString(string value)
        {
            SecureString accumulator = new();

            foreach (char item in value)
            {
                try
                {
                    accumulator.AppendChar(item);
                }
                catch (ObjectDisposedException odex)
                {
                    this.Log.LogErrorFromException(odex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, null);
                    return null;
                }
                catch (InvalidOperationException ioex)
                {
                    this.Log.LogErrorFromException(ioex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, null);
                    return null;
                }
                catch (ArgumentOutOfRangeException aorex)
                {
                    this.Log.LogErrorFromException(aorex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, null);
                    return null;
                }
                catch (CryptographicException cex)
                {
                    this.Log.LogErrorFromException(cex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, null);
                    return null;
                }
            }

            return accumulator;
        }

        private string ConvertToString(SecureString value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            IntPtr binaryString = IntPtr.Zero;

            try
            {
                binaryString = Marshal.SecureStringToBSTR(value);
                return Marshal.PtrToStringBSTR(binaryString);
            }
            catch (OutOfMemoryException omex)
            {
                this.Log.LogErrorFromException(omex, showStackTrace: this.LogExceptionStack, showDetail: this.LogExceptionDetail, null);
                return string.Empty;
            }
            finally
            {
                if (binaryString != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(binaryString);
                }
            }
        }

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

        #region Internal Properties

        internal ManagementScope Scope { get; set; }

        #endregion Internal Properties

        #region Internal Methods

        internal void GetManagementScope(string wmiNamespace)
        {
            this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "ManagementScope Set: {0}", "\\\\" + this.MachineName + wmiNamespace));
            if (string.Equals(this.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                this.Scope = new ManagementScope("\\\\" + this.MachineName + wmiNamespace);
            }
            else
            {
                ConnectionOptions options = new()
                {
                    Authentication = this.authenticationLevel,
                    Username = this.UserName,
                    Password = this.UserPassword,
                    Authority = this.Authority
                };

                this.Scope = new ManagementScope("\\\\" + this.MachineName + wmiNamespace, options);
            }
        }

        internal void GetManagementScope(string wmiNamespace, ConnectionOptions options)
        {
            this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "ManagementScope Set: {0}", "\\\\" + this.MachineName + wmiNamespace));
            this.Scope = new ManagementScope("\\\\" + this.MachineName + wmiNamespace, options);
        }

        /// <summary>
        /// Determines whether the task is targeting the local machine
        /// </summary>
        /// <returns>bool</returns>
        internal bool TargetingLocalMachine()
        {
            return this.TargetingLocalMachine(false);
        }

        /// <summary>
        /// Determines whether the task is targeting the local machine
        /// </summary>
        /// <param name="canExecuteRemotely">True if the current TaskAction can run against a remote machine</param>
        /// <returns>bool</returns>
        internal bool TargetingLocalMachine(bool canExecuteRemotely)
        {
            if (!string.Equals(this.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase))
            {
                if (!canExecuteRemotely)
                {
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "This task does not support remote execution. Please remove the MachineName: {0}", this.MachineName));
                }

                return false;
            }

            return true;
        }

        #endregion Internal Methods

        #region Public Properties

        /// <summary>
        /// Sets the authentication level to be used to connect to WMI. Default is Default. Also supports: Call, Connect, None,
        /// Packet, PacketIntegrity, PacketPrivacy, Unchanged
        /// </summary>
        public string AuthenticationLevel
        {
            get { return this.authenticationLevel.ToString(); }
            set { this.authenticationLevel = Enum.Parse<AuthenticationLevel>(value); }
        }

        /// <summary>
        /// Sets the authority to be used to authenticate the specified user.
        /// </summary>
        public string Authority { get; set; }

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
        /// Sets the MachineName.
        /// </summary>
        public virtual string MachineName
        {
            get { return this.machineName ?? Environment.MachineName; }
            set { this.machineName = value; }
        }

        /// <summary>
        /// Set to true to suppress all Message logging by tasks. Errors and Warnings are not affected.
        /// </summary>
        public bool SuppressTaskMessages { get; set; }

        /// <summary>
        /// Sets the TaskAction.
        /// </summary>
        public virtual string TaskAction { get; set; }

        /// <summary>
        /// Sets the UserName
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Sets the UserPassword.
        /// </summary>
        public virtual string UserPassword
        {
            get
            {
                return ConvertToString(this.userPassword);
            }

            set
            {
                this.userPassword?.Clear();
                this.userPassword = ConvertToSecureString(value);
            }
        }

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
            ArgumentNullException.ThrowIfNull(log);

            log.LogCommandLine(MessageImportance.Low, $"{executable} {args}");

            ShellWrapper exec = new(executable, args);

            // stderr is logged as errors
            exec.ErrorDataReceived += (sender, e) => log.LogTaskError(() => e.Data is not null, e.Data!);

            // stdout is logged normally if requested
            exec.OutputDataReceived += (sender, e) => log.LogTaskMessage(() => logOutput && e.Data is not null, MessageImportance.Normal, e.Data!);

            // execute the process
            exec.Execute();

            // check the exit code
            log.LogTaskError(
                predicate: () => exec.ExitCode != 0,
                message: "The tool {0} exited with error code {1}",
                arguments: [executable, exec.ExitCode]);

            return exec.StandardOutput;
        }

        #endregion Public Methods
    }
}
