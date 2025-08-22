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

namespace MSBuild.ExtensionPack.VisualStudio
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;

    using MSBuild.ExtensionPack.Utility;

    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// This task provides a lightweight wrapper over Devenv.exe
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
    ///<MSBuild.ExtensionPack.VisualStudio.VSDevEnv FilePath="C:\a New Folder\WindowsFormsApplication1.sln" Configuration="Debug|Any CPU" Rebuild="true">
    ///<Output TaskParameter="ExitCode" PropertyName="Exit" />
    ///</MSBuild.ExtensionPack.VisualStudio.VSDevEnv>
    ///<Message Text="ExitCode: $(Exit)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class VSDevEnv : ToolTask
    {
        #region Protected Properties

        protected override string ToolName => "devenv.exe";

        #endregion Protected Properties

        #region Protected Methods

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            this.Log.LogMessage("Running " + pathToTool + " " + commandLineCommands);
            return base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
        }

        protected override string GenerateCommandLineCommands()
        {
            DirectoryInfo outputFolder;
            FileInfo outputfile;
            if (this.OutputFolder is null)
            {
                if (this.OutputFile is null)
                {
                    outputFolder = new DirectoryInfo(this.FilePath.GetMetadata("RootDir") + this.FilePath.GetMetadata("Directory") + @"\Output");
                    outputfile = new FileInfo(outputFolder.FullName + string.Format(CultureInfo.InvariantCulture, @"\{0}.{1}.txt", this.FilePath.GetMetadata("Filename"), this.Configuration.Replace("|", " ")));
                }
                else
                {
                    outputfile = new FileInfo(this.OutputFile.ItemSpec);
                }
            }
            else
            {
                outputFolder = new DirectoryInfo(this.OutputFolder.ItemSpec);
                outputfile = this.OutputFile is null ? new FileInfo(outputFolder.FullName + string.Format(CultureInfo.InvariantCulture, @"\{0}.{1}.txt", this.FilePath.GetMetadata("Filename"), this.Configuration.Replace("|", " "))) : new FileInfo(outputFolder.FullName + @"\" + this.OutputFile.GetMetadata("FileName") + this.OutputFile.GetMetadata("Extension"));
            }

            if (outputfile.Exists)
            {
                outputfile.Delete();
            }

            CommandLineBuilder builder = new CommandLineBuilder();
            builder.AppendSwitch(this.Rebuild ? "/Rebuild" : "/Build");
            builder.AppendSwitch("\"" + this.Configuration + "\"");
            builder.AppendSwitch("/out \"" + outputfile.FullName + "\"");
            builder.AppendSwitch("\"" + this.FilePath.GetMetadata("FullPath") + "\"");
            return builder.ToString();
        }

        protected override string GenerateFullPathToTool()
        {
            using (RegistryKey sw = Logging.SoftwareRegistry32Bit)
            {
                RegistryKey key = sw?.OpenSubKey(@"Microsoft\VisualStudio\" + this.Version);
                if (key is not null)
                {
                    string path = Convert.ToString(key.GetValue("InstallDir"), CultureInfo.InvariantCulture);
                    key.Close();
                    return System.IO.Path.Combine(path, this.ToolName);
                }
            }

            throw new Exception(string.Format(CultureInfo.InvariantCulture, "Visual Studio Registry Key not found: {0}", @"SOFTWARE\Microsoft\VisualStudio\" + this.Version));
        }

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            this.Log.LogMessage(MessageImportance.Normal, singleLine);
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// The Configuration to Build.
        /// </summary>
        [Required]
        public string Configuration { get; set; }

        /// <summary>
        /// The Path to the solution or Project to build
        /// </summary>
        [Required]
        public ITaskItem FilePath { get; set; }

        /// <summary>
        /// Specifies the File to log all output to. Defaults to the [Path.Dir]\Output\[Path.FileName].[Configuration].txt
        /// </summary>
        public ITaskItem OutputFile { get; set; }

        /// <summary>
        /// Specifies the output folder to log to. Default is [Path.Dir]\Output\
        /// </summary>
        public ITaskItem OutputFolder { get; set; }

        /// <summary>
        /// Specifies whether Clean and then build the solution or project with the specified configuration. Default is false
        /// </summary>
        public bool Rebuild { get; set; }

        /// <summary>
        /// The version of Visual Studio to run, e.g. 8.0, 9.0, 10.0. Default is 9.0
        /// </summary>
        public string Version { get; set; } = "9.0";

        #endregion Public Properties
    }
}
