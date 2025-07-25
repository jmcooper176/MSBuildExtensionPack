//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfoWrapper.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
// This task is based on the AssemblyInfo task written by Neil Enns (http://code.msdn.microsoft.com/AssemblyInfoTaskvers). It is used here with permission.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Framework.AssemblyInfo.AssemblyInfo
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    internal class AssemblyInfoWrapper
    {
        private readonly Regex attributeBooleanValuePattern = new Regex(@"\((?<attributeValue>([tT]rue|[fF]alse))\)", RegexOptions.Compiled);
        private readonly Dictionary<string, int> attributeIndex = new Dictionary<string, int>();
        private readonly Regex attributeNamePattern = new Regex(@"[aA]ssembly?\s*:?\s*(?<attributeName>\w+)\s*\(", RegexOptions.Compiled);
        private readonly Regex attributeStringValuePattern = new Regex(@"""(?<attributeValue>.*?)""", RegexOptions.Compiled);
        private readonly Regex multilineCSharpCommentEndPattern = new Regex(@".*?\*/", RegexOptions.Compiled);
        private readonly Regex multilineCSharpCommentStartPattern = new Regex(@"\s*/\*^\*", RegexOptions.Compiled);
        private readonly List<string> rawFileLines = new List<string>();
        private readonly Regex singleLineCSharpCommentPattern = new Regex(@"(?m:^(\s*//.*)$)", RegexOptions.Compiled);
        private readonly Regex singleLineVbCommentPattern = new Regex(@"^(\s*'|')", RegexOptions.Compiled);

        //// The ^\* is so the regex works with J# files that use /** to indicate the actual attribute lines.
        //// This does mean that lines like /** in C# will get treated as valid lines, but that's a real borderline case.
        public AssemblyInfoWrapper(string fileName)
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                int lineNumber = 0;
                string input;
                bool skipLine = false;

                while ((input = reader.ReadLine()) != null)
                {
                    rawFileLines.Add(input);

                    // Skip single comment lines
                    if (singleLineCSharpCommentPattern.IsMatch(input) || singleLineVbCommentPattern.IsMatch(input))
                    {
                        lineNumber++;
                        continue;
                    }

                    // Skip multi-line C# comments
                    if (multilineCSharpCommentStartPattern.IsMatch(input))
                    {
                        lineNumber++;
                        skipLine = true;
                        continue;
                    }

                    // Stop skipping when we're at the end of a C# multiline comment
                    if (multilineCSharpCommentEndPattern.IsMatch(input) && skipLine)
                    {
                        lineNumber++;
                        skipLine = false;
                        continue;
                    }

                    // If we're in the middle of a multiline comment, keep going
                    if (skipLine)
                    {
                        lineNumber++;
                        continue;
                    }

                    // Check to see if the current line is an attribute on the assembly info.
                    // If so we need to keep the line number in our dictionary so we can go
                    // back later and get it when this class is accessed through its indexer.
                    var matches = attributeNamePattern.Matches(input);
                    if (matches.Count > 0)
                    {
                        if (attributeIndex.ContainsKey(matches[0].Groups["attributeName"].Value) == false)
                        {
                            attributeIndex.Add(matches[0].Groups["attributeName"].Value, lineNumber);
                        }
                    }

                    lineNumber++;
                }
            }
        }

        public string this[string attribute]
        {
            get
            {
                if (!attributeIndex.ContainsKey(attribute))
                {
                    return null;
                }

                // Try to match string properties first
                MatchCollection matches = attributeStringValuePattern.Matches(rawFileLines[attributeIndex[attribute]]);
                if (matches.Count > 0)
                {
                    return matches[0].Groups["attributeValue"].Value;
                }

                // If that fails, try to match a boolean value
                matches = attributeBooleanValuePattern.Matches(rawFileLines[attributeIndex[attribute]]);
                if (matches.Count > 0)
                {
                    return matches[0].Groups["attributeValue"].Value;
                }

                return null;
            }

            set
            {
                // The set case requires fancy footwork. In this case we actually replace the attribute
                // value in the string using a regex to the value that was passed in.
                if (!attributeIndex.ContainsKey(attribute))
                {
                    throw new ArgumentOutOfRangeException(nameof(attribute), string.Format(CultureInfo.CurrentCulture, "{0} is not an attribute in the specified AssemblyInfo.cs file", attribute));
                }

                // Try setting it as a string property first
                MatchCollection matches = attributeStringValuePattern.Matches(rawFileLines[attributeIndex[attribute]]);
                if (matches.Count > 0)
                {
                    rawFileLines[attributeIndex[attribute]] = attributeStringValuePattern.Replace(rawFileLines[attributeIndex[attribute]], "\"" + value + "\"");
                    return;
                }

                // If that fails try setting it as a boolean property
                matches = attributeBooleanValuePattern.Matches(rawFileLines[attributeIndex[attribute]]);
                if (matches.Count > 0)
                {
                    rawFileLines[attributeIndex[attribute]] = attributeBooleanValuePattern.Replace(rawFileLines[attributeIndex[attribute]], "(" + value + ")");
                }
            }
        }

        public void Write(TextWriter streamWriter)
        {
            foreach (string line in rawFileLines)
            {
                streamWriter.WriteLine(line);
            }
        }
    }
}