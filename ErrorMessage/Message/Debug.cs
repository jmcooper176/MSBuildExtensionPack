namespace MSBuild.ExtensionPack.ErrorMessage.Message
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public static class Debug
    {
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
                System.Diagnostics.Debug.Fail(msg, detailedMessage);
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
    }
}
