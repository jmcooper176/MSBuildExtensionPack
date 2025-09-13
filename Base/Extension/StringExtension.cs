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

        public static string Append(this string? originalValue, string? appendValue)
        {
            return string.Concat(originalValue, appendValue);
        }

        public static string Append(this string? originalValue, object? appendValue)
        {
            return string.Concat(originalValue, appendValue);
        }

        public static string Append(this string? originalValue, char appendValue)
        {
            return originalValue.Append(appendValue, 1);
        }

        public static string Append(this string? originalValue, char appendValue, int repeatCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(repeatCount, nameof(repeatCount));

            return string.Concat(originalValue, new string(appendValue, repeatCount));
        }

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

        public static string AppendFormat(this string originalValue, string format, object? argument)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, argument);
        }

        public static string AppendFormat(this string originalValue, string format, object? first, object? second)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second);
        }

        public static string AppendFormat(this string originalValue, string format, object? first, object? second, object? third)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second, third);
        }

        public static string AppendFormat(this string originalValue, string format, params object?[] arguments)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, arguments);
        }

        public static string AppendFormat(this string originalValue, CompositeFormat format, params object?[] arguments)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, arguments);
        }

        public static string AppendFormat<TArg>(this string originalValue, CompositeFormat format, TArg argument)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, argument);
        }

        public static string AppendFormat<TFirst, TSecond>(
            this string originalValue,
            CompositeFormat format,
            TFirst first,
            TSecond second)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second);
        }

        public static string AppendFormat<TFirst, TSecond, TThird>(
            this string originalValue,
            CompositeFormat format,
            TFirst first,
            TSecond second,
            TThird third)
        {
            return AppendFormat(originalValue, CultureInfo.CurrentCulture, format, first, second, third);
        }

        public static string AppendJoin(this string?[] originalArray, char separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            StringBuilder? builder = new(16);
            builder.AppendJoin(separator, originalArray);
            return builder.ToString();
        }

        public static string AppendJoin(this object?[] originalArray, char separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            StringBuilder? builder = new(16);
            builder.AppendJoin(separator, originalArray);
            return builder.ToString();
        }

        public static string AppendJoin(this object?[] originalArray, string? separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            StringBuilder? builder = new(16);
            builder.AppendJoin(separator, originalArray);
            return builder.ToString();
        }

        public static string AppendJoin(this string?[] originalArray, string? separator)
        {
            if (originalArray is null || originalArray.Length < 1)
            {
                return string.Empty;
            }

            StringBuilder? builder = new(16);
            builder.AppendJoin(separator, originalArray);
            return builder.ToString();
        }

        public static string Prepend(this string? originalValue, string? prependValue)
        {
            return string.Concat(prependValue, originalValue);
        }

        public static string Prepend(this string? originalValue, object? prependValue)
        {
            return string.Concat(prependValue, originalValue);
        }

        public static string Prepend(this string? originalValue, char prependValue, int repeatCount)
        {
            return string.Concat(new string(prependValue, repeatCount), originalValue);
        }

        public static string Prepend(this string? originalValue, char prependValue)
        {
            return originalValue.Prepend(prependValue, 1);
        }

        public static string? Replace(this string? originalValue, string oldValue, string? newValue)
        {
            return originalValue?.Replace(oldValue, newValue);
        }

        public static string? Replace(this string? originalValue, char oldValue, char newValue)
        {
            return originalValue?.Replace(oldValue, newValue);
        }

        public static string Replace(this string? originalValue, char oldValue, char newValue, int startIndex, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, originalValue?.Length ?? 0, nameof(startIndex));

            return originalValue.Replace(oldValue, newValue, startIndex, count);
        }

        public static string Replace(this string? originalValue, string oldValue, string? newValue, int startIndex, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(startIndex, originalValue?.Length ?? 0, nameof(startIndex));

            return originalValue.Replace(oldValue, newValue, startIndex, count);
        }

        #endregion Public Methods
    }
}
