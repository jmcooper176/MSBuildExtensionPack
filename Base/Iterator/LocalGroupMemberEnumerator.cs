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
using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;

namespace MSBuild.ExtensionPack.Base.Iterator
{
    [SupportedOSPlatform("Windows")]
    public class LocalGroupMemberEnumerator : IDisposable, IEnumerator<Principal>
    {
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Group.Dispose();
                    Context.Dispose();
                }

                disposedValue = true;
            }
        }

        internal int index;

        internal PrincipalContext Context { get; }

        internal GroupPrincipal Group { get; }

        internal IEnumerable<Principal> Members => Group.GetMembers();

        public LocalGroupMemberEnumerator(string groupName)
        {
            Context = new(ContextType.Machine);
            GroupName = groupName;
            Group = new(Context, GroupName);
            index = 0;
            Current = Members.ElementAt(index);
        }

        public Principal Current { get; private set; }

        public string GroupName { get; set; }

        object? IEnumerator.Current
        {
            get
            {
                if (index < 0 || index > Members.Count())
                {
                    throw new ArgumentOutOfRangeException("index", index, $"Index {index} must be in the range [0, {Members.Count()}");
                }

                return Current;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<Principal> GetEnumerator() => new LocalGroupMemberEnumerator(GroupName);

        public bool MoveNext()
        {
            Current = Members.ElementAt(++index);
            return true;
        }

        public void Reset()
        {
            index = 0;
            Current = Members.ElementAt(index);
        }
    }
}
