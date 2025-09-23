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
namespace MSBuild.ExtensionPack.Base.Property
{
    /// <summary>
    /// Implements an abstract generic indexer to support updating the public properties of an object by name.
    /// </summary>
    /// <typeparam name="T">Specifies the <see cref="Type"/> of object instance to apply the indexer to.</typeparam>
    /// <typeparam name="TValue">Specifies the value <see cref="Type"/> returned or updated by the indexer.</typeparam>
    public abstract class GenericIndexer<T, TValue> where T : IList<TValue?>, new()
    {
        #region Public Indexers

        /// <summary>
        /// Indexer to support update the public properties of arbitrary objects.
        /// </summary>
        /// <param name="index">Specifies the indexer of <see cref="Type"/><see name="Index"/> for update</param>
        /// <returns>Returns a value of <typeparamref name="TValue"/> for the getter; otherwise, nothing for the setter.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <see cref="ValidateIndex(Index)"/> returns <see langref="false"/>.</exception>
        public TValue? this[Index index]
        {
            get => ValidateIndex(index) ? GetValue(index) : throw new ArgumentOutOfRangeException(nameof(index), index, $"Index '{nameof(index)}' with value '{index}' is out of range.");

            set
            {
                if (!ValidateIndex(index))
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, $"Index '{nameof(index)}' with value '{index}' is out of range.");
                }
                else
                {
                    SetValue(index, value);
                }
            }
        }

        #endregion Public Indexers

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating the instance to be indexed with implements <see cref="IList{T}"/>.
        /// </summary>
        public virtual T Instance { get; set; } = new();

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Encapsulates getting a value of <typeparamref name="TValue"/> for <see cref="Instance"/>.
        /// </summary>
        /// <param name="index">
        /// Specifies the indexer of <see cref="Index"/> to determine which value of <see cref="Instance"/> of <typeparamref
        /// name="T"/> to get.
        /// </param>
        /// <returns>Returns a value of <typeparamref name="TValue"/>.</returns>
        public virtual TValue? GetValue(Index index)
        {
            return ValidateIndex(index) ? Instance[index] : default;
        }

        /// <summary>
        /// Encapsulates setting a value of <typeparamref name="TValue"/> at position of <see cref="Type"/><see cref="Index"/> in
        /// <see cref="Instance"/> of <typeparamref name="T"/> to set.
        /// </summary>
        /// <param name="index">
        /// Specifies the indexer of <see cref="Index"/> to set a value of <see cref="Instance"/> of <typeparamref name="T"/> with
        /// value of <typeparamref name="TValue"/>..
        /// </param>
        /// <param name="value">
        /// Specifies the value of <typeparamref name="TValue"/> to set in <see cref="Instance"/> of <typeparamref name="T"/>.
        /// </param>
        public virtual void SetValue(Index index, TValue? value)
        {
            if (ValidateIndex(index))
            {
                Instance[index] = value;
            }
        }

        /// <summary>
        /// Encapsulates a range check on <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Specifies the <see cref="Index"/> value to check for being in range.</param>
        /// <returns><see langref="true"/> if <paramref name="index"/> is in range; otherwise <see langref="false"/>.</returns>
        public virtual bool ValidateIndex(Index index)
        {
            Range range = new(0, Instance.Count - 1);
            return index.Value >= range.Start.Value && index.Value <= range.End.Value;
        }

        #endregion Public Methods
    }
}
