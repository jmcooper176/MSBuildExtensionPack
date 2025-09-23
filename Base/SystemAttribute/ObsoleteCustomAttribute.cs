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

namespace MSBuild.ExtensionPack.Base.SystemAttribute
{
    /// <summary>
    /// Implements an enhanced <see cref="ObsoleteAttribute"/><see cref="Attribute"/>.
    /// </summary>
    /// <seealso cref="MSBuild.ExtensionPack.Base.SystemAttribute.CustomAttribute"/>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ObsoleteCustomAttribute : CustomAttribute
    {
        #region Private Fields

        /// <summary>
        /// The base number
        /// </summary>
        private const int BASE_NUMBER = 1;

        /// <summary>
        /// The prefix
        /// </summary>
        private const string PREFIX = "OBSATTR";

        /// <summary>
        /// The counter
        /// </summary>
        private static int counter = BASE_NUMBER;

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="number"> The number.</param>
        /// <param name="isError">if set to <see langref="true"/> [is error].</param>
        protected ObsoleteCustomAttribute(string? message, int number, bool isError)
            : base(message, number)
        {
            // fields
            counter = Math.Max(number, BASE_NUMBER);

            // properties
            DiagnosticId = $"{PREFIX}{number:D7}";  // uses a different prefix
            SetIsErrorAttribute(isError);

            // overridden properties
            IsDefault = string.IsNullOrEmpty(message) && number == BASE_NUMBER;
            Message = message ?? $"Protected {this.GetType().Name} Constructor(string?, int, bool)";
            TypeId = this.GetType().GUID;
        }

        #endregion Protected Constructors

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating whether this instance is default.
        /// </summary>
        /// <value><see langref="true"/> if this instance is default; otherwise, <see langref="false"/>.</value>
        protected override bool IsDefault { get; }

        #endregion Protected Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        public ObsoleteCustomAttribute()
                    : this(null, BASE_NUMBER, false)
        {
            Message = $"Default Public {this.GetType().Name} Constructor()";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="isError">if set to <see langref="true"/> [is error].</param>
        public ObsoleteCustomAttribute(bool isError)
                    : this(null, BASE_NUMBER, isError)
        {
            Message = $"Public {this.GetType().Name} Constructor(bool)";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ObsoleteCustomAttribute(string? message)
            : this(message, counter++, false)
        {
            Message = message ?? $"Public {this.GetType().Name} Constructor(string?)";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isError">if set to <see langref="true"/> [is error].</param>
        public ObsoleteCustomAttribute(string? message, bool isError)
            : this(message, counter++, isError)
        {
            Message = message ?? $"Public {this.GetType().Name} Constructor(string?, bool)";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="message">  The message.</param>
        /// <param name="urlFormat">The URL format.</param>
        public ObsoleteCustomAttribute(string? message, string? urlFormat)
            : this(message, counter++, false)
        {
            Message = message ?? $"Public {this.GetType().Name} Constructor(string?, string?)";
            UrlFormat = urlFormat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObsoleteCustomAttribute"/> class.
        /// </summary>
        /// <param name="message">  The message.</param>
        /// <param name="urlFormat">The URL format.</param>
        /// <param name="isError">  if set to <see langref="true"/> [is error].</param>
        public ObsoleteCustomAttribute(string? message, string? urlFormat, bool isError)
            : this(message, counter++, isError)
        {
            Message = message ?? $"Public {this.GetType().Name} Constructor(string?, string?, bool)";
            UrlFormat = urlFormat;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance is error.
        /// </summary>
        /// <value><see langref="true"/> if this instance is error; otherwise, <see langref="false"/>.</value>
        public bool IsError { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public override string? Message { get; }

        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref="Attribute"/>.
        /// </summary>
        public override object TypeId { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Tries the get custom <see cref="Attribute"/>.
        /// </summary>
        /// <param name="type">   The <see cref="Type"/>.</param>
        /// <param name="inherit">if set to <see langref="true"/> [inherit].</param>
        /// <param name="value">  The value.</param>
        /// <returns></returns>
        public static bool TryGetCustomAttribute(Type type, bool inherit, out ObsoleteCustomAttribute? value)
        {
            value = null;
            var result = CustomAttribute.IsDefinedOnType<ObsoleteCustomAttribute>(type, inherit);

            if (result)
            {
                value = CustomAttribute.GetTypeInfo(type).GetCustomAttribute<ObsoleteCustomAttribute>(inherit);
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
        public override bool Equals(Attribute? x, Attribute? y)
        {
            ObsoleteCustomAttribute? xPrime = x as ObsoleteCustomAttribute;
            ObsoleteCustomAttribute? yPrime = y as ObsoleteCustomAttribute;

            if (!base.Equals(x, y))
            {
                return false;
            }
            else if ((xPrime?.IsError == true) ^ (yPrime?.IsError == true))
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
            return HashCode.Combine(base.GetHashCode(), IsError);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode([DisallowNull] ObsoleteCustomAttribute obj)
        {
            return HashCode.Combine(GetHashCode(obj as CustomAttribute), this.GetHashCode());
        }

        /// <summary>
        /// When overridden in a derived class, indicates whether the value of this instance is the default value for the derived class.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if this instance is the default <see cref="Attribute"/> for the class; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute() || IsDefault;
        }

        /// <summary>
        /// Determines whether [is error attribute] is set.
        /// </summary>
        /// <returns><see langref="true"/> if [is error attribute is set]; otherwise, <see langref="false"/>.</returns>
        public virtual bool IsErrorAttribute()
        {
            return IsError;
        }

        /// <summary>
        /// Sets the is error <see cref="Attribute"/>.
        /// </summary>
        /// <param name="value">if set to <see langref="true"/> [value].</param>
        public virtual void SetIsErrorAttribute(bool value)
        {
            IsError = value;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            if (this.IsDefaultAttribute())
            {
                return $"{this.GetType().FullName} : In Error => {IsError} {DiagnosticId} : {Message}";
            }
            else
            {
                return $"{this.GetType().Name} : In Error => {IsError} {DiagnosticId} {UrlFormat} : {Message}";
            }
        }

        #endregion Public Methods
    }
}
