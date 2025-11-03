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
namespace MSBuild.ExtensionPack.ErrorMessage.Utility
{
    using System;
    using System.Globalization;

    public static class Environment
    {
        public static bool? TestEnvironmentValue(string variable, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            string? value = System.Environment.GetEnvironmentVariable(variable, target);

            return string.IsNullOrEmpty(value)
                ? null
                : int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out int nonZero)
                    ? nonZero >= 1 || nonZero < 0
                    : (bool.TryParse(value, out bool result) || (result = Convert.ToBoolean(value, CultureInfo.CurrentCulture))) && result;
        }
    }
}
