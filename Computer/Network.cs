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
    using System.Globalization;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;
    using MSBuild.ExtensionPack.Base.Logging;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>GetDnsHostName</i> ( <b>Required: HostName</b><b>Output:</b> DnsHostName)</para>
    /// <para><i>GetFreePort</i> ( <b>Output:</b> Port)</para>
    /// <para><i>GetInternalIP</i> ( <b>Output:</b> Ip)</para>
    /// <para><i>GetRemoteIP</i> ( <b>Required:</b> HostName <b>Output:</b> Ip)</para>
    /// <para><i>Ping</i> ( <b>Required:</b> HostName <b>Optional:</b> Timeout, PingCount <b>Output:</b> Exists)</para>
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
    ///<!-- Get the Machine IP Addresses -->
    ///<MSBuild.ExtensionPack.Computer.Network TaskAction="GetInternalIP">
    ///<Output TaskParameter="IP" ItemName="TheIP"/>
    ///</MSBuild.ExtensionPack.Computer.Network>
    ///<Message Text="The IP: %(TheIP.Identity)"/>
    ///<!-- Get Remote IP Addresses -->
    ///<MSBuild.ExtensionPack.Computer.Network TaskAction="GetRemoteIP" HostName="www.freetodev.com">
    ///<Output TaskParameter="IP" ItemName="TheRemoteIP"/>
    ///</MSBuild.ExtensionPack.Computer.Network>
    ///<Message Text="The Remote IP: %(TheRemoteIP.Identity)"/>
    ///<!-- Ping a host -->
    ///<MSBuild.ExtensionPack.Computer.Network TaskAction="Ping" HostName="www.freetodev.com">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.Network>
    ///<Message Text="Exists: $(DoesExist)"/>
    ///<!-- Gets the fully-qualified domain name for a hostname. -->
    ///<MSBuild.ExtensionPack.Computer.Network TaskAction="GetDnsHostName" HostName="192.168.0.15">
    ///<Output TaskParameter="DnsHostName" PropertyName="HostEntryName" />
    ///</MSBuild.ExtensionPack.Computer.Network>
    ///<Message Text="Host Entry name: $(HostEntryName)" />
    ///<!-- Get free port details -->
    ///<MSBuild.ExtensionPack.Computer.Network TaskAction="GetFreePort">
    ///<Output TaskParameter="Port" ItemName="FreePort"/>
    ///</MSBuild.ExtensionPack.Computer.Network>
    ///<Message Text="Free Port Address: %(FreePort.Address)"/>
    ///<Message Text="Free Port AddressFamily: %(FreePort.AddressFamily)"/>
    ///<Message Text="Free Port Port: %(FreePort.Port)"/>
    ///<Message Text="Free Port ToString: %(FreePort.ToString)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class Network : BaseTask
    {
        #region Private Fields

        private const string GetDnsHostNameTaskAction = "GetDnsHostName";
        private const string GetFreePortTaskAction = "GetFreePort";
        private const string GetInternalIPTaskAction = "GetInternalIP";
        private const string GetRemoteIPTaskAction = "GetRemoteIP";
        private const string PingTaskAction = "Ping";

        #endregion Private Fields

        #region Private Methods

        private void GetDnsHostName()
        {
            if (string.IsNullOrEmpty(this.HostName))
            {
                this.Log.LogError("HostName is required");
                return;
            }

            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Getting host entry name for: {0}", this.HostName);
            var hostEntry = Dns.GetHostEntry(this.HostName);
            this.DnsHostName = hostEntry.HostName;
        }

        private void GetFreePort()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Getting Free Port");
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var ipEndPoint = (IPEndPoint)listener.LocalEndpoint;
            listener.Stop();

            ITaskItem portItem = new TaskItem("Port");
            portItem.SetMetadata("Address", ipEndPoint.Address.ToString());
            portItem.SetMetadata("AddressFamily", ipEndPoint.AddressFamily.ToString());
            portItem.SetMetadata("Port", ipEndPoint.Port.ToString(CultureInfo.InvariantCulture));
            portItem.SetMetadata("ToString", ipEndPoint.ToString());
            this.Port = portItem;
        }

        private void GetInternalIP()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Get Internal IP for: {0}", Environment.MachineName);
            string hostName = Dns.GetHostName();
            if (string.IsNullOrEmpty(hostName))
            {
                this.Log.LogTaskWarning("Trying to determine IP addresses but Dns.GetHostName() returned an empty value");
                return;
            }

            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            if (hostEntry.AddressList is null || hostEntry.AddressList.Length <= 0)
            {
                this.Log.LogTaskWarning("Trying to determine internal IP addresses but address list is empty");
                return;
            }

            this.IP = new ITaskItem[hostEntry.AddressList.Length];
            for (int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                ITaskItem newItem = new TaskItem(hostEntry.AddressList[i].ToString());
                this.IP[i] = newItem;
            }
        }

        private void GetRemoteIP()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Get Remote IP for: {0}", this.HostName);
            IPAddress[] addresslist = Dns.GetHostAddresses(this.HostName);
            this.IP = new ITaskItem[addresslist.Length];
            for (int i = 0; i < addresslist.Length; i++)
            {
                ITaskItem newItem = new TaskItem(addresslist[i].ToString());
                this.IP[i] = newItem;
            }
        }

        private void Ping()
        {
            const int BufferSize = 32;
            const int TimeToLive = 128;

            byte[] buffer = new byte[BufferSize];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = unchecked((byte)i);
            }

            using (System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping())
            {
                PingOptions options = new PingOptions(TimeToLive, false);
                for (int i = 0; i < this.PingCount; i++)
                {
                    this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Pinging {0}", this.HostName);
                    PingReply response = pinger.Send(this.HostName, this.Timeout, buffer, options);
                    if (response is not null && response.Status == IPStatus.Success)
                    {
                        this.Exists = true;
                        return;
                    }

                    this.Log.LogTaskMessage(() => response is not null, MessageImportance.Low, "Response Status {0}", response.Status);

                    System.Threading.Thread.Sleep(1000);
                }

                this.Exists = false;
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            if (!this.TargetingLocalMachine())
            {
                return;
            }

            switch (this.TaskAction)
            {
                case PingTaskAction:
                    this.Ping();
                    break;

                case GetFreePortTaskAction:
                    this.GetFreePort();
                    break;

                case GetInternalIPTaskAction:
                    this.GetInternalIP();
                    break;

                case GetRemoteIPTaskAction:
                    this.GetRemoteIP();
                    break;

                case GetDnsHostNameTaskAction:
                    this.GetDnsHostName();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        /// <summary>
        /// Gets the DnsHostName
        /// </summary>
        [Output]
        public string DnsHostName { get; set; }

        /// <summary>
        /// Gets whether the Host Exists
        /// </summary>
        [Output]
        public bool Exists { get; private set; }

        /// <summary>
        /// Sets the HostName / IP address
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets the IP's
        /// </summary>
        [Output]
        public IEnumerable<ITaskItem> IP { get; set; }

        /// <summary>
        /// Sets the number of pings to attempt. Default is 5.
        /// </summary>
        public int PingCount { get; set; } = 5;

        /// <summary>
        /// Gets the free port. ItemSpec is Port. Metadata includes Address, AddressFamily, Port and ToString
        /// </summary>
        [Output]
        public ITaskItem Port { get; set; }

        /// <summary>
        /// Sets the timeout in ms for a Ping. Default is 3000
        /// </summary>
        public int Timeout { get; set; } = 3000;

        #endregion Public Properties
    }
}
