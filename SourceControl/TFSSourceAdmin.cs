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
namespace SourceControl
{
    using System;
    using System.Globalization;
    using System.IO;

    using Microsoft.Build.Framework;

    using MSBuild.ExtensionPack.Base;
    using MSBuild.ExtensionPack.ErrorMessage.Message;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>Branch</i> ( <b>Required:</b> OldItem, NewItem <b>Optional:</b> Version, WorkingDirectory, VersionSpec <b>Output:</b> ExitCode)
    /// </para>
    /// <para>
    /// <i>Rename</i> ( <b>Required:</b> OldItem, NewItem <b>Optional:</b> Version, WorkingDirectory, VersionSpec <b>Output:</b> ExitCode)
    /// </para>
    /// <para><b>Remote Execution Support:</b> NA</para>
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
    ///<!-- Perfrom various source administration operations -->
    ///<MSBuild.ExtensionPack.VisualStudio.TfsSourceAdmin TaskAction="Branch" OldItem="C:\Projects\SpeedCMMI\Demo" NewItem="C:\Projects\SpeedCMMI\Demo1\B4" WorkingDirectory="C:\projects\SpeedCMMI"/>
    ///<MSBuild.ExtensionPack.VisualStudio.TfsSource TaskAction="Checkin" ItemPath="C:\Projects\SpeedCMMI" WorkingDirectory="C:\projects\SpeedCMMI"/>
    ///<MSBuild.ExtensionPack.VisualStudio.TfsSource TaskAction="Get" ItemPath="C:\Projects\SpeedCMMI" WorkingDirectory="C:\projects\SpeedCMMI"/>
    ///<MSBuild.ExtensionPack.VisualStudio.TfsSourceAdmin TaskAction="Rename" OldItem="C:\Projects\SpeedCMMI\Demo1\B4\VersionNumber.cs" NewItem="C:\Projects\SpeedCMMI\Demo1\B4\VersionNumberNew.cs" WorkingDirectory="C:\projects\SpeedCMMI"/>
    ///<MSBuild.ExtensionPack.VisualStudio.TfsSource TaskAction="Checkin" ItemPath="C:\Projects\SpeedCMMI" WorkingDirectory="C:\projects\SpeedCMMI"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class TfsSourceAdmin : BaseTask
    {
        private ShellWrapper shellWrapper;
        private string teamFoundationExe;

        private void Branch()
        {
            string args = string.Format(CultureInfo.CurrentCulture, "\"{0}\" \"{1}\" /noprompt /noget", this.OldItem, this.NewItem);
            if (!string.IsNullOrEmpty(this.VersionSpec))
            {
                args += " /version:" + "\"" + this.VersionSpec + "\"";
            }

            this.ExecuteCommand("branch", args);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="action"> The action.</param>
        /// <param name="options">The options.</param>
        private void ExecuteCommand(string action, string options)
        {
            string arguments = string.Format(CultureInfo.CurrentCulture, "{0} {1}", action, options);

            this.shellWrapper = new ShellWrapper(this.teamFoundationExe, arguments);
            if (string.IsNullOrEmpty(this.WorkingDirectory) == false)
            {
                this.shellWrapper.WorkingDirectory = this.WorkingDirectory;
                this.Log.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "WorkingDirectory set to: {0}", this.WorkingDirectory));
            }

            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Executing {0} with {1}", this.shellWrapper.Executable, arguments));
            this.ExitCode = this.shellWrapper.Execute();
            this.Log.LogTaskMessage(MessageImportance.Low, this.shellWrapper.StandardOutput);
            this.SwitchReturnValue(this.shellWrapper.StandardError.Trim());
        }

        private void Rename()
        {
            string args = string.Format(CultureInfo.CurrentCulture, "\"{0}\" \"{1}\"", this.OldItem, this.NewItem);
            this.ExecuteCommand("rename ", args);
        }

        private void ResolveExePath()
        {
            this.Log.LogTaskMessage(MessageImportance.Low, "Resolve TF.exe path");

            string vstools = string.Empty;
            switch (this.Version)
            {
                case "2015":
                    vstools = Environment.GetEnvironmentVariable("VS140COMNTOOLS");
                    break;

                case "2013":
                    vstools = Environment.GetEnvironmentVariable("VS120COMNTOOLS");
                    break;

                case "2012":
                    vstools = Environment.GetEnvironmentVariable("VS110COMNTOOLS");
                    break;

                case "2010":
                    vstools = Environment.GetEnvironmentVariable("VS100COMNTOOLS");
                    break;

                case "2008":
                    vstools = Environment.GetEnvironmentVariable("VS90COMNTOOLS");
                    break;

                case "2005":
                    vstools = Environment.GetEnvironmentVariable("VS80COMNTOOLS");
                    break;
            }

            if (!string.IsNullOrEmpty(vstools))
            {
                this.teamFoundationExe = Path.Combine(vstools, @"..\IDE\tf.exe");
                this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "TF.exe path resolved to: {0}", this.teamFoundationExe));
            }

            if (!File.Exists(this.teamFoundationExe))
            {
                this.teamFoundationExe = "tf.exe";
                this.Log.LogTaskMessage("Unable to resolve TF.exe path. Assuming it is in the PATH environment variable.");
            }
        }

        private void SwitchReturnValue(string error)
        {
            switch (this.ExitCode)
            {
                case 1:
                    this.Log.LogTaskWarning("Exit Code 1. Partial success: " + error);
                    break;

                case 2:
                    this.Log.LogTaskError("Exit Code 2. Unrecognized command: " + error);
                    break;

                case 100:
                    this.Log.LogTaskError("Exit Code 100. Nothing Succeeded: " + error);
                    break;
            }
        }

        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            this.ResolveExePath();
            this.Log.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "TF Operation: {0}", this.TaskAction));
            switch (this.TaskAction)
            {
                case "Branch":
                    this.Branch();
                    break;

                case "Rename":
                    this.Rename();
                    break;

                default:
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Gets the ExitCode
        /// </summary>
        [Output]
        public int ExitCode { get; set; }

        /// <summary>
        /// ItemSpec to branch too
        /// </summary>
        public string NewItem { get; set; }

        /// <summary>
        /// ItemSpec to branch
        /// </summary>
        public string OldItem { get; set; }

        /// <summary>
        /// Sets the version of Tfs. Default is 2013
        /// </summary>
        public string Version { get; set; } = "2013";

        /// <summary>
        /// Sets the version spec for Branch
        /// </summary>
        public string VersionSpec { get; set; }

        /// <summary>
        /// Sets the working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }
    }
}
