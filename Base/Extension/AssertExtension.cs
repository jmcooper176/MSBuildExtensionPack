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
namespace MSBuild.ExtensionPack.Base.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class AssertExtension
    {
        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            IEquatable<TEquality> left,
            IEquatable<TEquality> right,
            IFormatProvider? provider,
            string format,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(!left.Equals(right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            IEquatable<TEquality> left,
            IEquatable<TEquality> right,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(left.Equals(right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments) where TEquality : IEqualityComparer<TEquality>
        {
            AreEqual(left, right, provider, format, EqualityComparer<TEquality>.Default, filePath, lineNumber, memberName, arguments);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null) where TEquality : IEqualityComparer<TEquality>
        {
            AreEqual(left, right, null, message, EqualityComparer<TEquality>.Default, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="comparer">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            IEqualityComparer<TEquality> comparer,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(comparer.Equals(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="comparer">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            IEqualityComparer<TEquality> comparer,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(comparer.Equals(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="equals">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            Func<TEquality?, TEquality?, bool> equals,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(equals.Invoke(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="equals">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            Func<TEquality?, TEquality?, bool> equals,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(equals.Invoke(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="comparison"></param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreEqual(
            string? left,
            string? right,
            IFormatProvider? provider,
            string format,
            StringComparison? comparison,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(string.Equals(left, right, comparison ?? StringComparison.Ordinal), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are NOT equal.
        /// </summary>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="comparison"></param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreEqual(
            string? left,
            string? right,
            string message,
            StringComparison? comparison,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(string.Equals(left, right, comparison ?? StringComparison.Ordinal), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            IEquatable<TEquality> left,
            IEquatable<TEquality> right,
            IFormatProvider? provider,
            string format,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(!left.Equals(right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            IEquatable<TEquality> left,
            IEquatable<TEquality> right,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(!left.Equals(right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments) where TEquality : IEqualityComparer<TEquality>
        {
            AreNotEqual(left, right, provider, format, EqualityComparer<TEquality>.Default, filePath, lineNumber, memberName, arguments);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments) where TEquality : IEqualityComparer<TEquality>
        {
            AreNotEqual(left, right, null, message, EqualityComparer<TEquality>.Default, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="comparer">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            IEqualityComparer<TEquality> comparer,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(!comparer.Equals(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="comparer">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            IEqualityComparer<TEquality> comparer,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(!comparer.Equals(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="notEqual">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            IFormatProvider? provider,
            string format,
            Func<TEquality?, TEquality?, bool> notEqual,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(notEqual.Invoke(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <typeparam name="TEquality"></typeparam>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="notEqual">  </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreNotEqual<TEquality>(
            TEquality? left,
            TEquality? right,
            string message,
            Func<TEquality?, TEquality?, bool> notEqual,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(notEqual.Invoke(left, right), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="provider">  </param>
        /// <param name="format">    </param>
        /// <param name="comparison"></param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        /// <param name="arguments"> </param>
        [Conditional("DEBUG")]
        public static void AreNotEqual(
            string? left,
            string? right,
            IFormatProvider? provider,
            string format,
            StringComparison? comparison,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments)
        {
            string message = string.Format(provider ?? CultureInfo.CurrentCulture, format, arguments);
            Assert(!string.Equals(left, right, comparison ?? StringComparison.Ordinal), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="left"/> is equal to <paramref name="right"/> and calls <see cref="Debug.Fail(string?)"/>
        /// if they are equal.
        /// </summary>
        /// <param name="left">      </param>
        /// <param name="right">     </param>
        /// <param name="message">   </param>
        /// <param name="comparison"></param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void AreNotEqual(
            string? left,
            string? right,
            string message,
            StringComparison? comparison,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(!string.Equals(left, right, comparison ?? StringComparison.Ordinal), message, filePath, lineNumber, memberName);
        }

        /// <summary>
        /// Tests whether <paramref name="condition"/> is true and calls <see cref="Debug.Fail(string?, string?)"/> if the condition
        /// is false.
        /// </summary>
        /// <param name="condition">            </param>
        /// <param name="message">              </param>
        /// <param name="detailedMessageFormat"></param>
        /// <param name="filePath">             </param>
        /// <param name="lineNumber">           </param>
        /// <param name="memberName">           </param>
        /// <param name="arguments">            </param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] bool condition,
            string? message = null,
            string? detailedMessageFormat = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[] arguments)
        {
            if (!condition)
            {
                string msg = message ?? $"[{DateTime.UtcNow:s}] {filePath}({lineNumber}) : {memberName} : Condition {nameof(condition)} evaluated to false.  Failing.";
                string detailedMessage = detailedMessageFormat ?? string.Format(CultureInfo.CurrentCulture, detailedMessageFormat!, arguments);
                Debug.Fail(msg, detailedMessage);
            }
        }

        /// <summary>
        /// Tests whether <paramref name="condition"/> is true and calls <see cref="Debug.Fail(string?, string?)"/> if the condition
        /// is false.
        /// </summary>
        /// <param name="predicate">            </param>
        /// <param name="message">              </param>
        /// <param name="detailedMessageFormat"></param>
        /// <param name="filePath">             </param>
        /// <param name="lineNumber">           </param>
        /// <param name="memberName">           </param>
        /// <param name="arguments">            </param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] Func<bool> predicate,
            string? message = null,
            string? detailedMessageFormat = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null,
            [AllowNull] params object?[]? arguments)
        {
            Assert(predicate.Invoke(), message, detailedMessageFormat, filePath, lineNumber, memberName, arguments);
        }

        /// <summary>
        /// Tests whether <paramref name="condition"/> is true and calls <see cref="Debug.Fail(string?)"/> if the condition is false.
        /// </summary>
        /// <param name="condition">      </param>
        /// <param name="message">        </param>
        /// <param name="detailedMessage"></param>
        /// <param name="filePath">       </param>
        /// <param name="lineNumber">     </param>
        /// <param name="memberName">     </param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] bool condition,
            string? message = null,
            string? detailedMessage = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(condition, message, detailedMessage, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="predicate"/> evaluates to true and calls <see cref="Debug.Fail(string?, string?)"/> if the
        /// <paramref name="predicate"/> evaluates to false.
        /// </summary>
        /// <param name="predicate">      </param>
        /// <param name="message">        </param>
        /// <param name="detailedMessage"></param>
        /// <param name="filePath">       </param>
        /// <param name="lineNumber">     </param>
        /// <param name="memberName">     </param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] Func<bool> predicate,
            string? message = null,
            string? detailedMessage = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(predicate, message, detailedMessage, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="condition"/> is true and calls <see cref="Debug.Fail(string?)"/> if the condition is false.
        /// </summary>
        /// <param name="condition"> </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] bool condition,
            string? message = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(condition, message, null, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="predicate"/> evaluates to true and calls <see cref="Debug.Fail(string?)"/> if the
        /// <paramref name="predicate"/> evaluates to false.
        /// </summary>
        /// <param name="predicate"> </param>
        /// <param name="message">   </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] Func<bool> predicate,
            string? message = null,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(predicate, message, null, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="condition"/> is true and calls <see cref="Debug.Fail(string?)"/> if the condition is false.
        /// </summary>
        /// <param name="condition"> </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] bool condition,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(condition, null, null, filePath, lineNumber, memberName, null);
        }

        /// <summary>
        /// Tests whether <paramref name="predicate"/> evaluates to true and calls <see cref="Debug.Fail(string?)"/> if the
        /// <paramref name="predicate"/> evaluates to false.
        /// </summary>
        /// <param name="predicate"> </param>
        /// <param name="filePath">  </param>
        /// <param name="lineNumber"></param>
        /// <param name="memberName"></param>
        [Conditional("DEBUG")]
        public static void Assert(
            [DoesNotReturnIf(false)] Func<bool> predicate,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string? memberName = null)
        {
            Assert(predicate, null, null, filePath, lineNumber, memberName, null);
        }

        public static ICollection<TElement> DefaultIfEmpty<TElement>(ICollection<TElement> instance)
        {
            return IsNotEmpty(instance) ? instance : default(T);
        }

        public static ICollection<TElement> EmptyIfNull<TCollection, TElement>(TCollection instance) where TCollection : class, ICollection<TElement>, new()
        {
            return IsNotNull(instance) ? instance : new();
        }

        /// <summary>
        /// Determines whether the specified <see cref="StringBuilder"/><paramref name="builder"/> is empty.
        /// </summary>
        /// <param name="builder">Specifies the <see cref="StringBuilder"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="StringBuilder"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty(StringBuilder builder)
        {
            return builder.Length < 1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Array"/><paramref name="array"/> is empty.
        /// </summary>
        /// <param name="array">Specifies the <see cref="Array"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="Array"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty<TElement>(TElement[] array)
        {
            return array.Length < 1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="IEnumerable{T}"/><paramref name="enumerable"/> is empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">Specifies the <see cref="IEnumerable{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="IEnumerable{T}"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty<TElement>(IEnumerable<TElement> enumerable)
        {
            return enumerable.Any();
        }

        /// <summary>
        /// Determines whether the specified <see cref="IDictionary{TKey, TValue}"/><paramref name="dictionary"/> is empty.
        /// </summary>
        /// <typeparam name="TKey">Specifies the key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">Specifies the value type of <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">Specifies the <see cref="IDictionary{TKey, TValue}"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if the specified <see cref="IDictionary{TKey, TValue}"/> is empty; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsEmpty<TKey, TValue>(IDictionary<TKey, TValue?> dictionary)
        {
            return IsEmpty(collection: dictionary);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ISet{T}"/><paramref name="set"/> is empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="enumerable"/>.</typeparam>
        /// <param name="set">Specifies the <see cref="ISet{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="ISet{T}"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty<TElement>(ISet<TElement> set)
        {
            return IsEmpty(collection: set);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ICollection{T}"/><paramref name="collection"/> is empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="collection"/>.</typeparam>
        /// <param name="collection">Specifies the <see cref="ICollection{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="ICollection{T}"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmpty<TElement>(ICollection<TElement> collection)
        {
            return collection.Count < 1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Array"/><paramref name="array"/> is empty using <see cref="Array.LongLength"/>.
        /// </summary>
        /// <param name="array">Specifies the <see cref="Array"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="Array"/> is empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsEmptyLong<TElement>(TElement[] array)
        {
            return array.LongLength < 1L;
        }

        /// <summary>
        /// Determines whether the specified <see cref="StringBuilder"/><paramref name="builder"/> is NOT empty.
        /// </summary>
        /// <param name="builder">Specifies the <see cref="StringBuilder"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="StringBuilder"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmpty(StringBuilder builder)
        {
            return builder.Length > 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Array"/><paramref name="array"/> is NOT empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of the <paramref name="array"/>.</typeparam>
        /// <param name="array">Specifies the <see cref="Array"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="Array"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmpty<TElement>(TElement[] array)
        {
            return array.Length > 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="IEnumerable{T}"/><paramref name="enumerable"/> is NOT empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">Specifies the <see cref="IEnumerable{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="IEnumerable{T}"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmpty<TElement>(IEnumerable<TElement> enumerable)
        {
            return !enumerable.Any();
        }

        /// <summary>
        /// Determines whether the specified <see cref="IDictionary{TKey, TValue}"/><paramref name="dictionary"/> is NOT empty.
        /// </summary>
        /// <typeparam name="TKey">Specifies the key type of <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">Specifies the value type of <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">Specifies the <see cref="IDictionary{TKey, TValue}"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if the specified <see cref="IDictionary{TKey, TValue}"/> is NOT empty; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNotEmpty<TKey, TValue>(IDictionary<TKey, TValue?> dictionary)
        {
            return IsNotEmpty(collection: dictionary);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ISet{T}"/><paramref name="set"/> is NOT empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="set"/>.</typeparam>
        /// <param name="set">Specifies the <see cref="ISet{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="ISet{T}"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmpty<TElement>(ISet<TElement> set)
        {
            return IsNotEmpty(collection: set);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ICollection{T}"/><paramref name="collection"/> is NOT empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="collection"/>.</typeparam>
        /// <param name="collection">Specifies the <see cref="ICollection{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="ICollection{T}"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmpty<TElement>(ICollection<TElement> collection)
        {
            return collection.Count > 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Array"/><paramref name="array"/> is NOT empty using <see cref="Array.LongLength"/>.
        /// </summary>
        /// <param name="array">Specifies the <see cref="Array"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="Array"/> is NOT empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNotEmptyLong<TElement>(TElement[] array)
        {
            return array.LongLength > 0L;
        }

        /// <summary>
        /// Determines whether the specified instance <paramref name="instance"/> is not null.
        /// </summary>
        /// <typeparam name="T">Specifies the <see cref="Type"/> of <paramref name="instance"/>.</typeparam>
        /// <param name="instance">Specifies the instance of <see cref="Type"/><typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified instance <paramref name="instance"/> is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull<T>(T? instance) where T : class
        {
            return instance is not null;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Nullable{T}"/><paramref name="nullable"/> is not null where <typeparamref
        /// name="T"/> is a <see langref="struct"/>.
        /// </summary>
        /// <typeparam name="T">Specifies the <see cref="Type"/> of <paramref name="nullable"/>.</typeparam>
        /// <param name="nullable">Specifies the <see cref="Nullable{T}"/> of <see cref="Type"/><typeparamref name="T"/>.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Nullable{T}"/><paramref name="nullable"/> is not null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNull<T>(T? nullable) where T : struct
        {
            return nullable is not null;
        }

        /// <summary>
        /// Determines whether the specified instance <paramref name="instance"/> is null.
        /// </summary>
        /// <typeparam name="T">Specifies the <see cref="Type"/> of <paramref name="instance"/>.</typeparam>
        /// <param name="instance">Specifies the instance of <see cref="Type"/><typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified instance <paramref name="instance"/> is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(T? instance) where T : class
        {
            return instance is null;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Nullable{T}"/><paramref name="nullable"/> is null where <typeparamref
        /// name="T"/> is a <see langref="struct"/>.
        /// </summary>
        /// <typeparam name="T">Specifies the <see cref="Type"/> of <paramref name="nullable"/>.</typeparam>
        /// <param name="nullable">Specifies the <see cref="Nullable{T}"/> of <see cref="Type"/><typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Nullable{T}"/><paramref name="nullable"/> is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(T? nullable) where T : struct
        {
            return nullable is null;
        }

        public static bool IsNullOrAll(StringBuilder? builder, Func<char, bool> predicate)
        {
            return IsNullOrEmpty(builder) || (builder.All(predicate));
        }

        public static bool IsNullOrAll<TElement>(TElement[]? array, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(array) || (array!.All(predicate));
        }

        public static bool IsNullOrAll<TElement>(IEnumerable<TElement>? enumerable, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(enumerable) || (enumerable!.All(predicate));
        }

        public static bool IsNullOrAll<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary, Func<KeyValuePair<TKey, TValue?>, bool> predicate)
        {
            return IsNullOrEmpty(dictionary) || (dictionary!.All(predicate));
        }

        public static bool IsNullOrAll<TElement>(ICollection<TElement>? collection, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(collection) || (collection!.All(predicate));
        }

        public static bool IsNullOrAll<TElement>(ISet<TElement>? set, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(set) || (set!.All(predicate));
        }

        public static bool IsNullOrAny(StringBuilder? builder, Func<char, bool> predicate)
        {
            return IsNullOrEmpty(builder) || (builder.Any(predicate));
        }

        public static bool IsNullOrAny<TElement>(TElement[]? array, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(array) || (array!.Any(predicate));
        }

        public static bool IsNullOrAny<TElement>(IEnumerable<TElement>? enumerable, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(enumerable) || (enumerable!.Any(predicate));
        }

        public static bool IsNullOrAny<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary, Func<KeyValuePair<TKey, TValue?>, bool> predicate)
        {
            return IsNullOrEmpty(dictionary) || (dictionary!.Any(predicate));
        }

        public static bool IsNullOrAny<TElement>(ICollection<TElement>? collection, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(collection) || (collection!.All(predicate));
        }

        public static bool IsNullOrAny<TElement>(ISet<TElement>? set, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(set) || (set!.Any(predicate));
        }

        /// <summary>
        /// Determines whether the specified <see cref="IEnumerable{T}"/><paramref name="enumerable"/> is null or empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the element type of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">Specifies the <see cref="IEnumerable{T}"/> under test.</param>
        /// <returns><see langref="true"/> if the specified <see cref="IEnumerable{T}"/> is null or empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty<TElement>(IEnumerable<TElement>? enumerable)
        {
            return IsNull(enumerable) || IsEmpty(enumerable!);
        }

        /// <summary> Determines whether the specific <see cref="StringBuilder"/> <paramref name="builder"/> is <see
        /// langref="null"/> or empty. </summary> <param name="builder">Specifies the <see cref="StringBuilder"/ under
        /// test.></param> <returns><see langref="true"/> if the specified <see cref="StringBuilder"/> <paramref name="builder"/> is
        /// <see langref="null"/> or empty; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty(StringBuilder? builder)
        {
            return IsNull(builder) || IsEmpty(builder!);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Array"/><paramref name="array"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <typeparam name="TElement">Specifies the <see cref="Type"/> of each element in <see cref="Array"/><paramref name="array"/>.</typeparam>
        /// <param name="array">Specifies the <see cref="Array"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if the specified <see cref="Array"/><paramref name="array"/> is <see langref="null"/> or empty;
        /// otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty<TElement>(TElement[]? array)
        {
            return IsNull(array) || IsEmpty(array: array!);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ICollection{T}"/><paramref name="collection"/> is <see langref="null"/> or empty.
        /// </summary>
        /// <typeparam name="TElement">Specified the <see cref="Type"/> of the elements of <paramref name="collection"/>.</typeparam>
        /// <param name="collection">Specifies the <see cref="ICollection{T}"/> under test.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or empty] [the specified <see cref="ICollection{T}"/>]; otherwise,
        /// <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty<TElement>(ICollection<TElement>? collection)
        {
            return IsNull(collection) || IsEmpty(collection!);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified dictionary].
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or empty] [the specified dictionary]; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary)
        {
            return IsNull(dictionary) || IsEmpty(collection: dictionary!);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified the set].
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="theSet">The set.</param>
        /// <returns><see langref="true"/> if [is <see langref="null"/> or empty] [the specified the set]; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmpty<TElement>(ISet<TElement>? set)
        {
            return IsNull(set) || IsEmpty(collection: set!);
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or empty] [the specified array].
        /// </summary>
        /// <typeparam name="TElement">The <see cref="Type"/> of the element.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns><see langref="true"/> if [is <see langref="null"/> or empty] [the specified array]; otherwise, <see langref="false"/>.</returns>
        public static bool IsNullOrEmptyLong<TElement>(TElement[]? array)
        {
            return IsNull(array) || IsEmptyLong(array: array!);
        }

        public static bool IsNullOrSingleton(StringBuilder? builder, Func<char, bool> predicate)
        {
            return IsNullOrEmpty(builder) || builder!.Count(predicate) == 1;
        }

        public static bool IsNullOrSingleton<TElement>(TElement[]? array, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(array) || array!.Count(predicate) == 1;
        }

        public static bool IsNullOrSingleton<TElement>(IEnumerable<TElement>? enumerable, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(enumerable) || enumerable!.Count(predicate) == 1;
        }

        public static bool IsNullOrSingleton<TKey, TValue>(IDictionary<TKey, TValue?>? dictionary, Func<KeyValuePair<TKey, TValue?>, bool> predicate)
        {
            return IsNullOrEmpty(dictionary) || dictionary!.Count(predicate) == 1;
        }

        public static bool IsNullOrSingleton<TElement>(ICollection<TElement>? collection, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(collection) || collection!.Count(predicate) == 1;
        }

        public static bool IsNullOrSingleton<TElement>(ISet<TElement>? set, Func<TElement, bool> predicate)
        {
            return IsNullOrEmpty(set) || (set!.Count(predicate) == 1;
        }

        /// <summary>
        /// Determines whether [is <see langref="null"/> or white space] [the specified builder].
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// <see langref="true"/> if [is <see langref="null"/> or white space] [the specified builder]; otherwise, <see langref="false"/>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(StringBuilder? builder)
        {
            return IsNullOrAll(builder, char.IsWhiteSpace);
        }
    }
}
