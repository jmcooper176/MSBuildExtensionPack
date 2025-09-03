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
namespace MSBuild.ExtensionPack.TaskFactory.Dlr
{
    using Microsoft.Build.Framework;

    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// A task factory that enables inline scripts to execute as part of an MSBuild-based build.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<?xml version="1.0" encoding="utf-8"?>
    ///<Project ToolsVersion="4.0"
    ///DefaultTargets="Build"
    ///xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<UsingTask
    ///TaskName="HelloWorld"
    ///TaskFactory="DlrTaskFactory"
    ///AssemblyFile="$(TaskFactoryPath)MSBuild.ExtensionPack.TaskFactory.Dlr.dll">
    ///<ParameterGroup>
    ///<Name Required="true"/>
    ///<TaskMessage Output="true"/>
    ///</ParameterGroup>
    ///<Task>
    ///<Code Type="Fragment"
    ///Language="rb">
    ///<!-- Make this a proper CDATA section before running. -->
    ///[CDATA[
    ///self.task_message = "Hello #{name} from Ruby".to_clr_string
    ///log.log_message(task_message);
    ///]
    ///</Code>
    ///</Task>
    ///</UsingTask>
    ///<PropertyGroup>
    ///<YourName Condition=" '$(YourName)'=='' ">World</YourName>
    ///</PropertyGroup>
    ///<Target Name="Build">
    ///<HelloWorld Name="$(YourName)">
    ///<Output PropertyName="RubyOut"
    ///TaskParameter="TaskMessage"/>
    ///</HelloWorld>
    ///<Message Text="Message from task: $(RubyOut)"
    ///Importance="high" />
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class DlrTaskFactory : ITaskFactory
    {
        /// <summary>
        /// The in and out parameters of the generated tasks.
        /// </summary>
        private IDictionary<string, TaskPropertyInfo> parameterGroup;

        /// <summary>
        /// The body of the script to execute.
        /// </summary>
        private XElement taskXml;

        /// <summary>
        /// Gets the name of the factory.
        /// </summary>
        /// <value>The name of the factory.</value>
        public string FactoryName => GetType().Name;

        /// <summary>
        /// Gets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public Type TaskType => typeof(DlrTask);

        /// <summary>
        /// Cleans up the task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void CleanupTask(ITask task)
        {
            IDisposable disposableTask = task as IDisposable;
            disposableTask?.Dispose();
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <param name="taskFactoryLoggingHost">The task factory logging host.</param>
        /// <returns>ITask item</returns>
        public ITask CreateTask(IBuildEngine taskFactoryLoggingHost)
        {
            return new DlrTask(this, taskXml, taskFactoryLoggingHost);
        }

        /// <summary>
        /// Gets the task parameters.
        /// </summary>
        /// <returns>TaskPropertyInfo[]</returns>
        public TaskPropertyInfo[] GetTaskParameters()
        {
            return [.. parameterGroup.Values];
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="taskName">              Name of task</param>
        /// <param name="parameterGroup">        IDictionary</param>
        /// <param name="taskBody">              Body of task</param>
        /// <param name="taskFactoryLoggingHost">IBuildEngine</param>
        /// <returns>bool</returns>
        public bool Initialize(string taskName, IDictionary<string, TaskPropertyInfo> parameterGroup, string taskBody, IBuildEngine taskFactoryLoggingHost)
        {
            this.parameterGroup = parameterGroup;
            taskXml = XElement.Parse(taskBody);
            return true;
        }
    }
}
