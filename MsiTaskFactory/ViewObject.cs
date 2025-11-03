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

    public class ViewObject : IMsiCom
    {
        private bool disposedValue;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ViewObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        protected ViewObject(string? name, OpenDatabaseMode mode)
                            : this(new DatabaseObject(name, mode))
        {
        }

        protected ViewObject(string? name, string? newName)
            : this(new DatabaseObject(name, newName))
        {
        }

        protected ViewObject(DatabaseObject database)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Installer = new InstallerObject();
        }

        protected DatabaseObject? Database { get; private set; }

        protected InstallerObject? Installer { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                    Database?.Dispose();
                    Installer?.Dispose();
                    LastError?.Dispose();
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        public ViewObject(DatabaseObject database, string sql)
                    : this(database)
        {
            Instance = Database?.OpenView(sql);
            ComType = Instance?.GetType();
        }

        public RecordObject? this[ColumnInfoResult infoResult]
        {
            get
            {
                return GetColumnInfo(infoResult);
            }
        }

        public Type? ComType { get; private set; }
        public Guid IID => new("000C109C-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }
        public string ProgId => string.Empty;

        public void Close()
        {
            if (ComType is not null)
            {
                ComUtility.InvokeComVoidMethod(ComType, "Close", Instance, null, null);
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Execute(RecordObject record)
        {
            if (ComType is not null)
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType, "Execute", Instance, [record.Instance], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public RecordObject? Fetch()
        {
            if (ComType is not null)
            {
                return (RecordObject?)ComUtility.InvokeComMethod(ComType, "Fetch", Instance, null, null);
            }
            else
            {
                return null;
            }
        }

        public RecordObject? GetColumnInfo(ColumnInfoResult infoResult)
        {
            if (ComType is not null)
            {
                return (RecordObject?)ComUtility.InvokeComPropertyGet(ComType, "ColumnInfo", Instance, [(int)infoResult], null) ?? null;
            }
            else
            {
                return null;
            }
        }

        public string GetError()
        {
            if (ComType is not null)
            {
                return (string?)ComUtility.InvokeComMethod(ComType, "GetError", Instance, null, null) ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public void Modify(ViewModifyAction action, RecordObject record)
        {
            if (ComType is not null)
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType, "Modify", Instance, [(int)action, record.Instance], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }
    }
}
