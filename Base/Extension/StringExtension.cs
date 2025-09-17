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

namespace MSBuild.ExtensionPack
{
    using System;
    using System.Globalization;
    using System.Text;

    public static class StringExtension
    {
        #region Public Methods

        /// <summary>
        /// Appends the specified append value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="appendValue">  The append value.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="originalValue"/> and <paramref name="appendValue"/>.</returns>
        public static string Append(this string? originalValue, string? appendValue)
        {
            return string.Concat(originalValue, appendValue);
        }

        /// <summary>
        /// Appends the specified append value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="appendValue">  The append value.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="originalValue"/> and <paramref name="appendValue"/>.</returns>
        public static string Append(this string? originalValue, object? appendValue)
        {
            return string.Concat(originalValue, appendValue);
        }

        /// <summary>
        /// Appends the specified append value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="appendValue">  The append value.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="originalValue"/> and <paramref name="appendValue"/>.</returns>
        public static string Append(this string? originalValue, char appendValue)
        {
            return originalValue.Append(appendValue, 1);
        }

        /// <summary>
        /// Appends the specified append value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="appendValue">  The append value.</param>
        /// <param name="repeatCount">  The repeat count.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="originalValue"/> and <paramref name="appendValue"/>.</returns>
        public static string Append(this string? originalValue, char appendValue, int repeatCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(repeatCount, nameof(repeatCount));

            return string.Concat(originalValue, new string(appendValue, repeatCount));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="arguments">    The arguments.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="arguments"/>.
        /// </returns>
        public static string AppendFormat(
            this string? originalValue,
            IFormatProvider? provider,
            string format,
            params object?[] arguments)
        {
            if (string.IsNullOrEmpty(format) || arguments is null)
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="argument">     The argument.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="argument"/>.
        /// </returns>
        public static string AppendFormat(
            this string? originalValue,
            IFormatProvider? provider,
            string format,
            object? argument)
        {
            if (string.IsNullOrEmpty(format) || argument is null)
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, argument));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/> and <paramref name="second"/>.
        /// </returns>
        public static string AppendFormat(
            this string? originalValue,
            IFormatProvider? provider,
            string format,
            object? first,
            object? second)
        {
            if (string.IsNullOrEmpty(format) || (first is null && second is null))
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, first, second));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <param name="third">        The third.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/>, <paramref name="second"/>, and <paramref name="third"/>.
        /// </returns>
        public static string AppendFormat(
            this string? originalValue,
            IFormatProvider? provider,
            string format,
            object? first,
            object? second,
            object? third)
        {
            if (string.IsNullOrEmpty(format) || (first is null && second is null && third is null))
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, first, second, third));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="arguments">    The arguments.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="arguments"/>.
        /// </returns>
        public static string AppendFormat(
            this string? originalValue,
            IFormatProvider? provider,
            CompositeFormat format,
            params object?[] arguments)
        {
            if (arguments is null || arguments.Length < 1)
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments));
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <typeparam name="TArg">The <see cref="Type"/> of the argument.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <param name="provider">     The provider.</param>
        /// <param name="format">       The format.</param>
        /// <param name="argument">     The argument.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="argument"/> of <typeparamref name="TArg"/>.
        /// </returns>
        public static string AppendFormat<TArg>(
            this string? originalValue,
            IFormatProvider? provider,
            CompositeFormat format,
            TArg argument)
        {
            if (format is null || string.IsNullOrEmpty(format.Format))
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, argument));
        }

        /// <summary> Appends the format. </summary> <typeparam name="TFirst">The <see cref="Type"/> of the first.</typeparam>
        /// <typeparam name="TSecond">The <see cref="Type"/> of the second.</typeparam> <param name="originalValue">The original
        /// value.</param> <param name="provider">The provider.</param> <param name="format">The format.</param> <param
        /// name="first">The first.</param> <param name="second">The second.</param> <returns>A <see cref="string"/> representing
        /// <paramref name="originalValue"/> and <see cref="string"/> resulting from the formatting of <paramref name="format"/>
        /// with <paramref name="first"/> of <typeparamref name="TFirst/> and <paramref name="second"/> of <typeparamref name="TSecond"/>.</returns>
        public static string AppendFormat<TFirst, TSecond>(
            this string? originalValue,
            IFormatProvider? provider,
            CompositeFormat format,
            TFirst first,
            TSecond second)
        {
            if (format is null || string.IsNullOrEmpty(format.Format))
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, first, second));
        }

        /// <summary> Appends the format. </summary> <typeparam name="TFirst">The <see cref="Type"/> of the first.</typeparam>
        /// <typeparam name="TSecond">The <see cref="Type"/> of the second.</typeparam> <typeparam name="TThird">The <see
        /// cref="Type"/> of the third.</typeparam> <param name="originalValue">The original value.</param> <param
        /// name="provider">The provider.</param> <param name="format">The format.</param> <param name="first">The first.</param>
        /// <param name="second">The second.</param> <param name="third">The third.</param> <returns>A <see cref="string"/>
        /// representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the formatting of <paramref
        /// name="format"/> with <paramref name="first"/> of <typeparamref name="TFirst/>, <paramref name="second"/> of
        /// <typeparamref name="TSecond"/>, and <paramref name="third"/> of <typeparamref name="TThird"/>.</returns>
        public static string AppendFormat<TFirst, TSecond, TThird>(
            this string? originalValue,
            IFormatProvider? provider,
            CompositeFormat format,
            TFirst first,
            TSecond second,
            TThird third)
        {
            if (format is null || string.IsNullOrEmpty(format.Format))
            {
                return originalValue ?? string.Empty;
            }

            return originalValue.Append(string.Format(provider ?? CultureInfo.CurrentCulture, format, first, second, third));
        }

        /// <summary> Appends the format. </summary> <param name="originalValue">The original value.</param> <param
        /// name="format">The format.</param> <param name="argument">The argument.</param> <returns>A<see cref = "string" />
        /// representing < paramref name="originalValue"/> and<see cref = "string" /> resulting from the formatting of<paramref
        /// name="format"/> with<paramref name = "argument" />.</returns>
        public static string AppendFormat(this string originalValue, string format, object? argument)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, argument);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/> and <paramref name="second"/>.
        /// </returns>
        public static string AppendFormat(this string originalValue, string format, object? first, object? second)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <param name="third">        The third.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/>, <paramref name="second"/>, and <paramref name="third"/>.
        /// </returns>
        public static string AppendFormat(this string originalValue, string format, object? first, object? second, object? third)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second, third);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="format">       The format.</param>
        /// <param name="arguments">    The arguments.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="arguments"/>.
        /// </returns>
        public static string AppendFormat(this string originalValue, string format, params object?[] arguments)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, arguments);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <typeparam name="TFirst">The <see cref="Type"/> of the first.</typeparam>
        /// <typeparam name="TSecond">The <see cref="Type"/> of the second.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/> of <typeparamref name="TFirst"/> and <paramref
        /// name="second"/> of <typeparamref name="TSecond"/>.
        /// </returns>
        public static string AppendFormat<TFirst, TSecond>(
            this string originalValue,
            CompositeFormat format,
            TFirst first,
            TSecond second)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <typeparam name="TFirst">The <see cref="Type"/> of the first.</typeparam>
        /// <typeparam name="TSecond">The <see cref="Type"/> of the second.</typeparam>
        /// <typeparam name="TThird">The <see cref="Type"/> of the third.</typeparam>
        /// <param name="originalValue">The original value.</param>
        /// <param name="format">       The format.</param>
        /// <param name="first">        The first.</param>
        /// <param name="second">       The second.</param>
        /// <param name="third">        The third.</param>
        /// <returns>
        /// A <see cref="string"/> representing <paramref name="originalValue"/> and <see cref="string"/> resulting from the
        /// formatting of <paramref name="format"/> with <paramref name="first"/> of <typeparamref name="TFirst"/>, <paramref
        /// name="second"/> of <typeparamref name="TSecond"/>, and <paramref name="third"/> of <typeparamref name="TThird"/>.
        /// </returns>
        public static string AppendFormat<TFirst, TSecond, TThird>(
            this string originalValue,
            CompositeFormat format,
            TFirst first,
            TSecond second,
            TThird third)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second, third);
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="originalArray">The original array.</param>
        /// <param name="separator">    The separator.</param>
        /// <returns>
        /// A new <see cref="string"/> representing the joining of each element of <paramref name="originalArray"/> with <paramref name="separator"/>.
        /// </returns>
        public static string AppendJoin(this string?[] originalArray, char separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            return string.Join(separator, originalArray);
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="originalArray">The original array.</param>
        /// <param name="separator">    The separator.</param>
        /// <returns>
        /// A new <see cref="string"/> representing the joining of each element of <paramref name="originalArray"/> with <paramref name="separator"/>.
        /// </returns>
        public static string AppendJoin(this object?[] originalArray, char separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            return string.Join(separator, originalArray);
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="originalArray">The original array.</param>
        /// <param name="separator">    The separator.</param>
        /// <returns>
        /// A new <see cref="string"/> representing the joining of each element of <paramref name="originalArray"/> with <paramref name="separator"/>.
        /// </returns>
        public static string AppendJoin(this object?[] originalArray, string? separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            return string.Join(separator ?? string.Empty, originalArray);
        }

        /// <summary>
        /// Appends the join.
        /// </summary>
        /// <param name="originalArray">The original array.</param>
        /// <param name="separator">    The separator.</param>
        /// <returns>
        /// A new <see cref="string"/> representing the joining of each element of <paramref name="originalArray"/> with <paramref name="separator"/>.
        /// </returns>
        public static string AppendJoin(this string?[] originalArray, string? separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            return string.Join(separator ?? string.Empty, originalArray);
        }

        /// <summary>
        /// Prepends the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="prependValue"> The prepend value.</param>
        /// <returns>A new <see cref="string"/> prefix <paramref name="prependValue"/> appended by <paramref name="originalValue"/>.</returns>
        public static string Prepend(this string? originalValue, string? prependValue)
        {
            return string.Concat(prependValue, originalValue);
        }

        /// <summary>
        /// Prepends the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="prependValue"> The prepend value.</param>
        /// <returns>A new <see cref="string"/> prefix <paramref name="prependValue"/> appended by <paramref name="originalValue"/>.</returns>
        public static string Prepend(this string? originalValue, object? prependValue)
        {
            return string.Concat(prependValue, originalValue);
        }

        /// <summary>
        /// Prepends the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="prependValue"> The prepend value.</param>
        /// <param name="repeatCount">  The repeat count.</param>
        /// <returns>
        /// A new <see cref="string"/> prefix <paramref name="prependValue"/> repeated <paramref name="repeatCount"/> times appended
        /// by <paramref name="originalValue"/>.
        /// </returns>
        public static string Prepend(this string? originalValue, char prependValue, int repeatCount)
        {
            return string.Concat(new string(prependValue, repeatCount), originalValue);
        }

        /// <summary>
        /// Prepends the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="prependValue"> The prepend value.</param>
        /// <returns>A new <see cref="string"/> prefix <paramref name="prependValue"/> appended by <paramref name="originalValue"/>.</returns>
        public static string Prepend(this string? originalValue, char prependValue)
        {
            return originalValue.Prepend(prependValue, 1);
        }

        /// <summary>
        /// Replaces the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="oldValue">     The old value.</param>
        /// <param name="newValue">     The new value.</param>
        /// <returns>
        /// A new <see cref="string"/> where all occurrences of <paramref name="oldValue"/> are replaced with <paramref name="newValue"/>.
        /// </returns>
        public static string? Replace(this string? originalValue, string oldValue, string? newValue)
        {
            return originalValue?.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Replaces the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="oldValue">     The old value.</param>
        /// <param name="newValue">     The new value.</param>
        /// <returns>
        /// A new <see cref="string"/> where all occurrences of <paramref name="oldValue"/> are replaced with <paramref name="newValue"/>.
        /// </returns>
        public static string? Replace(this string? originalValue, char oldValue, char newValue)
        {
            return originalValue?.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Replaces the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="oldValue">     The old value.</param>
        /// <param name="newValue">     The new value.</param>
        /// <param name="startIndex">   The start index.</param>
        /// <param name="count">        The count.</param>
        /// <returns>
        /// A new <see cref="string"/> where a sub-string from <paramref name="startIndex"/> count <paramref name="count"/> where
        /// all occurrences of <paramref name="oldValue"/> are replaced by <paramref name="newValue"/>.
        /// </returns>
        public static string Replace(this string? originalValue, char oldValue, char newValue, int startIndex, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, originalValue?.Length ?? 0, nameof(startIndex));

            return originalValue.Replace(oldValue, newValue, startIndex, count);
        }

        /// <summary>
        /// Replaces the specified original value.
        /// </summary>
        /// <param name="originalValue">The original value.</param>
        /// <param name="oldValue">     The old value.</param>
        /// <param name="newValue">     The new value.</param>
        /// <param name="startIndex">   The start index.</param>
        /// <param name="count">        The count.</param>
        /// <returns>
        /// A new <see cref="string"/> where a sub-string from <paramref name="startIndex"/> count <paramref name="count"/> where
        /// all occurrences of <paramref name="oldValue"/> are replaced by <paramref name="newValue"/>.
        /// </returns>
        public static string Replace(this string? originalValue, string oldValue, string? newValue, int startIndex, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, originalValue?.Length ?? 0, nameof(startIndex));

            return originalValue.Replace(oldValue, newValue, startIndex, count);
        }

        #endregion Public Methods
    }
}
