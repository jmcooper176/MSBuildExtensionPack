﻿// This file is part of CycloneDX CLI Tool
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
#pragma warning disable 618

namespace MSBuild.ExtensionPack.Compression
{
    using Microsoft.Build.Framework;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>AddFiles</i> ( <b>Required:</b> ZipFileName, CompressFiles or Path <b>Optional:</b> CompressionLevel,
    /// MaxOutputSegmentSize, Password; RemoveRoot, UseZip64WhenSaving) Existing files will be updated
    /// </para>
    /// <para>
    /// <i>Create</i> ( <b>Required:</b> ZipFileName, CompressFiles or Path <b>Optional:</b> CompressionLevel, MaxOutputSegmentSize,
    /// Password; RemoveRoot, UseZip64WhenSaving, PreserveAttributes)
    /// </para>
    /// <para><i>Extract</i> ( <b>Required:</b> ZipFileName, ExtractPath <b>Optional:</b> Password)</para>
    /// <para><b>Remote Execution Support:</b> NA</para>
    /// <para/>
    /// This task uses http://dotnetzip.codeplex.com v1.9.1.8 for compression.
    /// <para/>
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
    ///<Target Name="Default" DependsOnTargets="Sample1;Sample2"/>
    ///<Target Name="Sample1">
    ///<ItemGroup>
    ///<!-- Set the collection of files to Zip-->
    ///<FilesToZip Include="C:\Patches\**\*"/>
    ///</ItemGroup>
    ///<!-- Create a zip file based on the FilesToZip collection -->
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Create" CompressFiles="@(FilesToZip)" ZipFileName="C:\newZipByFile.zip"/>
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Create" Password="apassword" CompressionLevel="BestCompression" RemoveRoot="C:\Patches" CompressFiles="@(FilesToZip)" ZipFileName="C:\newZipByFileBestCompression.zip"/>
    ///<!-- Create a zip file based on a Path -->
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Create" CompressPath="C:\Patches" ZipFileName="C:\newZipByPath.zip"/>
    ///<!-- Extract a zip file-->
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Extract" ExtractPath="C:\aaa11\1" ZipFileName="C:\newZipByFile.zip"/>
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Extract" ExtractPath="C:\aaa11\2" ZipFileName="C:\newZipByPath.zip"/>
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Extract" Password="apassword"  ExtractPath="C:\aaa11\3" ZipFileName="C:\newZipByFileBestCompression.zip"/>
    ///</Target>
    ///<Target Name="Sample2">
    ///<PropertyGroup>
    ///<SourceDirectory>MotorData\</SourceDirectory>
    ///</PropertyGroup>
    ///<ItemGroup>
    ///<Files Include="$(SourceDirectory)*" Exclude="$(SourceDirectory).XYZ\**\*">
    ///<Group>Common</Group>
    ///</Files>
    ///<Files Include="$(SourceDirectory)Cars\*" Exclude="$(SourceDirectory)Cars\.XYZ\**\*">
    ///<Group>Cars</Group>
    ///</Files>
    ///<Files Include="$(SourceDirectory)Trucks\*" Exclude="$(SourceDirectory)Trucks\.XYZ\**\*">
    ///<Group>Trucks</Group>
    ///</Files>
    ///</ItemGroup>
    ///<!-- Create the output folder -->
    ///<ItemGroup>
    ///<OutputDirectory Include="output\"/>
    ///</ItemGroup>
    ///<MakeDir Directories="@(OutputDirectory)"/>
    ///<PropertyGroup>
    ///<WorkingDir>%(OutputDirectory.Fullpath)</WorkingDir>
    ///</PropertyGroup>
    ///<!-- Zip files based on the group they belong to -->
    ///<MSBuild.ExtensionPack.Compression.Zip TaskAction="Create" CompressFiles="@(Files)" ZipFileName="$(WorkingDir)%(Files.Group).zip"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class Zip : BaseTask
    {
        private const string AddFilesTaskAction = "AddFiles";
        private const string CreateTaskAction = "Create";
        private const string ExtractTaskAction = "Extract";
        private bool preserveAttributes = true;
        private Zip64Option useZip64WhenSaving = Zip64Option.Default;

        private void AddFiles()
        {
            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Adding files to ZipFile: {0}", this.ZipFileName));
            if (this.CompressFiles != null)
            {
                using (ZipArchive zip = ZipFile.OpenRead(this.ZipFileName.ItemSpec))
                {
                    zip.CompressionLevel = this.compressLevel;
                    if (!string.IsNullOrEmpty(this.Password))
                    {
                        zip.Password = this.Password;
                    }

                    foreach (ITaskItem f in this.CompressFiles)
                    {
                        ZipArchiveEntry updatedEntry;
                        if (this.RemoveRoot != null)
                        {
                            string location = (f.GetMetadata("RootDir") + f.GetMetadata("Directory")).Replace(this.RemoveRoot.GetMetadata("FullPath"), string.Empty, StringComparison.InvariantCulture);
                            updatedEntry = zip.CreateEntryFromFile(f.GetMetadata("FullPath"), location);
                        }
                        else
                        {
                            updatedEntry = zip.CreateEntry(f.GetMetadata("FullPath"));
                        }

                        if (!this.preserveAttributes)
                        {
                            updatedEntry.Attributes = FileAttributes.Normal;
                        }
                    }

                    if (this.MaxOutputSegmentSize > 0)
                    {
                        zip.MaxOutputSegmentSize = this.MaxOutputSegmentSize;
                    }

                    zip.UseZip64WhenSaving = this.useZip64WhenSaving;
                    zip.Save();
                }
            }
            else if (this.CompressPath != null)
            {
                using (ZipArchive zip = ZipFile.OpenRead(this.ZipFileName.ItemSpec))
                {
                    zip.CompressionLevel = this.compressLevel;
                    if (!string.IsNullOrEmpty(this.Password))
                    {
                        zip.Password = this.Password;
                    }

                    ZipArchiveEntry archiveEntry;
                    if (this.RemoveRoot != null)
                    {
                        DirectoryInfo d = new DirectoryInfo(this.CompressPath.ItemSpec);
                        string location = d.FullName.Replace(this.RemoveRoot.GetMetadata("FullPath"), string.Empty, StringComparison.InvariantCulture);
                        archiveEntry = zip.CreateEntryFromFile(this.CompressPath.ItemSpec, location);
                    }
                    else
                    {
                        archiveEntry = zip.CreateEntry(this.CompressPath.ItemSpec);
                    }

                    if (!this.preserveAttributes)
                    {
                        archiveEntry.Attributes = FileAttributes.Normal;
                    }

                    if (this.MaxOutputSegmentSize > 0)
                    {
                        zip.MaxOutputSegmentSize = this.MaxOutputSegmentSize;
                    }

                    zip.UseZip64WhenSaving = this.useZip64WhenSaving;
                    zip.Save();
                }
            }
            else
            {
                this.Log.LogError("CompressFiles or CompressPath must be specified");
            }
        }

        private void Create()
        {
            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Creating ZipFile: {0}", this.ZipFileName));
            if (this.CompressFiles != null)
            {
                using (ZipArchive zip = ZipFile.Open(this.ZipFileName, ZipArchiveMode.Create))
                {
                    zip.CompressionLevel = this.compressLevel;
                    if (!string.IsNullOrEmpty(this.Password))
                    {
                        zip.Password = this.Password;
                    }

                    foreach (ITaskItem f in this.CompressFiles)
                    {
                        ZipArchiveEntry addedEntry;
                        if (this.RemoveRoot != null)
                        {
                            string location = (f.GetMetadata("RootDir") + f.GetMetadata("Directory")).Replace(this.RemoveRoot.GetMetadata("FullPath"), string.Empty, StringComparison.InvariantCulture);
                            addedEntry = zip.AddFile(f.GetMetadata("FullPath"), location);
                        }
                        else
                        {
                            addedEntry = zip.AddFile(f.GetMetadata("FullPath"));
                        }

                        if (!this.preserveAttributes)
                        {
                            addedEntry.Attributes = FileAttributes.Normal;
                        }
                    }

                    if (this.MaxOutputSegmentSize > 0)
                    {
                        zip.MaxOutputSegmentSize = this.MaxOutputSegmentSize;
                    }

                    zip.UseZip64WhenSaving = this.useZip64WhenSaving;
                    zip.Save(this.ZipFileName.ItemSpec);
                }
            }
            else if (this.CompressPath != null)
            {
                using (ZipArchive zip = ZipFile.Open(this.ZipFileName.ItemSpec, ZipArchiveMode.Create))
                {
                    zip.CompressionLevel = this.compressLevel;
                    if (!string.IsNullOrEmpty(this.Password))
                    {
                        zip.Password = this.Password;
                    }

                    ZipArchiveEntry addedDirectory;
                    if (this.RemoveRoot != null)
                    {
                        DirectoryInfo d = new DirectoryInfo(this.CompressPath.ItemSpec);
                        string location = d.FullName.Replace(this.RemoveRoot.GetMetadata("FullPath"), string.Empty, StringComparison.InvariantCulture);
                        addedDirectory = zip.AddDirectory(this.CompressPath.ItemSpec, location);
                    }
                    else
                    {
                        DirectoryInfo d = new DirectoryInfo(this.CompressPath.ItemSpec);
                        addedDirectory = zip.AddDirectory(this.CompressPath.ItemSpec, d.Name);
                    }

                    if (!this.preserveAttributes)
                    {
                        addedDirectory.Attributes = FileAttributes.Normal;
                    }

                    if (this.MaxOutputSegmentSize > 0)
                    {
                        zip.MaxOutputSegmentSize = this.MaxOutputSegmentSize;
                    }

                    zip.UseZip64WhenSaving = this.useZip64WhenSaving;
                    zip.Save(this.ZipFileName.ItemSpec);
                }
            }
            else
            {
                this.Log.LogError("CompressFiles or CompressPath must be specified");
            }
        }

        private void Extract()
        {
            if (!File.Exists(this.ZipFileName.GetMetadata("FullPath")))
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "ZipFileName not found: {0}", this.ZipFileName));
                return;
            }

            if (string.IsNullOrEmpty(this.ExtractPath.GetMetadata("FullPath")))
            {
                this.Log.LogError("ExtractPath is required");
                return;
            }

            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Extracting ZipFile: {0} to: {1}", this.ZipFileName, this.ExtractPath));

            using (ZipArchive zip = ZipFile.OpenRead(this.ZipFileName.GetMetadata("FullPath")))
            {
                if (!string.IsNullOrEmpty(this.Password))
                {
                    zip.Password = this.Password;
                }

                foreach (ZipArchiveEntry e in zip.Entries)
                {
                    e.Extract(this.ExtractPath.GetMetadata("FullPath"), ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        /// <summary>
        /// This is the main InternalExecute method that all tasks should implement
        /// </summary>
        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            switch (this.TaskAction)
            {
                case CreateTaskAction:
                    this.Create();
                    break;

                case ExtractTaskAction:
                    this.Extract();
                    break;

                case AddFilesTaskAction:
                    this.AddFiles();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Sets the files to Compress
        /// </summary>
        public IEnumerable<ITaskItem> CompressFiles { get; set; }

        /// <summary>
        /// Sets the CompressionLevel to use. Default is Default, also supports BestSpeed and BestCompression
        /// </summary>
        public string CompressionLevel
        {
            get => this.compressLevel.ToString();
            set => this.compressLevel = Enum.Parse<CompressionLevel>(value);
        }

        /// <summary>
        /// Sets the Path to Zip.
        /// </summary>
        public ITaskItem CompressPath { get; set; }

        /// <summary>
        /// Path to extract the zip file to
        /// </summary>
        public ITaskItem ExtractPath { get; set; }

        /// <summary>
        /// Sets the maximum output segment size, which typically results in a split archive (an archive split into multiple files).
        /// This value is not required and if not set or set to 0 the resulting archive will not be split. For more details see the
        /// DotNetZip documentation.
        /// </summary>
        public int MaxOutputSegmentSize { get; set; }

        /// <summary>
        /// Sets the Password to be used
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// This is only applicable when the <b>TaskAction</b> is <i>Create</i> or <i>AddFiles</i>. Specifies whether file (folder)
        /// attributes like <i>Hidden</i> or <i>Read-only</i> should be left intact during adding to the archive. The default is <c>true</c>.
        /// </summary>
        public bool PreserveAttributes
        {
            get => this.preserveAttributes;
            set => this.preserveAttributes = value;
        }

        /// <summary>
        /// Sets the root to remove from the zip path. Note that this should be part of the file to compress path, not the target
        /// path of the ZipFileName
        /// </summary>
        public ITaskItem RemoveRoot { get; set; }

        /// <summary>
        /// Sets the UseZip64WhenSaving output of the DotNetZip library. For more details see the DotNetZip documentation.
        /// </summary>
        public string UseZip64WhenSaving
        {
            get => this.useZip64WhenSaving.ToString();
            set => this.useZip64WhenSaving = (Zip64Option)Enum.Parse(typeof(Zip64Option), value, true);
        }

        /// <summary>
        /// Sets the name of the Zip File
        /// </summary>
        [Required]
        public ITaskItem ZipFileName { get; set; }
    }
}
