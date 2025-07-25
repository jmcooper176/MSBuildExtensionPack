// This file is part of CycloneDX CLI Tool
//
// Licensed under the Apache License, Version 2.0 (the “License”); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an “AS IS”
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language
// governing permissions and limitations under the License.
//
// SPDX-License-Identifier: Apache-2.0 Copyright (c) OWASP Foundation. All Rights Reserved. Ignore Spelling: cyclonedx Cli
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace MSBuild.ExtensionPack
{
    public abstract class BaseToolTask : ToolTask
    {
        private AuthenticationLevel authenticationLevel = System.Management.AuthenticationLevel.Default;
        private string machineName;
        private SecureString? userPassword;

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
        /// This is the main InternalExecuteTool method that all tool tasks should implement
        /// </summary>
        /// <param name="pathToTool">          </param>
        /// <param name="responseFileCommands"></param>
        /// <param name="commandLineCommands"> </param>
        /// <returns></returns>
        /// <remarks>LogError should be thrown in the event of errors</remarks>
        protected abstract int InternalExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands);

        internal ManagementScope Scope { get; set; }

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

        internal void LogTaskMessage(MessageImportance messageImportance, string message)
        {
            this.LogTaskMessage(messageImportance, message, null);
        }

        internal void LogTaskMessage(string message, object[] arguments)
        {
            this.LogTaskMessage(MessageImportance.Normal, message, arguments);
        }

        internal void LogTaskMessage(string message)
        {
            this.LogTaskMessage(MessageImportance.Normal, message, null);
        }

        internal void LogTaskMessage(MessageImportance messageImportance, string message, params object[]? arguments)
        {
            if (!this.SuppressTaskMessages)
            {
                this.Log.LogMessage(messageImportance, message, arguments);
            }
        }

        internal void LogTaskWarning(string message)
        {
            this.Log.LogWarning(message);
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
                this.userPassword.Clear();
                this.userPassword = ConvertToSecureString(value);
            }
        }
    }
}
