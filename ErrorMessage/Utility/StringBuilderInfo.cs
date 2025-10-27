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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace MSBuild.ExtensionPack.ErrorMessage.Utility
{
    /// <summary>
    /// Implements iteration for <see cref="StringBuilder"/> similar to <see cref="StringInfo"/>.
    /// </summary>
    /// <seealso cref="IEnumerable{T}"/>
    /// <seealso cref="IEnumerator{T}"/>
    /// <seealso cref="IEnumerator"/>
    /// <seealso cref="ICloneable"/>
    /// <seealso cref="IEquatable{StringBuilder}"/>
    /// <seealso cref="IEqualityComparer{StringBuilder}"/>
    /// <seealso cref="IDisposable"/>
    public class StringBuilderInfo : IEnumerable<char>, IEnumerator<char>, IEnumerator, ICloneable, IEquatable<StringBuilder>, IEqualityComparer<StringBuilder>, IDisposable
    {
        /// <summary>
        /// If <see langref="true"/>, this instance has been disposed; otherwise, <see langref="false"/>.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langref="true"/> to release both managed and unmanaged resources; <see langref="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Builder.Clear();
                }

                Reset();
                disposedValue = true;
            }
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        protected virtual void Reset()
        {
            Index = -1;
        }

        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        internal StringBuilder Builder { get; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        internal int Index { get; set; }

        /// <summary>
        /// Gets the range.
        /// </summary>
        /// <value>The range.</value>
        internal Range Range { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderInfo"/> class.
        /// </summary>
        public StringBuilderInfo()
        {
            Builder = new(capacity: StringBuilderExtension.OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);
            Index = -1;
            Range = new(0, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderInfo"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringBuilderInfo(string? value)
        {
            Builder = new(value, Math.Max(value?.Length ?? 0, StringBuilderExtension.OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY));
            Index = -1;
            Range = new(0, Math.Max(1, Builder.Length));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderInfo"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringBuilderInfo(StringBuilder value)
            : this(value.ToString())
        {
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public char Current { get; private set; }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return new StringBuilderInfo(Builder);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of <see cref="Type"/><see cref="StringBuilder"/> to compare.</param>
        /// <param name="y">The second object of <see cref="Type"/><see cref="StringBuilder"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(StringBuilder? x, StringBuilder? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x is null ^ y is null)
            {
                return false;
            }
            else if (x!.Length != y!.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < x.Length && i < y.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same <see cref="Type"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(StringBuilder? other)
        {
            return Equals(Builder, other);
        }

        /// <summary>
        /// Gets the chunk enumerator.
        /// </summary>
        /// <returns></returns>
        public StringBuilder.ChunkEnumerator GetChunkEnumerator() => Builder.GetChunks();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<char> GetEnumerator()
        {
            if (MoveNext())
            {
                yield return Current;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode([DisallowNull] StringBuilder obj)
        {
            return HashCode.Combine(obj.Length, obj.Capacity, obj.MaxCapacity, obj.ToString().GetHashCode());
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the
        /// enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            ++Index;

            if (Index >= Range.Start.Value && Index < Range.End.Value)
            {
                Current = Builder[Index];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the
        /// enumerator has passed the end of the collection.
        /// </returns>
        bool IEnumerator.MoveNext()
        {
            return MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        void IEnumerator.Reset()
        {
            Reset();
        }
    }
}
