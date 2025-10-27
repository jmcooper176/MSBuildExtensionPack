namespace MSBuild.ExtensionPack.ErrorMessage.Message
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Utilities;

    public static class Critical
    {
        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task critical message.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="subcategory">    
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="code">           Specifies the critical code which is usually a resource identifier to multi-lingual string.</param>
        /// <param name="helpKeyword">    Specifies the help keyword.</param>
        /// <param name="message">        Specifies the critical message to log.</param>
        /// <param name="filePath">       Specifies the source file path for the critical message.</param>
        /// <param name="lineNumber">     Specifies the source beginning line number for the critical message.</param>
        /// <param name="columnNumber">   Specifies the source beginning column number for the critical message.</param>
        /// <param name="endLineNumber">  Specifies the source end line number for the critical message.</param>
        /// <param name="endColumnNumber">Specifies the source end column number for the critical message.</param>
        /// <param name="arguments">      Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogCriticalMessage(subcategory, code, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task critical message.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="subcategory">    
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="code">           Specifies the critical code which is usually a resource identifier to multi-lingual string.</param>
        /// <param name="helpKeyword">    Specifies the help keyword.</param>
        /// <param name="message">        Specifies the critical message to log.</param>
        /// <param name="filePath">       Specifies the source file path for the critical message.</param>
        /// <param name="lineNumber">     Specifies the source beginning line number for the critical message.</param>
        /// <param name="columnNumber">   Specifies the source beginning column number for the critical message.</param>
        /// <param name="endLineNumber">  Specifies the source end line number for the critical message.</param>
        /// <param name="endColumnNumber">Specifies the source end column number for the critical message.</param>
        /// <param name="arguments">      Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskCriticalMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string code,
            string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            Contract.Requires(predicate.Invoke(), string.Format(CultureInfo.CurrentCulture, "Parameter {0} must return true.", nameof(predicate)));

            if (!predicate.Invoke())
            {
                log.LogCriticalMessage(subcategory, code, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
            }
        }
    }
}
