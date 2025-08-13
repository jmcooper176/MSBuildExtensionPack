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

using Microsoft.Build.Framework;

using System.CommandLine.Parsing;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace MSBuild.ExtensionPack.Base.Extension
{
    public class CommandLineListBuilder
    {
        #region Private Fields

        /// <summary>
        /// Constant <see cref="Regex"/> to use for detecting strings containing characters that do not require quoting when
        /// escaping of hyphens is supposed to take place.
        /// </summary>
        private const string ALLOWED_UNQUOTED_NO_QUOTE_HYPHEN_REGEX =
                        "^"                             // Beginning of line
                       + @"[a-z\\/:0-9\._\-+=]*"        //  Allow hyphen to be unquoted
                       + "$";

        /// <summary>
        /// Constant <see cref="Regex"/> to use for detecting strings containing characters that do not require quoting when
        /// escaping of hyphens is supposed to take place.
        /// </summary>
        private const string ALLOWED_UNQUOTED_QUOTE_HYPHEN_REGEX =
                         "^"                             // Beginning of line
                       + @"[a-z\\/:0-9\._+=]*"           // Quote hyphen
                       + "$";

        private const string DEFINITELY_NEED_QUOTES_NO_QUOTE_HYPHEN_REGEX = @"[|><\s,;\-""]+";

        /// <summary>
        /// Constant <see cref="Regex"/> to use for detecting strings containing one or more characters that require quoting when
        /// </summary>
        private const string DEFINITELY_NEED_QUOTES_QUOTE_HYPHEN_REGEX = @"[|><\s,;""]+";

        #endregion Private Fields

        #region Private Properties

        /// <summary>
        /// Gets a <see cref="Regex"/> value indicating a parameter or file name that can safely be unquoted.
        /// </summary>
        private Regex AllowedUnquoted => new(this.QuoteHyphens ? ALLOWED_UNQUOTED_QUOTE_HYPHEN_REGEX : ALLOWED_UNQUOTED_NO_QUOTE_HYPHEN_REGEX, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>
        /// Gets a <see cref="Regex"/> value indicating a parameter or file name that must be quoted.
        /// </summary>
        private Regex DefinitelyNeedQuotes => new(this.QuoteHyphens ? DEFINITELY_NEED_QUOTES_NO_QUOTE_HYPHEN_REGEX : DEFINITELY_NEED_QUOTES_QUOTE_HYPHEN_REGEX, RegexOptions.CultureInvariant);

        #endregion Private Properties

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating the command line <see cref="IList{T}"/>.
        /// </summary>
        protected IList<string> CommandList { get; }

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Append <paramref name="textToAppend"/> to <paramref name="buffer"/> unquoted if <paramref name="textToAppend"/> is not
        /// <see langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">      Specifies the accumulator for <paramref name="textToAppend"/>.</param>
        /// <param name="textToAppend">Specifies the text to append.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        protected static void AppendTextUnquoted(StringBuilder? buffer, string? textToAppend)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (!string.IsNullOrEmpty(textToAppend))
            {
                buffer.Append(textToAppend);
            }
        }

        /// <summary>
        /// Append <paramref name="fileName"/> with quoting, if necessary, to <see cref="CommandList"/> if <paramref
        /// name="fileName"/> is not <see langref="null"/> or empty.
        /// </summary>
        /// <param name="fileName">Specifies the file name to be append.</param>
        protected void AppendFileNameWithQuoting(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                ItemBuffer.Clear();
                AppendTextWithQuoting(ItemBuffer, NormalizeFileName(fileName));
                CommandList.Add(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// Append <paramref name="fileName"/> with quoting, if necessary, to <see cref="CommandList"/> if <paramref
        /// name="fileName"/> is not <see langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">  Specifies the accumulator for <paramref name="fileName"/>.</param>
        /// <param name="fileName">Specifies the file name to be append.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        protected void AppendFileNameWithQuoting(StringBuilder? buffer, string? fileName)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (!string.IsNullOrEmpty(fileName))
            {
                // Don't let injection attackers escape from our quotes by sticking in their own quotes. Quotes are illegal.
                ThrowOnEmbeddedDoubleQuote(nameof(fileName), fileName);
                AppendTextWithQuoting(buffer, NormalizeFileName(fileName));
            }
        }

        /// <summary>
        /// Append quoted text <paramref name="unquotedTextToAppend"/> to <paramref name="buffer"/> if <paramref
        /// name="unquotedTextToAppend"/> is not <see langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">              Specifies the accumulator for <paramref name="unquotedTextToAppend"/>.</param>
        /// <param name="unquotedTextToAppend">Specifies the unquoted text to be appended after quoting.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        protected void AppendQuotedTextToBuffer(StringBuilder? buffer, string? unquotedTextToAppend)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (!string.IsNullOrEmpty(unquotedTextToAppend))
            {
                if (unquotedTextToAppend.Any(c => c == '"'))
                {
                    unquotedTextToAppend = unquotedTextToAppend.Replace("\\\"", "\\\\\"").Replace("\"", "\\\"");
                }

                bool addQuotes = IsQuotingRequired(unquotedTextToAppend);

                // Be careful any trailing slash doesn't escape the quote we're about to add
                if (addQuotes && unquotedTextToAppend.EndsWith('\\'))
                {
                    buffer.Append('"').Append(unquotedTextToAppend).Append('\\').Append('"');
                }
                else if (addQuotes)
                {
                    buffer.Append('"').Append(unquotedTextToAppend).Append('"');
                }
                else
                {
                    buffer.Append(unquotedTextToAppend);
                }
            }
        }

        /// <summary>
        /// Append quoted text <paramref name="unquotedTextToAppend"/> to <paramref name="list"/> if <paramref
        /// name="unquotedTextToAppend"/> is not <see langref="null"/> or empty.
        /// </summary>
        /// <param name="list">                Specifies the <see cref="IList{T}"/> accumulator for <paramref name="unquotedTextToAppend"/>.</param>
        /// <param name="unquotedTextToAppend">Specifies the unquoted text to be appended after quoting.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="list"/> is <see langref="null"/>.</exception>
        protected void AppendQuotedTextToList(IList<string> list, string? unquotedTextToAppend)
        {
            ArgumentNullException.ThrowIfNull(list, nameof(list));

            if (!string.IsNullOrEmpty(unquotedTextToAppend))
            {
                ItemBuffer.Clear();
                AppendQuotedTextToBuffer(ItemBuffer, unquotedTextToAppend);
                CommandList.Add(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// Appends a space if the last element of <see cref="CommandList"/> does not end in space, or <see
        /// cref="Environment.NewLine"/> to <paramref name="buffer"/> which usually points to the <see cref="ItemBuffer"/>.
        /// </summary>
        /// <param name="buffer">Specifies the item buffer accumulator.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws if <paramref name="buffer"/> or <see cref="CommandList"/> is <see langref="null"/>.
        /// </exception>
        protected void AppendSpaceIfNotEmpty(StringBuilder buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            ArgumentNullException.ThrowIfNull(CommandList);

            const char SPACE = ' ';

            if (Length > 0)
            {
                buffer.Clear();

                if (this.UseNewLine)
                {
                    buffer.Append(Environment.NewLine);
                }
                else if (!CommandList[Length - 1].EndsWith(SPACE))
                {
                    buffer.Append(SPACE);
                }
            }
        }

        /// <summary>
        /// Append <paramref name="switchName"/> to <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">    Specifies the accumulator for <paramref name="switchName"/>.</param>
        /// <param name="switchName">Specifies the command line switch to append to <paramref name="buffer"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws if either <paramref name="buffer"/> is <see langref="null"/> or <paramref name="switchName"/> is <see
        /// langref="null"/>, empty, or all whitespace.
        /// </exception>
        protected void AppendSwitch(StringBuilder? buffer, string? switchName)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(switchName, nameof(switchName));

            AppendSpaceIfNotEmpty(buffer);
            AppendTextUnquoted(buffer, switchName);
        }

        /// <summary>
        /// Append text <paramref name="textToAppend"/> to <see cref="CommandList"/> if <paramref name="textToAppend"/> is not <see
        /// langref="null"/> or empty.
        /// </summary>
        /// <param name="textToAppend">Specifies the text to append as an item to <see cref="CommandList"/>.</param>
        /// <exception cref="ArgumentNullException">Throws if <see cref="CommandList"/> is <see langref="null"/>.</exception>
        protected void AppendTextWithQuoting(string? textToAppend) => AppendQuotedTextToList(CommandList, textToAppend);

        /// <summary>
        /// Append text <paramref name="textToAppend"/> to <paramref name="buffer"/> if <paramref name="textToAppend"/> is not <see
        /// langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">      Specifies the accumulator for <paramref name="textToAppend"/>.</param>
        /// <param name="textToAppend">Specifies the text to append as an item to <see cref="CommandList"/>.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        protected void AppendTextWithQuoting(StringBuilder buffer, string? textToAppend) => AppendQuotedTextToBuffer(buffer, textToAppend);

        /// <summary>
        /// Tests <paramref name="parameter"/> for whether quoting is required on the command line.
        /// </summary>
        /// <param name="parameter">Specifies the parameter string to scan.</param>
        /// <returns><see langref="true"/> if <paramref name="parameter"/> requires quoting; otherwise, <see langref="false"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Throws if either <see cref="AllowedUnquoted"/> is not a match or <see cref="DefinitelyNeedQuotes"/> is not a match or
        /// both are not a match.
        /// </exception>
        protected virtual bool IsQuotingRequired(string? parameter)
        {
            // CROSS-PARAMETER CODE INJECTION
            //
            // If parameter has embedded whitespace, then a possible attack could, for example, be like:
            //
            // <Win32Icon> MyFile.ico /out:c:\windows\system32\notepad.exe </Win32Icon>
            //
            // <Csc
            //
            // Win32Icon = "$(Win32Icon)"... />
            //
            // Since inner text of <Win32Icon> is a command line for CSC.EXE, without quoting, the project could, for example,
            // overwrite the system notepad.exe. THEREFORE, spaces in parameters require quoting such parameters.

            if (string.IsNullOrEmpty(parameter))
            {
                return false;
            }
            else if (DefinitelyNeedQuotes.IsMatch(parameter))
            {
                return true;
            }
            else if (AllowedUnquoted.IsMatch(parameter))
            {
                return false;
            }
            else
            {
                throw new InvalidOperationException("AllowedUnquoted and DefinitelyNeedQuote are mutually exclusive.");
            }
        }

        /// <summary>
        /// Tests <paramref name="parameter"/> of <paramref name="switchName"/> for embedded double quotes.
        /// </summary>
        /// <param name="switchName">Specifies the command line switch under test.</param>
        /// <param name="parameter"> Specifies the command line switch parameter to scan.</param>
        /// <exception cref="SecurityException">Throws if <paramref name="parameter"/> contains a double quote.</exception>
        protected virtual void ThrowOnEmbeddedDoubleQuote(string? switchName, string? parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                if (string.IsNullOrEmpty(switchName))
                {
                    if (parameter.Contains('"'))
                    {
                        throw new SecurityException($"Quotes are not allowed with no switch name on task parameter '{parameter}'.");
                    }
                }
                else
                {
                    if (parameter.Contains('"'))
                    {
                        throw new SecurityException($"Quotes are not allowed with switch name '{switchName}' on the task parameter '{parameter}'.");
                    }
                }
            }
        }

        #endregion Protected Methods

        #region Internal Properties

        /// <summary>
        /// Gets a value indicating the line buffer to use for assembling individual <see cref="CommandList"/> string elements.
        /// </summary>
        internal StringBuilder ItemBuffer { get; }

        /// <summary>
        /// Gets a value indicating whether to quote hyphens on the elements of <see cref="CommandList"/>.
        /// </summary>
        internal bool QuoteHyphens { get; }

        /// <summary>
        /// Gets a value indicating whether to use <see cref="Environment.NewLine"/> instead of the space character as whitespace in
        /// <see cref="CommandList"/> elements.
        /// </summary>
        internal bool UseNewLine { get; }

        #endregion Internal Properties

        #region Internal Methods

        /// <summary>
        /// Tests whether <paramref name="buffer"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">Specifies the <see cref="StringBuilder"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="buffer"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(StringBuilder? buffer)
        {
            return buffer is null || buffer.Length < 1;
        }

        /// <summary>
        /// Tests whether <paramref name="item"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="item">Specifies the <see cref="ITaskItem"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="item"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(ITaskItem? item)
        {
            return item is null || string.IsNullOrEmpty(item?.ItemSpec);
        }

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <typeparam name="T">Specifies the element type of <paramref name="items"/>.</typeparam>
        /// <param name="items">Specifies the <see cref="IEnumerable{T}"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty<T>(IEnumerable<T> items)
        {
            return items is null || !items.Any();
        }

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="items">Specifies the <see cref="IEnumerable{T}"/> of element type <see cref="string"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(IEnumerable<string> items)
        {
            return IsNullOrEmpty<string>(items);
        }

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="items">Specifies the <see cref="IEnumerable{T}"/> of element type <see cref="ITaskItem"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(IEnumerable<ITaskItem> items)
        {
            return IsNullOrEmpty<ITaskItem>(items);
        }

        /// <summary>
        /// Normalize a directory name <paramref name="path"/> to <c>MSBuild</c> format which expects a terminating <see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        /// <param name="path">Specifies the directory name to normalize.</param>
        /// <returns>The normalized directory string; otherwise, <see cref="string.Empty"/> if an exception is thrown.</returns>
        /// <exception cref="SecurityException">
        /// Thrown and re-thrown if the <see cref="DirectoryInfo"/> constructor throws a <see cref="SecurityException"/> usually
        /// related to accessing an existing directory.
        /// </exception>
        internal static string NormalizeDirectoryName(string? path)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            try
            {
                DirectoryInfo normal = new(path);

                if (Path.EndsInDirectorySeparator(normal.FullName))
                {
                    return normal.FullName;
                }
                else
                {
                    return string.Concat(normal.FullName, Path.DirectorySeparatorChar);
                }
            }
            catch (SecurityException sex)
            {
                Console.Error.WriteLine(sex.ToString());
                throw;
            }
            catch (ArgumentException aex)
            {
                Console.Error.WriteLine(aex.ToString());
                return string.Empty;
            }
            catch (PathTooLongException ptlex)
            {
                Console.Error.WriteLine(ptlex.ToString());
                return string.Empty;
            }
        }

        internal static string NormalizeFileName(string? fileName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(fileName);

            try
            {
                FileInfo normal = new(fileName);

                if (normal.Name.StartsWith('-'))
                {
                    return $".{Path.DirectorySeparatorChar}{normal.Name}";
                }
                else
                {
                    return normal.Name;
                }
            }
            catch (SecurityException sex)
            {
                Console.Error.WriteLine(sex.ToString());
                throw;
            }
            catch (ArgumentException aex)
            {
                Console.Error.WriteLine(aex.ToString());
                return string.Empty;
            }
            catch (UnauthorizedAccessException uaex)
            {
                Console.Error.WriteLine(uaex.ToString());
                throw;
            }
        }

        internal static string NormalizeFilePath(string? path)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            FileInfo normal = new(path);
            return normal.FullName;
        }

        #endregion Internal Methods

        #region Public Constructors

        public CommandLineListBuilder()
            : this(quoteHyphensOnCommandLine: false, useNewLineSeparator: false)
        {
        }

        public CommandLineListBuilder(bool quoteHyphensOnCommandLine)
            : this(quoteHyphensOnCommandLine, useNewLineSeparator: false)
        {
        }

        public CommandLineListBuilder(bool quoteHyphensOnCommandLine, bool useNewLineSeparator)
        {
            this.QuoteHyphens = quoteHyphensOnCommandLine;
            this.UseNewLine = useNewLineSeparator;
            CommandList = [];
            ItemBuffer = new(16);
        }

        public CommandLineListBuilder(string commandLine)
            : this(commandLine, quoteHyphensOnCommandLine: false, useNewLineSeparator: false)
        {
        }

        public CommandLineListBuilder(string commandLine, bool quoteHyphensOnCommandLine)
           : this(commandLine, quoteHyphensOnCommandLine, useNewLineSeparator: false)
        {
        }

        public CommandLineListBuilder(string commandLine, bool quoteHyphensOnCommandLine, bool useNewLineSeparator)
            : this(quoteHyphensOnCommandLine, useNewLineSeparator)
        {
            CommandList = CommandLineParser.SplitCommandLine(commandLine).ToList();
        }

        #endregion Public Constructors

        #region Public Properties

        public int Length => CommandList.Count;

        #endregion Public Properties

        #region Public Methods

        public static IEnumerable<string> ToList(string commandLine)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(commandLine);

            return CommandLineParser.SplitCommandLine(commandLine);
        }

        public void AppendFileNameIfNotNull(ITaskItem fileItem)
        {
            AppendFileNameIfNotNull(fileItem.ItemSpec);
        }

        public void AppendFileNameIfNotNull(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Don't let injection attackers escape from our quotes by sticking in their own quotes. Quotes are illegal.
                ThrowOnEmbeddedDoubleQuote(nameof(fileName), fileName);

                AppendSpaceIfNotEmpty(ItemBuffer);
                AppendFileNameWithQuoting(ItemBuffer, fileName);
                AppendTextUnquoted(ItemBuffer.ToString());
                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendFileNamesIfNotNull(IEnumerable<ITaskItem> fileItems, string? delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));

            if (!IsNullOrEmpty(fileItems))
            {
                // Don't let injection attackers escape from our quotes by sticking in their own quotes. Quotes are illegal.
                fileItems.ToList().ForEach(f => ThrowOnEmbeddedDoubleQuote(string.Empty, f.ItemSpec));

                AppendSpaceIfNotEmpty(ItemBuffer);
                bool first = true;

                foreach (ITaskItem item in fileItems)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendFileNameWithQuoting(ItemBuffer, item.ItemSpec);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendFileNamesIfNotNull(IEnumerable<string> fileNames, string? delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));

            if (!IsNullOrEmpty(fileNames))
            {
                // Don't let injection attackers escape from our quotes by sticking in their own quotes. Quotes are illegal.
                fileNames.ToList().ForEach(f => ThrowOnEmbeddedDoubleQuote(string.Empty, f));

                AppendSpaceIfNotEmpty(ItemBuffer);
                bool first = true;

                foreach (var item in fileNames)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendFileNameWithQuoting(ItemBuffer, item);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitch(string? switchName)
        {
            AppendSwitch(ItemBuffer, switchName);
        }

        public void AppendSwitchIfNotNull(string? switchName, IEnumerable<string> parameters, string? delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter);

            if (!IsNullOrEmpty(parameters))
            {
                AppendSwitch(ItemBuffer, switchName);
                bool first = true;

                foreach (string parameter in parameters)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendTextWithQuoting(ItemBuffer, parameter);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchIfNotNull(string? switchName, IEnumerable<ITaskItem> parameters, string delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));

            if (!IsNullOrEmpty(parameters))
            {
                AppendSwitch(ItemBuffer, switchName);
                bool first = true;

                foreach (ITaskItem parameter in parameters)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendTextWithQuoting(ItemBuffer, parameter.ItemSpec);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchIfNotNull(string? switchName, string parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!string.IsNullOrEmpty(parameter))
            {
                AppendSwitch(ItemBuffer, switchName);
                AppendTextWithQuoting(ItemBuffer, parameter);
                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchIfNotNull(string? switchName, ITaskItem parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!IsNullOrEmpty(parameter))
            {
                AppendSwitchIfNotNull(switchName, parameter.ItemSpec);
            }
        }

        public void AppendSwitchUnquotedIfNotNull(string? switchName, IEnumerable<string> parameters, string delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));

            if (!IsNullOrEmpty(parameters))
            {
                AppendSwitch(ItemBuffer, switchName);
                bool first = true;

                foreach (string parameter in parameters)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendTextUnquoted(ItemBuffer, parameter);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchUnquotedIfNotNull(string? switchName, IEnumerable<ITaskItem> parameters, string delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));

            if (!IsNullOrEmpty(parameters))
            {
                AppendSwitch(ItemBuffer, switchName);
                bool first = true;

                foreach (ITaskItem parameter in parameters)
                {
                    if (!first)
                    {
                        AppendTextUnquoted(ItemBuffer, delimiter);
                    }

                    first = false;
                    AppendTextUnquoted(ItemBuffer, parameter.ItemSpec);
                }

                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchUnquotedIfNotNull(string? switchName, string? parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!string.IsNullOrEmpty(parameter))
            {
                AppendSwitch(ItemBuffer, switchName);
                AppendTextUnquoted(ItemBuffer, parameter);
                CommandList.Add(ItemBuffer.ToString());
            }
        }

        public void AppendSwitchUnquotedIfNotNull(string? switchName, ITaskItem parameter)
        {
            AppendSwitchUnquotedIfNotNull(switchName, parameter.ItemSpec);
        }

        public void AppendTextUnquoted(string? textToAppend)
        {
            if (!string.IsNullOrEmpty(textToAppend))
            {
                CommandList.Add(textToAppend);
            }
        }

        public string[] ToArray()
        {
            return CommandList.ToArray();
        }

        public List<string> ToList()
        {
            return CommandList.ToList();
        }

        public override string ToString()
        {
            StringBuilder buffer = new(16);
            ToList().ForEach(i => buffer.Append(i));
            return buffer.ToString();
        }

        #endregion Public Methods
    }
}
