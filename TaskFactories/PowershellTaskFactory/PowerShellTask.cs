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
    using System.Management.Automation.Runspaces;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// A task that executes a Windows PowerShell script.
    /// </summary>
    internal class PowerShellTask : Task, IGeneratedTask, IDisposable
    {
        #region Private Fields

        private bool disposedValue;

        /// <summary>
        /// The context that the Windows PowerShell script will run under.
        /// </summary>
        private Pipeline pipeline;

        #endregion Private Fields

        #region Protected Methods

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

        #endregion Protected Methods

        #region Internal Constructors

        internal PowerShellTask(string script)
        {
            pipeline = RunspaceFactory.CreateRunspace().CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Runspace.Open();
            pipeline.Runspace.SessionStateProxy.SetVariable("log", Log);
        }

        #endregion Internal Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}
