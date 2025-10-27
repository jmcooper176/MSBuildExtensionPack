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
namespace Compression
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>AddFile</i> ( <b>Required:</b> NewFile, CabFile, CabExePath, ExtractExePath, NewFileDestination)</para>
    /// <para>
    /// <i>Create</i> ( <b>Required:</b> PathToCab or FilesToCab, CabFile, ExePath. <b>Optional:</b> PreservePaths, StripPrefixes, Recursive)
    /// </para>
    /// <para><i>Extract</i> ( <b>Required:</b> CabFile, ExtractExePath, ExtractTo <b>Optional:</b> ExtractFile)</para>
    /// <para><b>Compatible with:</b></para>
    /// <para>Microsoft (R) Cabinet Tool (cabarc.exe) - Version 5.2.3790.0</para>
    /// <para>Microsoft (R) CAB File Extract Utility (extrac32.exe)- Version 5.2.3790.0</para>
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
    ///<ItemGroup>
    ///<!-- Create a collection of files to CAB -->
    ///<Files Include="C:\ddd\**\*"/>
    ///</ItemGroup>
    ///<!-- Create the CAB using the File collection and preserve the paths whilst stripping a prefix -->
    ///<MSBuild.ExtensionPack.Compression.Cab TaskAction="Create" FilesToCab="@(Files)" CabExePath="D:\BuildTools\CabArc.Exe" CabFile="C:\newcabbyitem.cab" PreservePaths="true" StripPrefixes="ddd\"/>
    ///<!-- Create the same CAB but this time based on the Path. Note that Recursive is required -->
    ///<MSBuild.ExtensionPack.Compression.Cab TaskAction="Create" PathToCab="C:\ddd" CabExePath="D:\BuildTools\CabArc.Exe" CabFile="C:\newcabbypath.cab" PreservePaths="true" StripPrefixes="ddd\" Recursive="true"/>
    ///<!-- Add a file to the CAB -->
    ///<MSBuild.ExtensionPack.Compression.Cab TaskAction="AddFile" NewFile="c:\New Text Document.txt" CabExePath="D:\BuildTools\CabArc.Exe" ExtractExePath="D:\BuildTools\Extrac32.EXE" CabFile="C:\newcabbyitem.cab" NewFileDestination="\Any Path"/>
    ///<!-- Extract a CAB-->
    ///<MSBuild.ExtensionPack.Compression.Cab TaskAction="Extract" ExtractTo="c:\a111" ExtractExePath="D:\BuildTools\Extrac32.EXE" CabFile="C:\newcabbyitem.cab"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseToolTask"/>
    public class Cab : BaseToolTask
    {
        /// <summary>
        /// Adds the file.
        /// </summary>
        private void AddFile()
        {
            // Validation
            if (!this.ValidateExtract())
            {
                return;
            }

            if (!File.Exists(this.NewFile.GetMetadata("FullPath")))
            {
                this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "New File not found: {0}", this.NewFile.GetMetadata("FullPath")));
                return;
            }

            FileInfo f = new(this.NewFile.GetMetadata("FullPath"));

            this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Adding File: {0} to Cab: {1}", this.NewFile.GetMetadata("FullPath"), this.CabFile.GetMetadata("FullPath"));
            string tempFolderName = Guid.NewGuid() + "\\";

            DirectoryInfo dirInfo = new(Path.Combine(Path.GetTempPath(), tempFolderName));
            Directory.CreateDirectory(dirInfo.FullName);

            if (dirInfo.Exists)
            {
                this.Log.LogTaskMessage(() => dirInfo.Exists, MessageImportance.Low, "Created: {0}", dirInfo.FullName);
            }
            else
            {
                this.Log.LogError("Failed to create temp folder: {0}", dirInfo.FullName);
                return;
            }

            // configure the process we need to run
            using (Process cabProcess = new())
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Extracting Cab: {0}", this.CabFile.GetMetadata("FullPath"));
                cabProcess.StartInfo.FileName = this.ExtractExePath.GetMetadata("FullPath");
                cabProcess.StartInfo.UseShellExecute = true;
                cabProcess.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"/Y /L ""{0}"" ""{1}"" ""{2}""", dirInfo.FullName, this.CabFile.GetMetadata("FullPath"), "/E");
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Calling {0} with {1}", this.ExtractExePath.GetMetadata("FullPath"), cabProcess.StartInfo.Arguments);
                cabProcess.Start();
                cabProcess.WaitForExit();
            }

            Directory.CreateDirectory(dirInfo.FullName + "\\" + this.NewFileDestination);

            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Copying new File: {0} to {1}", this.NewFile, dirInfo.FullName + "\\" + this.NewFileDestination + "\\" + f.Name);
            File.Copy(this.NewFile.GetMetadata("FullPath"), dirInfo.FullName + this.NewFileDestination + @"\" + f.Name, true);

            using (Process cabProcess = new())
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Creating Cab: {0}", this.CabFile.GetMetadata("FullPath"));
                cabProcess.StartInfo.FileName = this.CabExePath.GetMetadata("FullPath");
                cabProcess.StartInfo.UseShellExecute = false;
                cabProcess.StartInfo.RedirectStandardOutput = true;

                StringBuilder options = new();
                options.Append("-r -p");
                options.AppendFormat(CultureInfo.CurrentCulture, " -P \"{0}\"\\", dirInfo.FullName[..^1].Replace(@"C:\", string.Empty));
                cabProcess.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"{0} N ""{1}"" ""{2}""", options, this.CabFile.GetMetadata("FullPath"), "\"" + dirInfo.FullName + "*.*\" ");
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Calling {0} with {1}", this.CabExePath.GetMetadata("FullPath"), cabProcess.StartInfo.Arguments);

                // start the process
                cabProcess.Start();

                // Read any messages from CABARC...and log them
                string output = cabProcess.StandardOutput.ReadToEnd();
                cabProcess.WaitForExit();

                this.Log.LogTaskMessage(() => output.Contains("Completed successfully"), MessageImportance.Normal, output);
                this.Log.LogTaskError(() => !output.Contains("Completed successfully"), output);
            }

            string dirObject = string.Format(CultureInfo.CurrentCulture, "win32_Directory.Name='{0}'", dirInfo.FullName[..^1]);
            using ManagementObject mdir = new(dirObject);
            this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Deleting Temp Folder: {0}", dirObject);
            mdir.Get();
            ManagementBaseObject outParams = mdir.InvokeMethod("Delete", null, null);

            // ReturnValue should be 0, else failure
            if (outParams is not null)
            {
                if (Convert.ToInt32(outParams.Properties["ReturnValue"].Value, CultureInfo.CurrentCulture) != 0)
                {
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Directory deletion error: ReturnValue: {0}", outParams.Properties["ReturnValue"].Value));
                }
            }
            else
            {
                this.Log.LogError("The ManagementObject call to invoke Delete returned null.");
            }
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        private void Create()
        {
            // Validation
            if (!File.Exists(this.CabExePath.GetMetadata("FullPath")))
            {
                this.Log.LogError("Executable not found: {0}", this.CabExePath.GetMetadata("FullPath"));
                return;
            }

            using (Process cabProcess = new())
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Creating Cab: {0}", this.CabFile.GetMetadata("FullPath"));
                cabProcess.StartInfo.FileName = this.CabExePath.GetMetadata("FullPath");
                cabProcess.StartInfo.UseShellExecute = false;
                cabProcess.StartInfo.RedirectStandardOutput = true;

                StringBuilder options = new();
                if (this.PreservePaths)
                {
                    options.Append("-p");
                }

                if (this.PathToCab is not null && this.Recursive)
                {
                    options.Append(" -r ");
                }

                // Could be more than one prefix to strip...
                if (string.IsNullOrEmpty(this.StripPrefixes) == false)
                {
                    string[] prefixes = this.StripPrefixes.Split(';');
                    foreach (string prefix in prefixes)
                    {
                        options.AppendFormat(CultureInfo.CurrentCulture, " -P {0}", prefix);
                    }
                }

                string files = string.Empty;
                if ((this.FilesToCab is null || this.FilesToCab.Count() == 0) && this.PathToCab is null)
                {
                    this.Log.LogError("FilesToCab or PathToCab must be supplied");
                    return;
                }

                if (this.PathToCab is not null)
                {
                    files = this.PathToCab.GetMetadata("FullPath");
                    if (!files.EndsWith(@"\*", StringComparison.OrdinalIgnoreCase))
                    {
                        files += @"\*";
                    }
                }
                else
                {
                    files = this.FilesToCab.Aggregate(files, (current, file) => current + ("\"" + file.ItemSpec + "\"" + " "));
                }

                cabProcess.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"{0} N ""{1}"" ""{2}""", options, this.CabFile.GetMetadata("FullPath"), files);
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Calling {0} with {1}", this.CabExePath.GetMetadata("FullPath"), cabProcess.StartInfo.Arguments);

                // start the process
                cabProcess.Start();

                // Read any messages from CABARC...and log them
                string output = cabProcess.StandardOutput.ReadToEnd();
                cabProcess.WaitForExit();

                this.Log.LogTaskMessage(() => output.Contains("Completed successfully"), MessageImportance.Low, output);
                this.Log.LogTaskError(() => !output.Contains("Completed successfully"), output);
            }
        }

        /// <summary>
        /// Extracts this instance.
        /// </summary>
        private void Extract()
        {
            // Validation
            if (this.ValidateExtract() == false)
            {
                return;
            }

            if (this.ExtractTo is null)
            {
                this.Log.LogError("ExtractTo required.");
                return;
            }

            // configure the process we need to run
            using (Process cabProcess = new Process())
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Extracting Cab: {0}", this.CabFile.GetMetadata("FullPath"));
                cabProcess.StartInfo.FileName = this.ExtractExePath.GetMetadata("FullPath");
                cabProcess.StartInfo.UseShellExecute = true;
                cabProcess.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"/Y /L ""{0}"" ""{1}"" ""{2}""", this.ExtractTo.GetMetadata("FullPath"), this.CabFile.GetMetadata("FullPath"), this.ExtractFile);
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Calling {0} with {1}", this.ExtractExePath.GetMetadata("FullPath"), cabProcess.StartInfo.Arguments);
                cabProcess.Start();
                cabProcess.WaitForExit();
            }
        }

        /// <summary>
        /// Validates the extract.
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidateExtract()
        {
            // Validation
            if (System.IO.File.Exists(this.CabFile.GetMetadata("FullPath")) == false)
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "CAB file not found: {0}", this.CabFile.GetMetadata("FullPath")));
                return false;
            }

            if (this.ExtractExePath is null)
            {
                if (System.IO.File.Exists(Environment.SystemDirectory + "extrac32.exe"))
                {
                    this.ExtractExePath = new TaskItem();
                    this.ExtractExePath.SetMetadata("FullPath", Environment.SystemDirectory + "extrac32.exe");
                }
                else
                {
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Executable not found: {0}", this.ExtractExePath.GetMetadata("FullPath")));
                    return false;
                }
            }
            else
            {
                if (System.IO.File.Exists(this.ExtractExePath.GetMetadata("FullPath")) == false)
                {
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Executable not found: {0}", this.ExtractExePath.GetMetadata("FullPath")));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            // Resolve TaskAction
            switch (this.TaskAction)
            {
                case "Create":
                    this.Create();
                    break;

                case "Extract":
                    this.Extract();
                    break;

                case "AddFile":
                    this.AddFile();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Sets the path to CabArc.Exe
        /// </summary>
        public ITaskItem CabExePath { get; set; }

        /// <summary>
        /// Sets the CAB file. Required.
        /// </summary>
        [Required]
        public ITaskItem CabFile { get; set; }

        /// <summary>
        /// Sets the path to extrac32.exe
        /// </summary>
        public ITaskItem ExtractExePath { get; set; }

        /// <summary>
        /// Sets the files to extract. Default is /E, which is all.
        /// </summary>
        public string ExtractFile { get; set; } = "/E";

        /// <summary>
        /// Sets the path to extract to
        /// </summary>
        public ITaskItem ExtractTo { get; set; }

        /// <summary>
        /// Sets the files to cab
        /// </summary>
        public IEnumerable<ITaskItem> FilesToCab { get; set; }

        /// <summary>
        /// Sets the new file to add to the Cab File
        /// </summary>
        public ITaskItem NewFile { get; set; }

        /// <summary>
        /// Sets the path to add the file to
        /// </summary>
        public string NewFileDestination { get; set; }

        /// <summary>
        /// Sets the path to cab
        /// </summary>
        public ITaskItem PathToCab { get; set; }

        /// <summary>
        /// Sets a value indicating whether [preserve paths]
        /// </summary>
        public bool PreservePaths { get; set; }

        /// <summary>
        /// Sets whether to add files and folders recursively if PathToCab is specified.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Sets the prefixes to strip. Delimit with ';'
        /// </summary>
        public string StripPrefixes { get; set; }
    }
}
