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
    using System.Collections;
    using System.Collections.Generic;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class RecordListObject : IMsiCom, ICollection<RecordObject>
    {
        private bool disposedValue;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RecordListObject()
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
                    // TODO: dispose managed state (managed objects)
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        public Type? ComType { get; private set; }
        public int Count { get; }
        public Guid IID => new("000C1096-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public bool IsReadOnly { get; }
        public string ProgId => string.Empty;

        public void Add(RecordObject item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(RecordObject item) => throw new NotImplementedException();

        public void CopyTo(RecordObject[] array, int arrayIndex) => throw new NotImplementedException();

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<RecordObject> GetEnumerator() => throw new NotImplementedException();

        public bool Remove(RecordObject item) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
