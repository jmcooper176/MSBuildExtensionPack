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
namespace MSBuild.ExtensionPack.SdkProject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    internal partial class AssemblyInfoWrapper
    {
        /// <summary>
        /// The attribute dictionary
        /// </summary>
        private readonly Dictionary<string, int> attributeDictionary = [];

        /// <summary>
        /// Assembly attribute name regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> that will match the assembly attribute name.</returns>
        [GeneratedRegex(@"Assembly?\s*:?\s*(?<attributeName>\w+)\s*\(", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
        private static partial Regex AssemblyAttributeNameRegex();

        /// <summary>
        /// Attribute the boolean value regular expression.
        /// </summary>
        /// <returns>A <see cref="Regex"/> that will match the attribute <see cref="bool"/> value.</returns>
        [GeneratedRegex(@"\((?<attributeValue>(true|false))\)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
        private static partial Regex AttributeBooleanValueRegex();

        [GeneratedRegex(@"""(?<attributeValue>.*?)""", RegexOptions.CultureInvariant)]
        private static partial Regex AttributeStringValueRegex();

        private static string FormatAttributeValue(string attributeValue, string attributeFormat)
        {
            string value = attributeValue.StartsWith("\"", StringComparison.OrdinalIgnoreCase) && attributeValue.EndsWith("\"", StringComparison.OrdinalIgnoreCase)
                ? attributeValue.Trim('"')
                : attributeValue;
            return string.Format(CultureInfo.InvariantCulture, attributeFormat, value);
        }

        private static string GetGroupValue(MatchCollection matches, string groupName)
        {
            return matches.Select(m => m.Groups).Where(g => g[groupName].Success).Select(g => g[groupName].Value).FirstOrDefault() ?? string.Empty;
        }

        [GeneratedRegex(@".*?\*/", RegexOptions.CultureInvariant)]
        private static partial Regex MultilineCSharpCommentEndRegex();

        [GeneratedRegex(@"\s*/\*^\*", RegexOptions.CultureInvariant)]
        private static partial Regex MultilineCSharpCommentStartRegex();

        private static string ParentheticalAttributeValue(string attributeValue)
        {
            return FormatAttributeValue(attributeValue, "({0})");
        }

        private static string QuoteAttributeValue(string attributeValue)
        {
            return FormatAttributeValue(attributeValue, "\"{0}\"");
        }

        [GeneratedRegex(@"(?m:^(\s*//.*)$)", RegexOptions.CultureInvariant)]
        private static partial Regex SingleLineCSharpCommentLineRegex();

        [GeneratedRegex(@"^(\s*'|')", RegexOptions.CultureInvariant)]
        private static partial Regex SingleLineVisualBasicCommentLineRegex();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoWrapper"/> class.
        /// </summary>
        /// <param name="fileName">Specifies the file name path to the project file to process.</param>
        /// <remarks>
        /// The ^\* is so the regex works with J# files that use /** to indicate the actual attribute lines. This does mean that
        /// lines like /** in C# will get treated as valid lines, but that's a real borderline case.
        /// </remarks>
        public AssemblyInfoWrapper(string fileName)
        {
            FileLines = [];
            FileName = new FileInfo(fileName);

            if (!FileName.Exists)
            {
                throw new FileNotFoundException("The specified AssemblyInfo file could not be found", FileName.FullName);
            }

            using StreamReader reader = FileName.OpenText();
            int lineNumber = 0;
            string? input;
            bool skipLine = false;

            while ((input = reader.ReadLine()) is not null)
            {
                FileLines.Add(input);

                // Skip single comment lines
                if (SingleLineCSharpCommentLineRegex().IsMatch(input) || SingleLineVisualBasicCommentLineRegex().IsMatch(input))
                {
                    lineNumber++;
                    continue;
                }

                // Skip multi-line C# comments
                if (MultilineCSharpCommentStartRegex().IsMatch(input))
                {
                    lineNumber++;
                    skipLine = true;
                    continue;
                }

                // Stop skipping when we're at the end of a C# multi-line comment
                if (MultilineCSharpCommentEndRegex().IsMatch(input) && skipLine)
                {
                    lineNumber++;
                    skipLine = false;
                    continue;
                }

                // If we're in the middle of a multi-line comment, keep going
                if (skipLine)
                {
                    lineNumber++;
                    continue;
                }

                // Check to see if the current line is an attribute on the assembly info. If so we need to keep the line number in
                // our dictionary so we can go back later and get it when this class is accessed through its indexer.
                if (AssemblyAttributeNameRegex().IsMatch(input))
                {
                    _ = attributeDictionary.TryAdd(GetGroupValue(AssemblyAttributeNameRegex().Matches(input), "attributeName"), lineNumber);
                }

                // no attributes on this line so go to next
                lineNumber++;
            }
        }

        public string this[string attribute]
        {
            get
            {
                if (!attributeDictionary.TryGetValue(attribute, out int valueGet))
                {
                    return string.Empty;
                }

                // Try to match string properties first
                if (AttributeStringValueRegex().IsMatch(FileLines[valueGet]))
                {
                    MatchCollection matches = AttributeStringValueRegex().Matches(FileLines[valueGet]);
                    return GetGroupValue(matches, "attributeValue");
                }

                // If that fails, try to match a boolean value
                if (AttributeBooleanValueRegex().IsMatch(FileLines[valueGet]))
                {
                    MatchCollection matches = AttributeBooleanValueRegex().Matches(FileLines[valueGet]);
                    return GetGroupValue(matches, "attributeValue");
                }

                return string.Empty;
            }

            set
            {
                // The set case requires fancy footwork. In this case we actually replace the attribute value in the string using a
                // regex to the value that was passed in.
                if (!attributeDictionary.TryGetValue(attribute, out int valueSet))
                {
                    throw new ArgumentOutOfRangeException(nameof(attribute), attribute, $"Attribute '{attribute}' is not an attribute in the specified AssemblyInfo.cs file '{FileName.FullName}'");
                }

                // Try setting it as a string property first
                if (AttributeStringValueRegex().IsMatch(FileLines[valueSet]))
                {
                    MatchCollection matches = AttributeStringValueRegex().Matches(FileLines[valueSet]);
                    FileLines[valueSet] = AttributeStringValueRegex().Replace(FileLines[valueSet], QuoteAttributeValue(value));
                }
                else if (AttributeBooleanValueRegex().IsMatch(FileLines[valueSet]))
                {
                    MatchCollection matches = AttributeBooleanValueRegex().Matches(FileLines[valueSet]);
                    FileLines[valueSet] = AttributeBooleanValueRegex().Replace(FileLines[valueSet], ParentheticalAttributeValue(value));
                }
            }
        }

        public IList<string> FileLines { get; }

        public FileInfo FileName { get; }

        public static void Write<TLine>(TextWriter streamWriter, IEnumerable<TLine> input) where TLine : class, IEnumerable, ICloneable, IComparable, IConvertible
        {
            input.ToList().ForEach(streamWriter.WriteLine);
        }

        public static void Write<TElement>(TextWriter streamWriter, IList<TElement> input) where TElement : struct, IComparable, IConvertible, IFormattable
        {
            input.ToList().ForEach(e => streamWriter.WriteLine(e));
        }
    }
}
