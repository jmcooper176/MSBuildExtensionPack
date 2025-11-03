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
namespace MSBuild.ExtensionPack.Framework
{
    using System.Globalization;

    using MSBuild.ExtensionPack.Base;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>Abort</i> (Warning: use only in exceptional circumstances to force an abort)</para>
    /// <para><i>Sleep</i> ( <b>Required:</b> Timeout)</para>
    /// <para><i>SpinWait</i> ( <b>Required:</b> Iterations)</para>
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
    ///<!-- Set a thread to sleep for a period -->
    ///<MSBuild.ExtensionPack.Framework.Thread TaskAction="Sleep" Timeout="2000"/>
    ///<!-- Set a thread to spinwait for a period -->
    ///<MSBuild.ExtensionPack.Framework.Thread TaskAction="SpinWait" Iterations="1000000000"/>
    ///<!-- Abort a thread. Only use in exceptional circumstances -->
    ///<!--<MSBuild.ExtensionPack.Framework.Thread TaskAction="Abort"/>-->
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class Thread : BaseTask
    {
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
                case "Abort":
                    this.LogTaskMessage("Aborting Current Thread");
                    System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
                    thisThread.Abort();
                    break;

                case "Sleep":
                    this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "Sleeping all threads for: {0}ms", this.Timeout));
                    System.Threading.Thread.Sleep(this.Timeout);
                    break;

                case "SpinWait":
                    this.LogTaskMessage(string.Format(CultureInfo.CurrentCulture, "SpinWait all threads for: {0} iterations", this.Iterations));
                    System.Threading.Thread.SpinWait(this.Iterations);
                    break;

                default:
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Number of iterations to wait for
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Number of millseconds to sleep for
        /// </summary>
        public int Timeout { get; set; }
    }
}
