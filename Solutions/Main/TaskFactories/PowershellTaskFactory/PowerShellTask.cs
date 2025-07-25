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
    using Microsoft.Build.Utilities;

    using System;
    using System.Management.Automation.Runspaces;

    /// <summary>
    /// A task that executes a Windows PowerShell script.
    /// </summary>
    internal class PowerShellTask : Task, IGeneratedTask, IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// The context that the Windows PowerShell script will run under.
        /// </summary>
        private Pipeline pipeline;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (pipeline.Runspace is not null)
                    {
                        pipeline.Runspace.Dispose();
                        pipeline.Dispose();
                    }
                }

                pipeline = null;

                disposedValue = true;
            }
        }

        internal PowerShellTask(string script)
        {
            pipeline = RunspaceFactory.CreateRunspace().CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Runspace.Open();
            pipeline.Runspace.SessionStateProxy.SetVariable("log", Log);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override bool Execute()
        {
            pipeline.Invoke();
            return !Log.HasLoggedErrors;
        }

        public object GetPropertyValue(TaskPropertyInfo property)
        {
            return pipeline.Runspace.SessionStateProxy.GetVariable(property.Name);
        }

        public void SetPropertyValue(TaskPropertyInfo property, object value)
        {
            pipeline.Runspace.SessionStateProxy.SetVariable(property.Name, value);
        }
    }
}
