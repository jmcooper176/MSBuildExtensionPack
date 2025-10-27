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
namespace Computer
{
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>Backup</i> ( <b>Required:</b> LogName, BackupPath <b>Optional:</b> MachineName)</para>
    /// <para><i>CheckExists</i> ( <b>Required:</b> LogName <b>Optional:</b> MachineName <b>Output:</b> Exists)</para>
    /// <para><i>Clear</i> ( <b>Required:</b> LogName <b>Optional:</b> MachineName)</para>
    /// <para>
    /// <i>Create</i> ( <b>Required:</b> LogName <b>Optional:</b> MaxSize, Retention, MachineName, CategoryCount,
    /// MessageResourceFile, CategoryResourceFile, ParameterResourceFile)
    /// </para>
    /// <para><i>Delete</i> ( <b>Required:</b> LogName <b>Optional:</b> MachineName)</para>
    /// <para><i>Modify</i> ( <b>Required:</b> LogName <b>Optional:</b> MaxSize, Retention, MachineName)</para>
    /// <para><b>Remote Execution Support:</b> Yes</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" DefaultTargets="Default" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<Target Name="Default">
    ///<!-- Backup an eventlog -->
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Backup" LogName="Security" BackupPath="C:\Securitybackup.evt"/>
    ///<!-- Delete an eventlog -->
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Delete" LogName="DemoEventLog"/>
    ///<!-- Check whether an eventlog exists -->
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="CheckExists" LogName="DemoEventLog">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.EventLog>
    ///<Message Text="DemoEventLog Exists: $(DoesExist)"/>
    ///<!-- Create whether an eventlog -->
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Create" LogName="DemoEventLog"  MaxSize="20" Retention="14"/>
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="CheckExists" LogName="DemoEventLog">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.EventLog>
    ///<Message Text="DemoEventLog Exists: $(DoesExist)"/>
    ///<!-- Various other quick tasks -->
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Clear" LogName="DemoEventLog"/>
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Modify" LogName="DemoEventLog"  MaxSize="55" Retention="25"/>
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="Delete" LogName="DemoEventLog"/>
    ///<MSBuild.ExtensionPack.Computer.EventLog TaskAction="CheckExists" LogName="DemoEventLog">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.EventLog>
    ///<Message Text="Exists: $(DoesExist)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class EventLog : BaseTask
    {
        private const string BackupTaskAction = "Backup";
        private const string CheckExistsTaskAction = "CheckExists";
        private const string ClearTaskAction = "Clear";
        private const string CreateTaskAction = "Create";
        private const string DeleteTaskAction = "Delete";
        private const string ModifyTaskAction = "Modify";

        private void Backup()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Backup EventLog: {0}", this.LogName);

            // check the backup path.
            if (string.IsNullOrEmpty(this.BackupPath))
            {
                this.Log.LogTaskError("Invalid BackupPath Supplied");
                return;
            }

            // check if the eventlog exists
            if (System.Diagnostics.EventLog.SourceExists(this.LogName))
            {
                // check if the file to backup to exists
                if (System.IO.File.Exists(this.BackupPath))
                {
                    // First make sure the file is writable.
                    FileAttributes fileAttributes = System.IO.File.GetAttributes(this.BackupPath);

                    // If readonly attribute is set, reset it.
                    if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        // make it readable.
                        System.IO.File.SetAttributes(this.BackupPath, fileAttributes ^ FileAttributes.ReadOnly);

                        // delete it
                        System.IO.File.Delete(this.BackupPath);
                    }
                }

                ConnectionOptions options = new ConnectionOptions
                {
                    Username = this.UserName,
                    Password = this.UserPassword,
                    Authority = this.Authority,
                    EnablePrivileges = true
                };

                // set the scope
                this.GetManagementScope(@"\root\cimv2", options);

                // set the query
                SelectQuery query = new SelectQuery("Select * from Win32_NTEventLogFile where LogFileName='" + this.LogName + "'");

                // configure the searcher and execute a get
                using (ManagementObjectSearcher search = new ManagementObjectSearcher(this.Scope, query))
                {
                    foreach (ManagementObject obj in search.Get())
                    {
                        object[] path = { this.BackupPath };
                        obj.InvokeMethod("BackupEventLog", path);
                    }
                }
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid LogName Supplied: {0}", this.LogName));
            }
        }

        private void CheckExists()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Checking EventLog exists: {0} on: {1}", this.LogName, this.MachineName);
            this.Exists = System.Diagnostics.EventLog.Exists(this.LogName, this.MachineName);
        }

        private void Clear()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Clearing EventLog: {0}", this.LogName);
            if (System.Diagnostics.EventLog.Exists(this.LogName, this.MachineName))
            {
                using (System.Diagnostics.EventLog targetLog = new System.Diagnostics.EventLog(this.LogName, this.MachineName))
                {
                    targetLog.Clear();
                }
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid LogName Supplied: {0}", this.LogName));
            }
        }

        private void ConfigureEventLog(System.Diagnostics.EventLog el)
        {
            if (this.MaxSize > 0)
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Setting EventLog Size: {0}Mb", this.MaxSize);
                el.MaximumKilobytes = this.MaxSize * 1024;
            }

            if (this.Retention > 0)
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Setting Retention: {0} days", this.Retention);
                el.ModifyOverflowPolicy(OverflowAction.OverwriteOlder, this.Retention);
            }
            else if (this.Retention == -1)
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Setting Retention to 'Overwrite As Needed'");
                el.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
            }
            else if (this.Retention == -2)
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Setting Retention to 'Do Not Overwrite'");
                el.ModifyOverflowPolicy(OverflowAction.DoNotOverwrite, 0);
            }
        }

        private void Create()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Creating EventLog: {0} on: {1}", this.LogName, this.MachineName);
            if (!System.Diagnostics.EventLog.Exists(this.LogName, this.MachineName))
            {
                EventSourceCreationData ecd = new EventSourceCreationData(this.LogName, this.LogName)
                {
                    MachineName = this.MachineName,
                    CategoryCount = this.CategoryCount,
                    MessageResourceFile = this.MessageResourceFile ?? string.Empty,
                    CategoryResourceFile = this.CategoryResourceFile ?? string.Empty,
                    ParameterResourceFile = this.ParameterResourceFile ?? string.Empty
                };

                System.Diagnostics.EventLog.CreateEventSource(ecd);

                using (System.Diagnostics.EventLog el = new System.Diagnostics.EventLog(this.LogName, this.MachineName))
                {
                    this.ConfigureEventLog(el);
                }
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "EventLog already exists: {0} on: {1}", this.LogName, this.MachineName));
            }
        }

        private void Delete()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Deleting EventLog: {0} on: {1}", this.LogName, this.MachineName);
            if (System.Diagnostics.EventLog.Exists(this.LogName, this.MachineName))
            {
                System.Diagnostics.EventLog.Delete(this.LogName, this.MachineName);
            }
        }

        private void Modify()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Modifying EventLog: {0} on {1}", this.LogName, this.MachineName);
            if (System.Diagnostics.EventLog.Exists(this.LogName, this.MachineName))
            {
                using (System.Diagnostics.EventLog el = new System.Diagnostics.EventLog(this.LogName, this.MachineName))
                {
                    this.ConfigureEventLog(el);
                }
            }
            else
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "EventLog does not exist: {0}", this.LogName));
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            switch (this.TaskAction)
            {
                case BackupTaskAction:
                    this.Backup();
                    break;

                case CreateTaskAction:
                    this.Create();
                    break;

                case CheckExistsTaskAction:
                    this.CheckExists();
                    break;

                case DeleteTaskAction:
                    this.Delete();
                    break;

                case ClearTaskAction:
                    this.Clear();
                    break;

                case ModifyTaskAction:
                    this.Modify();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Sets the Backup Path
        /// </summary>
        public string BackupPath { get; set; }

        /// <summary>
        /// Sets the number of categories in the category resource file
        /// </summary>
        public int CategoryCount { get; set; }

        /// <summary>
        /// Sets the path of the category resource file to write events with localized category strings
        /// </summary>
        public string CategoryResourceFile { get; set; }

        /// <summary>
        /// Gets a value indicating whether the event log exists.
        /// </summary>
        [Output]
        public bool Exists { get; set; }

        /// <summary>
        /// Sets the name of the Event Log
        /// </summary>
        [Required]
        public string LogName { get; set; }

        /// <summary>
        /// Sets the size of the max.
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// Sets the path of the message resource file to configure an event log source to write localized event messages
        /// </summary>
        public string MessageResourceFile { get; set; }

        /// <summary>
        /// Sets the path of the parameter resource file to configure an event log source to write localized event messages with
        /// inserted parameter strings
        /// </summary>
        public string ParameterResourceFile { get; set; }

        /// <summary>
        /// Sets the retention. Any value &gt; 0 is interpreted as days to retain. Use -1 for 'Overwrite as needed'. Use -2 for
        /// 'Never Overwrite'
        /// </summary>
        public int Retention { get; set; }
    }
}
