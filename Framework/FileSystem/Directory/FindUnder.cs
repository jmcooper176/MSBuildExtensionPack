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
namespace MSBuild.ExtensionPack.FileSystem.Directory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>FindFiles</i> ( <b>Required:</b> Path <b>Optional:</b> ModifiedAfterDate, ModifiedBeforeDate, Recursive, SearchPattern
    /// <b>Output:</b> FoundItems)
    /// </para>
    /// <para>
    /// <i>FindDirectories</i> ( <b>Required:</b> Path <b>Optional:</b> ModifiedAfterDate, ModifiedBeforeDate, Recursive,
    /// SearchPattern <b>Output:</b> FoundItems)
    /// </para>
    /// <para>
    /// <i>FindFilesAndDirectories</i> ( <b>Required:</b> Path <b>Optional:</b> ModifiedAfterDate, ModifiedBeforeDate, Recursive,
    /// SearchPattern <b>Output:</b> FoundItems)
    /// </para>
    /// <para><b>Remote Execution Support:</b> NA</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" DefaultTargets="Demo">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<Target Name="Demo">
    ///<!-- Only finds files -->
    ///<MSBuild.ExtensionPack.FileSystem.FindUnder TaskAction="FindFiles" Path="$(MSBuildProjectDirectory)">
    ///<Output ItemName="AllFoundFiles" TaskParameter="FoundItems"/>
    ///</MSBuild.ExtensionPack.FileSystem.FindUnder>
    ///<Message Text="===== Found Files =====" Importance="high"/>
    ///<Message Text="AllFoundFiles:%0d%0a@(AllFoundFiles,'%0d%0a')"/>
    ///<!-- Only finds directories -->
    ///<MSBuild.ExtensionPack.FileSystem.FindUnder TaskAction="FindDirectories" Path="$(MSBuildProjectDirectory)\..\">
    ///<Output ItemName="AllFoundDirectories" TaskParameter="FoundItems"/>
    ///</MSBuild.ExtensionPack.FileSystem.FindUnder>
    ///<Message Text="===== Found Directories =====" Importance="high"/>
    ///<Message Text="AllFoundDirectories:%0d%0a@(AllFoundDirectories,'%0d%0a')"/>
    ///<!-- Find both files and directories -->
    ///<MSBuild.ExtensionPack.FileSystem.FindUnder TaskAction="FindFilesAndDirectories" Path="$(MSBuildProjectDirectory)\..\">
    ///<Output ItemName="AllFoundItems" TaskParameter="FoundItems"/>
    ///</MSBuild.ExtensionPack.FileSystem.FindUnder>
    ///<Message Text="===== Found Files and Directories =====" Importance="high"/>
    ///<Message Text="AllFoundItems:%0d%0a@(AllFoundItems,'%0d%0a')"/>
    ///<!-- Find both files with SearchPattern = "F*" -->
    ///<MSBuild.ExtensionPack.FileSystem.FindUnder TaskAction="FindFiles" Path="$(MSBuildProjectDirectory)\..\" SearchPattern="F*">
    ///<Output ItemName="AllFilesStartingWithF" TaskParameter="FoundItems"/>
    ///</MSBuild.ExtensionPack.FileSystem.FindUnder>
    ///<Message Text="===== Found Files Starting with 'F' =====" Importance="high"/>
    ///<Message Text="AllFilesStartingWithF:%0d%0a@(AllFilesStartingWithF,'%0d%0a')"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class FindUnder : BaseTask
    {
        #region Private Fields

        private const string FindDirectoriesTaskAction = "FindDirectories";
        private const string FindFilesAndDirectoriesTaskAction = "FindFilesAndDirectories";
        private const string FindFilesTaskAction = "FindFiles";
        private List<ITaskItem> items = new List<ITaskItem>();

        #endregion Private Fields

        #region Private Methods

        private void FindMatchingDirectories(DirectoryInfo dir)
        {
            DirectoryInfo[] subDirs = this.Recursive ? dir.GetDirectories(this.SearchPattern, SearchOption.AllDirectories) : dir.GetDirectories(this.SearchPattern, SearchOption.TopDirectoryOnly);
            DirectoryInfo[] tempdirs = new DirectoryInfo[1];

            if (this.ModifiedAfterDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedBeforeDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                tempdirs = (from f in subDirs
                            where f.LastWriteTime > this.ModifiedAfterDate
                            select f).ToArray();
            }

            if (this.ModifiedBeforeDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                tempdirs = (from f in subDirs
                            where f.LastWriteTime < this.ModifiedBeforeDate
                            select f).ToArray();
            }

            if (this.ModifiedBeforeDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                tempdirs = (from f in subDirs
                            where f.LastWriteTime < this.ModifiedBeforeDate & f.LastWriteTime > this.ModifiedAfterDate
                            select f).ToArray();
            }

            if (this.ModifiedBeforeDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                tempdirs = subDirs;
            }

            foreach (DirectoryInfo dirInfo in tempdirs)
            {
                TaskItem item = new TaskItem(dirInfo.FullName);
                item.SetMetadata("DirectoryName", dirInfo.Name);
                this.items.Add(item);
            }
        }

        private void FindMatchingFiles(DirectoryInfo dir, FileInfo[] files)
        {
            FileInfo[] tempfiles = this.Recursive ? dir.GetFiles(this.SearchPattern, SearchOption.AllDirectories) : dir.GetFiles(this.SearchPattern, SearchOption.TopDirectoryOnly);
            if (this.ModifiedAfterDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedBeforeDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                files = (from f in tempfiles
                         where f.LastWriteTime > this.ModifiedAfterDate
                         select f).ToArray();
            }

            if (this.ModifiedBeforeDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                files = (from f in tempfiles
                         where f.LastWriteTime < this.ModifiedBeforeDate
                         select f).ToArray();
            }

            if (this.ModifiedBeforeDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate != Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                files = (from f in tempfiles
                         where f.LastWriteTime < this.ModifiedBeforeDate & f.LastWriteTime > this.ModifiedAfterDate
                         select f).ToArray();
            }

            if (this.ModifiedBeforeDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture) && this.ModifiedAfterDate == Convert.ToDateTime("01/01/0001 00:00:00", CultureInfo.CurrentCulture))
            {
                files = tempfiles;
            }

            this.items = files.Select(fileInfo => new TaskItem(fileInfo.FullName)).Cast<ITaskItem>().ToList();
        }

        #endregion Private Methods

        #region Protected Properties

        /// <summary>
        /// Gets or sets a value indicating if directories should be included in the result. <br/> The default value for this is
        /// <see langref="false"/>. Both <c>FindFiles</c> and <c>FindDirectories</c> cannot be <see langref="false"/>, atleast one
        /// <b>must</b> be <see langref="true"/>.
        /// </summary>
        /// <value><see langref="true"/> if directories should be included in the find result; otherwise, <see langref="false"/>.</value>
        protected bool FindDirectories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if files should be included in the result. <br/> The default value for this is <see
        /// langref="false"/>. <br/> Both <c>FindFiles</c> and <c>FindDirectories</c> cannot be <see langref="false"/>, atleast one
        /// <b>must</b> be <see langref="true"/>.
        /// </summary>
        /// <value><see langref="true"/> if file should be included in the find result; otherwise, <see langref="false"/>.</value>
        protected bool FindFiles { get; set; }

        #endregion Protected Properties

        #region Protected Methods

        protected override void InternalExecute()
        {
            switch (this.TaskAction)
            {
                case FindFilesTaskAction:
                    this.FindFiles = true;
                    this.FindDirectories = false;
                    break;

                case FindDirectoriesTaskAction:
                    this.FindFiles = false;
                    this.FindDirectories = true;
                    break;

                case FindFilesAndDirectoriesTaskAction:
                    this.FindFiles = true;
                    this.FindDirectories = true;
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }

            if (!this.FindFiles && !this.FindDirectories)
            {
                this.Log.LogError("Either FindFiles or FindDirectories must be true");
                return;
            }

            string fullPath = this.Path.GetMetadata("Fullpath");
            this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Searching under path [{0}]", fullPath), null);
            if (string.IsNullOrEmpty(fullPath) || !Directory.Exists(fullPath))
            {
                this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Path specified {0} doesn't exist", fullPath));
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(fullPath);
            FileInfo[] files = new FileInfo[0];

            if (this.FindFiles)
            {
                this.FindMatchingFiles(dir, files);
            }

            if (this.FindDirectories)
            {
                this.FindMatchingDirectories(dir);
            }

            this.FoundItems = this.items.ToArray();
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// The list of items (files and or directories) which were found.
        /// </summary>
        [Output]
        public IEnumerable<ITaskItem> FoundItems { get; set; }

        /// <summary>
        /// Set this value to only return files or folders modified after the given value
        /// </summary>
        public DateTime ModifiedAfterDate { get; set; }

        /// <summary>
        /// Set this value to only return files or folders modified before the given value
        /// </summary>
        public DateTime ModifiedBeforeDate { get; set; }

        /// <summary>
        /// The path that the <c>FindUnder</c> will be executed against. This is a <b>Required</b> value.
        /// </summary>
        [Required]
        public ITaskItem Path { get; set; }

        /// <summary>
        /// Sets whether the File search is recursive. Default is true
        /// </summary>
        public bool Recursive { get; set; } = true;

        /// <summary>
        /// This in an optional input property. This will set the <c>SearchPattern</c> to be used in the search. <br/> The default
        /// value for this is <c>"*"</c>; <br/> This value is passed to either the System.IO.DirectoryInfo.GetDirectories method
        /// and/or the System.IO.FileInfo.GetFiles method. See that documentation for usage guidlines.
        /// </summary>
        public string SearchPattern { get; set; } = "*";

        #endregion Public Properties
    }
}
