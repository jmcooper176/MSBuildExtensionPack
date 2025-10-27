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
namespace Computer
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>CheckRunning</i> ( <b>Required:</b> ProcessName <b>Output:</b> IsRunning)</para>
    /// <para><i>Create</i> ( <b>Required:</b> Parameters <b>Output:</b> ReturnValue, ProcessId)</para>
    /// <para>
    /// <i>Get</i> ( <b>Required:</b> ProcessName, Value <b>Optional:</b> User, ProcessName, IncludeUserInfo <b>Output:</b> Processes)
    /// </para>
    /// <para><i>Terminate</i> ( <b>Required:</b> ProcessName or ProcessId)</para>
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
    ///<WmiExec3 Include="CommandLine#~#notepad.exe"/>
    ///</ItemGroup>
    ///<Target Name="Default">
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="Terminate" ProcessId="9564"/>
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="Create" Parameters="@(WmiExec3)">
    ///<Output TaskParameter="ReturnValue" PropertyName="Rval2"/>
    ///<Output TaskParameter="ProcessId" PropertyName="PID"/>
    ///</MSBuild.ExtensionPack.Computer.Process>
    ///<Message Text="ReturnValue: $(Rval2). ProcessId: $(PID)"/>
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="CheckRunning" ProcessName="notepad.exe">
    ///<Output PropertyName="Running" TaskParameter="IsRunning"/>
    ///</MSBuild.ExtensionPack.Computer.Process>
    ///<Message Text="notepad.exe IsRunning: $(Running)"/>
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="Terminate" ProcessName="notepad.exe"/>
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="CheckRunning" ProcessName="notepad.exe">
    ///<Output PropertyName="Running" TaskParameter="IsRunning"/>
    ///</MSBuild.ExtensionPack.Computer.Process>
    ///<Message Text="notepad.exe IsRunning: $(Running)"/>
    ///<MSBuild.ExtensionPack.Computer.Process TaskAction="Get" IncludeUserInfo="true">
    ///<Output ItemName="ProcessList" TaskParameter="Processes"/>
    ///</MSBuild.ExtensionPack.Computer.Process>
    ///<Message Text="%(ProcessList.Identity)  - %(ProcessList.User) - %(ProcessList.OwnerSID)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class Process : BaseTask
    {
        private const string CheckRunningTaskAction = "CheckRunning";
        private const string CreateTaskAction = "Create";
        private const string GetTaskAction = "Get";
        private const string TerminateTaskAction = "Terminate";

        private void CheckRunning()
        {
            ArgumentNullException.ThrowIfNullOrEmpty(this.ProcessName);

            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Checking whether Process is running: {0}", this.ProcessName);

            ObjectQuery query = new($"SELECT * FROM Win32_Process WHERE Name = '{this.ProcessName}'");
            using ManagementObjectSearcher searcher = new(this.Scope, query, null);
            ManagementObjectCollection moc = searcher.Get();
            if (moc.Count > 0)
            {
                this.IsRunning = true;
            }
        }

        private void Create()
        {
            if (this.Parameters is null)
            {
                this.Log.LogError("Parameters is required");
                return;
            }

            using (ManagementClass mgmtClass = new ManagementClass(this.Scope, new ManagementPath("Win32_Process"), null))
            {
                // Obtain in-parameters for the method
                ManagementBaseObject inParams = mgmtClass.GetMethodParameters("Create");
                if (this.Parameters is not null)
                {
                    // Add the input parameters.
                    foreach (string[] data in this.Parameters.Select(param => param.ItemSpec.Split(["#~#"], StringSplitOptions.RemoveEmptyEntries)))
                    {
                        this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Param: {0}. Value: {1}", data[0], data[1]);
                        inParams[data[0]] = data[1];
                    }
                }

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams = mgmtClass.InvokeMethod("Create", inParams, null);
                if (outParams is not null)
                {
                    this.ReturnValue = outParams["ReturnValue"].ToString();
                    this.ProcessId = Convert.ToInt32(outParams["ProcessId"], CultureInfo.CurrentCulture);
                }
            }
        }

        private void Get()
        {
            ArgumentNullException.ThrowIfNullOrEmpty(this.ProcessName);

            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Getting Processes matching: {0}", this.ProcessName);

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process");
            Regex userFilter = new(this.User, RegexOptions.Compiled);
            Regex processFilter = new(this.ProcessName, RegexOptions.Compiled);
            using ManagementObjectSearcher searcher = new(this.Scope, query, null);
            this.Processes = new ITaskItem[searcher.Get().Count];
            int i = 0;
            foreach (ManagementObject ret in searcher.Get().Cast<ManagementObject>())
            {
                if (processFilter.IsMatch(ret["Name"].ToString()!))
                {
                    var processItem = new TaskItem(ret["Name"].ToString());
                    processItem.SetMetadata("Caption", ret["Caption"].ToString());
                    processItem.SetMetadata("Description", ret["Description"].ToString());
                    processItem.SetMetadata("Handle", ret["Handle"].ToString());
                    processItem.SetMetadata("HandleCount", ret["HandleCount"].ToString());
                    processItem.SetMetadata("KernelModeTime", ret["KernelModeTime"].ToString());
                    processItem.SetMetadata("PageFaults", ret["PageFaults"].ToString());
                    processItem.SetMetadata("PageFileUsage", ret["PageFileUsage"].ToString());
                    processItem.SetMetadata("ParentProcessId", ret["ParentProcessId"].ToString());
                    processItem.SetMetadata("PeakPageFileUsage", ret["PeakPageFileUsage"].ToString());
                    processItem.SetMetadata("PeakVirtualSize", ret["PeakVirtualSize"].ToString());
                    processItem.SetMetadata("PeakWorkingSetSize", ret["PeakWorkingSetSize"].ToString());
                    processItem.SetMetadata("Priority", ret["Priority"].ToString());
                    processItem.SetMetadata("PrivatePageCount", ret["PrivatePageCount"].ToString());
                    processItem.SetMetadata("ProcessId", ret["ProcessId"].ToString());
                    processItem.SetMetadata("QuotaNonPagedPoolUsage", ret["QuotaNonPagedPoolUsage"].ToString());
                    processItem.SetMetadata("QuotaPagedPoolUsage", ret["QuotaPagedPoolUsage"].ToString());
                    processItem.SetMetadata("QuotaPeakNonPagedPoolUsage", ret["QuotaPeakNonPagedPoolUsage"].ToString());
                    processItem.SetMetadata("QuotaPeakPagedPoolUsage", ret["QuotaPeakPagedPoolUsage"].ToString());
                    processItem.SetMetadata("ReadOperationCount", ret["ReadOperationCount"].ToString());
                    processItem.SetMetadata("ReadTransferCount", ret["ReadTransferCount"].ToString());
                    processItem.SetMetadata("SessionId", ret["SessionId"].ToString());
                    processItem.SetMetadata("ThreadCount", ret["ThreadCount"].ToString());
                    processItem.SetMetadata("UserModeTime", ret["UserModeTime"].ToString());
                    processItem.SetMetadata("VirtualSize", ret["VirtualSize"].ToString());
                    processItem.SetMetadata("WindowsVersion", ret["WindowsVersion"].ToString());
                    processItem.SetMetadata("WorkingSetSize", ret["WorkingSetSize"].ToString());
                    processItem.SetMetadata("WriteOperationCount", ret["WriteOperationCount"].ToString());
                    processItem.SetMetadata("WriteTransferCount", ret["WriteTransferCount"].ToString());
                    if (this.IncludeUserInfo)
                    {
                        string[] o = new string[2];
                        ret.InvokeMethod("GetOwner", o);

                        if (o[0] is null)
                        {
                            continue;
                        }

                        if (!userFilter.IsMatch(o[0]))
                        {
                            continue;
                        }

                        processItem.SetMetadata("User", o[0]);

                        if (o[1] is not null)
                        {
                            processItem.SetMetadata("Domain", o[1]);
                        }

                        string[] sid = new string[1];
                        ret.InvokeMethod("GetOwnerSid", sid);
                        if (sid[0] is not null)
                        {
                            processItem.SetMetadata("OwnerSID", sid[0]);
                        }
                    }

                    this.Processes[i] = processItem;
                    i++;
                }
            }
        }

        private void Kill()
        {
            if (this.ProcessName == ".*" && this.ProcessId == 0)
            {
                this.Log.LogError("ProcessName or ProcessId is required");
                return;
            }

            ObjectQuery query = this.ProcessName != ".*" ? new ObjectQuery("SELECT * FROM Win32_Process WHERE Name ='" + this.ProcessName + "'") : new ObjectQuery("SELECT * FROM Win32_Process WHERE Handle ='" + this.ProcessId + "'");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.Scope, query, null))
            {
                foreach (ManagementObject returnedProcess in searcher.Get())
                {
                    this.Log.LogTaskMessage(() => this.ProcessName != ".*", MessageImportance.Normal, "Terminating: {0}", this.ProcessName);
                    this.Log.LogTaskMessage(() => this.ProcessName == ".*", MessageImportance.Normal, "Terminating: {0}", this.ProcessId);

                    ManagementBaseObject inParams = returnedProcess.GetMethodParameters("Terminate");
                    ManagementBaseObject outParams = returnedProcess.InvokeMethod("Terminate", inParams, null);

                    // ReturnValue should be 0, else failure
                    if (outParams is not null)
                    {
                        switch (Convert.ToInt32(outParams.Properties["ReturnValue"].Value, CultureInfo.CurrentCulture))
                        {
                            case 0:
                                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "...Process Terminated");
                                break;

                            case 2:
                                this.Log.LogTaskError("...Access Denied");
                                break;

                            case 3:
                                this.Log.LogTaskError("...Insufficient Privilege");
                                break;

                            case 8:
                                this.Log.LogTaskError("...Unknown Failure");
                                break;

                            case 9:
                                this.Log.LogTaskError("...Path Not Found");
                                break;

                            case 21:
                                this.Log.LogTaskError("...Invalid Parameter");
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            this.GetManagementScope(@"\root\cimv2");
            switch (this.TaskAction)
            {
                case CreateTaskAction:
                    this.Create();
                    break;

                case GetTaskAction:
                    this.Get();
                    break;

                case TerminateTaskAction:
                    this.Kill();
                    break;

                case CheckRunningTaskAction:
                    this.CheckRunning();
                    break;

                default:
                    this.Log.LogTaskError("Invalid TaskAction passed: {0}", this.TaskAction ?? "<null TaskAction>");
                    return;
            }
        }

        /// <summary>
        /// Sets whether to include user information for processes. Including this will slow the query. Default is false;
        /// </summary>
        public bool IncludeUserInfo { get; set; }

        /// <summary>
        /// Gets whether the process is running
        /// </summary>
        [Output]
        public bool IsRunning { get; set; }

        /// <summary>
        /// Sets the Parameters for Create. Use #~# separate name and value.
        /// </summary>
        public IEnumerable<ITaskItem> Parameters { get; set; }

        /// <summary>
        /// Gets the list of processes. The process name is used as the identity and the following metadata is set: Caption,
        /// Description, Handle, HandleCount, KernelModeTime, PageFaults, PageFileUsage, ParentProcessId, PeakPageFileUsage,
        /// PeakVirtualSize, PeakWorkingSetSize, Priority, PrivatePageCount, ProcessId, QuotaNonPagedPoolUsage, QuotaPagedPoolUsage,
        /// QuotaPeakNonPagedPoolUsage, QuotaPeakPagedPoolUsage, ReadOperationCount, ReadTransferCount, SessionId, ThreadCount,
        /// UserModeTime, VirtualSize, WindowsVersion, WorkingSetSize, WriteOperationCount, WriteTransferCount
        /// </summary>
        [Output]
        public IEnumerable<ITaskItem> Processes { get; set; }

        /// <summary>
        /// Gets or Sets the ProcessId
        /// </summary>
        [Output]
        public int ProcessId { get; set; }

        /// <summary>
        /// Sets the regular expression to use for filtering processes. Default is .*
        /// </summary>
        public string ProcessName { get; set; } = ".*";

        /// <summary>
        /// Gets the ReturnValue for Create
        /// </summary>
        [Output]
        public string ReturnValue { get; set; }

        /// <summary>
        /// Sets the regular expression to use for filtering processes. Default is .*
        /// </summary>
        public string User { get; set; } = ".*";
    }
}
