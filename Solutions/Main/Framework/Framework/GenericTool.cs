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

namespace MSBuild.ExtensionPack.Framework
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// This task can be used to wrap any executable
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
    ///<ItemGroup>
    ///<Param Include="Source">
    ///<value>c:\b</value>
    ///</Param>
    ///<Param Include="Destination">
    ///<value>c:\bb 3</value>
    ///</Param>
    ///<Param Include="Files">
    ///<value>*.*</value>
    ///</Param>
    ///<Param Include="Options">
    ///<value>/Mir</value>
    ///</Param>
    ///</ItemGroup>
    ///<MSBuild.ExtensionPack.Framework.GenericTool Executable="robocopy.exe" Parameters="@(Param)" SuccessExitCodes="0;1" WarningExitCodes="2;4" ErrorExitCodes="8;16"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class GenericTool : ToolTask
    {
        #region Protected Properties

        protected override string ToolName => this.Executable.ItemSpec;

        #endregion Protected Properties

        #region Protected Methods

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            this.Log.LogMessage("Running " + pathToTool + " " + commandLineCommands);
            this.RealExitCode = base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);

            if (this.WarningExitCodes is not null)
            {
                if (this.WarningExitCodes.Any(i => this.RealExitCode == Convert.ToInt32(i.ItemSpec, CultureInfo.InvariantCulture)))
                {
                    this.Log.LogWarning(string.Format(CultureInfo.InvariantCulture, "Return Code: {0}. Warning Code. Returning 0.", this.RealExitCode));
                    return 0;
                }
            }

            if (this.ErrorExitCodes is not null)
            {
                if (this.ErrorExitCodes.Any(i => this.RealExitCode == Convert.ToInt32(i.ItemSpec, CultureInfo.InvariantCulture)))
                {
                    this.Log.LogError(string.Format(CultureInfo.InvariantCulture, "Return Code: {0}. Error Code. Returning -1.", this.RealExitCode));
                    return -1;
                }
            }

            if (this.SuccessExitCodes.Any(i => this.RealExitCode == Convert.ToInt32(i.ItemSpec, CultureInfo.InvariantCulture)))
            {
                this.Log.LogMessage(string.Format(CultureInfo.InvariantCulture, "Return Code: {0}. Success Code. Returning 0.", this.RealExitCode));
                return 0;
            }

            this.Log.LogError(string.Format(CultureInfo.InvariantCulture, "Unhandled Return Code: {0}. Returning -1.", this.RealExitCode));
            return -1;
        }

        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder builder = new CommandLineBuilder();
            if (this.Parameters is not null)
            {
                foreach (ITaskItem i in this.Parameters)
                {
                    builder.AppendFileNameIfNotNull(i.GetMetadata("value"));
                }
            }

            return builder.ToString();
        }

        protected override string GenerateFullPathToTool()
        {
            return string.IsNullOrEmpty(this.ToolPath) ? this.ToolName : System.IO.Path.Combine(this.ToolPath, this.ToolName);
        }

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            if (this.LogToConsole)
            {
                this.Log.LogMessage(MessageImportance.Normal, singleLine);
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// The list of Error Exit Codes
        /// </summary>
        [Required]
        public ITaskItem[] ErrorExitCodes { get; set; }

        /// <summary>
        /// The Executable to call
        /// </summary>
        [Required]
        public ITaskItem Executable { get; set; }

        /// <summary>
        /// Set to true to log output to the console. Default is false
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Sets the parameters to pass to the Executable. The parameter should be defined in the 'value' metadata of an Item.
        /// </summary>
        public ITaskItem[] Parameters { get; set; }

        /// <summary>
        /// The real exit code returned from the Executable
        /// </summary>
        [Output]
        public int RealExitCode { get; set; }

        /// <summary>
        /// The list of Success Exit Codes
        /// </summary>
        [Required]
        public ITaskItem[] SuccessExitCodes { get; set; }

        /// <summary>
        /// The list of Warning Exit Codes
        /// </summary>
        [Required]
        public ITaskItem[] WarningExitCodes { get; set; }

        #endregion Public Properties
    }
}
