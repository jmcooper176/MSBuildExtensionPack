// This file is part of MSBuildExtensionPack re-write to support .NET 9.0 and to modernize.
//
// Copyright (c) 2008-2025, John Merryweather Cooper. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
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

using Microsoft.Build.Framework;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public static class StringBuilderExtension
    {
        #region Public Fields

        /// <summary>
        /// The optimal initial stringbuilder capacity.
        /// </summary>
        public const int OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY = 16;

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        /// Alls the specified predicate.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder or predicate</exception>
        public static bool All([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            foreach (char item in builder)
            {
                if (!predicate.Invoke(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Anies the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static bool Any([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder.Count() < 1;
        }

        /// <summary>
        /// Anies the specified predicate.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder or predicate</exception>
        public static bool Any([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            foreach (var item in builder)
            {
                if (predicate.Invoke(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="ArgumentOutOfRangeException">capacity</exception>
        public static StringBuilder Append(this StringBuilder builder, ITaskItem value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(builder.Capacity + value.ItemSpec.Length, builder.MaxCapacity, nameof(builder.Capacity));

            return builder.Append(value.ItemSpec);
        }

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="ArgumentOutOfRangeException">capacity</exception>
        public static StringBuilder Append(this StringBuilder builder, DirectoryInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(builder.Capacity + value.FullName.Length, builder.MaxCapacity, nameof(builder.Capacity));

            return builder.Append(value.FullName);
        }

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="ArgumentOutOfRangeException">capacity</exception>
        public static StringBuilder Append(this StringBuilder builder, FileInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(builder.Capacity + value.FullName.Length, builder.MaxCapacity, nameof(builder));
            return builder.Append(value.FullName);
        }

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="ArgumentOutOfRangeException">capacity</exception>
        public static StringBuilder Append(this StringBuilder builder, FileSystemInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(builder.Capacity + value.FullName.Length, builder.MaxCapacity, nameof(builder.Capacity));

            if (value is FileInfo file)
            {
                return builder.Append(file.FullName);
            }
            else if (value is DirectoryInfo directory)
            {
                return builder.Append(directory.FullName);
            }
            else
            {
                return builder.Append(value);
            }
        }

        /// <summary>
        /// Appends the specified element.
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="source"> The source.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static StringBuilder Append<TElement>([AllowNull] this StringBuilder source, TElement element) where TElement : IFormattable
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return source.Append(element.ToString());
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="list">     The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static StringBuilder AppendJoin([AllowNull] this StringBuilder builder, string? separator, IEnumerable<ITaskItem> list)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            List<string?> convertedList = [];
            list.ToList().ForEach(i => convertedList.Add(i.ItemSpec));
            return builder.AppendJoin(separator, convertedList);
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="list">     The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static StringBuilder AppendJoin([AllowNull] this StringBuilder builder, string? separator, IEnumerable<string?> list)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(builder.Capacity + string.Join(separator, list).Length, builder.MaxCapacity, nameof(builder));

            return builder.AppendJoin(separator, list);
        }

        /// <summary>
        /// Ases the enumerable.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IEnumerable<char> AsEnumerable([AllowNull] this StringBuilder builder)
        {
            return builder.ToCharArray();
        }

        /// <summary>
        /// Casts the specified builder.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type"/> of the result.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IEnumerable<TResult> Cast<TResult>([AllowNull] this StringBuilder builder) where TResult : IConvertible
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder.AsEnumerable().Cast<TResult>() ?? Enumerable.Empty<TResult>();
        }

        /// <summary>
        /// Concats the specified second.
        /// </summary>
        /// <param name="first"> The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">first or second</exception>
        public static StringBuilder Concat([AllowNull] this StringBuilder first, StringBuilder? second)
        {
            ArgumentNullException.ThrowIfNull(first, nameof(first));
            ArgumentNullException.ThrowIfNull(second, nameof(second));

            return first.Append(second);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns><see langref="true"/> if [contains] [the specified character]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, char character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(character, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns><see langref="true"/> if [contains] [the specified character]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, char character, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(character: new ReadOnlySpan<char>(in character), comparison);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns><see langref="true"/> if [contains] [the specified character]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(character, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns><see langref="true"/> if [contains] [the specified character]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            foreach (var chunk in builder!.GetChunks())
            {
                if (chunk.Span.IndexOf(character, comparison) > -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, string value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(value: new ReadOnlySpan<char>(value.ToCharArray()), 0);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, string value, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(value: new ReadOnlySpan<char>(value.ToCharArray()), 0, comparison);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder"> The builder.</param>
        /// <param name="value">   The value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, string value, IEqualityComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(value: new ReadOnlySpan<char>(value.ToCharArray()), 0, comparer ?? EqualityComparer<char>.Default);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) && builder!.Contains(value, startIndex, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            foreach (var item in value[startIndex..])
            {
                if (!builder.Contains(item, comparison))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="comparer">  The comparer.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, IEqualityComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            foreach (var item in value[startIndex..])
            {
                if (!builder.ToString().Contains(item, comparer ?? EqualityComparer<char>.Default))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            foreach (var item in value.Slice(startIndex, length))
            {
                if (!builder.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, int length, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            foreach (var item in value.Slice(startIndex, length))
            {
                if (!builder.Contains(item, comparison))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <param name="comparer">  The comparer.</param>
        /// <returns><see langref="true"/> if [contains] [the specified value]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">builder</exception>
        /// <exception cref="System.ArgumentException">
        /// Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.
        /// </exception>
        public static bool Contains([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, int length, IEqualityComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (startIndex + length > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            foreach (var item in value.Slice(startIndex, length))
            {
                if (!builder.ToString().Contains(item, comparer))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="builder">         The builder.</param>
        /// <param name="startIndex">      The start index.</param>
        /// <param name="destination">     The destination.</param>
        /// <param name="destinationIndex">Index of the destination.</param>
        /// <param name="count">           The count.</param>
        /// <exception cref="System.ArgumentNullException">builder or destination</exception>
        /// <exception cref="System.ArgumentException">
        /// Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'. or Destination Index
        /// '{destinationIndex}' plus Count '{count}' is greater than Length '{destination.Count()}'.
        /// </exception>
        public static void CopyTo([AllowNull] this StringBuilder builder, int startIndex, StringBuilder? destination, int destinationIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(destination, nameof(destination));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            ArgumentOutOfRangeException.ThrowIfNegative(destinationIndex, nameof(destinationIndex));

            if (destinationIndex + count > destination.Count())
            {
                throw new ArgumentException($"Destination Index '{destinationIndex}' plus Count '{count}' is greater than Length '{destination.Count()}'.");
            }

            destination.Capacity = destinationIndex + count;
            destination.Insert(destinationIndex, builder.ToCharArray(startIndex, count));
        }

        /// <summary>
        /// Counts the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static int Count([AllowNull] this StringBuilder builder)
        {
            return builder is not null ? builder!.Length : 0;
        }

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">builder or predicate</exception>
        public static int Count([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            int counter = 0;

            foreach (var item in builder)
            {
                if (predicate.Invoke(item))
                {
                    counter++;
                }
            }

            return counter;
        }

        /// <summary>
        /// Creates the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return new StringBuilder(capacity);
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">  The format.</param>
        /// <param name="first">   The first.</param>
        /// <returns></returns>
        public static StringBuilder Create(IFormatProvider? provider, string format, object? first)
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first));
        }

        public static StringBuilder Create(IFormatProvider? provider, string format, object? first, object? second)
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first, second));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create(IFormatProvider? provider, string format, params object?[] arguments)
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">  The format.</param>
        /// <param name="first">   The first.</param>
        /// <param name="second">  The second.</param>
        /// <param name="third">   The third.</param>
        /// <returns></returns>
        public static StringBuilder Create(IFormatProvider? provider, string format, object? first, object? second, object? third)
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first, second, third));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T>(IFormatProvider? provider, string format, Tuple<T> arguments)
            where T : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the 1.</typeparam>
        /// <typeparam name="T2">The <see cref="Type"/> of the 2.</typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T1, T2>(IFormatProvider? provider, string format, Tuple<T1, T2> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the 1.</typeparam>
        /// <typeparam name="T2">The <see cref="Type"/> of the 2.</typeparam>
        /// <typeparam name="T3">The <see cref="Type"/> of the 3.</typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T1, T2, T3>(IFormatProvider? provider, string format, Tuple<T1, T2, T3> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3));
        }

        public static StringBuilder Create<T1, T2, T3, T4>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the 1.</typeparam>
        /// <typeparam name="T2">The <see cref="Type"/> of the 2.</typeparam>
        /// <typeparam name="T3">The <see cref="Type"/> of the 3.</typeparam>
        /// <typeparam name="T4">The <see cref="Type"/> of the 4.</typeparam>
        /// <typeparam name="T5">The <see cref="Type"/> of the 5.</typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T1, T2, T3, T4, T5>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the 1.</typeparam>
        /// <typeparam name="T2">The <see cref="Type"/> of the 2.</typeparam>
        /// <typeparam name="T3">The <see cref="Type"/> of the 3.</typeparam>
        /// <typeparam name="T4">The <see cref="Type"/> of the 4.</typeparam>
        /// <typeparam name="T5">The <see cref="Type"/> of the 5.</typeparam>
        /// <typeparam name="T6">The <see cref="Type"/> of the 6.</typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T1, T2, T3, T4, T5, T6>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5, T6> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
            where T6 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5, arguments.Item6));
        }

        /// <summary>
        /// Creates the specified provider.
        /// </summary>
        /// <typeparam name="T1">The <see cref="Type"/> of the 1.</typeparam>
        /// <typeparam name="T2">The <see cref="Type"/> of the 2.</typeparam>
        /// <typeparam name="T3">The <see cref="Type"/> of the 3.</typeparam>
        /// <typeparam name="T4">The <see cref="Type"/> of the 4.</typeparam>
        /// <typeparam name="T5">The <see cref="Type"/> of the 5.</typeparam>
        /// <typeparam name="T6">The <see cref="Type"/> of the 6.</typeparam>
        /// <typeparam name="T7">The <see cref="Type"/> of the 7.</typeparam>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder Create<T1, T2, T3, T4, T5, T6, T7>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5, T6, T7> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
            where T6 : IFormattable
            where T7 : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5, arguments.Item6, arguments.Item7));
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   if set to <see langref="true"/> [value].</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(bool value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(byte value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static StringBuilder Create(DateTime value, IFormatProvider? provider)
        {
            return Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(decimal value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(double value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(float value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">          The value.</param>
        /// <param name="capacity">       The capacity.</param>
        /// <param name="maximumCapacity">The maximum capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(int value, int capacity, int maximumCapacity = int.MaxValue)
        {
            return Create(capacity, maximumCapacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(long value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
/        public static StringBuilder Create(object? value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(sbyte value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(short value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(uint value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(ulong value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(ushort value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(capacity).Append(value);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="value">   The value.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static StringBuilder Create<TValue>(TValue value, IFormatProvider? provider) where TValue : IFormattable
        {
            return Create(string.Format(provider ?? CultureInfo.InvariantCulture, "{0}", value));
        }

        /// <summary>
        /// Creates the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static StringBuilder Create(FileInfo source)
        {
            return Create(source.OpenText());
        }

        /// <summary>
        /// Creates the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
/        public static StringBuilder Create(StreamReader reader)
        {
            return Create(reader.ReadToEnd());
        }

        /// <summary>
        /// Creates the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static StringBuilder Create(XmlDocument xml)
        {
            return Create(xml.InnerXml);
        }

        /// <summary>
        /// Creates the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static StringBuilder Create(JsonDocument json)
        {
            return Create(json.ToString());
        }

        /// <summary>
        /// Creates the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static StringBuilder Create(XDocument xml)
        {
            return Create(xml.Root?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Creates the specified <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="capacity">  The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(ICollection<char> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
            StringBuilder accumulator = Create(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified ordered list.
        /// </summary>
        /// <param name="orderedList">The ordered list.</param>
        /// <param name="capacity">   The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(IOrderedEnumerable<char> orderedList, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = Create(capacity);

            foreach (var item in orderedList)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified ordered list.
        /// </summary>
        /// <param name="orderedList">The ordered list.</param>
        /// <param name="capacity">   The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(IOrderedEnumerable<string?> orderedList, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = Create(capacity);

            foreach (var item in orderedList)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="capacity">  The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(ICollection<string?> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = Create(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/>.</param>
        /// <param name="capacity">  The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create<TElement>(ICollection<TElement> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY) where TElement : IFormattable
        {
            StringBuilder accumulator = Create(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item.ToString());
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static StringBuilder Create(char value, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            return Create(new string(value, count));
        }

        /// <summary>
        /// Creates the specified capacity.
        /// </summary>
        /// <param name="capacity">       The capacity.</param>
        /// <param name="maximumCapacity">The maximum capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(int capacity, int maximumCapacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maximumCapacity, capacity, nameof(maximumCapacity));

            return new StringBuilder(capacity, maximumCapacity);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">   The value.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(string? value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return new StringBuilder(value, capacity);
        }

        /// <summary>
        /// Creates the specified array.
        /// </summary>
        /// <param name="array">   The array.</param>
        /// <param name="capacity">The capacity.</param>
        /// <returns></returns>
        public static StringBuilder Create(char[]? array, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return Create(new string(array), capacity);
        }

        /// <summary>
        /// Creates the specified value.
        /// </summary>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <param name="capacity">  The capacity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{value.Length}'.</exception>
        public static StringBuilder Create([AllowNull] string value, int startIndex, int count, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(value);
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, value.Length, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > value.Length)
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{value.Length}'.");
            }

            return new StringBuilder(value, startIndex, count, capacity);
        }

        /// <summary>
        /// Creates the specified array.
        /// </summary>
        /// <param name="array">     The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <param name="capacity">  The capacity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{array.Length}'.</exception>
        public static StringBuilder Create(char[] array, int startIndex, int count, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, array.Length, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > array.Length)
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{array.Length}'.");
            }

            return Create(new string(array, startIndex, count), capacity);
        }

        /// <summary>
        /// Creates the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder Create([AllowNull] StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            StringBuilder accumulator = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in builder)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static StringBuilder Create([AllowNull] StringBuilder builder, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            StringBuilder accumulator = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in builder.Slice(startIndex))
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Create([AllowNull] StringBuilder builder, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            StringBuilder accumulator = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in builder.Slice(startIndex, count))
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        /// <summary>
        /// Creates the with base64 character array.
        /// </summary>
        /// <param name="array">  The array.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithBase64CharArray([AllowNull] byte[] array, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            return CreateWithBase64CharArray(array, 0, array.Length, 0, options);
        }

        /// <summary>
        /// Creates the with base64 character array.
        /// </summary>
        /// <param name="array">    The array.</param>
        /// <param name="offsetIn"> The offset in.</param>
        /// <param name="length">   The length.</param>
        /// <param name="offsetOut">The offset out.</param>
        /// <param name="options">  The options.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithBase64CharArray([AllowNull] byte[] array, int offsetIn, int length, int offsetOut, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentOutOfRangeException.ThrowIfNegative(offsetIn, nameof(offsetIn));
            ArgumentOutOfRangeException.ThrowIfNegative(offsetOut, nameof(offsetOut));

            char[] destination = new char[array.Length];
            Convert.ToBase64CharArray(array, offsetIn, length, destination, offsetOut, options);
            return Create(destination);
        }

        /// <summary>
        /// Creates the with base64 string.
        /// </summary>
        /// <param name="array">  The array.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithBase64String([AllowNull] byte[] array, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            return CreateWithBase64String(array, 0, array.Length, options);
        }

        /// <summary>
        /// Creates the with base64 string.
        /// </summary>
        /// <param name="array">  The array.</param>
        /// <param name="offset"> The offset.</param>
        /// <param name="length"> The length.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.</exception>
        public static StringBuilder CreateWithBase64String([AllowNull] byte[] array, int offset, int length, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(offset, array.Length, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (offset + length > array.Length)
            {
                throw new ArgumentException($"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.");
            }

            return Create(Convert.ToBase64String(array, offset, length, options));
        }

        /// <summary>
        /// Creates the with directories.
        /// </summary>
        /// <param name="source">       The source.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="options">      The options.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithDirectories(DirectoryInfo source, string searchPattern, EnumerationOptions options)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateDirectories(searchPattern, options))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with directories.
        /// </summary>
        /// <param name="source">       The source.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="option">       The option.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithDirectories(DirectoryInfo source, string searchPattern, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateDirectories(searchPattern, option))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with directories.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithDirectories(DirectoryInfo source)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateDirectories())
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with files.
        /// </summary>
        /// <param name="source">       The source.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="options">      The options.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithFiles(DirectoryInfo source, string searchPattern, EnumerationOptions options)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateFiles(searchPattern, options))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with files.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithFiles(DirectoryInfo source)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateFiles())
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with files.
        /// </summary>
        /// <param name="source">       The source.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="option">       The option.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithFiles(DirectoryInfo source, string searchPattern, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            StringBuilder builder = Create();

            foreach (var item in source.EnumerateFiles(searchPattern, option))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        /// <summary>
        /// Creates the with hexadecimal lower string.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithHexLowerString([AllowNull] byte[] array)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            return CreateWithHexLowerString(array, 0, array.Length);
        }

        /// <summary>
        /// Creates the with hexadecimal lower string.
        /// </summary>
        /// <param name="array"> The array.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.</exception>
        public static StringBuilder CreateWithHexLowerString([AllowNull] byte[] array, int offset, int length)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(offset, array.Length, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (offset + length > array.Length)
            {
                throw new ArgumentException($"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.");
            }

            return Create(Convert.ToHexString(array, offset, length));
        }

        /// <summary>
        /// Creates the with hexadecimal lower string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithHexLowerString(int value)
        {
            return Create(null, "0x{0:x8}", value);
        }

        /// <summary>
        /// Creates the with hexadecimal string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithHexString(int value)
        {
            return Create(null, "0x{0:X8}", value);
        }

        /// <summary>
        /// Creates the with hexadecimal string.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static StringBuilder CreateWithHexString(byte[] array)
        {
            return CreateWithHexString(array, 0, array.Length);
        }

        /// <summary>
        /// Creates the with hexadecimal string.
        /// </summary>
        /// <param name="array"> The array.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.</exception>
        public static StringBuilder CreateWithHexString([AllowNull] byte[] array, int offset, int length)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(offset, array.Length, nameof(offset));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (offset + length > array.Length)
            {
                throw new ArgumentException($"Offset '{offset}' plus Length '{length}' is greater than Length '{array.Length}'.");
            }

            return Create(Convert.ToHexString(array, offset, length));
        }

        /// <summary>
        /// Defaults if empty.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder DefaultIfEmpty([AllowNull] this StringBuilder builder)
        {
            return builder.DefaultIfEmpty(char.MinValue);
        }

        /// <summary>
        /// Defaults if empty.
        /// </summary>
        /// <param name="builder">     The builder.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static StringBuilder DefaultIfEmpty([AllowNull] this StringBuilder builder, char defaultValue)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return IsEmpty(builder) ? Create(defaultValue, 1) : builder;
        }

        /// <summary>
        /// Elements at.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <returns></returns>
        public static char ElementAt([AllowNull] this StringBuilder builder, int index)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));

            return builder[index];
        }

        /// <summary>
        /// Elements at.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// nameof(index), index, $"Parameter {nameof(index)} {index} is out of range.
        /// </exception>
        public static char ElementAt([AllowNull] this StringBuilder builder, Index index)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(index.Value, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index.Value, builder.Count(), nameof(index));

            if (ExceptionExtension.IsOutOfRange(index, 0..^builder.Count()))
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Parameter {nameof(index)} {index} is out of range.");
            }

            return builder[index];
        }

        /// <summary>
        /// Elements at or default.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <returns></returns>
        public static char ElementAtOrDefault([AllowNull] this StringBuilder builder, int index)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));

            if (ExceptionExtension.IsOutOfRange(index, 0..^builder.Count()))
            {
                return char.MinValue;
            }

            return builder![index];
        }

        /// <summary>
        /// Elements at or default.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <returns></returns>
        public static char ElementAtOrDefault([AllowNull] this StringBuilder builder, Index index)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(index.Value, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index.Value, builder.Count(), nameof(index));

            if (!ExceptionExtension.IsInRange(index, 0..^builder.Count()))
            {
                return char.MinValue;
            }

            return builder![index];
        }

        /// <summary>
        /// Empties this instance.
        /// </summary>
        /// <returns></returns>
        public static StringBuilder Empty()
        {
            return new(capacity: 0, maxCapacity: 1);
        }

        /// <summary>
        /// Equals the specified left.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool Equals(StringBuilder? left, StringBuilder? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null ^ right is null)
            {
                return false;
            }
            else if (left.Count() != right.Count())
            {
                return false;
            }
            else
            {
                return left!.Equals(right);
            }
        }

        /// <summary>
        /// Firsts the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"Parameter {nameof(builder)} is empty.</exception>
        public static char First([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() < 1)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            return builder.ElementAt(0);
        }

        /// <summary>
        /// Firsts the specified builder.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// $"Parameter {nameof(builder)} is empty. or $"No value satisfying {nameof(predicate)} was found.
        /// </exception>
        public static char First([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            if (builder.Count() < 1)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            foreach (var item in builder)
            {
                if (predicate.Invoke(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static char FirstOrDefault([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Count() > 0 ? builder.ElementAtOrDefault(0) : char.MinValue;
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static char FirstOrDefault([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            return builder.FirstOrDefault(predicate, char.MinValue);
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <param name="builder">     The builder.</param>
        /// <param name="predicate">   The predicate.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static char FirstOrDefault([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate, char defaultValue)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            foreach (var item in builder)
            {
                if (predicate.Invoke(item))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IEnumerator<char> GetEnumerator(this StringBuilder builder)
        {
            StringBuilderInfo info = new(builder);

            return info.GetEnumerator();
        }

        /// <summary>
        /// Gets the lower bound.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static int GetLowerBound(this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return 0;
        }

        /// <summary>
        /// Gets the upper bound.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static int GetUpperBound([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return IsEmpty(builder) ? 0 : builder.Count() - 1;
        }

        /// <summary>
        /// Indexes the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, char>> Index([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            int index = 0;
            List<Tuple<int, char>> accumulator = new(builder.Count());

            while (ExceptionExtension.IsInRange(index, 0..^builder.Count()))
            {
                accumulator.Add(Tuple.Create<int, char>(index, builder.ElementAtOrDefault(index)));

                index++;
            }

            return accumulator;
        }

        /// <summary>
        /// Indexes the index of the by.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<System.Index, char>> IndexByIndex([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            System.Index index = 0;
            List<Tuple<Index, char>> accumulator = new(builder.Count());

            while (ExceptionExtension.IsInRange(index, 0..^builder.Count()))
            {
                accumulator.Add(Tuple.Create<Index, char>(index, builder.ElementAtOrDefault(index)));

                index = index.Value + 1;
            }

            return accumulator;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, char character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) ? builder!.IndexByIndexOf(character, StringComparison.Ordinal) : -1;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, char character, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) ? builder.IndexByIndexOf(character: new ReadOnlySpan<char>(in character), comparison) : -1;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) ? builder.IndexByIndexOf(character, StringComparison.Ordinal) : -1;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            Index index = -1;

            foreach (var chunk in builder.GetChunks())
            {
                index = chunk.Span.IndexOf(character, comparison);

                if (index.Value > -1)
                {
                    return index;
                }
            }

            return index;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, string value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexByIndexOf(value: new ReadOnlySpan<char>(value.ToCharArray()), startIndex: 0);
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, Index startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex.Value, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex.Value, builder.Count(), nameof(startIndex));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            Index first = -1;

            foreach (var item in value[startIndex..])
            {
                Index index = builder.IndexByIndexOf(item);

                if (index.Value == -1)
                {
                    return index;
                }
                else if (first.Value == -1)
                {
                    first = index.Value + startIndex.Value;
                }
            }

            return first;
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// $"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.
        /// </exception>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, Index startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex.Value, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex.Value, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (startIndex.Value + length > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexByIndexOf(value, startIndex, length, StringComparison.Ordinal);
        }

        /// <summary>
        /// Indexes the by index of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// $"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.
        /// </exception>
        public static Index IndexByIndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, Index startIndex, int length, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex.Value, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex.Value, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (startIndex.Value + length > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            Index first = -1;

            foreach (var item in value.Slice(startIndex.Value, length))
            {
                Index index = builder.IndexByIndexOf(item, comparison);

                if (index.Value == -1)
                {
                    return index;
                }
                else if (first.Value == -1)
                {
                    first = index.Value + startIndex.Value;
                }
            }

            return first;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, char character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) ? builder!.IndexOf(character, StringComparison.Ordinal) : -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, char character, StringComparison comparison)
        {
            return !IsNullOrEmpty(builder) ? builder.IndexOf(character: new ReadOnlySpan<char>(in character), comparison) : -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return !IsNullOrEmpty(builder) ? builder.IndexOf(character, StringComparison.Ordinal) : -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="character"> The character.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> character, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            int index = -1;

            foreach (var chunk in builder.GetChunks())
            {
                index = chunk.Span.IndexOf(character, comparison);

                if (index > -1)
                {
                    return index;
                }
            }

            return index;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, string value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexOf(value: new ReadOnlySpan<char>(value.ToCharArray()), 0);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            int first = -1;

            foreach (var item in value[startIndex..])
            {
                int index = builder.IndexOf(item);

                if (index == -1)
                {
                    return index;
                }
                else if (first == -1)
                {
                    first = index + startIndex;
                }
            }

            return first;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// $"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.
        /// </exception>
        public static int IndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (startIndex + length > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexOf(value, startIndex, length, StringComparison.Ordinal);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">    The length.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// $"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.
        /// </exception>
        public static int IndexOf([AllowNull] this StringBuilder builder, ReadOnlySpan<char> value, int startIndex, int length, StringComparison comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (startIndex + length > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            if (IsNullOrEmpty(builder))
            {
                return -1;
            }

            int first = -1;

            foreach (var item in value.Slice(startIndex, length))
            {
                int index = builder.IndexOf(item, comparison);

                if (index == -1)
                {
                    return index;
                }
                else if (first == -1)
                {
                    first = index + startIndex;
                }
            }

            return first;
        }

        /// <summary>
        /// Inserts the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// $"Enlarging Source {nameof(builder)} by Value {nameof(value)} would exceed MaxCapacity '{builder.MaxCapacity}'.
        /// </exception>
        public static StringBuilder Insert(this StringBuilder builder, int index, StringBuilder? value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));

            if (builder.Count() + value.Count() > builder.MaxCapacity)
            {
                throw new ArgumentException($"Enlarging Source {nameof(builder)} by Value {nameof(value)} would exceed MaxCapacity '{builder.MaxCapacity}'.");
            }

            foreach (var item in value.Reverse())
            {
                builder.Insert(index, item);
            }

            return builder;
        }

        /// <summary>
        /// Inserts the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="index">     The index.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static StringBuilder Insert(this StringBuilder builder, int index, StringBuilder? value, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));
            ArgumentOutOfRangeException.ThrowIfLessThan(startIndex, 0, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, value.Count(), nameof(startIndex));

            return builder.Insert(index, value.Slice(startIndex));
        }

        /// <summary>
        /// Inserts the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="index">     The index.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Insert(this StringBuilder builder, int index, StringBuilder? value, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));
            ArgumentOutOfRangeException.ThrowIfLessThan(startIndex, 0, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, value.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return builder.Insert(index, value.Slice(startIndex, count));
        }

        /// <summary>
        /// Determines whether the specified builder is empty.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns><see langref="true"/> if the specified builder is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty(StringBuilder builder)
        {
            return builder.Count() < 1;
        }

        /// <summary>
        /// Determines whether the specified builder is match.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns><see langref="true"/> if the specified builder is match; otherwise, <see langref="false"/>.</returns>
        public static bool IsMatch([AllowNull] this StringBuilder builder, Regex pattern)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Count() > 0 && pattern.IsMatch(builder.ToString());
        }

        /// <summary>
        /// Determines whether the specified builder is match.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="pattern">   The pattern.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns><see langref="true"/> if the specified builder is match; otherwise, <see langref="false"/>.</returns>
        public static bool IsMatch([AllowNull] this StringBuilder builder, Regex pattern, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return builder.Count() > 0 && builder.Slice(startIndex).IsMatch(pattern);
        }

        /// <summary>
        /// Determines whether the specified builder is match.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="pattern">   The pattern.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns><see langref="true"/> if the specified builder is match; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static bool IsMatch([AllowNull] this StringBuilder builder, Regex pattern, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return builder.Count() > 0 && builder.Slice(startIndex, count).IsMatch(pattern);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified builder].
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns><see langref="true"/> if [is <see langref="null"/> or empty] [the specified builder]; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty(StringBuilder? builder)
        {
            return builder?.Count() < 1;
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified array].
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns><see langref="true"/> if [is <see langref="null"/> or empty] [the specified array]; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty<TElement>(TElement[]? array)
        {
            return IsNullOrEmpty(collection: array);
        }

        /// <summary>
        /// Determines whether /[is <see langref="null"/> or empty] [the specified <see cref="ICollection{T}"/>].
        /// </summary>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="collection">The <see cref="ICollection{T}"/>.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or empty] [the specified <see cref="ICollection{T}"/>]; otherwise,
        /// <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty<TValue>(ICollection<TValue>? collection)
        {
            return collection?.Count < 1;
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified dictionary].
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or empty] [the specified dictionary]; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary)
        {
            return IsNullOrEmpty(collection: dictionary);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified the set].
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="theSet">The set.</param>
        /// <returns><see langref="true"/> if [is <see langref="null"/> or empty] [the specified the set]; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty<TElement>(ISet<TElement> theSet)
        {
            return IsNullOrEmpty(collection: theSet);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or white space] [the specified builder].
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or white space] [the specified builder]; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(StringBuilder? builder)
        {
            return IsNullOrEmpty(builder) || (builder.All(c => char.IsWhiteSpace(c)));
        }

        /// <summary>
        /// Lasts the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"Parameter {nameof(builder)} is empty.</exception>
        public static char Last([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            return builder.ElementAt(^0);
        }

        /// <summary>
        /// Lasts the specified builder.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// $"Parameter {nameof(builder)} is empty. or $"No value satisfying {nameof(predicate)} was found.
        /// </exception>
        public static char Last([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            if (builder.Count() <= 0)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            foreach (var item in builder.Reverse())
            {
                if (predicate.Invoke(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        /// <summary>
        /// Lasts the or default.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static char LastOrDefault([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Count() > 0 ? builder![^1] : char.MinValue;
        }

        /// <summary>
        /// Lasts the or default.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static char LastOrDefault([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            return builder.LastOrDefault(predicate, char.MinValue);
        }

        /// <summary>
        /// Lasts the or default.
        /// </summary>
        /// <param name="builder">     The builder.</param>
        /// <param name="predicate">   The predicate.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static char LastOrDefault([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate, char defaultValue)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            if (builder.Count() <= 0)
            {
                return defaultValue;
            }

            foreach (var item in builder.Reverse())
            {
                if (predicate.Invoke(item))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Matcheses the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"Parameter {nameof(builder)} is empty.</exception>
        public static MatchCollection Matches([AllowNull] this StringBuilder builder, Regex pattern)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            return pattern.Matches(builder!.ToString());
        }

        /// <summary>
        /// Matcheses the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="pattern">   The pattern.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"Parameter {nameof(builder)} is empty.</exception>
        public static MatchCollection Matches([AllowNull] this StringBuilder builder, Regex pattern, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            if (builder.Count() <= 0)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            return builder.Slice(startIndex).Matches(pattern);
        }

        /// <summary>
        /// Matcheses the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="pattern">   The pattern.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        /// <exception cref="InvalidOperationException">$"Parameter {nameof(builder)} is empty.</exception>
        public static MatchCollection Matches([AllowNull] this StringBuilder builder, Regex pattern, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            if (builder.Count() <= 0)
            {
                throw new InvalidOperationException($"Parameter {nameof(builder)} is empty.");
            }

            return builder.Slice(startIndex, count).Matches(pattern);
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder Order([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Order(comparer: Comparer<char>.Default);
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder">        The builder.</param>
        /// <param name="caseInsensitive">if set to <see langref="true"/> [case insensitive].</param>
        /// <returns></returns>
        public static StringBuilder Order([AllowNull] this StringBuilder builder, bool caseInsensitive)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Order(null, caseInsensitive);
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder">        The builder.</param>
        /// <param name="culture">        The culture.</param>
        /// <param name="caseInsensitive">if set to <see langref="true"/> [case insensitive].</param>
        /// <returns></returns>
        public static StringBuilder Order([AllowNull] this StringBuilder builder, CultureInfo? culture, bool caseInsensitive)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (!caseInsensitive)
            {
                return builder.Order(comparer: Comparer<char>.Default);
            }
            else
            {
                // converting to ToUpper() is stable; but, converting to ToLower() is not stable with Unicode
                if (caseInsensitive)
                {
                    return builder.Order((l, r) => char.ToUpper(l, culture ?? CultureInfo.InvariantCulture).CompareTo(char.ToUpper(r, culture ?? CultureInfo.InvariantCulture)));
                }
                else
                {
                    return builder.Order((l, r) => l.CompareTo(r));
                }
            }
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static StringBuilder Order([AllowNull] this StringBuilder builder, Comparison<char> comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Count() > 0 ? builder.Order((x, y) => comparison.Invoke(x, y)) : Empty();
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder"> The builder.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static StringBuilder Order([AllowNull] this StringBuilder builder, IComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            List<char> list = [.. builder.ToCharArray()];
            list.Sort(comparer);
            return Create(list);
        }

        /// <summary>
        /// Orders the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <param name="comparer">  The comparer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Order([AllowNull] this StringBuilder builder, int startIndex, int count, IComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            List<char> list = builder.ToCharArray().ToList();
            list.Sort(startIndex, count, comparer);
            return Create(list);
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <param name="comparer">  The comparer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder, int startIndex, int count, IComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            List<char> list = builder.ToCharArray(startIndex, count).ToList();
            return Create(list.OrderDescending(comparer));
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.OrderDescending(comparer: null);
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder">        The builder.</param>
        /// <param name="caseInsensitive">if set to <see langref="true"/> [case insensitive].</param>
        /// <returns></returns>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder, bool caseInsensitive)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            return builder.OrderDescending(null, caseInsensitive);
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder">        The builder.</param>
        /// <param name="culture">        The culture.</param>
        /// <param name="caseInsensitive">if set to <see langref="true"/> [case insensitive].</param>
        /// <returns></returns>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder, CultureInfo? culture, bool caseInsensitive)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (!caseInsensitive)
            {
                return builder.OrderDescending(comparer: Comparer<char>.Default);
            }
            else
            {
                // converting to ToUpper() is stable; but, converting to ToLower() is not stable with Unicode
                if (caseInsensitive)
                {
                    return builder.OrderDescending((l, r) => char.ToUpper(l, culture ?? CultureInfo.InvariantCulture).CompareTo(char.ToUpper(r, culture ?? CultureInfo.InvariantCulture)));
                }
                else
                {
                    return builder.OrderDescending((l, r) => l.CompareTo(r));
                }
            }
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder, Comparison<char> comparison)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            return builder.OrderDescending((x, y) => comparison.Invoke(x, y));
        }

        /// <summary>
        /// Orders the descending.
        /// </summary>
        /// <param name="builder"> The builder.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static StringBuilder OrderDescending([AllowNull] this StringBuilder builder, IComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() <= 0)
            {
                return Empty();
            }

            List<char> list = [.. builder!.ToCharArray() ?? [char.MinValue]];
            return Create(list.OrderDescending(comparer));
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, FileSystemInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (value is FileInfo file)
            {
                return builder.Insert(0, file.FullName);
            }
            else if (value is DirectoryInfo directory)
            {
                return builder.Insert(0, directory.FullName);
            }
            else
            {
                return builder.Insert(0, value);
            }
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, DirectoryInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value.FullName);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, FileInfo value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value.FullName);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, char value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, ITaskItem value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            return builder.Insert(0, value.ItemSpec);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, char value, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            return builder.Prepend(new string(value, count));
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, char[]? value, int startIndex, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return builder.Prepend(value?.Skip(startIndex + 1).Take(count).ToArray());
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, string? value, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return builder.Prepend(value?.Skip(startIndex + 1).Take(count).ToString());
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  if set to <see langref="true"/> [value].</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, bool value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, byte value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, char[]? value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, decimal value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, double value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, float value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, int value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, long value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, object? value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, sbyte value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, short value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, string? value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, StringBuilder? value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return value is not null ? value.Append(builder) : builder;
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, StringBuilder? value, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return value.Slice(startIndex).Append(builder);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="value">     The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Prepend([AllowNull] this StringBuilder builder, StringBuilder? value, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return value.Slice(startIndex, count).Append(builder);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend(this StringBuilder builder, uint value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend(this StringBuilder builder, ulong value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend(this StringBuilder builder, ushort value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static StringBuilder Prepend(this StringBuilder builder, ReadOnlySpan<char> value)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Insert(0, value);
        }

        /// <summary>
        /// Prepends the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="value">  The value.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder Prepend(this StringBuilder builder, string? value, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            return builder.Insert(0, value, count);
        }

        /// <summary>
        /// Prepends the format.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder PrependFormat(this StringBuilder builder, IFormatProvider? provider, string format, params object?[] arguments)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Prepend(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments));
        }

        /// <summary>
        /// Prepends the format.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder PrependFormat(this StringBuilder builder, string format, params object?[] arguments)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.PrependFormat(null, format, arguments);
        }

        /// <summary>
        /// Prepends the join.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="list">     The list.</param>
        /// <returns></returns>
        public static StringBuilder PrependJoin([AllowNull] this StringBuilder builder, string? separator, IEnumerable<ITaskItem> list)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            StringBuilder value = Create();
            List<string?> convertedList = [];

            list.ToList().ForEach(i => convertedList.Add(i.ItemSpec));
            return value.PrependJoin(separator, convertedList);
        }

        /// <summary>
        /// Prepends the join.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="list">     The list.</param>
        /// <returns></returns>
        public static StringBuilder PrependJoin([AllowNull] this StringBuilder builder, string? separator, IEnumerable<string?> list)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            StringBuilder value = Create();

            value.AppendJoin(separator, list);

            return value.Append(builder);
        }

        /// <summary>
        /// Prepends the line.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder PrependLine([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Prepend(Environment.NewLine);
        }

        /// <summary>
        /// Prepends the line.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="provider"> The provider.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder PrependLine([AllowNull] this StringBuilder builder, IFormatProvider? provider, string format, params object?[] arguments)
        {
            return builder.PrependLine().PrependFormat(provider, format, arguments);
        }

        /// <summary>
        /// Prepends the line.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="format">   The format.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static StringBuilder PrependLine([AllowNull] this StringBuilder builder, string format, params object?[] arguments)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.PrependLine(null, format, arguments);
        }

        /// <summary>
        /// Repeats the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="element">The element.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder Repeat([AllowNull] this StringBuilder builder, char element, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            return builder.Append(element, count);
        }

        /// <summary>
        /// Resizes the specified builder.
        /// </summary>
        /// <param name="builder">        The builder.</param>
        /// <param name="capacity">       The capacity.</param>
        /// <param name="maximumCapacity">The maximum capacity.</param>
        /// <returns></returns>
        public static StringBuilder Resize([AllowNull] this StringBuilder builder, int capacity, int maximumCapacity)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return Create(Math.Max(builder.Count(), capacity), Math.Max(builder.Count(), maximumCapacity)).Append(builder);
        }

        /// <summary>
        /// Reverses the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static StringBuilder Reverse([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            char[] temp = builder.ToCharArray();
            Array.Reverse(temp);
            return Create(temp);
        }

        /// <summary>
        /// Reverses the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="index">  The index.</param>
        /// <param name="length"> The length.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Index '{index}' plus Length '{length}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Reverse([AllowNull] this StringBuilder builder, int index, int length)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(index, builder.Count(), nameof(index));
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            if (index + length > builder.Count())
            {
                throw new ArgumentException($"Index '{index}' plus Length '{length}' is greater than Length '{builder.Count()}'.");
            }

            char[] temp = builder.ToCharArray();
            Array.Reverse(temp, index, length);
            return Create(temp);
        }

        /// <summary>
        /// Selects the specified builder.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type"/> of the result.</typeparam>
        /// <param name="builder"> The builder.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Select<TResult>([AllowNull] this StringBuilder builder, [AllowNull] Func<char, TResult> selector)
            where TResult : IConvertible
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(selector, nameof(selector));

            List<TResult> accumulator = [];

            if (!IsNullOrEmpty(builder))
            {
                foreach (var item in builder)
                {
                    accumulator.Add(selector.Invoke(item));
                }
            }

            return accumulator.Cast<TResult>();
        }

        /// <summary>
        /// Selects the specified builder.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type"/> of the result.</typeparam>
        /// <param name="builder"> The builder.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Select<TResult>([AllowNull] this StringBuilder builder, [AllowNull] Func<char, int, TResult> selector)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(selector, nameof(selector));

            List<TResult> accumulator = [];

            if (!IsNullOrEmpty(builder))
            {
                int index = 0;

                foreach (var item in builder)
                {
                    accumulator.Add(selector.Invoke(item, index++));
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Sequences the equal.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right)
        {
            return left.SequenceEqual(right, comparer: null);
        }

        /// <summary>
        /// Sequences the equal.
        /// </summary>
        /// <param name="left">  The left.</param>
        /// <param name="right"> The right.</param>
        /// <param name="equals">The equals.</param>
        /// <returns></returns>
        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right, Func<char, char, bool> equals)
        {
            return Equals(left, right) || Enumerable.Range(0, left.GetUpperBound()).All(i => equals.Invoke(left![i], right![i]));
        }

        /// <summary>
        /// Sequences the equal.
        /// </summary>
        /// <param name="left">    The left.</param>
        /// <param name="right">   The right.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right, IEqualityComparer<char>? comparer)
        {
            comparer ??= EqualityComparer<char>.Default;

            return Equals(left, right) || Enumerable.Range(0, left.GetUpperBound()).All(i => comparer.Equals(left![i], right![i]));
        }

        /// <summary>
        /// Singles the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"StringBuilder {nameof(builder)} is not a singleton.</exception>
        public static char Single([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.First();
            }
        }

        /// <summary>
        /// Singles the specified builder.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"StringBuilder {nameof(builder)} is not a singleton.</exception>
        public static char Single([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.First(predicate);
            }
        }

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"StringBuilder {nameof(builder)} is not a singleton.</exception>
        public static char SingleOrDefault([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.ElementAtOrDefault(0);
            }
        }

        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">$"StringBuilder {nameof(builder)} is not a singleton.</exception>
        public static char SingleOrDefault([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                var item = builder.ElementAtOrDefault(0);
                return predicate.Invoke(item) ? item : char.MinValue;
            }
        }

        /// <summary>
        /// Skips the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder? Skip([AllowNull] this StringBuilder builder, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            return Create(builder, count - 1);
        }

        /// <summary>
        /// Skips the last.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder? SkipLast([AllowNull] this StringBuilder builder, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            return builder.Take(builder.Count() - count);
        }

        /// <summary>
        /// Skips the while.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder? SkipWhile([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            int index = 0;

            while (predicate.Invoke(builder.ElementAtOrDefault(index++)))
            {
            }

            return builder?.Skip(index);
        }

        /// <summary>
        /// Skips the while.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder? SkipWhile([AllowNull] this StringBuilder builder, [AllowNull] Func<char, int, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            int index = 0;

            while (predicate.Invoke(builder.ElementAtOrDefault(index), index++))
            {
            }

            return builder?.Skip(index);
        }

        /// <summary>
        /// Slices the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static StringBuilder Slice([AllowNull] this StringBuilder builder, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return Create(builder.ToString(startIndex));
        }

        /// <summary>
        /// Slices the specified builder.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static StringBuilder Slice([AllowNull] this StringBuilder builder, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return Create(builder?.ToString(startIndex, count));
        }

        /// <summary>
        /// Takes the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder? Take([AllowNull] this StringBuilder builder, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            return Create(builder.ToString(0, count));
        }

        /// <summary>
        /// Takes the last.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">  The count.</param>
        /// <returns></returns>
        public static StringBuilder? TakeLast([AllowNull] this StringBuilder builder, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            return builder.Skip(builder.Count() - count);
        }

        /// <summary>
        /// Takes the while.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder TakeWhile([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            StringBuilder accumulator = Create();
            int index = 0;
            char element = builder.ElementAtOrDefault(index);

            while (predicate.Invoke(element))
            {
                accumulator.Append(element);
                element = builder.ElementAtOrDefault(++index);
            }

            return accumulator;
        }

        /// <summary>
        /// Takes the while.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder? TakeWhile([AllowNull] this StringBuilder builder, [AllowNull] Func<char, int, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            StringBuilder accumulator = Create();
            int index = 0;
            char element = builder.ElementAtOrDefault(index);

            while (predicate.Invoke(element, index))
            {
                accumulator.Append(element);
                element = builder.ElementAtOrDefault(++index);
            }

            return accumulator;
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static TElement[] ToArray<TElement>([AllowNull] this StringBuilder builder) where TElement : IConvertible
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return [.. builder.ToList().Cast<TElement>()];
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static TElement[] ToArray<TElement>([AllowNull] this StringBuilder builder, int startIndex) where TElement : IConvertible
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return [.. builder.ToList(startIndex).Cast<TElement>()];
        }

        /// <summary>
        /// Converts to array.
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static TElement[] ToArray<TElement>([AllowNull] this StringBuilder builder, int startIndex, int count) where TElement : IConvertible
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return [.. builder.ToList(startIndex, count).Cast<TElement>()];
        }

        /// <summary>
        /// Converts to chararray.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static char[] ToCharArray([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.ToArray<char>();
        }

        /// <summary>
        /// Converts to chararray.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static char[] ToCharArray(this StringBuilder builder, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return builder.ToArray<char>(startIndex);
        }

        /// <summary>
        /// Converts to chararray.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static char[] ToCharArray(this StringBuilder builder, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return builder.ToArray<char>(startIndex, count);
        }

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static Dictionary<int, char> ToDictionary([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Index().ToDictionary(null);
        }

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="builder"> The builder.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static Dictionary<int, char> ToDictionary([AllowNull] this StringBuilder builder, IEqualityComparer<int>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.Index().ToDictionary(comparer ?? EqualityComparer<int>.Default);
        }

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="tuples">The tuples.</param>
        /// <returns></returns>
        public static Dictionary<int, char> ToDictionary([AllowNull] this IEnumerable<Tuple<int, char>> tuples)
        {
            ArgumentNullException.ThrowIfNull(tuples, nameof(tuples));

            return tuples.ToDictionary(null);
        }

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="tuples">  The tuples.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static Dictionary<int, char> ToDictionary([AllowNull] this IEnumerable<Tuple<int, char>> tuples, IEqualityComparer<int>? comparer)
        {
            ArgumentNullException.ThrowIfNull(tuples, nameof(tuples));

            Dictionary<int, char> accumulator = new(comparer ?? EqualityComparer<int>.Default);

            foreach (var item in tuples)
            {
                if (item is not null)
                {
                    accumulator.Add(item.Item1, item.Item2);
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Converts to hashset.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static HashSet<char> ToHashSet([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return builder.ToHashSet(null);
        }

        /// <summary>
        /// Converts to hashset.
        /// </summary>
        /// <param name="builder"> The builder.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static HashSet<char> ToHashSet([AllowNull] this StringBuilder builder, IEqualityComparer<char>? comparer)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return new(builder.ToCharArray(), comparer ?? EqualityComparer<char>.Default);
        }

        /// <summary>
        /// Converts to list.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static List<char> ToList([AllowNull] this StringBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            return [.. builder.ToCharArray()];
        }

        /// <summary>
        /// Converts to list.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static List<char> ToList([AllowNull] this StringBuilder builder, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return [.. builder.ToCharArray(startIndex)];
        }

        /// <summary>
        /// Converts to list.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">     The count.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">$"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.</exception>
        public static List<char> ToList([AllowNull] this StringBuilder builder, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

            if (startIndex + count > builder.Count())
            {
                throw new ArgumentException($"Start Index '{startIndex}' plus Count '{count}' is greater than Length '{builder.Count()}'.");
            }

            return [.. builder.ToCharArray(startIndex, count)];
        }

        /// <summary>
        /// Converts to lookup.
        /// </summary>
        /// <param name="source">         The source.</param>
        /// <param name="keySelector">    The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <returns></returns>
        public static ILookup<int, char> ToLookup(
            [AllowNull] this StringBuilder source,
            Func<StringBuilder, int>? keySelector,
            Func<StringBuilder, char>? elementSelector)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return source.ToLookup(keySelector, elementSelector, null);
        }

        /// <summary>
        /// Converts to lookup.
        /// </summary>
        /// <param name="source">         The source.</param>
        /// <param name="keySelector">    The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="comparer">       The comparer.</param>
        /// <returns></returns>
        public static ILookup<int, char> ToLookup(
            [AllowNull] this StringBuilder source,
            [AllowNull] Func<StringBuilder, int> keySelector,
            [AllowNull] Func<StringBuilder, char> elementSelector,
            IEqualityComparer<int>? comparer)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
            ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

            return source.ToLookup(keySelector, elementSelector, comparer ?? EqualityComparer<int>.Default);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="builder">   The builder.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static string ToString([AllowNull] this StringBuilder builder, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, builder.Count(), nameof(startIndex));

            return new string(builder.ToCharArray(startIndex));
        }

        /// <summary>
        /// Wheres the specified builder.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder Where([AllowNull] this StringBuilder builder, [AllowNull] Func<char, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            StringBuilder accumulator = Create();

            if (builder.Count() < 1)
            {
                return Empty();
            }

            foreach (var item in builder)
            {
                if (predicate.Invoke(item))
                {
                    accumulator.Append(item);
                }
            }

            return accumulator;
        }

        /// <summary>
        /// Wheres the specified builder.
        /// </summary>
        /// <param name="builder">  The builder.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static StringBuilder Where([AllowNull] this StringBuilder builder, [AllowNull] Func<char, int, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            StringBuilder accumulator = Create();

            if (builder.Count() < 1)
            {
                return Empty();
            }

            int index = 0;

            foreach (var item in builder)
            {
                if (predicate.Invoke(item, index++))
                {
                    accumulator.Append(item);
                }
            }

            return accumulator;
        }

        #endregion Public Methods
    }
}
