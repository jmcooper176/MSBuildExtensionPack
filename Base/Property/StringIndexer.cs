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

namespace MSBuild.ExtensionPack.Base.Property
{
    public abstract class StringIndexer : IList<string?>
    {
        #region Private Fields

        private readonly List<string?> storage;

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndexer"/> class.
        /// </summary>
        protected StringIndexer()
            : this([])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndexer"/> class.
        /// </summary>
        /// <param name="collection">
        /// Specifies the <see cref="ICollection{T}"/> of values to initialize <see cref="StringIndexer"/> with.
        /// </param>
        protected StringIndexer(ICollection<string?> collection)
        {
            storage = [.. collection];
        }

        #endregion Protected Constructors

        #region Public Indexers

        /// <summary>
        /// Implements the <see cref="IList{T}"/> indexer.
        /// </summary>
        /// <param name="index">Specifies the index into <see cref="storage"/>.</param>
        /// <returns>Returns a string for the getter and void for the setter.</returns>
        public string? this[int index]
        {
            get
            {
                return storage[index];
            }

            set
            {
                storage[index] = value;
            }
        }

        /// <summary>
        /// Implements the <see cref="IList{T}"/> indexer using <see cref="Index"/>.
        /// </summary>
        /// <param name="index">Specifies the value of <see cref="Index"/> to index into <see cref="storage"/>.</param>
        /// <returns>Returns a string for the getter and void for the setter.</returns>
        public string? this[Index index]
        {
            get
            {
                return this[index.Value];
            }

            set
            {
                this[index.Value] = value;
            }
        }

        /// <summary>
        /// Specifies a string indexer into <see cref="storage"/>.
        /// </summary>
        /// <param name="index">Specifies the string into <see cref="storage"/>.</param>
        /// <returns>Returns a string for the getter and void for the setter.</returns>
        public abstract string? this[string index] { get; set; }

        #endregion Public Indexers

        #region Public Properties

        public int Count => storage.Count;

        public bool IsReadOnly => false;

        #endregion Public Properties

        #region Public Methods

        public void Add(string? item)
        {
            storage.Add(item);
        }

        public void Clear()
        {
            storage.Clear();
        }

        public bool Contains(string? item)
        {
            return storage.Contains(item);
        }

        public void CopyTo(string?[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string?> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        public int IndexOf(string? item)
        {
            return storage.IndexOf(item);
        }

        public void Insert(int index, string? item)
        {
            storage.Insert(index, item);
        }

        public bool Remove(string? item)
        {
            return storage.Remove(item);
        }

        public void RemoveAt(int index)
        {
            storage.RemoveAt(index);
        }

        public abstract Index ToIndex(string index);

        public abstract string ToStringIndexer(Index index);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Public Methods
    }
}
