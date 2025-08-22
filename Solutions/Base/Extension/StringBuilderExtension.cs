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
        #region Internal Methods

        internal static StringBuilder? SortFunctional(this StringBuilder? builder, int startIndex, int count, IComparer<char>? comparer)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return builder;
            }
            else
            {
                List<char>? list = builder!.ToCharArray()?.ToList();
                list!.Sort(startIndex, count, comparer);
                return StringBuilderExtension.Create(list);
            }
        }

        #endregion Internal Methods

        #region Public Fields

        public const int OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY = 16;

        #endregion Public Fields

        #region Public Methods

        public static bool All(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }
            else
            {
                for (var i = 0; i < builder?.Count(); i++)
                {
                    if (!predicate.Invoke(builder[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool Any(this StringBuilder? builder)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder);
        }

        public static bool Any(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }
            else
            {
                for (var i = 0; i < builder?.Count(); i++)
                {
                    if (predicate.Invoke(builder[i]))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, ITaskItem value)
        {
            return builder.Append(value.ItemSpec);
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, IEnumerable<ITaskItem> list, string separator)
        {
            bool first = true;

            foreach (var item in list)
            {
                if (first)
                {
                    builder.Append(item);
                    first = false;
                }
                else
                {
                    builder.Append(separator).Append(item);
                }
            }

            return builder;
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, IEnumerable<string?> list, string separator)
        {
            bool first = true;

            foreach (var item in list)
            {
                if (first)
                {
                    builder.Append(item);
                    first = false;
                }
                else
                {
                    builder.Append(separator).Append(item);
                }
            }

            return builder;
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, DirectoryInfo value)
        {
            return builder.Append(value.FullName);
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, FileInfo value)
        {
            return builder.Append(value.FullName);
        }

        public static StringBuilder Append([DisallowNull] this StringBuilder builder, FileSystemInfo value)
        {
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

        public static IEnumerable<char>? AsEnumerable(this StringBuilder? builder)
        {
            return builder.ToList();
        }

        public static IEnumerable<TResult> Cast<TResult>([DisallowNull] this StringBuilder builder) where TResult : IConvertible
        {
            return builder.AsEnumerable()?.Cast<TResult>() ?? Enumerable.Empty<TResult>();
        }

        public static StringBuilder Concat([DisallowNull] this StringBuilder first, StringBuilder? second)
        {
            return first.Append(second);
        }

        public static bool Contains(this StringBuilder? builder, char character)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(character, StringComparison.Ordinal);
        }

        public static bool Contains(this StringBuilder? builder, char character, StringComparison comparison)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(character: new ReadOnlySpan<char>(in character), comparison);
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> character)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(character, StringComparison.Ordinal);
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> character, StringComparison comparison)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }

            foreach (var chunk in builder!.GetChunks())
            {
                if (chunk.Span.IndexOf(character, comparison) > -1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains(this StringBuilder? builder, string value)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(value: new ReadOnlySpan<char>(value.ToCharArray()), 0);
        }

        public static bool Contains(this StringBuilder? builder, string value, StringComparison comparison)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(value: new ReadOnlySpan<char>(value.ToCharArray()), 0, comparison);
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && builder!.Contains(value, startIndex, StringComparison.Ordinal);
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex, StringComparison comparison)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }

            foreach (var item in value[startIndex..])
            {
                if (!builder.Contains(item, comparison))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex, int length)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }

            foreach (var item in value.Slice(startIndex, length))
            {
                if (!builder.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex, int length, StringComparison comparison)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return false;
            }

            foreach (var item in value.Slice(startIndex, length))
            {
                if (!builder.Contains(item, comparison))
                {
                    return false;
                }
            }

            return true;
        }

        public static void CopyTo([DisallowNull] this StringBuilder builder, int startIndex, [DisallowNull] StringBuilder destination, int destinationIndex, int count)
        {
            destination.Clear();
            destination.Capacity = count;
            destination.Insert(destinationIndex, builder.ToCharArray(startIndex, count));
        }

        public static int Count(this StringBuilder? builder)
        {
            return builder?.Length ?? 0;
        }

        public static int Count(this StringBuilder? builder, Func<char, bool> countable)
        {
            int counter = 0;

            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return counter;
            }
            else
            {
                for (int i = 0; i < builder?.Count(); i++)
                {
                    if (countable.Invoke(builder[i]))
                    {
                        counter++;
                    }
                }

                return counter;
            }
        }

        public static StringBuilder Create(int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return new StringBuilder(capacity);
        }

        public static StringBuilder Create(IFormatProvider? provider, string format, object? first)
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first));
        }

        public static StringBuilder Create(IFormatProvider? provider, string format, object? first, object? second)
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first, second));
        }

        public static StringBuilder Create(IFormatProvider? provider, string format, params object?[] arguments)
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments));
        }

        public static StringBuilder Create(IFormatProvider? provider, string format, object? first, object? second, object? third)
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, first, second, third));
        }

        public static StringBuilder Create<T>(IFormatProvider? provider, string format, Tuple<T> arguments)
            where T : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1));
        }

        public static StringBuilder Create<T1, T2>(IFormatProvider? provider, string format, Tuple<T1, T2> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2));
        }

        public static StringBuilder Create<T1, T2, T3>(IFormatProvider? provider, string format, Tuple<T1, T2, T3> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3));
        }

        public static StringBuilder Create<T1, T2, T3, T4>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4));
        }

        public static StringBuilder Create<T1, T2, T3, T4, T5>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5));
        }

        public static StringBuilder Create<T1, T2, T3, T4, T5, T6>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5, T6> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
            where T6 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5, arguments.Item6));
        }

        public static StringBuilder Create<T1, T2, T3, T4, T5, T6, T7>(IFormatProvider? provider, string format, Tuple<T1, T2, T3, T4, T5, T6, T7> arguments)
            where T1 : IFormattable
            where T2 : IFormattable
            where T3 : IFormattable
            where T4 : IFormattable
            where T5 : IFormattable
            where T6 : IFormattable
            where T7 : IFormattable
        {
            return StringBuilderExtension.Create(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, arguments.Item5, arguments.Item6, arguments.Item7));
        }

        public static StringBuilder Create(bool value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(byte value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(char value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(DateTime value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(decimal value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(double value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(float value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(int value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(long value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(object? value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(sbyte value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(short value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(uint value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(ulong value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(ushort value, IFormatProvider? provider)
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create<TValue>(TValue value, IFormatProvider? provider) where TValue : IConvertible
        {
            return StringBuilderExtension.Create(Convert.ToString(value, provider ?? CultureInfo.InvariantCulture));
        }

        public static StringBuilder Create(FileInfo source)
        {
            return StringBuilderExtension.Create(source.OpenText());
        }

        public static StringBuilder Create(StreamReader reader)
        {
            return StringBuilderExtension.Create(reader.ReadToEnd());
        }

        public static StringBuilder Create(XmlDocument xml)
        {
            return StringBuilderExtension.Create(xml.InnerXml);
        }

        public static StringBuilder Create(JsonDocument json)
        {
            return StringBuilderExtension.Create(json.ToString());
        }

        public static StringBuilder Create(XDocument xml)
        {
            return StringBuilderExtension.Create(xml.Root?.ToString() ?? string.Empty);
        }

        public static StringBuilder Create(ICollection<char> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = new(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        public static StringBuilder Create(IOrderedEnumerable<char> orderedList, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = new(capacity);

            foreach (var item in orderedList)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        public static StringBuilder Create(IOrderedEnumerable<string?> orderedList, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = new(capacity);

            foreach (var item in orderedList)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        public static StringBuilder Create(ICollection<string?> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            StringBuilder accumulator = new(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item);
            }

            return accumulator;
        }

        public static StringBuilder Create<TElement>(ICollection<TElement> collection, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY) where TElement : IFormattable
        {
            StringBuilder accumulator = new(capacity);

            foreach (var item in collection)
            {
                accumulator.Append(item.ToString());
            }

            return accumulator;
        }

        public static StringBuilder Create(char value, int count)
        {
            return StringBuilderExtension.Create(new string(value, count));
        }

        public static StringBuilder Create(int capacity, int maximumCapacity)
        {
            return new StringBuilder(capacity, maximumCapacity);
        }

        public static StringBuilder Create(string? value, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return new StringBuilder(value, capacity);
        }

        public static StringBuilder Create(char[]? array, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return StringBuilderExtension.Create(new string(array), capacity);
        }

        public static StringBuilder Create(string? value, int startIndex, int count, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return new StringBuilder(value, startIndex, count, capacity);
        }

        public static StringBuilder Create(char[] array, int startIndex, int count, int capacity = OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY)
        {
            return StringBuilderExtension.Create(new string(array, startIndex, count), capacity);
        }

        public static StringBuilder CreateWithBase64CharArray(byte[] array, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return StringBuilderExtension.CreateWithBase64CharArray(array, 0, array.Length, 0, options);
        }

        public static StringBuilder CreateWithBase64CharArray(byte[] array, int offsetIn, int length, int offsetOut, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            char[] destination = new char[array.Length];
            Convert.ToBase64CharArray(array, offsetIn, length, destination, offsetOut, options);
            return StringBuilderExtension.Create(destination);
        }

        public static StringBuilder CreateWithBase64String(byte[] array, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return StringBuilderExtension.CreateWithBase64String(array, 0, array.Length, options);
        }

        public static StringBuilder CreateWithBase64String(byte[] array, int offset, int length, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return StringBuilderExtension.Create(Convert.ToBase64String(array, offset, length, options));
        }

        public static StringBuilder CreateWithDirectories(DirectoryInfo source, string searchPattern, EnumerationOptions options)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateDirectories(searchPattern, options))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithDirectories(DirectoryInfo source, string searchPattern, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateDirectories(searchPattern, option))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithDirectories(DirectoryInfo source)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateDirectories())
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithFiles(DirectoryInfo source, string searchPattern, EnumerationOptions options)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateFiles(searchPattern, options))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithFiles(DirectoryInfo source)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateFiles())
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithFiles(DirectoryInfo source, string searchPattern, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            StringBuilder builder = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            foreach (var item in source.EnumerateFiles(searchPattern, option))
            {
                builder.AppendLine(item.FullName);
            }

            return builder;
        }

        public static StringBuilder CreateWithHexLowerString(byte[] array)
        {
            return StringBuilderExtension.CreateWithHexLowerString(array, 0, array.Length);
        }

        public static StringBuilder CreateWithHexLowerString(byte[] array, int offset, int length)
        {
            return StringBuilderExtension.Create(Convert.ToHexString(array, offset, length));
        }

        public static StringBuilder CreateWithHexLowerString(int value)
        {
            return StringBuilderExtension.Create(null, "0x{0:x8}", value);
        }

        public static StringBuilder CreateWithHexString(int value)
        {
            return StringBuilderExtension.Create(null, "0x{0:X8}", value);
        }

        public static StringBuilder CreateWithHexString(byte[] array)
        {
            return StringBuilderExtension.CreateWithHexString(array, 0, array.Length);
        }

        public static StringBuilder CreateWithHexString(byte[] array, int offset, int length)
        {
            return StringBuilderExtension.Create(Convert.ToHexString(array, offset, length));
        }

        public static StringBuilder DefaultIfEmpty(this StringBuilder? builder)
        {
            return StringBuilderExtension.IsNullOrEmpty(builder) ? StringBuilderExtension.Create() : builder!;
        }

        public static char ElementAt(this StringBuilder? builder, int index)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (index < 0 || index >= builder.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Parameter {nameof(index)} {index} is out of range.");
            }

            try
            {
                return builder[index];
            }
        }

        public static char ElementAtOrDefault(this StringBuilder? builder, int index)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return char.MinValue;
            }

            if (index < 0 || index >= builder.Count())
            {
                return char.MinValue;
            }

            try
            {
                return builder![index];
            }
        }

        public static StringBuilder Empty()
        {
            return StringBuilderExtension.Create();
        }

        public static char First(this StringBuilder? builder)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            return builder![0];
        }

        public static char First(this StringBuilder? builder, Func<char, bool> predicate)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            for (int i = 0; i < builder.Count(); i++)
            {
                if (predicate.Invoke(builder![i]))
                {
                    return builder[i];
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        public static char First(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            for (int i = 0; i < builder.Count(); i++)
            {
                if (predicate.Invoke(builder![i], i))
                {
                    return builder[i];
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        public static char FirstOrDefault(this StringBuilder? builder)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? builder![0] : char.MinValue;
        }

        public static char FirstOrDefault(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return char.MinValue;
            }

            for (int i = 0; i < builder.Count(); i++)
            {
                if (predicate.Invoke(builder![i]))
                {
                    return builder[i];
                }
            }

            return char.MinValue;
        }

        public static char FirstOrDefault(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return char.MinValue;
            }

            for (int i = 0; i < builder.Count(); i++)
            {
                if (predicate.Invoke(builder![i], i))
                {
                    return builder[i];
                }
            }

            return char.MinValue;
        }

        public static IEnumerable<Tuple<int, char>> Index(this StringBuilder builder)
        {
            List<Tuple<int, char>> accumulator = new();

            for (int i = 0; i < builder.Count(); i++)
            {
                accumulator.Add(Tuple.Create(i, builder[i]));
            }

            return accumulator;
        }

        public static int IndexOf(this StringBuilder? builder, char character)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? builder!.IndexOf(character, StringComparison.Ordinal) : -1;
        }

        public static int IndexOf(this StringBuilder? builder, char character, StringComparison comparison)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? builder.IndexOf(character: new ReadOnlySpan<char>(in character), comparison) : -1;
        }

        public static int IndexOf(this StringBuilder? builder, ReadOnlySpan<char> character)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? builder.IndexOf(character, StringComparison.Ordinal) : -1;
        }

        public static int IndexOf(this StringBuilder? builder, ReadOnlySpan<char> character, StringComparison comparison)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
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

        public static int IndexOf(this StringBuilder? builder, string value)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexOf(value: new ReadOnlySpan<char>(value.ToCharArray()), 0);
        }

        public static int IndexOf(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
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

        public static int IndexOf(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex, int length)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return -1;
            }

            return builder.IndexOf(value, startIndex, length, StringComparison.Ordinal);
        }

        public static int IndexOf(this StringBuilder? builder, ReadOnlySpan<char> value, int startIndex, int length, StringComparison comparison)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
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

        public static StringBuilder Insert([DisallowNull] this StringBuilder builder, int index, [DisallowNull] StringBuilder value)
        {
            return builder.Insert(index, value.ToString());
        }

        public static bool IsMatch(this StringBuilder? builder, Regex pattern)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && pattern.IsMatch(builder!.ToString());
        }

        public static bool IsMatch(this StringBuilder? builder, Regex pattern, int startIndex)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && pattern.IsMatch(builder!.ToString(startIndex) ?? string.Empty);
        }

        public static bool IsMatch(this StringBuilder? builder, Regex pattern, int startIndex, int count)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) && pattern.IsMatch(builder!.ToString(startIndex, count) ?? string.Empty);
        }

        public static bool IsNullOrEmpty(StringBuilder? builder)
        {
            return builder.Count() < 1;
        }

        public static bool IsNullOrEmpty<TElement>(TElement[]? array)
        {
            return StringBuilderExtension.IsNullOrEmpty(collection: array);
        }

        public static bool IsNullOrEmpty<TValue>(ICollection<TValue>? collection)
        {
            return collection?.Count < 1;
        }

        public static bool IsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary)
        {
            return StringBuilderExtension.IsNullOrEmpty(collection: dictionary);
        }

        public static bool IsNullOrEmpty<TElement>(ISet<TElement> theSet)
        {
            return StringBuilderExtension.IsNullOrEmpty(collection: theSet);
        }

        public static bool IsNullOrWhiteSpace(StringBuilder? builder)
        {
            return StringBuilderExtension.IsNullOrEmpty(builder) || (builder.All(c => char.IsWhiteSpace(c)));
        }

        public static char Last(this StringBuilder? builder)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            return builder![^1];
        }

        public static char Last(this StringBuilder? builder, Func<char, bool> predicate)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            for (int i = builder.Count() - 1; i >= 0; i--)
            {
                if (predicate.Invoke(builder![i]))
                {
                    return builder[i];
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        public static char Last(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            StringBuilderExtension.ThrowIfNullOrEmpty(builder);

            for (int i = builder.Count() - 1; i >= 0; i--)
            {
                if (predicate.Invoke(builder![i], i))
                {
                    return builder[i];
                }
            }

            throw new InvalidOperationException($"No value satisfying {nameof(predicate)} was found.");
        }

        public static char LastOrDefault(this StringBuilder? builder)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? builder![^1] : char.MinValue;
        }

        public static char LastOrDefault(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return char.MinValue;
            }

            for (int i = builder.Count() - 1; i >= 0; i--)
            {
                if (predicate.Invoke(builder![i]))
                {
                    return builder[i];
                }
            }

            return char.MinValue;
        }

        public static char LastOrDefault(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return char.MinValue;
            }

            for (int i = builder.Count() - 1; i >= 0; i--)
            {
                if (predicate.Invoke(builder![i], i))
                {
                    return builder[i];
                }
            }

            return char.MinValue;
        }

        public static MatchCollection? Matches(this StringBuilder? builder, Regex pattern)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? pattern.Matches(builder!.ToString()) : default;
        }

        public static MatchCollection? Matches(this StringBuilder? builder, Regex pattern, int startIndex)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? pattern.Matches(builder!.ToString(startIndex) ?? string.Empty) : default;
        }

        public static MatchCollection Matches(this StringBuilder? builder, Regex pattern, int startIndex, int count)
        {
            return !StringBuilderExtension.IsNullOrEmpty(builder) ? pattern.Matches(builder!.ToString(startIndex, count) ?? string.Empty) : default;
        }

        public static StringBuilder Order([DisallowNull] this StringBuilder builder)
        {
            return builder.Order(comparer: null);
        }

        public static StringBuilder Order([DisallowNull] this StringBuilder builder, bool caseInsensitive)
        {
            return builder.Order(null, caseInsensitive);
        }

        public static StringBuilder Order([DisallowNull] this StringBuilder builder, CultureInfo? culture, bool caseInsensitive)
        {
            if (!caseInsensitive)
            {
                return builder.Order(comparer: null);
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

        public static StringBuilder Order([DisallowNull] this StringBuilder builder, Comparison<char> comparison)
        {
            return builder.Order((x, y) => comparison.Invoke(x, y));
        }

        public static StringBuilder Order([DisallowNull] this StringBuilder builder, IComparer<char>? comparer)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return builder;
            }
            else
            {
                List<char> list = [.. builder!.ToCharArray() ?? [char.MinValue]];
                list.Sort(comparer);
                return StringBuilderExtension.Create(list);
            }
        }

        public static StringBuilder Order([DisallowNull] this StringBuilder builder, int startIndex, int count, IComparer<char>? comparer)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return builder;
            }
            else
            {
                List<char>? list = builder!.ToCharArray()?.ToList();
                list!.Sort(startIndex, count, comparer);
                return StringBuilderExtension.Create(list);
            }
        }

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder, int startIndex, int count, IComparer<char>? comparer)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return builder;
            }
            else
            {
                List<char>? list = builder.ToCharArray(startIndex, count)?.ToList();
                return !StringBuilderExtension.IsNullOrEmpty(list) ? StringBuilderExtension.Create(list!.OrderDescending(comparer)) : builder;
            }
        }

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder)
        {
            return builder.OrderDescending(comparer: null);
        }

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder, bool caseInsensitive)
        {
            return builder.OrderDescending(null, caseInsensitive);
        }

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder, CultureInfo? culture, bool caseInsensitive)
        {
            if (!caseInsensitive)
            {
                return builder.OrderDescending(comparer: null);
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

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder, Comparison<char> comparison)
        {
            return builder.OrderDescending((x, y) => comparison.Invoke(x, y));
        }

        public static StringBuilder OrderDescending([DisallowNull] this StringBuilder builder, IComparer<char>? comparer)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return builder;
            }
            else
            {
                List<char> list = [.. builder!.ToCharArray() ?? [char.MinValue]];
                return StringBuilderExtension.Create(list.OrderDescending(comparer));
            }
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, IEnumerable<ITaskItem> list, string separator)
        {
            bool first = true;

            foreach (var item in list)
            {
                if (first)
                {
                    builder.Prepend(item);
                    first = false;
                }
                else
                {
                    builder.Prepend(item).Prepend(separator);
                }
            }

            return builder;
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, IEnumerable<string?> list, string separator)
        {
            bool first = true;

            foreach (var item in list)
            {
                if (first)
                {
                    builder.Prepend(item);
                    first = false;
                }
                else
                {
                    builder.Prepend(item).Prepend(separator);
                }
            }

            return builder;
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, FileSystemInfo value)
        {
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

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, DirectoryInfo value)
        {
            return builder.Insert(0, value.FullName);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, FileInfo value)
        {
            return builder.Insert(0, value.FullName);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, char value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, ITaskItem value)
        {
            return builder.Insert(0, value.ItemSpec);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, char value, int count)
        {
            return builder.Prepend(new string(value, count));
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, char[]? value, int startIndex, int count)
        {
            return builder.Prepend(value?.Skip(startIndex + 1).Take(count).ToArray());
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, string? value, int startIndex, int count)
        {
            return builder.Prepend(value?.Skip(startIndex + 1).Take(count).ToString());
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, bool value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, byte value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, char[]? value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, decimal value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, double value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, float value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, int value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, long value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, object? value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, sbyte value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, short value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, string? value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, StringBuilder? value)
        {
            return builder.Prepend(value?.ToString());
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, StringBuilder? value, int startIndex)
        {
            return builder.Prepend(value?.ToString(startIndex));
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, StringBuilder? value, int startIndex, int count)
        {
            return builder.Prepend(value?.ToString(startIndex, count));
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, uint value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, ulong value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, ushort value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, ReadOnlySpan<char> value)
        {
            return builder.Insert(0, value);
        }

        public static StringBuilder Prepend([DisallowNull] this StringBuilder builder, string? value, int count)
        {
            return builder.Insert(0, value, count);
        }

        public static StringBuilder PrependFormat([DisallowNull] this StringBuilder builder, IFormatProvider? provider, string format, params object?[] arguments)
        {
            return builder.Prepend(string.Format(provider ?? CultureInfo.InvariantCulture, format, arguments));
        }

        public static StringBuilder PrependFormat([DisallowNull] this StringBuilder builder, string format, params object?[] arguments)
        {
            return builder.PrependFormat(null, format, arguments);
        }

        public static StringBuilder PrependLine([DisallowNull] this StringBuilder builder)
        {
            return builder.Prepend(Environment.NewLine);
        }

        public static StringBuilder PrependLine([DisallowNull] this StringBuilder builder, IFormatProvider? provider, string format, params object?[] arguments)
        {
            return builder.PrependLine().PrependFormat(provider, format, arguments);
        }

        public static StringBuilder PrependLine([DisallowNull] this StringBuilder builder, string format, params object?[] arguments)
        {
            return builder.PrependLine(null, format, arguments);
        }

        public static StringBuilder Repeat([DisallowNull] this StringBuilder builder, char element, int count)
        {
            builder.Clear();
            return builder.Append(element, count);
        }

        public static void Reverse([DisallowNull] StringBuilder builder)
        {
            StringBuilder accumulator = StringBuilderExtension.Create(capacity: builder.Count());

            for (int i = builder.Count() - 1; i >= 0; i--)
            {
                accumulator.Append(builder![i]);
            }

            builder = accumulator;
        }

        public static IEnumerable<TResult> Select<TResult>(this StringBuilder? builder, Func<char, TResult> selector)
        {
            List<TResult> accumulator = [];

            if (!StringBuilderExtension.IsNullOrEmpty(builder))
            {
                for (int i = 0; i < builder.Count(); i++)
                {
                    accumulator.Add(selector.Invoke(builder![i]));
                }
            }

            return accumulator;
        }

        public static IEnumerable<TResult> Select<TResult>(this StringBuilder? builder, Func<char, int, TResult> selector)
        {
            List<TResult> accumulator = [];

            if (!StringBuilderExtension.IsNullOrEmpty(builder))
            {
                for (int i = 0; i < builder.Count(); i++)
                {
                    accumulator.Add(selector.Invoke(builder![i], i));
                }
            }

            return accumulator;
        }

        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right)
        {
            return left.SequenceEqual(right, EqualityComparer<char>.Default);
        }

        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right, Func<char, char, bool> equals)
        {
            if (left is null && right is null)
            {
                return true;
            }
            else if (left is null ^ right is null)
            {
                return false;
            }
            else if (!left.Count().Equals(right.Count()))
            {
                return false;
            }
            else
            {
                for (var i = 0; i < left.Count(); i++)
                {
                    if (!equals.Invoke(left![i], right![i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool SequenceEqual(this StringBuilder? left, StringBuilder? right, IEqualityComparer<char> comparer)
        {
            if (left is null && right is null)
            {
                return true;
            }
            else if (left is null ^ right is null)
            {
                return false;
            }
            else if (!left.Count().Equals(right.Count()))
            {
                return false;
            }
            else
            {
                for (var i = 0; i < left.Count(); i++)
                {
                    if (!comparer.Equals(left![i], right![i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static char Single(this StringBuilder? builder)
        {
            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.First();
            }
        }

        public static char Single(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.First(predicate);
            }
        }

        public static char SingleOrDefault(this StringBuilder? builder)
        {
            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.FirstOrDefault();
            }
        }

        public static char SingleOrDefault(this StringBuilder? builder, Func<char, bool> predicate)
        {
            if (builder.Count() != 1)
            {
                throw new InvalidOperationException($"StringBuilder {nameof(builder)} is not a singleton.");
            }
            else
            {
                return builder.FirstOrDefault(predicate);
            }
        }

        public static StringBuilder? Skip(this StringBuilder? builder, int count)
        {
            return new StringBuilder(builder?.ToString(count - 1) ?? string.Empty);
        }

        public static StringBuilder? SkipLast(this StringBuilder? builder, int count)
        {
            return builder.Take(builder.Count() - count);
        }

        public static StringBuilder? SkipWhile(this StringBuilder? builder, Func<char, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < builder?.Count(); i++)
            {
                if (predicate.Invoke(builder[i]))
                {
                    count++;
                }
            }

            return builder?.Skip(count);
        }

        public static StringBuilder? SkipWhile(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < builder?.Count(); i++)
            {
                if (predicate.Invoke(builder[i], i))
                {
                    count++;
                }
            }

            return builder?.Skip(count);
        }

        public static StringBuilder? Slice(this StringBuilder? builder, int startIndex)
        {
            return new StringBuilder(builder?.ToString(startIndex));
        }

        public static StringBuilder? Slice(this StringBuilder? builder, int startIndex, int count)
        {
            return new StringBuilder(builder?.ToString(startIndex, count));
        }

        public static StringBuilder? Take(this StringBuilder? builder, int count)
        {
            return new StringBuilder(builder?.ToString(0, count) ?? string.Empty);
        }

        public static StringBuilder? TakeLast(this StringBuilder? builder, int count)
        {
            return builder.Skip(builder.Count() - count);
        }

        public static StringBuilder? TakeWhile(this StringBuilder? builder, Func<char, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < builder?.Count(); i++)
            {
                if (predicate.Invoke(builder[i]))
                {
                    count++;
                }
            }

            return builder?.Take(count);
        }

        public static StringBuilder? TakeWhile(this StringBuilder? builder, Func<char, int, bool> predicate)
        {
            int count = 0;

            for (int i = 0; i < builder?.Count(); i++)
            {
                if (predicate.Invoke(builder[i], i))
                {
                    count++;
                }
            }

            return builder?.Take(count);
        }

        public static void ThrowIfNullOrEmpty(this StringBuilder? builder)
        {
            builder.ThrowIfNullOrEmpty(null, null);
        }

        public static void ThrowIfNullOrEmpty(this StringBuilder? builder, string? paramName)
        {
            builder.ThrowIfNullOrEmpty(paramName, null);
        }

        public static void ThrowIfNullOrEmpty(this StringBuilder? builder, string? paramName, string? message)
        {
            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                throw new ArgumentNullException(paramName ?? nameof(builder), message);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(this StringBuilder? builder)
        {
            builder.ThrowIfNullOrWhiteSpace(null, null);
        }

        public static void ThrowIfNullOrWhiteSpace(this StringBuilder? builder, string? paramName)
        {
            builder.ThrowIfNullOrWhiteSpace(paramName, null);
        }

        public static void ThrowIfNullOrWhiteSpace(this StringBuilder? builder, string? paramName, string? message)
        {
            if (StringBuilderExtension.IsNullOrWhiteSpace(builder))
            {
                throw new ArgumentNullException(paramName ?? nameof(builder), message);
            }
        }

        public static char[]? ToCharArray(this StringBuilder? builder)
        {
            return builder?.ToCharArray(0, builder.Count());
        }

        public static char[]? ToCharArray(this StringBuilder? builder, int startIndex)
        {
            return builder?.ToCharArray()?.Skip(startIndex + 1).ToArray();
        }

        public static char[] ToCharArray(this StringBuilder? builder, int startIndex, int count)
        {
            char[] accumulator = new char[count];

            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                return [];
            }

            builder!.CopyTo(startIndex, accumulator, 0, count);
            return accumulator;
        }

        public static IDictionary<int, char> ToDictionary(this StringBuilder builder)
        {
            return builder.Index().ToDictionary();
        }

        public static IDictionary<int, char> ToDictionary(this StringBuilder builder, IEqualityComparer<int>? comparer)
        {
            Dictionary<int, char> accumulator = new(comparer);

            foreach (var item in builder.Index())
            {
                if (item is not null)
                {
                    accumulator.Add(item.Item1, item.Item2);
                }
            }

            return accumulator;
        }

        public static IDictionary<int, char> ToDictionary(this IEnumerable<Tuple<int, char>> tuples)
        {
            Dictionary<int, char> accumulator = new();

            foreach (var item in tuples)
            {
                if (item is not null)
                {
                    accumulator.Add(item.Item1, item.Item2);
                }
            }

            return accumulator;
        }

        public static IList<char>? ToList(this StringBuilder? builder)
        {
            return builder?.ToCharArray()?.ToList();
        }

        public static IList<char>? ToList(this StringBuilder? builder, int startIndex)
        {
            return builder?.ToCharArray(startIndex)?.ToList();
        }

        public static IList<char>? ToList(this StringBuilder? builder, int startIndex, int count)
        {
            return builder?.ToCharArray(startIndex, count)?.ToList();
        }

        public static string? ToString(this StringBuilder? builder, int startIndex)
        {
            return builder?.ToCharArray(startIndex)?.ToString();
        }

        public static StringBuilder Where(this StringBuilder? builder, Func<char, bool> filter)
        {
            StringBuilder accumulator = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                accumulator.Capacity = 0;
                return accumulator;
            }

            for (var i = 0; i < builder.Count(); i++)
            {
                if (filter.Invoke(builder![i]))
                {
                    accumulator.Append(builder[i]);
                }
            }

            return accumulator;
        }

        public static StringBuilder Where(this StringBuilder? builder, Func<char, int, bool> filter)
        {
            StringBuilder accumulator = new(OPTIMAL_INITIAL_STRINGBUILDER_CAPACITY);

            if (StringBuilderExtension.IsNullOrEmpty(builder))
            {
                accumulator.Capacity = 0;
                return accumulator;
            }

            for (var i = 0; i < builder.Count(); i++)
            {
                if (filter.Invoke(builder![i], i))
                {
                    accumulator.Append(builder[i]);
                }
            }

            return accumulator;
        }

        #endregion Public Methods
    }
}
