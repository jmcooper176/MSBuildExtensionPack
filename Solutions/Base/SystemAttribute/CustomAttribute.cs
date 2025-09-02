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
    public class CustomAttribute : Attribute
    {
        #region Private Fields

        private const int BASE_NUMBER = 1;
        private const string PREFIX = "CSTATTR";
        private static int counter = BASE_NUMBER;
        private bool isDefault;

        #endregion Private Fields

        #region Protected Constructors

        protected CustomAttribute(string? message, int number)
            : base()
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number, nameof(number));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 0xFFFFFFF, nameof(number));

            counter = number;
            isDefault = string.IsNullOrEmpty(message) && number == BASE_NUMBER;
            DiagnosticId = $"{PREFIX}{number:D7}";
            Message = message ?? "Default Attribute Constructor";
        }

        #endregion Protected Constructors

        #region Public Constructors

        public CustomAttribute()
                    : this(null, BASE_NUMBER)
        {
        }

        public CustomAttribute(string? message)
            : this(message, "https://github.com/jmcooper176")
        {
        }

        public CustomAttribute(string? message, string? urlFormat)
            : this(message, counter++)
        {
            UrlFormat = urlFormat;
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual string? DiagnosticId { get; set; }

        public virtual string? Message { get; }

        public override object TypeId => Guid.NewGuid();

        public virtual string? UrlFormat { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Attribute? GetCustomAttribute([AllowNull] ParameterInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(attributeType);

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            throw new NotImplementedException();
        }

        public static Attribute? GetCustomAttribute([AllowNull] MemberInfo element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(attributeType);

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            if (element.MemberType == MemberTypes.Constructor)
            {
                // continue
            }
            else if (element.MemberType == MemberTypes.Method)
            {
                // continue
            }
            else if (element.MemberType == MemberTypes.Property)
            {
                // continue
            }
            else if (element.MemberType == MemberTypes.Event)
            {
                // continue
            }
            else if (element.MemberType == MemberTypes.TypeInfo)
            {
                // continue
            }
            else if (element.MemberType == MemberTypes.Field)
            {
                // continue
            }
            else
            {
                throw new NotSupportedException();
            }

            throw new NotImplementedException();
        }

        public static Attribute? GetCustomAttribute([AllowNull] Assembly element, [AllowNull] Type attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(attributeType);

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            throw new NotImplementedException();
        }

        public static Attribute? GetCustomAttribute(Module? element, Type? attributeType, bool inherit)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(attributeType);

            if (!attributeType.IsAssignableTo(typeof(Attribute)))
            {
                throw new ArgumentException($"Parameter '{nameof(attributeType)}' is not derived from 'Attribute'", nameof(attributeType));
            }

            throw new NotImplementedException();
        }

        public static Attribute? GetCustomAttribute(Module? element, Type? attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        public static Attribute? GetCustomAttribute(MemberInfo? element, Type? attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        public static Attribute? GetCustomAttribute(Assembly? element, Type? attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        public static Attribute? GetCustomAttribute(ParameterInfo? element, Type? attributeType)
        {
            return GetCustomAttribute(element, attributeType, inherit: false);
        }

        public virtual bool Equals(CustomAttribute? x, CustomAttribute? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x is null ^ y is null)
            {
                return false;
            }
            else if (!string.Equals(x?.DiagnosticId, y?.DiagnosticId, StringComparison.Ordinal))
            {
                return false;
            }
            else if (!string.Equals(x?.Message, y?.Message, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (!string.Equals(x?.UrlFormat, y?.UrlFormat, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if ((x?.IsDefaultAttribute() == true) ^ (y?.IsDefaultAttribute() == true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.DiagnosticId, this.TypeId);
        }

        public override bool IsDefaultAttribute()
        {
            return isDefault;
        }

        public override bool Match(object? obj)
        {
            return base.Match(obj);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion Public Methods
    }
}
