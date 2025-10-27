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
using System.CommandLine.Parsing;
using System.Runtime.Versioning;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Build.Framework;

namespace MSBuild.ExtensionPack.Base.Extension
{
    /// <summary>
    /// </summary>
    /// <param name="quoteHyphensOnCommandLine"></param>
    /// <param name="useNewLineSeparator">      </param>
    public class CommandLineListBuilder(bool quoteHyphensOnCommandLine, bool useNewLineSeparator)
    {
        /// <summary>
        /// Constant used to build a <see cref="Regex"/> for detecting strings containing characters that do not require quoting
        /// when escaping of hyphens is supposed to take place.
        /// </summary>
        private const string ALLOWED_UNQUOTED_NO_QUOTE_HYPHEN_REGEX =
                        "^"                             // Beginning of line
                       + @"[a-z\\/:0-9\._\-+=]*"        //  Allow hyphen to be unquoted
                       + "$";

        /// <summary>
        /// Constant used to build a <see cref="Regex"/> for detecting strings containing characters that do not require quoting
        /// when quoting of hyphens is supposed to take place.
        /// </summary>
        private const string ALLOWED_UNQUOTED_QUOTE_HYPHEN_REGEX =
                         "^"                             // Beginning of line
                       + @"[a-z\\/:0-9\._+=]*"           // Quote hyphen
                       + "$";

        /// <summary>
        /// Constant used to build a <see cref="Regex"/> for detecting strings containing one or more characters that require
        /// quoting when quoting of hyphens is not required.
        /// </summary>
        private const string DEFINITELY_NEED_QUOTES_NO_QUOTE_HYPHEN_REGEX = @"[|><\s,;""]+";

        /// <summary>
        /// Constant used to build a <see cref="Regex"/> for detecting strings containing one or more characters that require
        /// quoting when quoting of hyphens is required.
        /// </summary>
        private const string DEFINITELY_NEED_QUOTES_QUOTE_HYPHEN_REGEX = @"[|><\s,;\-""]+";

        /// <summary>
        /// Gets a <see cref="Regex"/> value indicating a parameter or file name that can safely be unquoted.
        /// </summary>
        private Regex AllowedUnquoted => new(this.QuoteHyphens ? ALLOWED_UNQUOTED_QUOTE_HYPHEN_REGEX : ALLOWED_UNQUOTED_NO_QUOTE_HYPHEN_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>
        /// Gets a <see cref="Regex"/> value indicating a parameter or file name that must be quoted.
        /// </summary>
        private Regex DefinitelyNeedQuotes => new(this.QuoteHyphens ? DEFINITELY_NEED_QUOTES_NO_QUOTE_HYPHEN_REGEX : DEFINITELY_NEED_QUOTES_QUOTE_HYPHEN_REGEX, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Gets a value indicating the command line <see cref="IList{T}"/>.
        /// </summary>
        protected IList<string> CommandList { get; } = [];

        /// <summary>
        /// Appends <paramref name="textToAppend"/> to <paramref name="buffer"/> if <paramref name="textToAppend"/> is not <see
        /// langref="null"/> or empty. No quotes are added.
        /// </summary>
        /// <param name="buffer">      Specifies the accumulator for <paramref name="textToAppend"/>.</param>
        /// <param name="textToAppend">Specifies the text to append.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        /// <remarks>This method does not append a <see cref="SPACE"/> to the command line before executing.</remarks>
        protected static void AppendTextUnquoted(StringBuilder? buffer, string? textToAppend)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

            if (!string.IsNullOrEmpty(textToAppend))
            {
                buffer.Append(textToAppend);
            }
        }

        /// <summary>
        /// Appends a <paramref name="fileName"/> to <see cref="CommandList"/> if <paramref name="fileName"/> is not <see
        /// langref="null"/> or empty. If the first character of the file name is a dash, a "." and a directory separator are
        /// prepended to avoid confusing the file name with a switch.
        /// </summary>
        /// <param name="fileName">Specifies the file name to be append.</param>
        /// <remarks>This method does not append a space to the command line before executing.</remarks>
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
        /// Appends a <paramref name="fileName"/> to <paramref name="buffer"/> if <paramref name="fileName"/> is not <see
        /// langref="null"/> or empty. If the first character of the file name is a dash, a "." and a directory separator are
        /// prepended to avoid confusing the file name with a switch.
        /// </summary>
        /// <param name="buffer">  Specifies the accumulator for <paramref name="fileName"/>.</param>
        /// <param name="fileName">Specifies the file name to be append.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        /// <remarks>This method does not append a space to the command line before executing.</remarks>
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
        /// Appends the given text <paramref name="unquotedTextToAppend"/> to the buffer <paramref name="buffer"/>, after first
        /// quoting the text if necessary, if <paramref name="unquotedTextToAppend"/> is not <see langref="null"/> or empty.
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
        /// Appends the given text <paramref name="unquotedTextToAppend"/> to the list <paramref name="list"/>, after first quoting
        /// the text if necessary, if <paramref name="unquotedTextToAppend"/> is not <see langref="null"/> or empty.
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
                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// Appends a space or <see cref="Environment.NewLine"/> to the specified string if and only if the last element of <see
        /// cref="CommandList"/> does not end in space, to <paramref name="buffer"/> which usually points to the <see cref="ItemBuffer"/>.
        /// </summary>
        /// <param name="buffer">Specifies the item buffer accumulator.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws if <paramref name="buffer"/> or <see cref="CommandList"/> is <see langref="null"/>.
        /// </exception>
        protected void AppendSpaceIfNotEmpty(StringBuilder buffer)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            ArgumentNullException.ThrowIfNull(CommandList);

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
        /// Appends a command-line switch <paramref name="switchName"/> that has no separator value, without quoting, to <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">    Specifies the accumulator for <paramref name="switchName"/>.</param>
        /// <param name="switchName">Specifies the command-line switch to append to <paramref name="buffer"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws if either <paramref name="buffer"/> is <see langref="null"/> or <paramref name="switchName"/> is <see
        /// langref="null"/>, empty, or all whitespace.
        /// </exception>
        /// <remarks>This method appends a space to the command-line (if it is not currently empty) before <paramref name="switchName"/>.</remarks>
        protected void AppendSwitch(StringBuilder? buffer, string? switchName)
        {
            ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(switchName, nameof(switchName));

            AppendSpaceIfNotEmpty(buffer);
            AppendTextUnquoted(buffer, switchName);
        }

        /// <summary>
        /// Appends a string <paramref name="textToAppend"/> to <see cref="CommandList"/> if <paramref name="textToAppend"/> is not
        /// <see langref="null"/> or empty. Quotes are added if they are needed.
        /// </summary>
        /// <param name="textToAppend">Specifies the text to append as an item to <see cref="CommandList"/>.</param>
        /// <exception cref="ArgumentNullException">Throws if <see cref="CommandList"/> is <see langref="null"/>.</exception>
        /// <remarks>This method does not append a space to the command-line before executing.</remarks>
        protected void AppendTextWithQuoting(string? textToAppend) => AppendQuotedTextToList(CommandList, textToAppend);

        /// <summary>
        /// Appends a string <paramref name="textToAppend"/> to <paramref name="buffer"/> if <paramref name="textToAppend"/> is not
        /// <see langref="null"/> or empty. Quotes are added if they are needed.
        /// </summary>
        /// <param name="buffer">      Specifies the accumulator for <paramref name="textToAppend"/>.</param>
        /// <param name="textToAppend">Specifies the text to append as an item to <see cref="CommandList"/>.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="buffer"/> is <see langref="null"/>.</exception>
        protected void AppendTextWithQuoting(StringBuilder buffer, string? textToAppend) => AppendQuotedTextToBuffer(buffer, textToAppend);

        /// <summary>
        /// Checks the give switch parameter <paramref name="parameter"/> for whether quoting is required or optional on the command line.
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
        /// Throws a <see cref="SecurityException"/> if the parameter <paramref name="parameter"/> of <paramref name="switchName"/>
        /// has a double-quote in it. This is used to prevent parameter/code injection.
        /// </summary>
        /// <param name="switchName">Specifies the command line switch under test.</param>
        /// <param name="parameter"> Specifies the command line switch parameter to scan.</param>
        /// <exception cref="SecurityException">Throws if <paramref name="parameter"/> contains a double quote.</exception>
        /// <remarks>This method is virtual so that tools can override it if they want to have quotes escaped in file names.</remarks>
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

        /// <summary>
        /// Gets a value indicating the line buffer to use for assembling individual <see cref="CommandList"/> string elements.
        /// </summary>
        internal StringBuilder ItemBuffer { get; } = new(16);

        /// <summary>
        /// Gets a value indicating whether to quote hyphens on the elements of <see cref="CommandList"/>.
        /// </summary>
        internal bool QuoteHyphens { get; } = quoteHyphensOnCommandLine;

        /// <summary>
        /// Gets a value indicating whether to use <see cref="Environment.NewLine"/> instead of the space character as whitespace in
        /// <see cref="CommandList"/> elements.
        /// </summary>
        internal bool UseNewLine { get; } = useNewLineSeparator;

        /// <summary>
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="paths">     </param>
        /// <returns></returns>
        internal static FileSystemInfo Combine(FileAttributes attributes, params string[] paths)
        {
            List<string> filtered = [];

            foreach (string path in paths)
            {
                var stripped = path.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                if (!string.IsNullOrWhiteSpace(stripped))
                {
                    filtered.Add(stripped);
                }
            }

            FileSystemInfo fileSystemInfo = attributes.HasFlag(FileAttributes.Directory) ? new DirectoryInfo(Path.Combine([.. filtered])) : new FileInfo(Path.Combine([.. filtered]));
            fileSystemInfo.Attributes = attributes;
            return fileSystemInfo;
        }

        /// <summary>
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="firstPath"> </param>
        /// <param name="secondPath"></param>
        /// <returns></returns>
        internal static FileSystemInfo Combine(FileAttributes attributes, string firstPath, string secondPath) => CommandLineListBuilder.Combine(attributes, [firstPath, secondPath]);

        /// <summary>
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="firstPath"> </param>
        /// <param name="secondPath"></param>
        /// <param name="thirdPath"> </param>
        /// <returns></returns>
        internal static FileSystemInfo Combine(FileAttributes attributes, string firstPath, string secondPath, string thirdPath) => CommandLineListBuilder.Combine(attributes, [firstPath, secondPath, thirdPath]);

        /// <summary>
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="firstPath"> </param>
        /// <param name="secondPath"></param>
        /// <param name="thirdPath"> </param>
        /// <param name="fourthPath"></param>
        /// <returns></returns>
        internal static FileSystemInfo Combine(FileAttributes attributes, string firstPath, string secondPath, string thirdPath, string fourthPath) => CommandLineListBuilder.Combine(attributes, [firstPath, secondPath, thirdPath, fourthPath]);

        /// <summary>
        /// Tests whether <paramref name="buffer"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="buffer">Specifies the <see cref="StringBuilder"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="buffer"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(StringBuilder? buffer) => buffer is null || buffer.Length < 1;

        /// <summary>
        /// Tests whether <paramref name="item"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="item">Specifies the <see cref="ITaskItem"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="item"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(ITaskItem? item) => item is null || string.IsNullOrEmpty(item?.ItemSpec);

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <typeparam name="T">Specifies the element <see cref="Type"/> of <paramref name="items"/>.</typeparam>
        /// <param name="items">Specifies the <see cref="IEnumerable{T}"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty<T>(IEnumerable<T> items) => items?.Any() != true;

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="items">
        /// Specifies the <see cref="IEnumerable{T}"/> of element <see cref="Type"/><see cref="string"/> under test.
        /// </param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(IEnumerable<string> items) => CommandLineListBuilder.IsNullOrEmpty<string>(items);

        /// <summary>
        /// Tests whether <paramref name="items"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <param name="items">
        /// Specifies the <see cref="IEnumerable{T}"/> of element <see cref="Type"/><see cref="ITaskItem"/> under test.
        /// </param>
        /// <returns>
        /// <see langref="true"/> if <paramref name="items"/> is either <see langref="null"/> or empty; otherwise, <see langref="false"/>.
        /// </returns>
        internal static bool IsNullOrEmpty(IEnumerable<ITaskItem> items)
        {
            return IsNullOrEmpty<ITaskItem>(items);
        }

        internal static FileSystemInfo Join(FileAttributes attributes, params string?[] paths)
        {
            List<string> filtered = [];

            foreach (string path in paths)
            {
                if (!string.IsNullOrWhiteSpace(path))
                {
                    filtered.Add(path);
                }
            }

            FileSystemInfo fileSystemInfo = attributes.HasFlag(FileAttributes.Directory) ? new DirectoryInfo(Path.Join([.. filtered])) : new FileInfo(Path.Join([.. filtered]));
            fileSystemInfo.Attributes = attributes;
            return fileSystemInfo;
        }

        internal static FileSystemInfo Join(FileAttributes attributes, string? firstPath, string? secondPath)
        {
            return CommandLineListBuilder.Join(attributes, [firstPath, secondPath]);
        }

        internal static FileSystemInfo Join(FileAttributes attributes, string? firstPath, string? secondPath, string? thirdPath)
        {
            return CommandLineListBuilder.Join(attributes, [firstPath, secondPath, thirdPath]);
        }

        internal static FileSystemInfo Join(FileAttributes attributes, string? firstPath, string? secondPath, string? thirdPath, string? fourthPath)
        {
            return CommandLineListBuilder.Join(attributes, [firstPath, secondPath, thirdPath, fourthPath]);
        }

        internal static DirectoryInfo? MakeSecureTempDirectory(string leaf = ".tmp")
        {
            const FileAttributes attributes = FileAttributes.Directory | FileAttributes.Temporary | FileAttributes.NotContentIndexed;
            return CommandLineListBuilder.Join(attributes, Path.GetTempPath(), Guid.NewGuid().ToString(), leaf) as DirectoryInfo;
        }

        internal static string MakeSecureTempFileName(string baseName, string? extension)
        {
            DateTime utc = DateTime.UtcNow;
            Span<char> destination = new("XXXXXXXX".ToCharArray());

            if (Convert.TryToHexString(BitConverter.GetBytes(utc.Ticks), destination, out int charsWritten) && charsWritten >= 6)
            {
                return string.IsNullOrEmpty(extension) ? string.Concat(baseName, destination) : string.Concat(baseName, destination, extension);
            }
            else
            {
                throw new InvalidOperationException($"Could not parse UTC ticks to a Span with at least six characters.");
            }
        }

        internal static FileInfo? MakeSecureTempPath(string baseName, string leaf = ".tmp", string? extension = ".tmp", int maxRetry = 1000)
        {
            const FileAttributes attributes = FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.NotContentIndexed;

            var directory = MakeSecureTempDirectory(leaf);
            FileInfo? tempInfo = null;
            int count = maxRetry;

            do
            {
                tempInfo = CommandLineListBuilder.Join(attributes, directory?.FullName, MakeSecureTempFileName(baseName, extension)) as FileInfo;
            }
            while (tempInfo?.Exists != false && count-- >= 1);

            if (tempInfo is null)
            {
                throw new InvalidOperationException("Error Joining parameters to form FileInfo.");
            }

            if (count < 1)
            {
                throw new InvalidOperationException($"Cannot generate a unique file name in {maxRetry} attempts.");
            }

            if (OperatingSystem.IsWindows())
            {
                CommandLineListBuilder.SetDefaultAccessControl(
                    tempInfo,
                    owner: FileSystemRights.FullControl,
                    group: FileSystemRights.Read,
                    world: FileSystemRights.Read);
            }
            else
            {
                CommandLineListBuilder.SetDefaultFileMode(
                    tempInfo,
                    UnixFileMode.UserExecute | UnixFileMode.UserWrite | UnixFileMode.UserRead
                    | UnixFileMode.GroupRead
                    | UnixFileMode.OtherRead);
            }

            return tempInfo;
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

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
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

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string NormalizeFilePath(string? path)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            try
            {
                FileInfo normal = new(path);
                return Path.Combine(normal.DirectoryName ?? ".", CommandLineListBuilder.NormalizeFileName(normal.Name));
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
            catch (PathTooLongException ptlex)
            {
                Console.Error.WriteLine(ptlex.ToString());
                return string.Empty;
            }
            catch (NotSupportedException nsex)
            {
                Console.Error.WriteLine(nsex.ToString());
                throw;
            }
        }

        [SupportedOSPlatform("Windows")]
        internal static void SetDefaultAccessControl(FileInfo path, FileSystemRights owner, FileSystemRights group, FileSystemRights world)
        {
            var ownerSecurityIdentifier = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
            var groupSecurityIdentifier = new SecurityIdentifier(WellKnownSidType.CreatorGroupSid, null);
            var worldSecurityIdentifier = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            FileSecurity security = path.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(ownerSecurityIdentifier, owner, AccessControlType.Allow));
            security.PurgeAccessRules(groupSecurityIdentifier);
            security.AddAccessRule(new FileSystemAccessRule(groupSecurityIdentifier, group, AccessControlType.Allow));
            security.PurgeAccessRules(worldSecurityIdentifier);
            security.AddAccessRule(new FileSystemAccessRule(worldSecurityIdentifier, world, AccessControlType.Allow));
            path.SetAccessControl(security);
        }

        [UnsupportedOSPlatform("Windows")]
        internal static void SetDefaultFileMode(FileInfo path, UnixFileMode fileMode)
        {
            path.UnixFileMode = fileMode;
        }

        /// <summary>
        /// Constant field representing the space character.
        /// </summary>
        public const char SPACE = ' ';

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineListBuilder"/> class.
        /// </summary>
        public CommandLineListBuilder()
            : this(quoteHyphensOnCommandLine: false, useNewLineSeparator: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineListBuilder"/> class.
        /// </summary>
        /// <param name="quoteHyphensOnCommandLine"></param>
        public CommandLineListBuilder(bool quoteHyphensOnCommandLine)
            : this(quoteHyphensOnCommandLine, useNewLineSeparator: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineListBuilder"/> class.
        /// </summary>
        /// <param name="commandLine"></param>
        public CommandLineListBuilder(string commandLine)
            : this(commandLine, quoteHyphensOnCommandLine: false, useNewLineSeparator: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineListBuilder"/> class.
        /// </summary>
        /// <param name="commandLine">              </param>
        /// <param name="quoteHyphensOnCommandLine"></param>
        public CommandLineListBuilder(string commandLine, bool quoteHyphensOnCommandLine)
           : this(commandLine, quoteHyphensOnCommandLine, useNewLineSeparator: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineListBuilder"/> class.
        /// </summary>
        /// <param name="commandLine">              </param>
        /// <param name="quoteHyphensOnCommandLine"></param>
        /// <param name="useNewLineSeparator">      </param>
        public CommandLineListBuilder(string commandLine, bool quoteHyphensOnCommandLine, bool useNewLineSeparator)
            : this(quoteHyphensOnCommandLine, useNewLineSeparator)
        {
            CommandList = [.. CommandLineParser.SplitCommandLine(commandLine)];
        }

        /// <summary>
        /// Gets a value indicating the length of <see cref="CommandList"/> in elements.
        /// </summary>
        public int Length => CommandList.Count;

        /// <summary>
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<string> ToList(string commandLine)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(commandLine);

            return CommandLineParser.SplitCommandLine(commandLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="fileItem"></param>
        public void AppendFileNameIfNotNull(ITaskItem fileItem)
        {
            AppendFileNameIfNotNull(fileItem.ItemSpec);
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="SecurityException"></exception>
        public void AppendFileNameIfNotNull(string? fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Don't let injection attackers escape from our quotes by sticking in their own quotes. Quotes are illegal.
                ThrowOnEmbeddedDoubleQuote(nameof(fileName), fileName);

                AppendSpaceIfNotEmpty(ItemBuffer);
                AppendFileNameWithQuoting(ItemBuffer, fileName);
                AppendTextUnquoted(ItemBuffer.ToString());
                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileItems"></param>
        /// <param name="delimiter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="delimiter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SecurityException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        public void AppendSwitch(string? switchName)
        {
            AppendSwitch(ItemBuffer, switchName);
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameters"></param>
        /// <param name="delimiter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameters"></param>
        /// <param name="delimiter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AppendSwitchIfNotNull(string? switchName, string parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!string.IsNullOrEmpty(parameter))
            {
                AppendSwitch(ItemBuffer, switchName);
                AppendTextWithQuoting(ItemBuffer, parameter);
                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AppendSwitchIfNotNull(string? switchName, ITaskItem parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!IsNullOrEmpty(parameter))
            {
                AppendSwitchIfNotNull(switchName, parameter.ItemSpec);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameters"></param>
        /// <param name="delimiter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameters"></param>
        /// <param name="delimiter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
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

                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameter"> </param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AppendSwitchUnquotedIfNotNull(string? switchName, string? parameter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(switchName);

            if (!string.IsNullOrEmpty(parameter))
            {
                AppendSwitch(ItemBuffer, switchName);
                AppendTextUnquoted(ItemBuffer, parameter);
                AppendTextUnquoted(ItemBuffer.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="switchName"></param>
        /// <param name="parameter"> </param>
        public void AppendSwitchUnquotedIfNotNull(string? switchName, ITaskItem parameter)
        {
            AppendSwitchUnquotedIfNotNull(switchName, parameter.ItemSpec);
        }

        /// <summary>
        /// </summary>
        /// <param name="textToAppend"></param>
        public void AppendTextUnquoted(string? textToAppend)
        {
            if (!string.IsNullOrEmpty(textToAppend))
            {
                CommandList.Add(textToAppend);
            }
        }

        /// <summary>
        /// Convert <see cref="CommandList"/> to an array of strings.
        /// </summary>
        /// <returns>Returns the command list as an array of strings.</returns>
        public string[] ToArray()
        {
            return [.. CommandList];
        }

        /// <summary>
        /// Convert <see cref="CommandList"/> to a <see cref="List{T}"/> of <see cref="Type"/> string.
        /// </summary>
        /// <returns>Returns the command list as a <see cref="List{T}"/>.</returns>
        public List<string> ToList()
        {
            return [.. CommandList];
        }

        /// <summary>
        /// Convert <see cref="CommandList"/> to a string.
        /// </summary>
        /// <returns>Returns the command line as a string.</returns>
        public override string ToString()
        {
            StringBuilder buffer = new(16);

            try
            {
                ToList().ForEach(i => buffer.Append(i.Trim()).Append(UseNewLine ? Environment.NewLine : SPACE));
                return buffer.ToString();
            }
            finally
            {
                buffer.Clear();
            }
        }
    }
}
