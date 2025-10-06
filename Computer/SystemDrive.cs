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
namespace MSBuild.ExtensionPack.Computer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;
    using MSBuild.ExtensionPack.Base.Logging;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>CheckDriveSpace</i> ( <b>Required:</b> Drive, MinSpace <b>Optional:</b> Unit)</para>
    /// <para><i>GetDrives</i> ( <b>Optional:</b> SkipDrives, Unit <b>Output:</b> Drives)</para>
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
    ///<ItemGroup>
    ///<DrivesToSkip Include="A:\"/>
    ///</ItemGroup>
    ///<Target Name="Default">
    ///<!--- Check drive space -->
    ///<MSBuild.ExtensionPack.Computer.SystemDrive TaskAction="CheckDriveSpace" Drive="c:\" MachineName="AMachine" UserName="Administrator" UserPassword="APassword" MinSpace="46500" Unit="Mb" ContinueOnError="true"/>
    ///<!--- Check drive space on a remote machine -->
    ///<MSBuild.ExtensionPack.Computer.SystemDrive TaskAction="GetDrives" SkipDrives="@(DrivesToSkip)" MachineName="AMachine" UserName="Administrator" UserPassword="APassword">
    ///<Output TaskParameter="Drives" ItemName="SystemDrivesRemote"/>
    ///</MSBuild.ExtensionPack.Computer.SystemDrive>
    ///<Message Text="Remote Drive: %(SystemDrivesRemote.Identity), DriveType: %(SystemDrivesRemote.DriveType), Name: %(SystemDrivesRemote.Name), VolumeLabel: %(SystemDrivesRemote.VolumeLabel), DriveFormat: %(SystemDrivesRemote.DriveFormat), TotalSize: %(SystemDrivesRemote.TotalSize), TotalFreeSpace=%(SystemDrivesRemote.TotalFreeSpace), AvailableFreeSpace=%(SystemDrivesRemote.AvailableFreeSpace)IsReady=%(SystemDrivesRemote.IsReady), RootDirectory=%(SystemDrivesRemote.RootDirectory)"/>
    ///<!--- Check drive space using different units -->
    ///<MSBuild.ExtensionPack.Computer.SystemDrive TaskAction="CheckDriveSpace" Drive="c:\" MinSpace="46500" Unit="Mb" ContinueOnError="true"/>
    ///<MSBuild.ExtensionPack.Computer.SystemDrive TaskAction="CheckDriveSpace" Drive="c:\" MinSpace="1" Unit="Gb"/>
    ///<!-- Get the drives on a machine -->
    ///<MSBuild.ExtensionPack.Computer.SystemDrive TaskAction="GetDrives" SkipDrives="@(DrivesToSkip)">
    ///<Output TaskParameter="Drives" ItemName="SystemDrives"/>
    ///</MSBuild.ExtensionPack.Computer.SystemDrive>
    ///<Message Text="Drive: %(SystemDrives.Identity), DriveType: %(SystemDrives.DriveType), Name: %(SystemDrives.Name), VolumeLabel: %(SystemDrives.VolumeLabel), DriveFormat: %(SystemDrives.DriveFormat), TotalSize: %(SystemDrives.TotalSize), TotalFreeSpace=%(SystemDrives.TotalFreeSpace), AvailableFreeSpace=%(SystemDrives.AvailableFreeSpace)IsReady=%(SystemDrives.IsReady), RootDirectory=%(SystemDrives.RootDirectory)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class SystemDrive : BaseTask
    {
        #region Private Fields

        private List<ITaskItem> drives;
        private List<ITaskItem> skipDrives;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Checks the drive space.
        /// </summary>
        private void CheckDriveSpace()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Checking Drive Space: {0} (min {1}{2}) on: {3}", this.Drive, this.MinSpace, this.Unit, this.MachineName);

            long unitSize = this.ReadUnitSize();

            if (this.MachineName == Environment.MachineName)
            {
                foreach (string drive1 in Environment.GetLogicalDrives())
                {
                    if (string.Equals(this.Drive, drive1, StringComparison.OrdinalIgnoreCase))
                    {
                        DriveInfo driveInfo = new DriveInfo(drive1);
                        if (driveInfo.IsReady)
                        {
                            long freespace = driveInfo.AvailableFreeSpace;
                            if ((freespace / unitSize) < this.MinSpace)
                            {
                                this.Log.LogTaskError("Insufficient free space. Drive {0} has {1}{2}", this.Drive, driveInfo.AvailableFreeSpace / unitSize, this.Unit);
                            }
                            else
                            {
                                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Free drive space on {0} is {1}{2}", this.Drive, driveInfo.AvailableFreeSpace / unitSize, this.Unit);
                            }
                        }
                        else
                        {
                            this.Log.LogTaskWarning("Drive not ready to be read: {0}", drive1);
                        }
                    }
                }
            }
            else
            {
                this.GetManagementScope(@"\root\cimv2");
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Volume");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.Scope, query))
                {
                    ManagementObjectCollection moc = searcher.Get();
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo is null)
                        {
                            this.Log.LogError("WMI Failed to get drives from: {0}", this.MachineName);
                            return;
                        }

                        // only check fixed drives.
                        if (mo["DriveType"] is not null && mo["DriveType"].ToString() == "3")
                        {
                            if (mo["DriveLetter"] is null)
                            {
                                this.Log.LogTaskWarning("WMI Failed to query the DriveLetter from: {0}", this.MachineName);
                                break;
                            }

                            string? drive = mo["DriveLetter"].ToString();
                            double freeSpace = Convert.ToDouble(mo["FreeSpace"], CultureInfo.CurrentCulture) / unitSize;

                            if (freeSpace < this.MinSpace)
                            {
                                this.Log.LogTaskError("Insufficient free space. Drive {0} has {1}{2}", drive ?? string.Empty, freeSpace, this.Unit);
                            }
                            else
                            {
                                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Free drive space on {0} is {1}{2}", drive ?? string.Empty, freeSpace, this.Unit);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the drives.
        /// </summary>
        private void GetDrives()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Getting Drives from: {0}", this.MachineName);

            long unitSize = this.ReadUnitSize();

            this.drives = new List<ITaskItem>();
            if (this.MachineName == Environment.MachineName)
            {
                foreach (string drive1 in Environment.GetLogicalDrives())
                {
                    bool skip = false;
                    if (this.skipDrives is not null)
                    {
                        skip = this.SkipDrives.Any(driveToSkip => driveToSkip.ItemSpec == drive1);
                    }

                    if (skip == false)
                    {
                        DriveInfo driveInfo = new DriveInfo(drive1);
                        if (driveInfo.IsReady)
                        {
                            ITaskItem item = new TaskItem(drive1);
                            item.SetMetadata("DriveType", driveInfo.DriveType.ToString());
                            if (driveInfo.DriveType == DriveType.Fixed || driveInfo.DriveType == DriveType.Removable)
                            {
                                item.SetMetadata("Name", driveInfo.Name);
                                item.SetMetadata("VolumeLabel", driveInfo.VolumeLabel);
                                item.SetMetadata("AvailableFreeSpace", (driveInfo.AvailableFreeSpace / unitSize).ToString(CultureInfo.CurrentCulture));
                                item.SetMetadata("DriveFormat", driveInfo.DriveFormat);
                                item.SetMetadata("TotalSize", (driveInfo.TotalSize / unitSize).ToString(CultureInfo.CurrentCulture));
                                item.SetMetadata("TotalFreeSpace", (driveInfo.TotalFreeSpace / unitSize).ToString(CultureInfo.CurrentCulture));
                                item.SetMetadata("IsReady", driveInfo.IsReady.ToString());
                                item.SetMetadata("RootDirectory", driveInfo.RootDirectory.ToString());
                            }

                            this.drives.Add(item);
                        }
                        else
                        {
                            this.Log.LogWarning("Drive not ready to be read: {0}", drive1);
                        }
                    }
                }
            }
            else
            {
                this.GetManagementScope(@"\root\cimv2");
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Volume");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.Scope, query))
                {
                    ManagementObjectCollection moc = searcher.Get();
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo is null)
                        {
                            this.Log.LogError("WMI Failed to get drives from: {0}", this.MachineName);
                            return;
                        }

                        // only check fixed drives.
                        if (mo["DriveType"] is not null && mo["DriveType"].ToString() == "3")
                        {
                            bool skip = false;
                            string drive1 = mo["DriveLetter"].ToString();
                            if (this.skipDrives is not null)
                            {
                                skip = this.SkipDrives.Any(driveToSkip => driveToSkip.ItemSpec == drive1);
                            }

                            if (skip == false)
                            {
                                ITaskItem item = new TaskItem(drive1);
                                item.SetMetadata("DriveType", mo["DriveType"].ToString());
                                if (mo["DriveType"].ToString() == "3" || mo["DriveType"].ToString() == "2")
                                {
                                    item.SetMetadata("Name", mo["Name"].ToString());
                                    item.SetMetadata("VolumeLabel", mo["Label"].ToString());
                                    item.SetMetadata("AvailableFreeSpace", mo["FreeSpace"].ToString());
                                    item.SetMetadata("DriveFormat", mo["FileSystem"].ToString());
                                    item.SetMetadata("TotalSize", mo["Capacity"].ToString());
                                    item.SetMetadata("TotalFreeSpace", mo["FreeSpace"].ToString());
                                }

                                this.drives.Add(item);
                            }
                        }
                    }
                }
            }
        }

        private long ReadUnitSize()
        {
            if (string.IsNullOrEmpty(this.Unit))
            {
                this.Unit = "Mb";
            }

            long unitSize;

            switch (this.Unit.ToUpperInvariant())
            {
                case "TB":
                    unitSize = 1099511627776;
                    break;

                case "GB":
                    unitSize = 1073741824;
                    break;

                case "KB":
                    unitSize = 1024;
                    break;

                default:
                    unitSize = 1048576;
                    break;
            }

            return unitSize;
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            switch (this.TaskAction)
            {
                case "GetDrives":
                    this.GetDrives();
                    break;

                case "CheckDriveSpace":
                    this.CheckDriveSpace();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Sets the drive.
        /// </summary>
        public string Drive { get; set; }

        /// <summary>
        /// Sets the drives. ITaskItem
        /// <para/>
        /// Identity: Name
        /// <para/>
        /// Metadata: Name, VolumeLabel, AvailableFreeSpace, DriveFormat, TotalSize, TotalFreeSpace, IsReady (LocalMachine only),
        /// RootDirectory (LocalMachine only)
        /// </summary>
        [Output]
        public IEnumerable<ITaskItem> Drives
        {
            get => this.drives.ToArray();
            set => this.drives = new List<ITaskItem>(value);
        }

        /// <summary>
        /// Sets the min space.
        /// </summary>
        public long MinSpace { get; set; }

        /// <summary>
        /// Sets the drives to skip. ITaskItem
        /// </summary>
        public IEnumerable<ITaskItem> SkipDrives
        {
            get => this.skipDrives.ToArray();
            set => this.skipDrives = new List<ITaskItem>(value);
        }

        /// <summary>
        /// Sets the unit. Supports Kb, Mb(default), Gb, Tb
        /// </summary>
        public string Unit { get; set; }

        #endregion Public Properties
    }
}
