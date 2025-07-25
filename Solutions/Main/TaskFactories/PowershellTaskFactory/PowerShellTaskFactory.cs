// This file is part of CycloneDX CLI Tool
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
namespace MSBuild.ExtensionPack.TaskFactory.PowerShell
{
    using Microsoft.Build.Framework;

    using System;
    using System.Collections.Generic;

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
    ///$log.LogError("Oops!  I can't count that high. :(")
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
        /// The type of Task
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
