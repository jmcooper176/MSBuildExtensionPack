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
using System.Reflection;

namespace MSBuild.ExtensionPack.Base.SystemAttribute
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ObsoleteCustomAttribute : CustomAttribute
    {
        #region Private Fields

        private const int BASE_NUMBER = 1;
        private const string PREFIX = "OBSATTR";
        private static int counter = BASE_NUMBER;

        #endregion Private Fields

        #region Protected Constructors

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
            Message = message ?? $"Default {this.GetType().Name} Constructor";
            TypeId = this.GetType().GUID;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected override bool IsDefault { get; }

        #endregion Protected Properties

        #region Public Constructors

        public ObsoleteCustomAttribute()
                    : this(null, BASE_NUMBER, false)
        {
        }

        public ObsoleteCustomAttribute(bool isError)
                    : this(null, BASE_NUMBER, isError)
        {
        }

        public ObsoleteCustomAttribute(string? message)
            : this(message, counter++, false)
        {
        }

        public ObsoleteCustomAttribute(string? message, bool isError)
            : this(message, counter++, isError)
        {
        }

        public ObsoleteCustomAttribute(string? message, string? urlFormat)
            : this(message, counter++, false)
        {
            UrlFormat = urlFormat;
        }

        public ObsoleteCustomAttribute(string? message, string? urlFormat, bool isError)
            : this(message, counter++, isError)
        {
            UrlFormat = urlFormat;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsError { get; private set; }

        public override string? Message { get; }
        public override object TypeId { get; }

        #endregion Public Properties

        #region Public Methods

        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element)
        {
            return GetCustomAttributes<ObsoleteCustomAttribute>(element, false);
        }

        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] MemberInfo element)
        {
            return GetCustomAttributes<ObsoleteCustomAttribute>(element, false);
        }

        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Assembly element)
        {
            return GetCustomAttributes<ObsoleteCustomAttribute>(element, false);
        }

        public new static IEnumerable<Attribute> GetCustomAttributes([AllowNull] Module element, bool inherit)
        {
            return GetCustomAttributes<ObsoleteCustomAttribute>(element, inherit);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj) && Equals(this, obj);
        }

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

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IsError);
        }

        public int GetHashCode([DisallowNull] ObsoleteCustomAttribute obj)
        {
            return HashCode.Combine(GetHashCode(obj as CustomAttribute), this.GetHashCode());
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute() || isDefault;
        }

        public virtual bool IsErrorAttribute()
        {
            return IsError;
        }

        public virtual void SetIsErrorAttribute(bool value)
        {
            IsError = value;
        }

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

        public bool TryGetCustomAttribute(Type type, bool inherit, out ObsoleteCustomAttribute? value)
        {
            value = null;
            var result = IsDefinedOnType<ObsoleteCustomAttribute>(type, inherit);

            if (result)
            {
                value = type.GetTypeInfo().GetCustomAttribute<ObsoleteCustomAttribute>(inherit);
            }

            return result;
        }

        #endregion Public Methods
    }
}
