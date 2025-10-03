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
using System.Collections;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace MSBuild.ExtensionPack.Base.Iterator
{
    [SupportedOSPlatform("Windows")]
    public class LocalGroupEnumerator : IDisposable, IEnumerator<DirectoryEntry>
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose of managed code
                    LocalMachine.Dispose();
                }

                // set large managed properties to nul

                // dispose of unmanaged code

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Internal Fields

        internal int index = 0;

        #endregion Internal Fields

        #region Internal Properties

        internal DirectoryEntry LocalMachine { get; }

        #endregion Internal Properties

        #region Public Constructors

        public LocalGroupEnumerator(string? machineName = null)
                    : this(new DirectoryEntry($"WinNT://{machineName ?? Environment.MachineName}"))
        {
        }

        public LocalGroupEnumerator(Func<DirectoryEntry, bool> filter, string? machineName = null)
            : this(localMachine: new DirectoryEntry($"WinNT://{machineName ?? Environment.MachineName}"), filter: filter)
        {
        }

        public LocalGroupEnumerator(DirectoryEntry localMachine)
        {
            LocalMachine = localMachine;
            List<DirectoryEntry> allEntries = [.. LocalMachine.Children.Cast<DirectoryEntry>()];
            LocalGroups = [.. allEntries.Where(e => e.SchemaClassName.Equals("Group", StringComparison.Ordinal))];
            index = 0;
            Current = LocalGroups[index];
        }

        public LocalGroupEnumerator(DirectoryEntry localMachine, Func<DirectoryEntry, bool> filter)
        {
            LocalMachine = localMachine;
            List<DirectoryEntry> allEntries = [.. LocalMachine.Children.Cast<DirectoryEntry>()];
            LocalGroups = [.. allEntries.Where(e => e.SchemaClassName.Equals("Group", StringComparison.Ordinal) && filter.Invoke(e))];
            index = 0;
            Current = LocalGroups[index];
        }

        #endregion Public Constructors

        #region Public Properties

        public DirectoryEntry Current { get; private set; }
        public List<DirectoryEntry> LocalGroups { get; }

        object IEnumerator.Current
        {
            get
            {
                if (index < 0 || index > LocalGroups.Count)
                {
                    throw new ArgumentOutOfRangeException("index", index, $"Index must be in the range [0, {LocalGroups.Count - 1}]");
                }

                return Current;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<DirectoryEntry> GetEnumerator() => new LocalGroupEnumerator(Environment.MachineName);

        public bool MoveNext()
        {
            Current = LocalGroups[++index];
            return true;
        }

        public void Reset()
        {
            index = 0;
            Current = LocalGroups[index];
        }

        #endregion Public Methods
    }
}
