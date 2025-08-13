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

namespace MSBuild.ExtensionPack
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Utility;

    using System;
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// Provides a common task for all the MSBuildExtensionPack Tasks
    /// </summary>
    public abstract class BaseTask : Task
    {
        #region Private Fields

        private AuthenticationLevel authenticationLevel = System.Management.AuthenticationLevel.Default;
        private string? machineName;

        #endregion Private Fields

        #region Private Methods

        private void DetermineLogging()
        {
            string? s = Environment.GetEnvironmentVariable("SuppressTaskMessages", EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(s))
            {
                this.SuppressTaskMessages = Convert.ToBoolean(s, CultureInfo.CurrentCulture);
            }
        }

        private void GetExceptionLevel()
        {
            string? s = Environment.GetEnvironmentVariable("LogExceptionStack", EnvironmentVariableTarget.Machine);

            if (!string.IsNullOrEmpty(s))
            {
                this.LogExceptionStack = Convert.ToBoolean(s, CultureInfo.CurrentCulture);
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// This is the main InternalExecute method that all tasks should implement
        /// </summary>
        /// <remarks>LogError should be thrown in the event of errors</remarks>
        protected abstract void InternalExecute();

        #endregion Protected Methods

        #region Internal Properties

        internal ManagementScope? Scope { get; set; }

        #endregion Internal Properties

        #region Internal Methods

        internal void GetManagementScope(string wmiNamespace)
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "ManagementScope Set: {0}", "\\\\" + this.MachineName + wmiNamespace));
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
            this.Log.LogTaskMessage(() => true, MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "ManagementScope Set: {0}", "\\\\" + this.MachineName + wmiNamespace));
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
            set { this.authenticationLevel = (AuthenticationLevel)Enum.Parse<AuthenticationLevel>(value); }
        }

        /// <summary>
        /// Sets the authority to be used to authenticate the specified user.
        /// </summary>
        public string? Authority { get; set; }

        /// <summary>
        /// Set to true to error if the task has been deprecated
        /// </summary>
        public bool ErrorOnDeprecated { get; set; }

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
        public virtual string? TaskAction { get; set; }

        /// <summary>
        /// Sets the UserName
        /// </summary>
        public virtual string? UserName { get; set; }

        /// <summary>
        /// Sets the UserPassword.
        /// </summary>
        public virtual string? UserPassword { get; set; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}
