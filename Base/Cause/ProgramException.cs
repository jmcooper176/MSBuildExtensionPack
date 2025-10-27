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
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

using MSBuild.ExtensionPack.Base.Enumeration;

namespace MSBuild.ExtensionPack.Base.Cause
{
    /// <summary>
    /// Implements <see cref="ProgramException"/>.
    /// </summary>
    /// <seealso cref="Exception"/>
    public class ProgramException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramException"/> class.
        /// </summary>
        /// <param name="path">           Specifies the source file path for this <see cref="Exception"/>.</param>
        /// <param name="lineNumber">     Specifies the line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="columnNumber">   Specifies the column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endLineNumber">  Specifies the final line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endColumnNumber">Specifies the final column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="memberName">     Specifies the member name in <paramref name="path"/> for the cause of this <see cref="Exception"/>.</param>
        public ProgramException(
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerMemberName] string? memberName = null)
            : this($"An Exception '{nameof(ProgramException)}' has been thrown.", null, path, lineNumber, columnNumber, endLineNumber, endColumnNumber, memberName)
        {
        }

        public ProgramException()
            : this($"An Exception '{nameof(ProgramException)}' has been thrown.", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramException"/> class.
        /// </summary>
        /// <param name="message">        Specifies the message string to override part of <see cref="Message"/> with.</param>
        /// <param name="path">           Specifies the source file path for this <see cref="Exception"/>.</param>
        /// <param name="lineNumber">     Specifies the line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="columnNumber">   Specifies the column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endLineNumber">  Specifies the final line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endColumnNumber">Specifies the final column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="memberName">     Specifies the member name in <paramref name="path"/> for the cause of this <see cref="Exception"/>.</param>
        public ProgramException(
            string? message,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerMemberName] string? memberName = null)
            : this(message, null, path, lineNumber, columnNumber, endLineNumber, endColumnNumber, memberName)
        {
        }

        public ProgramException(string? message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramException"/> class.
        /// </summary>
        /// <param name="message">        Specifies the message string to override part of <see cref="Message"/> with.</param>
        /// <param name="innerException"> Specifies the <see cref="Exception"/> cause of this <see cref="Exception"/>.</param>
        /// <param name="path">           Specifies the source file path for this <see cref="Exception"/>.</param>
        /// <param name="lineNumber">     Specifies the line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="columnNumber">   Specifies the column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endLineNumber">  Specifies the final line number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="endColumnNumber">Specifies the final column number origin in <paramref name="path"/> for this <see cref="Exception"/>.</param>
        /// <param name="memberName">     Specifies the member name in <paramref name="path"/> for the cause of this <see cref="Exception"/>.</param>
        public ProgramException(
            string? message,
            Exception? innerException,
            [CallerFilePath] string? path = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            [CallerMemberName] string? memberName = null)
            : base(message, innerException)
        {
            Application = Assembly.GetEntryAssembly()?.FullName ?? AppDomain.CurrentDomain.FriendlyName;
            FilePath = new(path!);
            HResult = HResultExtension.ToHResultCode(FacilityCode.FACILITY_WIN32, WinError.ERROR_FATAL_APP_EXIT);
            MemberName = memberName!;
            Origin = Tuple.Create(lineNumber, columnNumber, endLineNumber, endColumnNumber);
#if MAN_CRITICAL
            IsPersonCritical = true;
            RecommendAction = $"Aborting Application '{Application}'";
#else
            IsPersonCritical = false;
            RecommendedAction = $"Catch this '{this.GetType().Name}' exception; DO NOT allow it to reach the Application '{Application}' unprocessed.";
#endif
            Source = string.Format(
                CultureInfo.InvariantCulture,
                "{0}({1}, {2}, {3}, {4}) : {5}",
                FilePath.FullName,
                Origin.Item1,
                Origin.Item2,
                Origin.Item3,
                Origin.Item4,
                MemberName);

            Timeout = TimeSpan.FromSeconds(5.0);
            Thrown = DateTime.UtcNow;

            if (IsPersonCritical)
            {
                Message = string.Format(
                    CultureInfo.InvariantCulture,
                    "[{0}] {1} : Un-caught Exception '{2}' in MAN-CRITICAL Program '{3}'.  {4}.  {5}.",
                    Thrown,
                    Source,
                    this.GetType().FullName,
                    Application,
                    message ?? "No message",
                    RecommendedAction);
            }
            else
            {
                Message = string.Format(
                    CultureInfo.InvariantCulture,
                    "[{0}] {1} : Un-caught Exception '{2}' in Program '{3}'.  {4}.",
                    Thrown,
                    Source,
                    this.GetType().FullName,
                    Application,
                    RecommendedAction);
            }
        }

        public ProgramException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets a value indicating the application name or path.
        /// </summary>
        /// <value>A <see cref="string"/> representing the application name or path.</value>
        public string Application { get; }

        public FileInfo FilePath { get; }
        public bool IsPersonCritical { get; }
        public string MemberName { get; }
        public override string Message { get; }
        public Tuple<int, int, int, int> Origin { get; }
        public Func<CancellationToken, int>? Process { get; set; }
        public string RecommendedAction { get; }
        public DateTime Thrown { get; }
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Calls the with timeout asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The <see cref="Type"/> of the result.</typeparam>
        /// <param name="processAsync">The process asynchronous.</param>
        /// <returns></returns>
        public virtual async Task<TResult?> CallWithTimeoutAsync<TResult>(Func<CancellationToken, Task>? processAsync)
        {
            static async Task<T?> NullTaskWithDelay<T>(TimeSpan delay)
            {
                await Task.Delay(delay);
                return default;
            }

            if (processAsync is null)
            {
                await Console.Error.WriteLineAsync($"No process passed for the Application '{Application}'.");
                return default;
            }

            using var tokenSource = new CancellationTokenSource();
            var taskToRun = processAsync(tokenSource.Token);
            var winner = (Task<TResult?>)await Task.WhenAny(processAsync(tokenSource.Token), NullTaskWithDelay<TResult>(Timeout));

            if (winner == taskToRun)
            {
                await Console.Out.WriteLineAsync($"Process passed for Application '{Application}' succeeded with Result '{winner.Result}'.");
                return winner.Result;
            }
            else
            {
                await Console.Error.WriteLineAsync($"Process passed for Application '{Application}' timed out after '{Timeout.TotalMilliseconds} ms'.");
                return winner.Result;
            }
        }

        public virtual bool TryCallWithTimeout<TResult>(Func<CancellationToken, TResult>? process, out TResult? result)
        {
            if (process is null)
            {
                Console.Error.WriteLine("No process passed for the Application '{0}'.", Application);
                result = default;
                return false;
            }

            using var tokenSource = new CancellationTokenSource(Timeout);

            try
            {
                result = process(tokenSource.Token);
                Console.WriteLine("Process passed for Application '{0}' succeeded with Result '{1}'.", Application, result);
                return true;
            }
            catch (TaskCanceledException)
            {
                Console.Error.WriteLine("Process passed for Application '{0}' timed out after '{1} ms'.", Application, Timeout.TotalMilliseconds);
                result = default;
                return false;
            }
        }
    }
}
