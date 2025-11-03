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
namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class RecordObject : IMsiCom
    {
        private bool disposedValue;

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RecordObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        protected InstallerObject? Installer { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Installer?.Dispose();
                }

                ComUtility.Release(Instance);
                Instance = null;
                disposedValue = true;
            }
        }

        public const int MAX_COUNT = 65535;

        public RecordObject(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MAX_COUNT, nameof(count));

            Installer = new InstallerObject();
            Instance = Installer.CreateRecord(count);
        }

        public Type? ComType { get; private set; }
        public Guid IID => new("000C1093-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }
        public string ProgId => string.Empty;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void SetStream(int field, string file)
        {
            if (ComType is not null)
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType, "SetStream", Instance, [field, file], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }
    }
}
