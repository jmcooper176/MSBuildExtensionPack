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
namespace MSBuild.ExtensionPack
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using System;
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// Provides a common task for all the MSBuildExtensionPack Tasks that need to be instantiated in their own app domain.
    /// </summary>
    public abstract class BaseAppDomainIsolatedTask : Task
    {
        private AuthenticationLevel authenticationLevel = System.Management.AuthenticationLevel.Default;
        private string machineName;

        private void DetermineLogging()
        {
            string s = Environment.GetEnvironmentVariable("SuppressTaskMessages", EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(s))
            {
                this.SuppressTaskMessages = Convert.ToBoolean(s, CultureInfo.CurrentCulture);
            }
        }

        private void GetExceptionLevel()
        {
            string s = Environment.GetEnvironmentVariable("LogExceptionStack", EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(s))
            {
                this.LogExceptionStack = Convert.ToBoolean(s, CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// This is the main InternalExecute method that all tasks should implement
        /// </summary>
        /// <remarks>LogError should be thrown in the event of errors</remarks>
        protected abstract void InternalExecute();

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

        internal void LogTaskMessage(MessageImportance messageImportance, string message, object[] arguments)
        {
            if (!this.SuppressTaskMessages)
            {
                if (arguments == null)
                {
                    this.Log.LogMessage(messageImportance, message);
                }
                else
                {
                    this.Log.LogMessage(messageImportance, message, arguments);
                }
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
        public virtual string UserPassword { get; set; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>bool</returns>
        public override sealed bool Execute()
        {
            this.DetermineLogging();
            try
            {
                this.InternalExecute();
                return !this.Log.HasLoggedErrors;
            }
            catch (Exception ex)
            {
                this.GetExceptionLevel();
                this.Log.LogErrorFromException(ex, this.LogExceptionStack, true, null);
                return !this.Log.HasLoggedErrors;
            }
        }
    }
}
