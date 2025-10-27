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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace MSBuild.ExtensionPack.ErrorMessage.Utility
{
    /// <summary>
    /// Implements extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Formats <paramref name="value"/> using <paramref name="format"/> to a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"> 
        /// Specifies the enumeration of <see cref="Type"/><typeparamref name="TEnum"/> as an <see cref="object"/> to format.
        /// </param>
        /// <param name="format">Specifies the <see cref="Enum"/> format string to apply.</param>
        /// <returns>A <see cref="string"/> representing <paramref name="value"/> as formatted by <paramref name="format"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Throws if <paramref name="format"/> is <see langref="null"/>, empty, or all whitespace.
        /// </exception>
        public static string Format<TEnum>([DisallowNull] object value, [AllowNull] string format) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(format, nameof(format));

            return Enum.Format(typeof(TEnum), value, format);
        }

        /// <summary>
        /// Gets the <see cref="string"/> name for <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"></param>
        /// <returns>A <see cref="string"/> representing the name of <paramref name="value"/>.</returns>
        public static string? GetName<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return Enum.GetName(value);
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of all the <see cref="string"/> names of the <typeparamref name="TEnum"/> enumeration.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the <see cref="Type"/> of the enumeration.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> of the names of the <typeparamref name="TEnum"/>.</returns>
        public static IEnumerable<string> GetNames<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetNames<TEnum>();
        }

        /// <summary>
        /// Gets the <see cref="Array"/> of <see cref="string"/> names for <typeparamref name="TEnum"/> and converts it to an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies <see cref="Type"/> of the enumeration to get the <see cref="string"/> names for.</typeparam>
        /// <returns>An <see cref="IQueryable{T}"/> representing the <see cref="string"/> names of <typeparamref name="TEnum"/>.</returns>
        public static IQueryable<string> GetNamesAsQueryable<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetNames<TEnum>().AsQueryable<string>();
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
        /// Gets the <see cref="Enum"/> value for <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the type of the <see cref="Enum"/>.</typeparam>
        /// <param name="value">Specifies the string or underlying type value.</param>
        /// <returns>A <see cref="TEnum"/> associated with <paramref name="value"/>.</returns>
        public static TEnum GetValue<TEnum>([DisallowNull] object value) where TEnum : struct, Enum
        {
            return ToObject<TEnum>(value);
        }

        /// <summary>
        /// Gets the <see cref="Enum"/> value for <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the type of the <see cref="Enum"/>.</typeparam>
        /// <param name="name">Specifies the <see cref="Enum"/> string name.</param>
        /// <returns>A <see cref="TEnum"/> associated with <paramref name="name"/>.</returns>
        public static TEnum GetValue<TEnum>([AllowNull] string name) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            return ToObject<TEnum>(name);
        }

        /// <summary>
        /// Gets the <see cref="Enum"/> value of <see cref="Type"/><paramref name="enumType"/> for <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType">Specifies the <see cref="Type"/> of the <see cref="Enum"/>.</param>
        /// <param name="value">   Specifies the string or underlying type value.</param>
        /// <returns>An <see cref="object"/> representing an <see cref="Enum"/> associated with <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException">Throws if <paramref name="enumType"/> is not an <see cref="Enum"/><see cref="Type"/>.</exception>
        public static object GetValue(Type enumType, [DisallowNull] object value)
        {
            if (!IsEnum(enumType))
            {
                throw new ArgumentException($"Parameter {nameof(enumType)} of Type '{enumType.FullName}' is not an Enum.", nameof(enumType));
            }

            return Enum.ToObject(enumType, value);
        }

        /// <summary>
        /// Gets the <see cref="Enum"/> value of <see cref="Type"/><paramref name="enumType"/> for <paramref name="name"/>.
        /// </summary>
        /// <param name="enumType">Specifies the type of the <see cref="Enum"/>.</param>
        /// <param name="name">    Specifies the string name of the <see cref="Enum"/>.</param>
        /// <returns>An <see cref="object"/> representing an <see cref="Enum"/> associated with <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="name"/> is null, empty, or all whitespace.</exception>
        /// <exception cref="ArgumentException"></exception>
        public static object GetValue(Type enumType, [AllowNull] string name)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            if (!IsEnum(enumType))
            {
                throw new ArgumentException($"Parameter {nameof(enumType)} of Type '{enumType.FullName}' is not an Enum.", nameof(enumType));
            }

            return Enum.ToObject(enumType, name);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetValueAsUnderlyingType<TEnum>([AllowNull] string name) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            return ToUnderlyingType<TEnum>(name);
        }

        public static object GetValueAsUnderlyingType<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            return ToUnderlyingType<TEnum>(Enum.GetName<TEnum>(value));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValueAsUnderlyingType<TEnum, TUnderlyingType>(TEnum value) where TEnum : struct, Enum where TUnderlyingType : struct
        {
            return ToUnderlyingType<TEnum, TUnderlyingType>(value);
        }

        public static object GetValueAsUnderlyingType(Type enumType, object? value)
        {
            if (!IsEnum(enumType))
            {
                throw new ArgumentException($"Parameter {nameof(enumType)} of Type '{enumType.FullName}' is not an Enum.", nameof(enumType));
            }

            return ToUnderlyingType(enumType, value);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>();
        }

        /// <summary>
        /// Return the <see cref="Enum.GetValues{TEnum}()"/> values as an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="TEnum">
        /// Specifies <see cref="Type"/> of the enumeration to get <see cref="Enum.GetValues{TEnum}()"/> values for.
        /// </typeparam>
        /// <returns>An <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<TEnum> GetValuesAsQueryable<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().AsQueryable<TEnum>();
        }

        /// <summary>
        /// Return the <see cref="Enum.GetValuesAsUnderlyingType{TEnum}()"/> values as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TEnum">
        /// Specifies <see cref="Type"/> of the enumeration to get <see cref="Enum.GetValuesAsUnderlyingType{TEnum}()"/> values for.
        /// </typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<object> GetValuesAsUnderlyingTypeAsEnumerable<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValuesAsUnderlyingType<TEnum>().Cast<object>();
        }

        /// <summary>
        /// Return the <see cref="Enum.GetValuesAsUnderlyingType{TEnum}()"/> values as an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="TEnum">
        /// Specifies <see cref="Type"/> of the enumeration to get <see cref="Enum.GetValuesAsUnderlyingType{TEnum}()"/> values for.
        /// </typeparam>
        /// <returns>An <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<object> GetValuesAsUnderlyingTypeAsQueryable<TEnum>() where TEnum : struct, Enum
        {
            return GetValuesAsUnderlyingTypeAsEnumerable<TEnum>().AsQueryable<object>();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<TEnum, char>> Index<TEnum>([AllowNull] this StringBuilder builder) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            var index = Enum.GetValuesAsUnderlyingType<TEnum>().Cast<int>().FirstOrDefault();
            List<Tuple<TEnum, char>> accumulator = new(builder.Count());

            while (ExceptionExtension.IsInRange(index, 0..^builder.Count()))
            {
                accumulator.Add(Tuple.Create<TEnum, char>((TEnum)Enum.ToObject(typeof(TEnum), index), builder.ElementAtOrDefault(index)));

                index++;
            }

            return accumulator;
        }

        /// <summary>
        /// Gets the value of <typeparamref name="TEnum"/> for the <paramref name="value"/> and returns it as an <see cref="object"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/> and the value type of <paramref name="value"/>.</typeparam>
        /// <returns>A <see cref="object"/> representing the value of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Throws if the <typeparamref name="TEnum"/> name <see cref="string"/> for <paramref name="value"/> is <see
        /// langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <exception cref="InvalidOperationException">Throws if the number of keys does not match the number of values.</exception>
        public static IDictionary<string, object> IndexAsUnderlyingType<TEnum>() where TEnum : struct, Enum
        {
            IEnumerable<object> values = GetValuesAsUnderlyingTypeAsEnumerable<TEnum>();
            IQueryable<string> keys = GetNamesAsQueryable<TEnum>();

            if (values.Count() != keys.Count())
            {
                throw new InvalidOperationException($"Count 'values' [{values.Count()}] does not match Length 'keys' [{keys.Count()}].");
            }

            return keys.Zip<string, object>(values).ToDictionary(t => t.First, t => t.Second);
        }

        /// <summary>
        /// </summary>
        /// <param name="underlyingType"></param>
        /// <param name="value">         </param>
        /// <returns></returns>
        public static bool IsAssignableTo(Type underlyingType, object value)
        {
            return value.GetType().IsAssignableTo(underlyingType);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAssignableTo(object value)
        {
            return IsAssignableTo<int>(value);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TUnderlyingType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAssignableTo<TUnderlyingType>(object value) where TUnderlyingType : struct, IConvertible
        {
            return IsAssignableTo(typeof(TUnderlyingType), value);
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// Create a <typeparamref name="TEnum"/> instance from a <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">Specifies the return enumeration <see cref="Type"/>.</typeparam>
        /// <param name="value"></param>
        /// <returns>A <typeparamref name="TEnum"/> constructed from <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Throws if the <see cref="Type"/> of <paramref name="value"/> is not assignable to the underlying value <see
        /// cref="Type"/> of <typeparamref name="TEnum"/>.
        /// </exception>
        public static TEnum ToObject<TEnum>([DisallowNull] object value) where TEnum : struct, Enum
        {
            Type valueType = GetUnderlyingType<TEnum>();

            return IsAssignableTo(valueType, value) ? (TEnum)Enum.ToObject(typeof(TEnum), value) : throw new ArgumentException($"Parameter {nameof(value)} of Type '{value.GetType().FullName}' is not assignable to the Enum Underlying Type {valueType.FullName}.", nameof(value));
        }

        public static object ToUnderlyingType<TEnum>([AllowNull] string name) where TEnum : struct, Enum
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            return ToUnderlyingType(typeof(TEnum), name);
        }

        public static object ToUnderlyingType(Type enumType, [AllowNull] string name)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            if (!IsEnum(enumType))
            {
                throw new ArgumentException($"Parameter {nameof(enumType)} of Type '{enumType.FullName}' is not an Enum.", nameof(enumType));
            }

            return ToUnderlyingType(enumType, Enum.ToObject(enumType, name));
        }

        public static object ToUnderlyingType(Type enumType, object? value)
        {
            if (!IsEnum(enumType))
            {
                throw new ArgumentException($"Parameter {nameof(enumType)} of Type '{enumType.FullName}' is not an Enum.", nameof(enumType));
            }

            Type underlyingType = Enum.GetUnderlyingType(enumType);

            return underlyingType switch
            {
                Type t when t == typeof(sbyte) => Convert.ToSByte(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(byte) => Convert.ToByte(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(short) => Convert.ToInt16(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(int) => Convert.ToInt32(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(long) => Convert.ToInt64(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(ushort) => Convert.ToUInt16(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(uint) => Convert.ToUInt32(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(ulong) => Convert.ToUInt64(value, CultureInfo.InvariantCulture),
                _ => throw new NotSupportedException($"Underlying type {underlyingType.FullName} is not supported as an 'Enum' underlying type."),
            };
        }

        public static object ToUnderlyingType<TEnum, TUnderlyingType>(TEnum value) where TEnum : struct, Enum where TUnderlyingType : struct
        {
            return ToUnderlyingType(typeof(TEnum), value);
        }
    }
}
