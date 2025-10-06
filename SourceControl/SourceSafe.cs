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
namespace MSBuild.ExtensionPack.VisualStudio
{
    using System;
    using System.Globalization;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Win32;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// Please be aware that this is a very light wrapper for ss.exe
    /// <para/>
    /// See the Command Line Reference on MSDN (http://msdn.microsoft.com/en-us/library/003ssz4z(VS.80).aspx) for full details.
    /// <para/>
    /// <b>Sample TaskActions are as follows, HOWEVER use the URL above for full guidance and additional TaskActions:</b>
    /// <para><i>Checkout</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Checkin</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Cloak</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Create</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Decloak</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Delete</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><i>Destroy</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, SSVersion, SuppressI)</para>
    /// <para><i>Get</i> ( <b>Required:</b> FilePath <b>Optional:</b> Arguments, Database, SSVersion, SuppressI)</para>
    /// <para><b>Remote Execution Support:</b> No</para>
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
    ///<!-- Perfrom various source control operations -->
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Get" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd" Arguments="-GLC:\Demo2 -R"/>
    ///<!-- This will perform a recursive checkout to C:\Demo2 -->
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Checkout" Database="C:\SourceSafe" UserName="AUser" FilePath="$//DemoFtd/*.*" Arguments="-GLC:\Demo2 -R" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Checkin" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd/*.*" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Checkout" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd/dts.wav" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Checkin" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd/dts.wav" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Cloak" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Decloak" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Create" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd22" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Delete" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd22" Arguments="-I-Y" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.VisualStudio.SourceSafe TaskAction="Destroy" Database="C:\SourceSafe\2005" UserName="AUser" FilePath="$//DemoFtd22" Arguments="-I-Y" ContinueOnError="true"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class SourceSafe : BaseTask
    {
        #region Private Fields

        private string fileName = "ss.exe";
        private ShellWrapper shellWrapper;

        #endregion Private Fields

        #region Private Methods

        private void ExecuteVisualSourceSafe(string args)
        {
            this.shellWrapper.Arguments = args;

            int returncode = this.shellWrapper.Execute();
            if (string.IsNullOrEmpty(this.shellWrapper.StandardOutput.Trim()) == false)
            {
                this.LogTaskMessage(this.shellWrapper.StandardOutput);
            }

            if (string.IsNullOrEmpty(this.shellWrapper.StandardError.Trim()) == false)
            {
                this.LogTaskMessage(this.shellWrapper.StandardError);
            }

            if (returncode != 0)
            {
                this.Log.LogError(this.shellWrapper.StandardError + "(" + returncode + ")");
            }
        }

        private bool GetVersionInformation()
        {
            this.LogTaskMessage(MessageImportance.Low, "Getting version information");
            RegistryKey vssKey;
            switch (this.SSVersion)
            {
                case "6d":
                    vssKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\6.0\Setup\Microsoft Visual SourceSafe Server") ??
                             Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\6.0\Setup\Microsoft Visual SourceSafe");
                    if (vssKey is not null)
                    {
                        string pathToSourceSafe = Convert.ToString(vssKey.GetValue("ProductDir"), CultureInfo.CurrentCulture);
                        this.fileName = Path.Combine(pathToSourceSafe, @"win32\ss.exe");
                        vssKey.Close();
                    }

                    break;

                case "2005":
                    vssKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS\VSS");
                    if (vssKey is not null)
                    {
                        string pathToSourceSafe = Convert.ToString(vssKey.GetValue("ProductDir"), CultureInfo.CurrentCulture);
                        this.fileName = Path.Combine(pathToSourceSafe, "ss.exe");
                        vssKey.Close();
                    }

                    break;

                default:
                    this.Log.LogError("Invalid SSVersion. Valid options are 6d or 2005");
                    return false;
            }

            this.shellWrapper = new ShellWrapper(this.fileName);

            if (this.Database is not null)
            {
                this.shellWrapper.EnvironmentVariables.Add("SSDIR", this.Database.GetMetadata("FullPath"));
            }

            if (string.IsNullOrEmpty(this.UserName))
            {
                this.UserName = Environment.UserName;
            }

            this.shellWrapper.EnvironmentVariables.Add("SSUSER", this.UserName);
            if (!string.IsNullOrEmpty(this.UserPassword))
            {
                this.shellWrapper.EnvironmentVariables.Add("SSPWD", this.UserPassword);
            }

            this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "Using UserName: {0} and Password: {1}", this.UserName, this.UserPassword));
            return true;
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            if (this.GetVersionInformation() == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.Arguments))
            {
                if (!this.SuppressI)
                {
                    this.Arguments = "-I-";
                }
            }
            else
            {
                if (this.Arguments.IndexOf("-I-", StringComparison.Ordinal) < 0 && !this.SuppressI)
                {
                    this.Arguments += " -I-";
                }
            }

            string args = string.Format(CultureInfo.CurrentCulture, "{0} \"{1}\" {2}", this.TaskAction, this.FilePath, this.Arguments);
            this.LogTaskMessage(MessageImportance.Low, string.Format(CultureInfo.CurrentCulture, "Executing: {0} {1}", this.fileName, args));
            this.ExecuteVisualSourceSafe(args);
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Sets the Arguments. Defaults to -I- (Ignores all and tells the command not to ask for input under any circumstances)
        /// unless SuppressI is set to true. See http://msdn.microsoft.com/en-us/library/hsxzf2az(VS.80).aspx for full options.
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Sets the database.
        /// </summary>
        public ITaskItem Database { get; set; }

        /// <summary>
        /// Sets the FilePath
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// Sets the SourceSafe version. Supports 2005 and 6d. Default is 2005
        /// </summary>
        public string SSVersion { get; set; } = "2005";

        /// <summary>
        /// Set to true to prevent -I- being added to your custom Arguments. Default is False
        /// </summary>
        public bool SuppressI { get; set; }

        #endregion Public Properties
    }
}
