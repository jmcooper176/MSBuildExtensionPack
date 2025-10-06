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
namespace MSBuild.ExtensionPack
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>AddSkipVerification</i> ( <b>Required:</b> PublicKeyToken <b>Optional:</b> ToolPath)</para>
    /// <para><i>RemoveAllSkipVerification</i> ( <b>Optional:</b> ToolPath)</para>
    /// <para><i>Sign</i> ( <b>Required:</b> Assemblies, KeyFile <b>Optional:</b> ToolPath)</para>
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
    ///<ItemGroup>
    ///<AssemblyToSign Include="C:\AnAssembly.dll"/>
    ///</ItemGroup>
    ///<!-- Sign an assembly -->
    ///<MSBuild.ExtensionPack.Framework.Signing TaskAction="Sign" ToolPath="C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin" KeyFile="c:\aPrivateKey.snk" Assemblies="@(AssemblyToSign)"/>
    ///<!-- Add SkipVerification for a public key -->
    ///<MSBuild.ExtensionPack.Framework.Signing TaskAction="AddSkipVerification" ToolPath="C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin" PublicKeyToken="119b85861667ee6a"/>
    ///<!-- Remove all SkipVerification -->
    ///<MSBuild.ExtensionPack.Framework.Signing TaskAction="RemoveAllSkipVerification" ToolPath="C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class Signing : BaseTask
    {
        #region Private Fields

        private const string ToolName = "sn.exe";

        #endregion Private Fields

        #region Private Methods

        private void RemoveAllSkipVerification()
        {
            this.LogTaskMessage("Removing all SkipVerification");
            CommandLineBuilder commandLine = new CommandLineBuilder();
            commandLine.AppendSwitch("-q -Vx");
            this.Run(commandLine.ToString());
        }

        private void Run(string args)
        {
            string fileName = this.ToolPath is not null ? System.IO.Path.Combine(this.ToolPath.GetMetadata("FullPath"), ToolName) : ToolName;
            if (!System.IO.File.Exists(fileName))
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "sn.exe not found: {0}", fileName));
                return;
            }

            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.Arguments = args;
                this.LogTaskMessage(MessageImportance.Low, "Running " + proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);
                proc.Start();
                string outputStream = proc.StandardOutput.ReadToEnd();
                if (outputStream.Length > 0)
                {
                    this.LogTaskMessage(MessageImportance.Low, outputStream);
                }

                string errorStream = proc.StandardError.ReadToEnd();
                if (errorStream.Length > 0)
                {
                    this.Log.LogError(errorStream);
                }

                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    this.Log.LogError("Non-zero exit code from sn.exe: " + proc.ExitCode);
                }
            }
        }

        private void Sign()
        {
            if (this.KeyFile is null)
            {
                this.Log.LogError("KeyFile not supplied");
                return;
            }

            if (!System.IO.File.Exists(this.KeyFile.GetMetadata("FullPath")))
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "KeyFile not found: {0}", this.KeyFile.GetMetadata("FullPath")));
                return;
            }

            if (this.Assemblies is null)
            {
                this.Log.LogError("Assemblies not supplied");
                return;
            }

            foreach (ITaskItem assembly in this.Assemblies)
            {
                FileInfo fi = new FileInfo(assembly.ItemSpec);
                if (fi.Exists)
                {
                    this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Signing Assembly: {0}", assembly.ItemSpec));

                    CommandLineBuilder commandLine = new CommandLineBuilder();
                    commandLine.AppendSwitch("-q -R");
                    commandLine.AppendFileNameIfNotNull(assembly);
                    commandLine.AppendFileNameIfNotNull(this.KeyFile.GetMetadata("FullPath"));
                    this.Run(commandLine.ToString());
                    commandLine = new CommandLineBuilder();
                    commandLine.AppendSwitch("-vf");
                    commandLine.AppendFileNameIfNotNull(assembly);
                    this.Run(commandLine.ToString());
                }
                else
                {
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Assembly not found: {0}", assembly.ItemSpec));
                }
            }
        }

        private void SkipVerification()
        {
            if (string.IsNullOrEmpty(this.PublicKeyToken))
            {
                this.Log.LogError("PublicKeyToken is required");
            }

            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Adding SkipVerification for: {0}", this.PublicKeyToken));
            CommandLineBuilder commandLine = new CommandLineBuilder();
            commandLine.AppendSwitch("-q -Vr");
            commandLine.AppendSwitch("*," + this.PublicKeyToken);
            this.Run(commandLine.ToString());
        }

        #endregion Private Methods

        #region Protected Methods

        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            switch (this.TaskAction)
            {
                case "Sign":
                    this.Sign();
                    break;

                case "AddSkipVerification":
                    this.SkipVerification();
                    break;

                case "RemoveAllSkipVerification":
                    this.RemoveAllSkipVerification();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Sets the Item Collection of Assemblies to sign
        /// </summary>
        public ITaskItem[] Assemblies { get; set; }

        /// <summary>
        /// Sets the KeyFile to use when Signing the Assemblies
        /// </summary>
        public ITaskItem KeyFile { get; set; }

        /// <summary>
        /// Sets the PublicKeyToken for AddSkipVerification
        /// </summary>
        public string PublicKeyToken { get; set; }

        /// <summary>
        /// Sets the folder path to sn.exe
        /// </summary>
        public ITaskItem ToolPath { get; set; }

        #endregion Public Properties
    }
}
