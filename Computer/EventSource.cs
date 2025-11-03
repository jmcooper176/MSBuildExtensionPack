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
namespace MSBuild.ExtensionPack.Computer
{
    using System;
    using System.Globalization;

    using Microsoft.Build.Framework;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>CheckExists</i> ( <b>Required:</b> Source <b>Optional:</b> MachineName <b>Output:</b> Exists)</para>
    /// <para>
    /// <i>Create</i> ( <b>Required:</b> Source, LogName <b>Optional:</b> Force, MachineName, CategoryCount, MessageResourceFile,
    /// CategoryResourceFile, ParameterResourceFile)
    /// </para>
    /// <para><i>Delete</i> ( <b>Required:</b> Source <b>Optional:</b> MachineName)</para>
    /// <para><i>Log</i> ( <b>Required:</b> Source, Description, LogType, EventId, LogName <b>Optional:</b> MachineName)</para>
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
    ///<!-- Delete an event source -->
    ///<MSBuild.ExtensionPack.Computer.EventSource TaskAction="Delete" Source="MyEventSource" LogName="Application"/>
    ///<!-- Check an event source exists -->
    ///<MSBuild.ExtensionPack.Computer.EventSource TaskAction="CheckExists" Source="MyEventSource" LogName="Application">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.EventSource>
    ///<Message Text="Exists: $(DoesExist)"/>
    ///<!-- Create an event source -->
    ///<MSBuild.ExtensionPack.Computer.EventSource TaskAction="Create" Source="MyEventSource" LogName="Application"/>
    ///<MSBuild.ExtensionPack.Computer.EventSource TaskAction="CheckExists" Source="MyEventSource" LogName="Application">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.EventSource>
    ///<Message Text="Exists: $(DoesExist)"/>
    ///<!-- Log an event -->
    ///<MSBuild.ExtensionPack.Computer.EventSource TaskAction="Log" Source="MyEventSource" Description="Hello" LogType="Information" EventId="222"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask" />
    public class EventSource : BaseTask
    {
        private System.Diagnostics.EventLogEntryType logType = System.Diagnostics.EventLogEntryType.Error;

        private void CheckExists()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Checking EventSource exists: {0}", this.Source);
            this.Exists = System.Diagnostics.EventLog.SourceExists(this.Source, this.MachineName);
        }

        private void Create()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Creating EventSource: {0}", this.Source);
            EventSourceCreationData data = new EventSourceCreationData(this.Source, this.LogName)
            {
                MachineName = this.MachineName,
                CategoryCount = this.CategoryCount,
                MessageResourceFile = this.MessageResourceFile ?? string.Empty,
                CategoryResourceFile = this.CategoryResourceFile ?? string.Empty,
                ParameterResourceFile = this.ParameterResourceFile ?? string.Empty
            };

            if (!System.Diagnostics.EventLog.SourceExists(this.Source, this.MachineName))
            {
                System.Diagnostics.EventLog.CreateEventSource(data);
            }
            else
            {
                if (this.Force)
                {
                    this.Log.LogTaskMessage(() => true, MessageImportance.Low, "The event source already exists. Force is true, attempting to delete: {0}", this.Source);
                    System.Diagnostics.EventLog.DeleteEventSource(this.Source, this.MachineName);
                    this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Creating EventSource: {0}", this.Source);
                    System.Diagnostics.EventLog.CreateEventSource(data);
                }
                else
                {
                    this.Log.LogTaskError("The event source already exists. Use Force to delete and create.");
                }
            }
        }

        private void Delete()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Deleting EventSource: {0}", this.Source);
            if (System.Diagnostics.EventLog.SourceExists(this.Source, this.MachineName))
            {
                System.Diagnostics.EventLog.DeleteEventSource(this.Source, this.MachineName);
            }
        }

        private void LogEvent()
        {
            // Validation
            if (string.IsNullOrEmpty(this.EventId))
            {
                this.Log.LogTaskError("EventId must be specified");
                return;
            }

            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Logging to EventSource: {0}", this.Source);

            if (!System.Diagnostics.EventLog.SourceExists(this.Source, this.MachineName))
            {
                this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "The EventSource does not exist: {0} on {1}", this.Source, this.MachineName));
            }
            else
            {
                string logName = this.LogName ?? "Application";
                using System.Diagnostics.EventLog log = new System.Diagnostics.EventLog(logName, this.MachineName, this.Source);
                log.WriteEntry(this.Description, this.logType, int.Parse(this.EventId, CultureInfo.CurrentCulture));
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            switch (this.TaskAction)
            {
                case "Create":
                    this.Create();
                    break;

                case "CheckExists":
                    this.CheckExists();
                    break;

                case "Delete":
                    this.Delete();
                    break;

                case "Log":
                    this.LogEvent();
                    break;

                default:
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Sets the number of categories in the category resource file
        /// </summary>
        public int CategoryCount { get; set; }

        /// <summary>
        /// Sets the path of the category resource file to write events with localized category strings
        /// </summary>
        public string CategoryResourceFile { get; set; }

        /// <summary>
        /// Sets the description for the logentry
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sets the event id.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the EventSource exists.
        /// </summary>
        [Output]
        public bool Exists { get; set; }

        /// <summary>
        /// Set to true to delete any existing matching eventsource when creating
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Sets the name of the log the source's entries are written to, e.g Application, Security, System, YOUREVENTLOG.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Sets the Event Log Entry Type. Possible values are: Error, FailureAudit, Information, SuccessAudit, Warning.
        /// </summary>
        public string LogType
        {
            get => this.logType.ToString();
            set => this.logType = (System.Diagnostics.EventLogEntryType)Enum.Parse(typeof(System.Diagnostics.EventLogEntryType), value);
        }

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
        /// Sets the source name
        /// </summary>
        [Required]
        public string Source { get; set; }
    }
}
