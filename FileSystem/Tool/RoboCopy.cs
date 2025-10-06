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
namespace MSBuild.ExtensionPack.FileSystem.Tool
{
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// This task wraps RoboCopy. Successful non-zero exit codes from RoboCopy are set to zero to not break MSBuild. Use the
    /// ReturnCode property to access the exit code from RoboCopy
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
    ///<MSBuild.ExtensionPack.FileSystem.RoboCopy Source="C:\b" Destination="C:\bbzz" Files="*.*" Options="/MIR">
    ///<Output TaskParameter="ExitCode" PropertyName="Exit" />
    ///<Output TaskParameter="ReturnCode" PropertyName="Return" />
    ///</MSBuild.ExtensionPack.FileSystem.RoboCopy>
    ///<Message Text="ExitCode = $(Exit)"/>
    ///<Message Text="ReturnCode = $(Return)"/>
    ///<MSBuild.ExtensionPack.FileSystem.RoboCopy Source="C:\a" Destination="C:\abzz" Files="*.txt" Options="/e">
    ///<Output TaskParameter="ExitCode" PropertyName="Exit" />
    ///<Output TaskParameter="ReturnCode" PropertyName="Return" />
    ///</MSBuild.ExtensionPack.FileSystem.RoboCopy>
    ///<Message Text="ExitCode = $(Exit)"/>
    ///<Message Text="ReturnCode = $(Return)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class RoboCopy : ToolTask
    {
        #region Protected Properties

        protected override string ToolName => "RoboCopy.exe";

        #endregion Protected Properties

        #region Protected Methods

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            this.Log.LogMessage("Running " + pathToTool + " " + commandLineCommands);
            int retVal = base.ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
            this.ReturnCode = retVal;
            switch (retVal)
            {
                case 0:
                    this.Log.LogMessage("Return Code 0. No errors occurred, and no copying was done. The source and destination directory trees are completely synchronized.");
                    break;

                case 1:
                    this.Log.LogMessage("Return Code 1. One or more files were copied successfully (that is, new files have arrived).");
                    retVal = 0;
                    break;

                case 2:
                    this.Log.LogMessage("Return Code 2. Some Extra files or directories were detected. Examine the output log. Some housekeeping may be needed.");
                    retVal = 0;
                    break;

                case 3:
                    this.Log.LogMessage("Return Code 3. One or more files were copied successfully (that is, new files have arrived). Some Extra files or directories were detected. Examine the output log. Some housekeeping may be needed.");
                    retVal = 0;
                    break;

                case 4:
                    this.Log.LogMessage("Return Code 4. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary.");
                    retVal = 0;
                    break;

                case 5:
                    this.Log.LogMessage("Return Code 5. One or more files were copied successfully (that is, new files have arrived). Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary.");
                    retVal = 0;
                    break;

                case 6:
                    this.Log.LogMessage("Return Code 6. Some Extra files or directories were detected. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary.");
                    retVal = 0;
                    break;

                case 7:
                    this.Log.LogMessage("Return Code 7. One or more files were copied successfully (that is, new files have arrived). Some Extra files or directories were detected. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary.");
                    retVal = 0;
                    break;

                case 8:
                    this.Log.LogError("Return Code 8. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 9:
                    this.Log.LogError("Return Code 9. One or more files were copied successfully (that is, new files have arrived). Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 10:
                    this.Log.LogError("Return Code 10. Some Extra files or directories were detected. Examine the output log. Some housekeeping may be needed. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 11:
                    this.Log.LogError("Return Code 11. One or more files were copied successfully (that is, new files have arrived). Some Extra files or directories were detected. Examine the output log. Some housekeeping may be needed. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 12:
                    this.Log.LogError("Return Code 12. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 13:
                    this.Log.LogError("Return Code 13. One or more files were copied successfully (that is, new files have arrived). Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 14:
                    this.Log.LogError("Return Code 14. Some Extra files or directories were detected. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 15:
                    this.Log.LogError("Return Code 15. One or more files were copied successfully (that is, new files have arrived). Some Extra files or directories were detected. Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary. Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.");
                    break;

                case 16:
                    this.Log.LogError("Return Code 16. Serious error. RoboCopy did not copy any files. This is either a usage error or an error due to insufficient access privileges on the source or destination directories.");
                    break;
            }

            return retVal;
        }

        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder builder = new CommandLineBuilder();
            builder.AppendFileNameIfNotNull(this.Source);
            builder.AppendFileNameIfNotNull(this.Destination);
            builder.AppendFileNamesIfNotNull(this.Files, " ");
            if (!string.IsNullOrEmpty(this.Options))
            {
                builder.AppendSwitch(this.Options);
            }

            return builder.ToString();
        }

        protected override string GenerateFullPathToTool()
        {
            return string.IsNullOrEmpty(this.ToolPath) ? this.ToolName : Path.Combine(this.ToolPath, this.ToolName);
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
        /// Destination Dir (drive:\path or \\server\share\path).
        /// </summary>
        [Required]
        public ITaskItem Destination { get; set; }

        /// <summary>
        /// File(s) to copy (names/wildcards: default is "*.*").
        /// </summary>
        [Required]
        public ITaskItem[] Files { get; set; }

        /// <summary>
        /// Set to true to log output to the console. Default is false
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Type 'robocopy.exe /?' at the command prompt for all available options
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// Gets the Return Code from RoboCopy
        /// </summary>
        [Output]
        public int ReturnCode { get; set; }

        /// <summary>
        /// Source Directory (drive:\path or \\server\share\path).
        /// </summary>
        [Required]
        public ITaskItem Source { get; set; }

        #endregion Public Properties
    }
}
