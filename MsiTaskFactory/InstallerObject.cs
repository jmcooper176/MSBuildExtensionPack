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

    public class InstallerObject : IMsiCom
    {
        private bool disposedValue;

        ~InstallerObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LastError?.Dispose();
                }

                ComUtility.Release(Instance);
                LastError?.Dispose();
                Instance = null;
                ComType = null;
                disposedValue = true;
            }
        }

        public InstallerObject()
        {
            ComType = ComUtility.GetTypeFromProgId(ProgId);
            Instance = ComUtility.CreateComInstance(ProgId);
        }

        public Type? ComType { get; private set; }
        public Guid IID => new(" 000C1090-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }
        public string ProgId => "WindowsInstaller.Installer";

        public static object? ToInstance(InstallerObject? thick) => thick?.Instance;

        public static InstallerObject? ToObject(object? thin) => new(thin);

        public RecordObject? CreateRecord(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, RecordObject.MAX_COUNT, nameof(count));

            return ComType is not null && Instance is not null ? (RecordObject?)ComUtility.InvokeComMethod(ComType, "CreateRecord", Instance, [count], null) : null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public RecordObject? LastErrorRecord()
        {
            return ComType is not null && Instance is not null ? (RecordObject?)ComUtility.InvokeComMethod(ComType, "LastErrorRecord", Instance, null, null) : null;
        }

        public DatabaseObject? OpenDatabase(string? name, OpenDatabaseMode mode)
        {
            try
            {
                return ComType is not null && Instance is not null ? (DatabaseObject?)ComUtility.InvokeComMethod(ComType, "OpenDatabase", Instance, [name, (int)mode], null) : null;
            }
            finally
            {
                LastError = LastErrorRecord();
            }
        }

        public DatabaseObject? OpenDatabase(string? name, string? newName)
        {
            try
            {
                return ComType is not null && Instance is not null ? (DatabaseObject?)ComUtility.InvokeComMethod(ComType, "OpenDatabase", Instance, [name, newName], null) : null;
            }
            finally
            {
                LastError = LastErrorRecord();
            }
        }

        public SessionObject? OpenPackage(string packagePath, int options)
        {
            return ComType is not null && Instance is not null ? (SessionObject?)ComUtility.InvokeComMethod(ComType, "OpenPackage", Instance, [packagePath, options], null) : null;
        }

        public SessionObject? OpenProduct(Guid productCode)
        {
            return ComType is not null && Instance is not null ? (SessionObject?)ComUtility.InvokeComMethod(ComType, "OpenProduct", Instance, [productCode.ToString()], null) : null;
        }
    }
}
