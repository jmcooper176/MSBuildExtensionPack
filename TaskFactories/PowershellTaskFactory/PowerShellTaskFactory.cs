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
namespace MSBuild.ExtensionPack.TaskFactory.PowerShell
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Build.Framework;

    /// <summary>
    /// A task factory that enables inline PowerShell scripts to execute as part of an MSBuild-based build.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<AssemblyFile>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.TaskFactory.PowerShell.dll</AssemblyFile>
    ///<AssemblyFile Condition="Exists('$(MSBuildProjectDirectory)\..\..\..\BuildBinaries\MSBuild.ExtensionPack.TaskFactory.PowerShell.dll')">$(MSBuildProjectDirectory)\..\..\..\BuildBinaries\MSBuild.ExtensionPack.TaskFactory.PowerShell.dll</AssemblyFile>
    ///</PropertyGroup>
    ///<UsingTask TaskFactory="PowerShellTaskFactory" TaskName="Add" AssemblyFile="$(AssemblyFile)">
    ///<ParameterGroup>
    ///<First Required="true" ParameterType="System.Int32" />
    ///<Second Required="true" ParameterType="System.Int32" />
    ///<Sum Output="true" />
    ///</ParameterGroup>
    ///<Task>
    ///<!-- Make this a proper CDATA section before running. -->
    ///CDATA[
    ///$log.LogMessage([Microsoft.Build.Framework.MessageImportance]"High", "Hello from PowerShell!  Now adding {0} and {1}.", $first, $second)
    ///if ($first + $second -gt 100) {
    ///$log.LogTaskError("Oops!  I can't count that high. :(")
    ///}
    ///$sum = $first + $second
    ///]]
    ///</Task>
    ///</UsingTask>
    ///<UsingTask TaskFactory="PowerShellTaskFactory" TaskName="Subtract" AssemblyFile="$(AssemblyFile)">
    ///<ParameterGroup>
    ///<First Required="true" ParameterType="System.Int32" />
    ///<Second Required="true" ParameterType="System.Int32" />
    ///<Difference Output="true" />
    ///</ParameterGroup>
    ///<Task>
    ///<!-- Make this a proper CDATA section before running. -->
    ///CDATA[
    ///$difference = $first - $second
    ///]
    ///</Task>
    ///</UsingTask>
    ///<PropertyGroup>
    ///<!-- Try making the sum go over 100 to see what happens. -->
    ///<FirstNumber>5</FirstNumber>
    ///<SecondNumber>8</SecondNumber>
    ///</PropertyGroup>
    ///<Target Name="Build">
    ///<Add First="$(FirstNumber)" Second="$(SecondNumber)">
    ///<Output TaskParameter="Sum" PropertyName="MySum" />
    ///</Add>
    ///<Message Importance="High" Text="The $(FirstNumber) + $(SecondNumber) = $(MySum)" />
    ///<Subtract First="$(FirstNumber)" Second="$(SecondNumber)">
    ///<Output TaskParameter="Difference" PropertyName="MyDifference" />
    ///</Subtract>
    ///<Message Importance="High" Text="The $(FirstNumber) - $(SecondNumber) = $(MyDifference)" />
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="ITaskFactory"/>
    public class PowerShellTaskFactory : ITaskFactory
    {
        /// <summary>
        /// The in and out parameters of the generated tasks.
        /// </summary>
        private IDictionary<string, TaskPropertyInfo> paramGroup;

        /// <summary>
        /// The body of the PowerShell script given by the project file.
        /// </summary>
        private string script;

        /// <summary>
        /// Get the Factory Name
        /// </summary>
        public string FactoryName => GetType().Name;

        /// <summary>
        /// The <see cref="Type"/> of Task
        /// </summary>
        public Type TaskType => typeof(PowerShellTask);

        /// <summary>
        /// Cleanup the Task
        /// </summary>
        /// <param name="task">ITask</param>
        public void CleanupTask(ITask task)
        {
            IDisposable disposableTask = task as IDisposable;
            disposableTask?.Dispose();
        }

        /// <summary>
        /// Create a Task.
        /// </summary>
        /// <param name="taskFactoryLoggingHost">IBuildEngine</param>
        /// <returns>ITask</returns>
        public ITask CreateTask(IBuildEngine taskFactoryLoggingHost)
        {
            return new PowerShellTask(script);
        }

        /// <summary>
        /// Get the Task Parameters
        /// </summary>
        /// <returns>TaskPropertyInfo</returns>
        public TaskPropertyInfo[] GetTaskParameters()
        {
            return [.. paramGroup.Values];
        }

        /// <summary>
        /// Initialize the Task Factory
        /// </summary>
        /// <param name="taskName">              The name of the Task</param>
        /// <param name="parameterGroup">        IDictionary</param>
        /// <param name="taskBody">              The Task body</param>
        /// <param name="taskFactoryLoggingHost">IBuildEngine</param>
        /// <returns>bool</returns>
        public bool Initialize(string taskName, IDictionary<string, TaskPropertyInfo> parameterGroup, string taskBody, IBuildEngine taskFactoryLoggingHost)
        {
            paramGroup = parameterGroup;
            script = taskBody;

            return true;
        }
    }
}
