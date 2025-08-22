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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;

namespace MSBuild.ExtensionPack.Base.Shared
{
    namespace MSBuild.ExtensionPack.Base.Shared
    {
        /// <summary>
        /// This class contains utility methods for dealing with resources.
        /// </summary>
        internal static class ResourceUtilities
        {
            #region Private Methods

            /// <summary>
            /// Retrieves the MSBuild F1-help keyword for the given resource string. Help keywords are used to index help topics in
            /// host IDEs.
            /// </summary>
            /// <param name="resourceName">Resource string to get the MSBuild F1-keyword for.</param>
            /// <returns>The MSBuild F1-help keyword string.</returns>
            private static string GetHelpKeyword(string resourceName)
                => "MSBuild." + resourceName;

            [Conditional("DEBUG")]
            private static void ValidateArgsIfDebug(object?[]? args)
            {
                // If you accidentally pass some random type in that can't be converted to a string, FormatResourceString calls
                // ToString() which returns the full name of the type!
                if (args is null || args.Length < 1)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                foreach (object? param in args)
                {
                    // Check it has a real implementation of ToString() and the type is not actually System.String
                    if (param is not null)
                    {
                        if (string.Equals(param.GetType().ToString(), param.ToString(), StringComparison.Ordinal) && param.GetType() != typeof(string))
                        {
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource parameter type, was {0}", param.GetType().FullName);
                        }
                    }
                }
            }

            #endregion Private Methods

            #region Internal Methods

            /// <summary>
            /// Extracts the message code (if any) prefixed to the given string. <![CDATA[ MSBuild codes match
            /// "^\s*(?<CODE>MSB\d\d\d\d):\s*(?<MESSAGE>.*)$" Arbitrary codes match "^\s*(?<CODE>[A-Za-z]+\d+):\s*(?<MESSAGE>.*)$"
            /// ]]> Thread safe.
            /// </summary>
            /// <param name="msbuildCodeOnly">Whether to match only MSBuild error codes, or any error code.</param>
            /// <param name="message">        The string to parse.</param>
            /// <param name="code">           [out] The message code, or null if there was no code.</param>
            /// <returns>The string without its message code prefix, if any.</returns>
            [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Microsoft.Build.Shared.ResourceUtilities.#ExtractMessageCode(System.Boolean,System.String,System.String&)", Justification = "Unavoidable complexity")]
            internal static string ExtractMessageCode(bool msbuildCodeOnly, string message, out string? code)
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

                code = null;
                Regex msBuildCode = new(@"^\s*(?<code>MSB\d\d\d\d)[:]?(?<message>.+)$");
                Regex nonMSBuildCode = new(@"^\s*(?<code>[A-Za-z]+[0-9]+)[:]?(?<message>.+)$");

                if (msbuildCodeOnly && msBuildCode.IsMatch(message))
                {
                    foreach (Match match in msBuildCode.Matches(message))
                    {
                        code = match.Groups["code"].Value;
                        message = match.Groups["message"].Value;
                    }

                    return message;
                }
                else if (nonMSBuildCode.IsMatch(message))
                {
                    foreach (Match match in nonMSBuildCode.Matches(message))
                    {
                        code = match.Groups["code"].Value;
                        message = match.Groups["message"].Value;
                    }

                    return message;
                }
                else
                {
                    return message.Trim();
                }
            }

            /// <summary>
            /// Formats the resource string with the given arguments. Ignores error codes and keywords.
            /// </summary>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="args">        Optional arguments for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            /// <remarks>the AssemblyResources.GetString() method is thread-safe.</remarks>
            internal static string FormatResourceStringIgnoreCodeAndKeyword(string resourceName, params object?[]? args)
                => FormatString(GetResourceString(resourceName), args);

            /// <summary>
            /// Formats the resource string. Ignores error codes and keywords.
            /// </summary>
            /// <param name="resourceName">Resource string to load.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringIgnoreCodeAndKeyword(string resourceName)
                => GetResourceString(resourceName);

            // Overloads with 0-3 arguments to avoid array allocations.
            /// <summary>
            /// Formats the resource string with the given argument. Ignores error codes and keywords.
            /// </summary>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        Argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringIgnoreCodeAndKeyword(string resourceName, object? arg1)
                => FormatString(GetResourceString(resourceName), arg1);

            /// <summary>
            /// Formats the resource string with the given arguments. Ignores error codes and keywords.
            /// </summary>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringIgnoreCodeAndKeyword(string resourceName, object? arg1, object? arg2)
                => FormatString(GetResourceString(resourceName), arg1, arg2);

            /// <summary>
            /// Formats the resource string with the given arguments. Ignores error codes and keywords.
            /// </summary>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            /// <param name="arg3">        Third argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringIgnoreCodeAndKeyword(string resourceName, object? arg1, object? arg2, object? arg3)
                => FormatString(GetResourceString(resourceName), arg1, arg2, arg3);

            /// <summary>
            /// Loads the specified string resource and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they too are returned.
            ///
            /// PERF WARNING: calling a method that takes a variable number of arguments is expensive, because memory is allocated
            /// for the array of arguments -- do not call this method repeatedly in performance-critical scenarios.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="code">        [out] The MSBuild message code, or null.</param>
            /// <param name="helpKeyword"> [out] The MSBuild F1-help keyword for the host IDE, or null.</param>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="args">        Optional arguments for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(out string? code, out string? helpKeyword, string resourceName, params object?[]? args)
            {
                helpKeyword = GetHelpKeyword(resourceName);

                // NOTE: the AssemblyResources.GetString() method is thread-safe
                return ExtractMessageCode(true, FormatString(GetResourceString(resourceName), args), out code);
            }

            /// <summary>
            /// Loads the specified string resource and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they too are returned.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="code">        [out] The MSBuild message code, or null.</param>
            /// <param name="helpKeyword"> [out] The MSBuild F1-help keyword for the host IDE, or null.</param>
            /// <param name="resourceName">Resource string to load.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(out string? code, out string? helpKeyword, string resourceName)
            {
                helpKeyword = GetHelpKeyword(resourceName);
                return ExtractMessageCode(true, GetResourceString(resourceName), out code);
            }

            // Overloads with 0-3 arguments to avoid array allocations.
            /// <summary>
            /// Loads the specified string resource and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they too are returned.
            /// </summary>
            /// <param name="code">        [out] The MSBuild message code, or null.</param>
            /// <param name="helpKeyword"> [out] The MSBuild F1-help keyword for the host IDE, or null.</param>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        Argument for formatting the resource string.</param>
            internal static string FormatResourceStringStripCodeAndKeyword(out string? code, out string? helpKeyword, string resourceName, object? arg1)
            {
                helpKeyword = GetHelpKeyword(resourceName);
                return ExtractMessageCode(true, FormatString(GetResourceString(resourceName), arg1), out code);
            }

            /// <summary>
            /// Loads the specified string resource and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they too are returned.
            /// </summary>
            /// <param name="code">        [out] The MSBuild message code, or null.</param>
            /// <param name="helpKeyword"> [out] The MSBuild F1-help keyword for the host IDE, or null.</param>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            internal static string FormatResourceStringStripCodeAndKeyword(out string? code, out string? helpKeyword, string resourceName, object? arg1, object? arg2)
            {
                helpKeyword = GetHelpKeyword(resourceName);
                return ExtractMessageCode(true, FormatString(GetResourceString(resourceName), arg1, arg2), out code);
            }

            /// <summary>
            /// Loads the specified string resource and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they too are returned.
            /// </summary>
            /// <param name="code">        [out] The MSBuild message code, or null.</param>
            /// <param name="helpKeyword"> [out] The MSBuild F1-help keyword for the host IDE, or null.</param>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            /// <param name="arg3">        Third argument for formatting the resource string.</param>
            internal static string FormatResourceStringStripCodeAndKeyword(out string? code, out string? helpKeyword, string resourceName, object? arg1, object? arg2, object? arg3)
            {
                helpKeyword = GetHelpKeyword(resourceName);
                return ExtractMessageCode(true, FormatString(GetResourceString(resourceName), arg1, arg2, arg3), out code);
            }

            /// <summary>
            /// Looks up a string in the resources, and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they are discarded.
            ///
            /// PERF WARNING: calling a method that takes a variable number of arguments is expensive, because memory is allocated
            /// for the array of arguments -- do not call this method repeatedly in performance-critical scenarios.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="args">        Optional arguments for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(string resourceName, params object?[]? args)
                => FormatResourceStringStripCodeAndKeyword(out _, out _, resourceName, args);

            /// <summary>
            /// Looks up a string in the resources. If the string resource has an MSBuild message code and help keyword associated
            /// with it, they are discarded.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to load.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(string resourceName)
               => FormatResourceStringStripCodeAndKeyword(out _, out _, resourceName);

            // Overloads with 0-3 arguments to avoid array allocations.
            /// <summary>
            /// Looks up a string in the resources, and formats it with the argument passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they are discarded.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        Argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(string resourceName, object? arg1)
               => FormatResourceStringStripCodeAndKeyword(out _, out _, resourceName, arg1);

            /// <summary>
            /// Looks up a string in the resources, and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they are discarded.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(string resourceName, object? arg1, object? arg2)
                => FormatResourceStringStripCodeAndKeyword(out _, out _, resourceName, arg1, arg2);

            /// <summary>
            /// Looks up a string in the resources, and formats it with the arguments passed in. If the string resource has an
            /// MSBuild message code and help keyword associated with it, they are discarded.
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to load.</param>
            /// <param name="arg1">        First argument for formatting the resource string.</param>
            /// <param name="arg2">        Second argument for formatting the resource string.</param>
            /// <param name="arg3">        Third argument for formatting the resource string.</param>
            /// <returns>The formatted resource string.</returns>
            internal static string FormatResourceStringStripCodeAndKeyword(string resourceName, object? arg1, object? arg2, object? arg3)
                => FormatResourceStringStripCodeAndKeyword(out _, out _, resourceName, arg1, arg2, arg3);

            /// <summary>
            /// Formats the given string using the variable arguments passed in.
            ///
            /// PERF WARNING: calling a method that takes a variable number of arguments is expensive, because memory is allocated
            /// for the array of arguments -- do not call this method repeatedly in performance-critical scenarios
            ///
            /// Thread safe.
            /// </summary>
            /// <param name="unformatted">The string to format.</param>
            /// <param name="args">       Optional arguments for formatting the given string.</param>
            /// <returns>The formatted string.</returns>
            internal static string FormatString(string unformatted, params object?[]? args)
            {
                ValidateArgsIfDebug(args);
                return string.Format(CultureInfo.CurrentCulture, unformatted, args);
            }

            /// <summary>
            /// Formats the given string using the variable arguments passed in.
            /// </summary>
            /// <param name="unformatted">The string to format.</param>
            /// <param name="arg1">       Argument for formatting the given string.</param>
            /// <returns>The formatted string.</returns>
            internal static string FormatString(string unformatted, object? arg1)
            {
                ValidateArgsIfDebug([arg1]);
                return string.Format(CultureInfo.CurrentCulture, unformatted, arg1);
            }

            // Overloads with 1-3 arguments to avoid array allocations.
            /// <summary>
            /// Formats the given string using the variable arguments passed in.
            /// </summary>
            /// <param name="unformatted">The string to format.</param>
            /// <param name="arg1">       First argument for formatting the given string.</param>
            /// <param name="arg2">       Second argument for formatting the given string.</param>
            /// <returns>The formatted string.</returns>
            internal static string FormatString(string unformatted, object? arg1, object? arg2)
            {
                ValidateArgsIfDebug([arg1, arg2]);
                return string.Format(CultureInfo.CurrentCulture, unformatted, arg1, arg2);
            }

            /// <summary>
            /// Formats the given string using the variable arguments passed in.
            /// </summary>
            /// <param name="unformatted">The string to format.</param>
            /// <param name="arg1">       First argument for formatting the given string.</param>
            /// <param name="arg2">       Second argument for formatting the given string.</param>
            /// <param name="arg3">       Third argument for formatting the given string.</param>
            /// <returns>The formatted string.</returns>
            internal static string FormatString(string unformatted, object? arg1, object? arg2, object? arg3)
            {
                ValidateArgsIfDebug([arg1, arg2, arg3]);
                return string.Format(CultureInfo.CurrentCulture, unformatted, arg1, arg2, arg3);
            }

            /// <summary>
            /// Retrieves the contents of the named resource string.
            /// </summary>
            /// <param name="resourceName">Resource string name.</param>
            /// <returns>Resource string contents.</returns>
            internal static string GetResourceString(string resourceName)
                => AssemblyResource.GetString(resourceName);

            /// <summary>
            /// Verifies that a particular resource string actually exists in the string table. This will only be called in debug
            /// builds. It helps catch situations where a dev calls VerifyThrowXXX with a new resource string, but forgets to add
            /// the resource string to the string table, or misspells it!
            /// </summary>
            /// <remarks>This method is thread-safe.</remarks>
            /// <param name="resourceName">Resource string to check.</param>
            [Conditional("DEBUG")]
            internal static void VerifyResourceStringExists(string resourceName)
            {
                try
                {
                    // Look up the resource string in the engine's string table.
                    // NOTE: the AssemblyResources.GetString() method is thread-safe
                    string unformattedMessage = AssemblyResource.GetString(resourceName);

                    if (string.IsNullOrEmpty(unformattedMessage))
                    {
                        throw new ArgumentException($"The resource string '{resourceName}' was not found.", nameof(resourceName));
                    }
                }
                catch (Exception e) when (e is ArgumentException or InvalidOperationException or MissingManifestResourceException)
                {
                    Console.Error.WriteLine(e.ToString());
                    throw;
                }
            }

            #endregion Internal Methods
        }
    }
