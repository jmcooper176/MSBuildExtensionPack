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
using System.Reflection;

using Microsoft.AnalysisServices;

using Assembly = System.Reflection.Assembly;

namespace MSBuild.ExtensionPack.ErrorMessage.AttributeAccess
{
    /// <summary>
    /// Implements an enhanced <see cref="Attribute"/> class.
    /// </summary>
    /// <seealso cref="System.Attribute"/>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CustomAttribute : Attribute
    {
        /// <summary>
        /// The base number for the <see cref="DiagnosticId"/>.
        /// </summary>
        private const int BASE_NUMBER = 1;

        /// <summary>
        /// The prefix for the <see cref="DiagnosticId"/>.
        /// </summary>
        private const string PREFIX = "CSTATTR";

        /// <summary>
        /// The counter that tracks the current number suffix for the <see cref="DiagnosticId"/> based on an incrementing this
        /// counter for each new <see cref="Message"/>.
        /// </summary>
        private static int counter = BASE_NUMBER;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttribute"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="number"> The number.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected CustomAttribute(string? message, int number)
            : base()
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number, nameof(number));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 0xFFFFFFF, nameof(number));

            // fields
            counter = Math.Max(number, BASE_NUMBER);

            // properties
            DiagnosticId = $"{PREFIX}{number:D7}";
            UrlFormat = "https://github.com/jmcooper176";

            // virtual properties
            IsDefault = string.IsNullOrWhiteSpace(message) && number == BASE_NUMBER;
            Message = message ?? $"Default {this.GetType().Name} Constructor";
            TypeId = this.GetType().GUID;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is default.
        /// </summary>
        /// <value><see langref="true"/> if this instance is default; otherwise, <see langref="false"/>.</value>
        protected virtual bool IsDefault { get; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        internal int GetHashCode([DisallowNull] Attribute obj)
        {
            return HashCode.Combine(obj.GetHashCode(), TypeId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttribute"/> class.
        /// </summary>
        public CustomAttribute()
            : this(null, BASE_NUMBER)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttribute"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CustomAttribute(string? message)
            : this(message, counter++)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttribute"/> class.
        /// </summary>
        /// <param name="message">  The message.</param>
        /// <param name="urlFormat">The URL format.</param>
        public CustomAttribute(string? message, string? urlFormat)
            : this(message, counter++)
        {
            UrlFormat = urlFormat;
        }

        /// <summary>
        /// Gets or sets the diagnostic identifier.
        /// </summary>
        /// <value>The diagnostic identifier.</value>
        public virtual string? DiagnosticId { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public virtual string? Message { get; }

        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref="Attribute"/>.
        /// </summary>
        public override object TypeId { get; }

        /// <summary>
        /// Gets or sets the URL format.
        /// </summary>
        /// <value>The URL format.</value>
        public virtual string? UrlFormat { get; set; }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return (TAttribute?)Attribute.GetCustomAttribute(element, typeof(TAttribute), inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            if (element.MemberType != MemberTypes.Constructor
                && element.MemberType != MemberTypes.Method
                && element.MemberType != MemberTypes.Property
                && element.MemberType != MemberTypes.Event
                && element.MemberType != MemberTypes.TypeInfo
                && element.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{element.Name} : Element Member Type {element.MemberType} is not supported.");
            }

            try
            {
                return (TAttribute?)Attribute.GetCustomAttribute(element, typeof(TAttribute), inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return (TAttribute?)Attribute.GetCustomAttribute(element, typeof(TAttribute), inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return (TAttribute?)Attribute.GetCustomAttribute(element, typeof(TAttribute), inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute<TAttribute>(element, inherit: false);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] MemberInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit: false);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Assembly element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit: false);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit: false);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="constructor">The constructor.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(ConstructorInfo? constructor, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(constructor, nameof(constructor));

            try
            {
                return constructor.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return constructor.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="inherit">  if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(EventInfo? eventInfo, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(eventInfo, nameof(eventInfo));

            try
            {
                return eventInfo.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return eventInfo.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="field">  The field.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(FieldInfo? field, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(field, nameof(field));

            try
            {
                return field.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return field.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="inherit"> if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(PropertyInfo? property, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            try
            {
                return property.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return property.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="type">   The <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(TypeInfo? type, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            try
            {
                return type.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return type.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="method"> The method.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">method</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<TAttribute>(MethodInfo? method, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(method, nameof(method));

            try
            {
                return method.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return method.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">module</exception>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            Type? targetType = module.FindTypes(filter, filterCriteria).FirstOrDefault(t => t.GetCustomAttribute<TAttribute>() is not null);

            return GetTypeInfo(targetType).GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <typeparam name="TEnum">The <see cref="Type"/> of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute, TEnum>(TEnum value)
            where TAttribute : Attribute
            where TEnum : struct, Enum
        {
            return GetCustomAttribute<TAttribute, TEnum>(value, inherit: false);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <typeparam name="TEnum">The <see cref="Type"/> of the enum.</typeparam>
        /// <param name="value">  The value.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute, TEnum>(TEnum value, bool inherit)
            where TAttribute : Attribute
            where TEnum : struct, Enum
        {
            return GetField<TEnum, TAttribute>(value, Enum.GetName<TEnum>(value))?.GetCustomAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Gets the custom <see cref="Attribute"/> s.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <typeparam name="TEnum">The <see cref="Type"/> of the enum.</typeparam>
        /// <param name="value">  The value.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute, TEnum>(TEnum value, bool inherit)
                    where TAttribute : Attribute
            where TEnum : struct, Enum
        {
            return GetField<TEnum, TAttribute>(value, Enum.GetName<TEnum>(value))?.GetCustomAttributes<TAttribute>(inherit) ?? [];
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <typeparam name="TEnum">The <see cref="Type"/> of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute, TEnum>(TEnum value)
                    where TAttribute : Attribute
            where TEnum : struct, Enum
        {
            return GetCustomAttributes<TAttribute, TEnum>(value, inherit: false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            if (element.MemberType != MemberTypes.Constructor
                && element.MemberType != MemberTypes.Method
                && element.MemberType != MemberTypes.Property
                && element.MemberType != MemberTypes.Event
                && element.MemberType != MemberTypes.TypeInfo
                && element.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{element.Name} : Element Member Type {element.MemberType} is not supported.");
            }

            try
            {
                return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            if (element.Member is null)
            {
                throw new ArgumentException($"Element member property of {element.Name} is null", nameof(element), new ArgumentNullException(nameof(element), $"Element member property of {element.Name} is null"));
            }

            try
            {
                return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return GetCustomAttributes(element, typeof(TAttribute)).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">element</exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return GetCustomAttributes(element, typeof(TAttribute)).Cast<TAttribute>();
            }
            catch (TypeLoadException ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">module</exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            Type? targetType = null;

            try
            {
                targetType = module.FindTypes(filter, filterCriteria).FirstOrDefault(t => t.GetCustomAttribute<TAttribute>() is not null);
                return GetTypeInfo(targetType).GetCustomAttributes<TAttribute>() ?? [];
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return targetType?.GetCustomAttributes<TAttribute>(inherit: false) ?? [];
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name"> The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public static FieldInfo? GetField<TValue, TAttribute>(TValue value, [AllowNull] string name) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            Type element = value.GetType();

            if (element.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{element.FullName} : Field MemberType Type {element.MemberType} is not supported.");
            }

            return element.GetField(name);
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static MemberInfo? GetMember<T, TAttribute>(string name) where TAttribute : Attribute
        {
            return typeof(T).GetMember(name).FirstOrDefault(m => m.GetCustomAttribute<TAttribute>(inherit: false) is not null && m.Name.Equals(name, StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <returns></returns>
        public static MemberInfo? GetMember<T, TAttribute>(string name, BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return typeof(T).GetMember(name, bindingAttr).FirstOrDefault(m => m.GetCustomAttribute<TAttribute>(inherit: false) is not null && m.Name.Equals(name, StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="value">  The value.</param>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public static MemberInfo? GetMember<TValue, TAttribute>(TValue value, string name, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            Type element = value.GetType();

            if (element.MemberType != MemberTypes.Constructor
                && element.MemberType != MemberTypes.Method
                && element.MemberType != MemberTypes.Property
                && element.MemberType != MemberTypes.Event
                && element.MemberType != MemberTypes.TypeInfo
                && element.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{element.Name} : Element Member Type {element.MemberType} is not supported.");
            }

            return element.GetMember(name).FirstOrDefault(m => m.GetCustomAttribute<TAttribute>(inherit) is not null && m.MemberType == element.MemberType && m.Name.Equals(name, StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static MemberInfo[] GetMembers<T, TAttribute>(string name) where TAttribute : Attribute
        {
            return typeof(T).GetMember(name);
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <returns></returns>
        public static MemberInfo[] GetMembers<T, TAttribute>(string name, BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return typeof(T).GetMember(name, bindingAttr);
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name"> The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public static IEnumerable<MemberInfo> GetMembers<TValue, TAttribute>(TValue value, string name) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            Type element = value.GetType();

            if (element.MemberType != MemberTypes.Constructor
                && element.MemberType != MemberTypes.Method
                && element.MemberType != MemberTypes.Property
                && element.MemberType != MemberTypes.Event
                && element.MemberType != MemberTypes.TypeInfo
                && element.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{element.Name} : Element Member Type {element.MemberType} is not supported.");
            }

            return element.GetMember(name);
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <typeparam name="TParent">The <see cref="Type"/> of the parent.</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public static MethodInfo? GetMethod<TParent>(string methodName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(methodName, nameof(methodName));

            return typeof(TParent).GetMethod(methodName);
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <typeparam name="TParent">The <see cref="Type"/> of the parent.</typeparam>
        /// <param name="methodName"> Name of the method.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <returns></returns>
        public static MethodInfo? GetMethod<TParent>(string methodName, BindingFlags bindingAttr)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(methodName, nameof(methodName));

            return typeof(TParent).GetMethod(methodName, bindingAttr);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <typeparam name="TParent">The <see cref="Type"/> of the parent.</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public static ParameterInfo[] GetParameters<TParent>(string methodName)
        {
            return GetMethod<TParent>(methodName)?.GetParameters() ?? [];
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <typeparam name="TParent">The <see cref="Type"/> of the parent.</typeparam>
        /// <param name="methodName"> Name of the method.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <returns></returns>
        public static ParameterInfo[] GetParameters<TParent>(string methodName, BindingFlags bindingAttr)
        {
            return GetMethod<TParent>(methodName, bindingAttr)?.GetParameters() ?? [];
        }

        /// <summary>
        /// Gets the <see cref="TypeInfo"/> information.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">targetType</exception>
        public static TypeInfo GetTypeInfo([AllowNull] Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType, nameof(targetType));

            return targetType.GetTypeInfo();
        }

        /// <summary>
        /// Gets the <see cref="TypeInfo"/> information.
        /// </summary>
        /// <typeparam name="TType">The <see cref="Type"/> of the <see cref="Type"/>.</typeparam>
        /// <returns></returns>
        public static TypeInfo GetTypeInfo<TType>() where TType : Type
        {
            return GetTypeInfo(typeof(TType));
        }

        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="type">   The <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified <see cref="Type"/> is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><see cref="Type"/></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] TypeInfo type, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            try
            {
                return type.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = type.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified property is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="inherit"> if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified property is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">property</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] PropertyInfo property, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            try
            {
                return property.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = property.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified method is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="method"> The method.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified method is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">method</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] MethodInfo method, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(method, nameof(method));

            try
            {
                return method.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = method.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified field is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="field">  The field.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified field is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">field</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] FieldInfo field, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(field, nameof(field));

            try
            {
                return field.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = field.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified event information is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="inherit">  if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified event information is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">eventInfo</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] EventInfo eventInfo, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(eventInfo, nameof(eventInfo));

            try
            {
                return eventInfo.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = eventInfo.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified constructor is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="constructor">The constructor.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified constructor is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">constructor</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] ConstructorInfo constructor, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(constructor, nameof(constructor));

            try
            {
                return constructor.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = constructor.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified module is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns><see langref="true"/> if the specified module is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(module, filter, filterCriteria) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefined<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));

            try
            {
                return GetCustomAttribute<TAttribute>(element, inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    Attribute? value = GetCustomAttributes<TAttribute>(element, inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] MemberInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Assembly element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><see langref="true"/> if the specified element is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified inherit is defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if the specified inherit is defined; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefined<T, TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return IsDefinedOnType<TAttribute>(typeof(T), inherit);
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified <see cref="Array"/> of <see cref="Type"/>].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="types">  The <see cref="Array"/> of <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns>
        /// <see langref="true"/> if [is defined on constructor] [the specified <see cref="Array"/> of <see cref="Type"/>];
        /// otherwise, <see langref="false"/>.
        /// </returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnConstructor<T, TAttribute>(Type[] types, bool inherit) where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(types);

            try
            {
                return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = constructor?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="types">      The <see cref="Array"/> of <see cref="Type"/>.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns>
        /// <see langref="true"/> if [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes];
        /// otherwise, <see langref="false"/>.
        /// </returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnConstructor<T, TAttribute>(BindingFlags bindingAttr, Type[] types, bool inherit) where T : class where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, types);

            try
            {
                return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = constructor?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="binder">     The <see cref="Binder"/> binder.</param>
        /// <param name="types">      The <see cref="Array"/> of <see cref="Type"/>.</param>
        /// <param name="modifiers">  The modifiers.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns>
        /// <see langref="true"/> if [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes];
        /// otherwise, <see langref="false"/>.
        /// </returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnConstructor<T, TAttribute>(BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers, bool inherit)
            where T : class
            where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, binder, types, modifiers);

            try
            {
                return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = constructor?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="binder">     The <see cref="Binder"/> binder.</param>
        /// <param name="convention"> The convention.</param>
        /// <param name="types">      The <see cref="Array"/> of <see cref="Type"/>.</param>
        /// <param name="modifiers">  The modifiers.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns>
        /// <see langref="true"/> if [is defined on constructor] [the specified <see cref="BindingFlags"/> binding attributes];
        /// otherwise, <see langref="false"/>.
        /// </returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnConstructor<T, TAttribute>(BindingFlags bindingAttr, Binder? binder, CallingConventions convention, Type[] types, ParameterModifier[]? modifiers, bool inherit)
            where T : class
            where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, binder, convention, types, modifiers);

            try
            {
                return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = constructor?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on default constructor] [the specified inherit].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on default constructor] [the specified inherit]; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefinedOnDefaultConstructor<T, TAttribute>(bool inherit) where TAttribute : Attribute
        {
            return IsDefinedOnConstructor<T, TAttribute>(Type.EmptyTypes, inherit);
        }

        /// <summary>
        /// Determines whether [is defined on event] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on event] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnEvent<T, TAttribute>(string name, bool inherit) where TAttribute : Attribute
        {
            EventInfo? eventInfo = typeof(T).GetEvent(name);

            try
            {
                return eventInfo?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = eventInfo?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on event] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on event] [the specified name]; otherwise, <see langref="false"/>.</returns>
        public static bool IsDefinedOnEvent<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where T : class where TAttribute : Attribute
        {
            EventInfo? eventInfo = typeof(T).GetEvent(name, bindingAttr);

            try
            {
                return eventInfo?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = eventInfo?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on field] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on field] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnField<T, TAttribute>(string name, bool inherit) where T : class where TAttribute : Attribute
        {
            FieldInfo? field = typeof(T).GetField(name);

            try
            {
                return field?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = field?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on field] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on field] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnField<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where TAttribute : Attribute
        {
            FieldInfo? field = typeof(T).GetField(name, bindingAttr);

            try
            {
                return field?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = field?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on member] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">      The name.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="inherit">   if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on member] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnMember<T, TAttribute>(string name, MemberTypes memberType, bool inherit) where TAttribute : Attribute
        {
            MemberInfo? member = typeof(T).GetMember(name).FirstOrDefault(m => m.MemberType == memberType);

            try
            {
                return member?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = member?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on member] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="memberType"> Type of the member.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on member] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnMember<T, TAttribute>(string name, MemberTypes memberType, BindingFlags bindingAttr, bool inherit) where TAttribute : Attribute
        {
            MemberInfo? member = null;

            try
            {
                member = typeof(T).GetMember(name, bindingAttr).FirstOrDefault(m => m.MemberType == memberType);
                return member?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = member?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on method] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on method] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnMethod<T, TAttribute>(string name, bool inherit) where TAttribute : Attribute
        {
            MethodInfo? method = null;

            try
            {
                method = GetMethod<T>(name);
                return method?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = method?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on method] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="inherit">    if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on method] [the specified name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnMethod<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where TAttribute : Attribute
        {
            MethodInfo? method = null;

            try
            {
                method = GetMethod<T>(name, bindingAttr);
                return method?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = method?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on parameter] [the specified method name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="methodName">   Name of the method.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="inherit">      if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on parameter] [the specified method name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnParameter<T, TAttribute>(string methodName, string parameterName, bool inherit) where TAttribute : Attribute
        {
            ParameterInfo? parameter = null;

            try
            {
                parameter = GetParameters<T>(methodName).FirstOrDefault(p => p.Name?.Equals(parameterName, StringComparison.Ordinal) == true);
                return parameter?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = parameter?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on parameter] [the specified method name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="methodName">   Name of the method.</param>
        /// <param name="bindingAttr">  The <see cref="BindingFlags"/> binding attributes.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="inherit">      if set to <see langref="true"/> [inherit].</param>
        /// <returns><see langref="true"/> if [is defined on parameter] [the specified method name]; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnParameter<T, TAttribute>(string methodName, BindingFlags bindingAttr, string parameterName, bool inherit) where T : class where TAttribute : Attribute
        {
            ParameterInfo? parameter = null;

            try
            {
                parameter = GetParameters<T>(methodName, bindingAttr).FirstOrDefault(p => p.Name?.Equals(parameterName, StringComparison.Ordinal) == true);
                return parameter?.GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = parameter?.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is defined on <see cref="Type"/>] [the specified <see cref="Type"/>].
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="type">   The <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <returns>
        /// <see langref="true"/> if [is defined on <see cref="Type"/>] [the specified <see cref="Type"/>]; otherwise, <see langref="false"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><see cref="Type"/></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static bool IsDefinedOnType<TAttribute>([AllowNull] Type type, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            try
            {
                return GetTypeInfo(type).GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = GetTypeInfo(type).GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                    return value is not null;
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Tries the get custom <see cref="Attribute"/>.
        /// </summary>
        /// <param name="type">   The <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        /// <exception cref="TypeLoadException"></exception>
        public static bool TryGetCustomAttribute<TAttribute>([AllowNull] Type type, bool inherit, out TAttribute? value) where TAttribute : Attribute
        {
            value = default;

            if (type is null)
            {
                Console.Error.WriteLine($"Parameter {nameof(type)} is null");
                return false;
            }

            if (type.MemberType != MemberTypes.Constructor
                && type.MemberType != MemberTypes.Event
                && type.MemberType != MemberTypes.Field
                && type.MemberType != MemberTypes.Method
                && type.MemberType != MemberTypes.Property
                && type.MemberType != MemberTypes.TypeInfo)
            {
                Console.Error.WriteLine($"Type {type.FullName} has a MemberType {type.MemberType} that is not supported.");
                return false;
            }

            var result = IsDefinedOnType<TAttribute>(type, inherit);

            if (result)
            {
                try
                {
                    value = GetTypeInfo(type).GetCustomAttribute<TAttribute>(inherit);
                }
                catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
                {
                    if (ex is AmbiguousMatchException)
                    {
                        Console.Error.WriteLine(ex.ToString());
                        value = GetTypeInfo(type).GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
                        return value is not null;
                    }
                    else
                    {
                        Console.Error.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="Object"/> to compare with this instance or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance are of the same <see cref="Type"/> and have identical
        /// field values; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj) && Equals(this, obj);
        }

        /// <summary>
        /// Equals the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public virtual bool Equals(Attribute? x, Attribute? y)
        {
            CustomAttribute? xPrime = x as CustomAttribute;
            CustomAttribute? yPrime = y as CustomAttribute;

            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x is null ^ y is null)
            {
                return false;
            }
            else if (!string.Equals(xPrime?.DiagnosticId, yPrime?.DiagnosticId, StringComparison.Ordinal))
            {
                return false;
            }
            else if (xPrime?.IsDefault != yPrime?.IsDefault)
            {
                return false;
            }
            else if (!string.Equals(xPrime?.Message, yPrime?.Message, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (!string.Equals(xPrime?.UrlFormat, yPrime?.UrlFormat, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.DiagnosticId, this.IsDefault, this.Message, this.UrlFormat);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode([DisallowNull] CustomAttribute obj)
        {
            return HashCode.Combine(GetHashCode(obj as Attribute), this.GetHashCode());
        }

        /// <summary>
        /// When overridden in a derived class, indicates whether the value of this instance is the default value for the derived class.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if this instance is the default <see cref="Attribute"/> for the class; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool IsDefaultAttribute()
        {
            return IsDefault;
        }

        /// <summary>
        /// When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.
        /// </summary>
        /// <param name="obj">Specifies an <see cref="Object"/> to compare with this instance of <see cref="Attribute"/>.</param>
        /// <returns><see langword="true"/> if this instance equals <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Match(object? obj)
        {
            return this.Equals(obj);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            if (this.IsDefaultAttribute())
            {
                return $"{this.GetType().FullName} : {DiagnosticId} : {Message}";
            }
            else
            {
                return $"{this.GetType().Name} : {DiagnosticId} {UrlFormat} : {Message}";
            }
        }
    }
}
