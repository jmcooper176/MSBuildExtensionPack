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
namespace MSBuild.ExtensionPack.SqlServer
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    using System.Globalization;

    using SMO = Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>GetConnectionCount</i> ( <b>Optional:</b> NoPooling <b>Output:</b> ConnectionCount)</para>
    /// <para><i>GetInfo</i> ( <b>Optional:</b> NoPooling <b>Output:</b> Information)</para>
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
    ///<!-- Get information for a server, not that this defaults to the default instance on the local server -->
    ///<MSBuild.ExtensionPack.SqlServer.Server TaskAction="GetInfo">
    ///<Output TaskParameter="Information" ItemName="AllInfo"/>
    ///</MSBuild.ExtensionPack.SqlServer.Server>
    ///<!-- All the server information properties are available as metadata on the Information item -->
    ///<Message Text="PhysicalMemory: %(AllInfo.PhysicalMemory)"/>
    ///<!-- Get all the active connections to the server -->
    ///<MSBuild.ExtensionPack.SqlServer.Server TaskAction="GetConnectionCount">
    ///<Output TaskParameter="ConnectionCount" PropertyName="Count"/>
    ///</MSBuild.ExtensionPack.SqlServer.Server>
    ///<Message Text="Server ConnectionCount: $(Count)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class Server : BaseTask
    {
        private SMO.Server sqlServer;
        private bool trustedConnection;

        private void GetConnectionCount()
        {
            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Getting Connection Count for: {0}", this.MachineName));
            foreach (SMO.Database db in this.sqlServer.Databases)
            {
                this.ConnectionCount += this.sqlServer.GetActiveDBConnectionCount(db.Name);
            }
        }

        private void GetInfo()
        {
            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Getting Information for: {0}", this.MachineName));
            this.Information = new TaskItem(this.MachineName);
            foreach (Property prop in this.sqlServer.Information.Properties)
            {
                this.Information.SetMetadata(prop.Name, prop.Value?.ToString() ?? string.Empty);
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                this.LogTaskMessage(MessageImportance.Low, "Using a Trusted Connection");
                this.trustedConnection = true;
            }

            ServerConnection con = new() { LoginSecure = this.trustedConnection, ServerInstance = this.MachineName, NonPooledConnection = this.NoPooling };
            if (!string.IsNullOrEmpty(this.UserName))
            {
                con.Login = this.UserName;
            }

            if (!string.IsNullOrEmpty(this.UserPassword))
            {
                con.Password = this.UserPassword;
            }

            this.sqlServer = new SMO.Server(con);

            switch (this.TaskAction)
            {
                case "GetInfo":
                    this.GetInfo();
                    break;

                case "GetConnectionCount":
                    this.GetConnectionCount();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }

            // Release the connection if we are not using pooling.
            if (this.NoPooling)
            {
                this.sqlServer.ConnectionContext.Disconnect();
            }
        }

        /// <summary>
        /// Gets the number of connections the server has open
        /// </summary>
        [Output]
        public int ConnectionCount { get; set; }

        /// <summary>
        /// Gets the Information TaskItem. Each available property is added as metadata.
        /// </summary>
        [Output]
        public ITaskItem Information { get; set; }

        /// <summary>
        /// Set to true to create a NonPooledConnection to the server. Default is false.
        /// </summary>
        public bool NoPooling { get; set; }
    }
}
