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
namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Linq;

    public static class ValidInstance
    {
        public static bool AllNotNull(params object?[] objects)
        {
            return objects.All(o => o is not null);
        }

        public static bool AllNull(params object?[] objects)
        {
            return objects.All(o => o is null);
        }

        public static bool AnyNotNull(params object?[] objects)
        {
            return objects.Any(o => o is not null);
        }

        public static bool AnyNull(params object?[] objects)
        {
            return objects.Any(o => o is null);
        }

        public static bool IsComInstance(this object? instance)
        {
            return instance?.GetType().IsCOMObject == true;
        }

        public static bool IsInstanceOfCom(this object? instance, Type? type)
        {
            return AllNotNull(instance, type) && instance.IsComInstance() && type!.IsInstanceOfType(instance);
        }

        public static bool IsInstanceOfCom<TCom>(this object? instance) where TCom : class
        {
            return instance.IsInstanceOfCom(typeof(TCom));
        }

        public static bool IsValidDirectory(string? path)
        {
            const int MAX_COMPONENT = 64;
            const int MAX_PATH = 255;

            if (string.IsNullOrWhiteSpace(path) || path.Length > MAX_PATH)
            {
                return false;
            }

            var components = path?.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries) ?? [];

            return components.All(c => c.Length <= MAX_COMPONENT) && components.All(c => c.IndexOfAny(Path.GetInvalidPathChars()) < 0);
        }

        public static bool IsValidFileName(string? name)
        {
            const int MAX_PATH = 255;
            const int MAX_FILENAME = 250;
            const int MAX_EXTENSION = 64;

            if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_PATH)
            {
                return false;
            }

            var fileName = Path.GetFileName(name) ?? string.Empty;
            var extension = Path.GetExtension(name) ?? string.Empty;

            return fileName.Length <= MAX_FILENAME && extension.Length <= MAX_EXTENSION
                && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        public static bool IsValidMsiIdentifier(string? id)
        {
            return !string.IsNullOrWhiteSpace(id) && id.Length <= 72
                && ((id.StartsWith('_') || char.IsAsciiLetter(id[0]))
                    && id.Skip(1).All(c => char.IsAsciiLetterOrDigit(c) || c == '_' || c == '.'));
        }

        public static bool IsValidPath(string? path)
        {
            const int MAX_PATH = 255;

            if (string.IsNullOrWhiteSpace(path) || path.Length > MAX_PATH)
            {
                return false;
            }

            var directory = Path.GetDirectoryName(path) ?? string.Empty;
            var fileName = Path.GetFileName(path) ?? string.Empty;

            return IsValidDirectory(directory) && IsValidFileName(fileName);
        }

        public static bool IsValidPrivateMsiIdentifier(string? id)
        {
            return IsValidMsiIdentifier(id) && !IsValidPublicMsiIdentifier(id);
        }

        public static bool IsValidPublicMsiIdentifier(string? id)
        {
            return !string.IsNullOrWhiteSpace(id) && id.Length <= 72
                && (char.IsAsciiLetterUpper(id[0])
                    && id.Skip(1).All(c => char.IsAsciiLetterUpper(c) || char.IsAsciiDigit(c) || c == '_' || c == '.'));
        }
    }
}
