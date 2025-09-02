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

namespace MSBuild.ExtensionPack.Base.SystemAttribute
{
    public class ObsoleteCustomAttribute : CustomAttribute
    {
        #region Public Properties

        public bool IsError { get; set; }

        public override object TypeId => this.GetType().GUID;

        #endregion Public Properties

        #region Public Methods

        public override bool Equals(ObsoleteCustomAttribute? x, ObsoleteCustomAttribute? y)
        {
            if (!base.Equals(x, y))
            {
                return false;
            }
            else if ((x?.IsError == true) ^ (y?.IsError == true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override ObsoleteAttribute? GetCustomAttributeForType<T>() where T : class
        {
            return base.GetCustomAttributeForType<T>() as ObsoleteAttribute;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), TypeId);
        }

        public int GetHashCode([DisallowNull] ObsoleteAttribute obj)
        {
            return HashCode.Combine(obj.GetHashCode(), this.GetHashCode());
        }

        public string? GetObsoleteAttributeDiagnosticId<T>(ObsoleteAttribute? instance, bool inherit = false) where T : class
        {
            return IsCustomAttributePresent<T>(instance, inherit) ? instance?.DiagnosticId : string.Empty;
        }

        public string? GetObsoleteAttributeDocumentionUrl<T>(ObsoleteAttribute? instance, bool inherit = false) where T : class
        {
            return IsCustomAttributePresent<T>(instance, inherit) ? instance?.UrlFormat : string.Empty;
        }

        public bool GetObsoleteAttributeIsError<T>(ObsoleteAttribute? instance, bool inherit = false) where T : class
        {
            return IsCustomAttributePresent<T>(instance, inherit) && (instance?.IsError == true);
        }

        public string? GetObsoleteAttributeWorkaround<T>(ObsoleteAttribute? instance, bool inherit = false) where T : class
        {
            return IsCustomAttributePresent<T>(instance, inherit) ? instance?.Message : string.Empty;
        }

        public override bool IsCustomAttributePresent<T>(ObsoleteAttribute? target, bool inherit = false) where T : class
        {
            return GetCustomAttributesForType<T>(inherit).Any(a => Equals(a, target));
        }

        #endregion Public Methods
    }
}
