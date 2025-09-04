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

using Microsoft.AnalysisServices;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Assembly = System.Reflection.Assembly;

namespace MSBuild.ExtensionPack.Base.SystemAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CustomAttribute : Attribute
    {
        #region Private Fields

        /// <summary>
        /// The base number
        /// </summary>
        private const int BASE_NUMBER = 1;

        /// <summary>
        /// The prefix
        /// </summary>
        private const string PREFIX = "CSTATTR";

        /// <summary>
        /// The counter
        /// </summary>
        private static int counter = BASE_NUMBER;

        #endregion Private Fields

        #region Protected Constructors

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
            IsDefault = string.IsNullOrEmpty(message) && number == BASE_NUMBER;
            Message = message ?? $"Default {this.GetType().Name}  Constructor";
            TypeId = this.GetType().GUID;
        }

        #endregion Protected Constructors

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        protected virtual bool IsDefault { get; }

        #endregion Protected Properties

        #region Internal Methods

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        internal int GetHashCode([DisallowNull] Attribute obj)
        {
            return HashCode.Combine(obj.GetHashCode(), TypeId);
        }

        #endregion Internal Methods

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

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
        /// When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute"/>.
        /// </summary>
        public override object TypeId { get; }

        /// <summary>
        /// Gets or sets the URL format.
        /// </summary>
        /// <value>The URL format.</value>
        public virtual string? UrlFormat { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static Attribute? GetCustomAttribute([AllowNull] ParameterInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            try
            {
                return Attribute.GetCustomAttribute(element, attributeType, inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, attributeType, inherit).FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute(element, typeof(TAttribute), inherit) as TAttribute;
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static Attribute? GetCustomAttribute([AllowNull] MemberInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

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
                return Attribute.GetCustomAttribute(element, attributeType, inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, attributeType, inherit).FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute), inherit);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static Attribute? GetCustomAttribute([AllowNull] Assembly element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            try
            {
                return Attribute.GetCustomAttribute(element, attributeType, inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, attributeType, inherit).FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute), inherit);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static Attribute? GetCustomAttribute([AllowNull] Module element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            try
            {
                return Attribute.GetCustomAttribute(element, attributeType, inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return Attribute.GetCustomAttributes(element, attributeType, inherit).FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute), inherit);
        }

        public new static Attribute? GetCustomAttribute([AllowNull] Module element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute));
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static Attribute? GetCustomAttribute([AllowNull] MemberInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] MemberInfo element) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute));
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static Attribute? GetCustomAttribute([AllowNull] Assembly element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Assembly element) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute));
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static Attribute? GetCustomAttribute([AllowNull] ParameterInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            return (TAttribute?)GetCustomAttribute(element, typeof(TAttribute));
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(bool inherit) where T : class where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="constructor">The constructor.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(ConstructorInfo? constructor, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="inherit">  if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(EventInfo? eventInfo, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="field">  The field.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(FieldInfo? field, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="member"> The member.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(MemberInfo? member, bool inherit) where T : class where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(member, nameof(member));

            if (member.MemberType != MemberTypes.Constructor
                && member.MemberType != MemberTypes.Method
                && member.MemberType != MemberTypes.Property
                && member.MemberType != MemberTypes.Event
                && member.MemberType != MemberTypes.TypeInfo
                && member.MemberType != MemberTypes.Field)
            {
                throw new NotSupportedException($"{member.Name} : Element Member Type {member.MemberType} is not supported.");
            }

            try
            {
                return member.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return member.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="inherit">  if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(ParameterInfo? parameter, bool inherit) where T : class where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            try
            {
                return parameter.GetCustomAttribute<TAttribute>(inherit);
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return parameter.GetCustomAttributes<TAttribute>().FirstOrDefault();
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="inherit"> if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(PropertyInfo? property, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">   The type.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(TypeInfo? type, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="method"> The method.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">method</exception>
        /// <exception cref="TypeLoadException"></exception>
        public static TAttribute? GetCustomAttribute<T, TAttribute>(MethodInfo? method, bool inherit) where T : class where TAttribute : Attribute
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
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">module</exception>
        public static TAttribute? GetCustomAttribute<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            Type? targetType = module.FindTypes(filter, filterCriteria).FirstOrDefault(t => t.GetCustomAttribute<TAttribute>() is not null);

            return targetType?.GetTypeInfo().GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] ParameterInfo element, bool inherit)
        {
            return GetCustomAttributes<CustomAttribute>(element, inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">element or attributeType</exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] MemberInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

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
                return Attribute.GetCustomAttributes(element, attributeType, inherit);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            return new List<Attribute>(GetCustomAttributes(element, typeof(TAttribute), inherit)).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// element or attributeType or element - Element member property of {element.Name} is null
        /// </exception>
        /// <exception cref="System.ArgumentException">Element member property of {element.Name} is null - element - element</exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] ParameterInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (element.Member is null)
            {
                throw new ArgumentException($"Element member property of {element.Name} is null", nameof(element), new ArgumentNullException(nameof(element), $"Element member property of {element.Name} is null"));
            }

            try
            {
                return Attribute.GetCustomAttributes(element, attributeType, inherit);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            return new List<Attribute>(GetCustomAttributes(element, typeof(TAttribute), inherit)).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">element or attributeType</exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            try
            {
                return Attribute.GetCustomAttributes(element, attributeType, inherit);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            return new List<Attribute>(GetCustomAttributes(element, typeof(TAttribute), inherit)).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] MemberInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttributes(element, attributeType, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">element or attributeType</exception>
        /// <exception cref="System.ArgumentException">
        /// Parameter '{nameof(attributeType)}' is not derived from 'Attribute' - attributeType
        /// </exception>
        /// <exception cref="TypeLoadException"></exception>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Assembly element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element, nameof(element));
            ArgumentNullException.ThrowIfNull(attributeType, nameof(attributeType));

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            try
            {
                return Attribute.GetCustomAttributes(element, attributeType, inherit);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            return new List<Attribute>(GetCustomAttributes(element, typeof(TAttribute), inherit)).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element, [AllowNull] Type attributeType)
        {
            return GetCustomAttributes(element, attributeType, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            return new List<Attribute>(GetCustomAttributes(element, typeof(TAttribute))).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] ParameterInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttributes(element, attributeType, false);
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            return GetCustomAttributes(element, typeof(TAttribute)).Cast<TAttribute>();
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] MemberInfo element, bool inherit)
        {
            return GetCustomAttributes<CustomAttribute>(element, inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Assembly element, [AllowNull] Type attributeType)
        {
            return GetCustomAttributes(element, attributeType, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Assembly element, bool inherit)
        {
            return GetCustomAttributes<CustomAttribute>(element, inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] ParameterInfo element)
        {
            return GetCustomAttributes<CustomAttribute>(element, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element)
        {
            return GetCustomAttributes<CustomAttribute>(element, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] MemberInfo element)
        {
            return GetCustomAttributes<CustomAttribute>(element, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Assembly element)
        {
            return GetCustomAttributes<CustomAttribute>(element, false);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element, bool inherit)
        {
            return GetCustomAttributes<CustomAttribute>(element, inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">module</exception>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(module, nameof(module));

            if (module)

                type
                Type? targetType = module.FindTypes(filter, filterCriteria).FirstOrDefault(t => t.GetCustomAttribute<TAttribute>() is not null);
            return targetType?.GetTypeInfo().GetCustomAttributes<TAttribute>() ?? [];
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] ParameterInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            return GetCustomAttribute(element, attributeType, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified type is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">   The type.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified type is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static bool IsDefined<TAttribute>([AllowNull] TypeInfo type, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return type.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified property is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="inherit"> if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified property is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">property</exception>
        public static bool IsDefined<TAttribute>([AllowNull] PropertyInfo property, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            return property.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified method is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="method"> The method.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified method is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">method</exception>
        public static bool IsDefined<TAttribute>([AllowNull] MethodInfo method, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(method, nameof(method));

            return method.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified field is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="field">  The field.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified field is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">field</exception>
        public static bool IsDefined<TAttribute>([AllowNull] FieldInfo field, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(field, nameof(field));

            return field.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified event information is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="eventInfo">The event information.</param>
        /// <param name="inherit">  if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified event information is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">eventInfo</exception>
        public static bool IsDefined<TAttribute>([AllowNull] EventInfo eventInfo, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(eventInfo, nameof(eventInfo));

            return eventInfo.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified constructor is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="constructor">The constructor.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified constructor is defined; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">constructor</exception>
        public static bool IsDefined<TAttribute>([AllowNull] ConstructorInfo constructor, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(constructor, nameof(constructor));

            return constructor.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified module is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="module">        The module.</param>
        /// <param name="filter">        The filter.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns><c>true</c> if the specified module is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module module, TypeFilter? filter, object? filterCriteria) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(module, filter, filterCriteria) is not null;
        }

        public static bool IsDefined<TAttribute>([AllowNull] ParameterInfo element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] Module element, [AllowNull] Type attributeType, bool inherit)
        {
            return GetCustomAttribute(element, attributeType, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] MemberInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            return GetCustomAttribute(element, attributeType, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] MemberInfo element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] Assembly element, [AllowNull] Type attributeType, bool inherit)
        {
            return GetCustomAttribute(element, attributeType, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Assembly element, bool inherit) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element, inherit) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] MemberInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] MemberInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] Module element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Module element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] Assembly element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] Assembly element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <param name="element">      The element.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public new static bool IsDefined([AllowNull] ParameterInfo element, [AllowNull] Type attributeType)
        {
            return GetCustomAttribute(element, attributeType) is not null;
        }

        /// <summary>
        /// Determines whether the specified element is defined.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if the specified element is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<TAttribute>([AllowNull] ParameterInfo element) where TAttribute : Attribute
        {
            return GetCustomAttribute<TAttribute>(element) is not null;
        }

        /// <summary>
        /// Determines whether the specified inherit is defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified inherit is defined; otherwise, <c>false</c>.</returns>
        public static bool IsDefined<T, TAttribute>(bool inherit) where T : class where TAttribute : Attribute
        {
            return IsDefinedOnType<TAttribute>(typeof(T), inherit);
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified types].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="types">  The types.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on constructor] [the specified types]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnConstructor<T, TAttribute>(Type[] types, bool inherit) where T : class where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(types);
            return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified binding attribute].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="types">      The types.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on constructor] [the specified binding attribute]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnConstructor<T, TAttribute>(BindingFlags bindingAttr, Type[] types, bool inherit) where T : class where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, types);
            return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on constructor] [the specified binding attribute].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="binder">     The binder.</param>
        /// <param name="types">      The types.</param>
        /// <param name="modifiers">  The modifiers.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on constructor] [the specified binding attribute]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnConstructor<T, TAttribute>(BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers, bool inherit)
            where T : class
            where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, binder, types, modifiers);
            return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on constuctor] [the specified binding attribute].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="binder">     The binder.</param>
        /// <param name="convention"> The convention.</param>
        /// <param name="types">      The types.</param>
        /// <param name="modifiers">  The modifiers.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on constuctor] [the specified binding attribute]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnConstuctor<T, TAttribute>(BindingFlags bindingAttr, Binder? binder, CallingConventions convention, Type[] types, ParameterModifier[]? modifiers, bool inherit)
            where T : class
            where TAttribute : Attribute
        {
            ConstructorInfo? constructor = typeof(T).GetConstructor(bindingAttr, binder, convention, types, modifiers);
            return constructor is not null && constructor?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on default constructor] [the specified inherit].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on default constructor] [the specified inherit]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnDefaultConstructor<T, TAttribute>(bool inherit) where T : class where TAttribute : Attribute
        {
            return IsDefinedOnConstructor<T, TAttribute>(Type.EmptyTypes, inherit);
        }

        /// <summary>
        /// Determines whether [is defined on event] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on event] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnEvent<T, TAttribute>(string name, bool inherit) where T : class where TAttribute : Attribute
        {
            EventInfo? eventInfo = typeof(T).GetEvent(name);
            return eventInfo?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on event] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on event] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnEvent<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where T : class where TAttribute : Attribute
        {
            EventInfo? eventInfo = typeof(T).GetEvent(name, bindingAttr);
            return eventInfo?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on field] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on field] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnField<T, TAttribute>(string name, bool inherit) where T : class where TAttribute : Attribute
        {
            FieldInfo? field = typeof(T).GetField(name);
            return field?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on field] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on field] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnField<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where T : class where TAttribute : Attribute
        {
            FieldInfo? field = typeof(T).GetField(name, bindingAttr);
            return field?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on member] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">      The name.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="inherit">   if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on member] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnMember<T, TAttribute>(string name, MemberTypes memberType, bool inherit) where T : class where TAttribute : Attribute
        {
            MemberInfo? member = typeof(T).GetMember(name).FirstOrDefault(m => m.MemberType == memberType);
            return member?.GetCustomAttribute<TAttribute>(inherit) is not null;
        }

        /// <summary>
        /// Determines whether [is defined on member] [the specified name].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="memberType"> Type of the member.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on member] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnMember<T, TAttribute>(string name, MemberTypes memberType, BindingFlags bindingAttr, bool inherit) where T : class where TAttribute : Attribute
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">   The name.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on method] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnMethod<T, TAttribute>(string name, bool inherit) where T : class where TAttribute : Attribute
        {
            MethodInfo? method = null;

            try
            {
                method = typeof(T).GetMethod(name);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="name">       The name.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <param name="inherit">    if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on method] [the specified name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnMethod<T, TAttribute>(string name, BindingFlags bindingAttr, bool inherit) where T : class where TAttribute : Attribute
        {
            MethodInfo? method = null;

            try
            {
                method = typeof(T).GetMethod(name, bindingAttr);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="methodName">   Name of the method.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on parameter] [the specified method name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnParameter<T, TAttribute>(string methodName, string parameterName, bool inherit) where T : class where TAttribute : Attribute
        {
            ParameterInfo? parameter = null;

            try
            {
                parameter = typeof(T).GetMethod(methodName)?.GetParameters().FirstOrDefault(p => p.Name?.Equals(parameterName, StringComparison.Ordinal) == true);
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
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="methodName">   Name of the method.</param>
        /// <param name="bindingAttr">  The binding attribute.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="inherit">      if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on parameter] [the specified method name]; otherwise, <c>false</c>.</returns>
        public static bool IsDefinedOnParameter<T, TAttribute>(string methodName, BindingFlags bindingAttr, string parameterName, bool inherit) where T : class where TAttribute : Attribute
        {
            ParameterInfo? parameter = null;

            try
            {
                parameter = typeof(T).GetMethod(methodName, bindingAttr)?.GetParameters().FirstOrDefault(p => p.Name?.Equals(parameterName, StringComparison.Ordinal) == true);
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
        /// Determines whether [is defined on type] [the specified type].
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">   The type.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if [is defined on type] [the specified type]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static bool IsDefinedOnType<TAttribute>([AllowNull] Type type, bool inherit) where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            try
            {
                return type.GetTypeInfo().GetCustomAttribute<TAttribute>(inherit) is not null;
            }
            catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
            {
                if (ex is AmbiguousMatchException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    TAttribute? value = type.GetTypeInfo().GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
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
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this instance or <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance are of the same type and have identical field values;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj) && Equals(this, obj);
        }

        /// <summary>
        /// Equalses the specified x.
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
        /// <returns><see langword="true"/> if this instance is the default attribute for the class; otherwise, <see langword="false"/>.</returns>
        public override bool IsDefaultAttribute()
        {
            return IsDefault;
        }

        /// <summary>
        /// When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this instance of <see cref="T:System.Attribute"/>.</param>
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

        /// <summary>
        /// Tries the get custom attribute.
        /// </summary>
        /// <param name="type">   The type.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public bool TryGetCustomAttribute(Type type, bool inherit, out CustomAttribute? value)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            value = null;
            var result = IsDefinedOnType<CustomAttribute>(type, inherit);

            if (type.MemberType != MemberTypes.Constructor
                && type.MemberType != MemberTypes.Event
                && type.MemberType != MemberTypes.Field
                && type.MemberType != MemberTypes.Method
                && type.MemberType != MemberTypes.Property
                && type.MemberType != MemberTypes.TypeInfo)
            {
                throw new NotSupportedException($"Type {type.Name} with Member Type{type.MemberType} is not supported.");
            }

            if (result)
            {
                try
                {
                    value = type.GetTypeInfo().GetCustomAttribute<CustomAttribute>(inherit);
                }
                catch (Exception ex) when (ex is AmbiguousMatchException || ex is TypeLoadException)
                {
                    if (ex is AmbiguousMatchException)
                    {
                        Console.Error.WriteLine(ex.ToString());
                        value = type.GetTypeInfo().GetCustomAttributes<CustomAttribute>(inherit).FirstOrDefault();
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

        #endregion Public Methods
    }
}
