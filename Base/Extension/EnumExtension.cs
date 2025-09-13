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

using System.Diagnostics.CodeAnalysis;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public static class EnumExtension
    {
        #region Public Methods

        /// <summary>
        /// Formats <paramref name="value"/> using <paramref name="format"/> to a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"> Specifies the enumeration of type <typeparamref name="TEnum"/> as an <see cref="object"/> to format.</param>
        /// <param name="format">Specifies the <see cref="Enum"/> format string to apply.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="value"/> as formatted by <paramref name="format"/>.</returns>
        public static string Format<TEnum>([DisallowNull] object value, [AllowNull] string format) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(format, nameof(format));

            return Enum.Format(typeof(TEnum), value, format);
        }

        /// <summary>
        /// Gets the underlying value <see cref="Type"/> of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <returns>A <see cref="Type"/> representing the underlying value <see cref="Type"/> of <typeparamref name="TEnum"/>.</returns>
        public static Type GetUnderlyingType<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetUnderlyingType(typeof(TEnum));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/> and the value type of <paramref name="value"/>.</typeparam>
        /// <param name="value">Specifies the enumeration of type <typeparamref name="TEnum"/> to convert to a value.</param>
        /// <returns>A <see cref="object"/> representing the value of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static object? GetValue<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            string? key = string.Empty;

            if (!Enum.IsDefined(value))
            {
                throw new ArgumentException($"Parameter {nameof(value)} of Type {typeof(TEnum).FullName} is not defined.", nameof(value));
            }
            else
            {
                key = Enum.GetName(value);
                ArgumentNullException.ThrowIfNullOrWhiteSpace(key);
            }

            Array values = Enum.GetValuesAsUnderlyingType<TEnum>();
            string[] keys = Enum.GetNames<TEnum>();

            if (values.Count() != keys.Length)
            {
                throw new InvalidOperationException($"Count 'values' [{values.Count()}] does not match Length 'keys' [{keys.Length}].");
            }

            Dictionary<string, object> keyValuePairs = keys.Zip<string, object>(values).ToDictionary(t => t.First, t => t.Second);

            return keyValuePairs.TryGetValue(key, out object? result) ? result : default;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"></param>
        /// <returns>A <typeparamref name="TEnum"/> constructed from <paramref name="value"/>.</returns>
        public static TEnum MakeEnum<TEnum>([DisallowNull] object value) where TEnum : struct, Enum
        {
            return ToObject<TEnum>(value);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"></param>
        /// <returns>A <typeparamref name="TEnum"/> constructed from <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static TEnum ToObject<TEnum>([DisallowNull] object value) where TEnum : struct, Enum
        {
            Type valueType = GetUnderlyingType<TEnum>();

            return value.GetType().IsAssignableTo(valueType) ? (TEnum)Enum.ToObject(typeof(TEnum), value) : throw new ArgumentException($"Parameter {nameof(value)} of Type '{value.GetType().FullName}' is not assignable to the Enum Underlying Type {valueType.FullName}.", nameof(value));
        }

        #endregion Public Methods
    }
}
