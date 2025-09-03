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
    using Microsoft.Build.Utilities;
    using Microsoft.Scripting.Hosting;

    using System;
    using System.Xml.Linq;

    /// <summary>
    /// A task that executes a custom script.
    /// </summary>
    /// <remarks>
    /// This task can implement <see cref="IGeneratedTask"/> to support task properties that are defined in the script itself and
    /// not known at compile-time of this task factory.
    /// </remarks>
    internal class DlrTask : Task, IDisposable, IGeneratedTask
    {
        private readonly ScriptEngine engine;
        private readonly dynamic scope;
        private readonly XElement xelement;

        private bool disposedValue;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose of referenced objects implementing IDisposable here.
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overridden in a derived class, executes the task.
        /// </summary>
        /// <returns>true if the task successfully executed; otherwise, false.</returns>
        public override bool Execute()
        {
            try
            {
                engine.Execute(xelement.Value, scope);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public object GetPropertyValue(TaskPropertyInfo property)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">   The value to set.</param>
        public void SetPropertyValue(TaskPropertyInfo property, object value)
        {
            ((ScriptScope)scope).SetVariable(property.Name, value);
        }
    }
}
