namespace MSBuild.ExtensionPack.Base.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    using Microsoft.Build.Framework;

    using Microsoft.Build.Utilities;

    public static class TaskMessage
    {
        #region Public Methods

        /// <summary>
        /// Logs the project finished.
        /// </summary>
        /// <param name="log">        The log.</param>
        /// <param name="message">    The message.</param>
        /// <param name="helpKeyword">The help keyword.</param>
        /// <param name="projectFile">The project file.</param>
        /// <param name="succeeded">  if set to <c>true</c> [succeeded].</param>
        public static void LogProjectFinished(this TaskLoggingHelper log, string message, string? helpKeyword, string projectFile, bool succeeded)
        {
            log.LogExternalProjectFinished(message, helpKeyword, projectFile, succeeded);
        }

        /// <summary>
        /// Logs the project started.
        /// </summary>
        /// <param name="log">        The log.</param>
        /// <param name="message">    The message.</param>
        /// <param name="helpKeyword">The help keyword.</param>
        /// <param name="projectFile">The project file.</param>
        /// <param name="succeeded">  if set to <c>true</c> [succeeded].</param>
        public static void LogProjectStarted(this TaskLoggingHelper log, string message, string? helpKeyword, string projectFile, string targetName)
        {
            log.LogExternalProjectStarted(message, helpKeyword, projectFile, targetName);
        }

        /// <summary>
        /// Logs the task file.
        /// </summary>
        /// <param name="log">     The log.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="content"> The content.</param>
        public static void LogTaskFile(this TaskLoggingHelper log, string filePath, string content)
        {
            log.LogTaskFile(new FileInfo(filePath), content);
        }

        /// <summary>
        /// Logs the task file.
        /// </summary>
        /// <param name="log">     The log.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="content"> The content.</param>
        public static void LogTaskFile(this TaskLoggingHelper log, string filePath, byte[] content)
        {
            log.LogTaskFile(new FileInfo(filePath), content);
        }

        /// <summary>
        /// Logs the task file <paramref name="path"/> after overwriting it with <paramref name="content"/>.
        /// </summary>
        /// <param name="log">    The log.</param>
        /// <param name="path">   The path.</param>
        /// <param name="content">The content.</param>
        /// <exception cref="IOException">Parameter 'path' with value '{path.FullName}' already exists.</exception>
        public static void LogTaskFile(this TaskLoggingHelper log, FileInfo path, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            log.LogTaskFile(path, Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// Logs the task file.
        /// </summary>
        /// <param name="log">    The log.</param>
        /// <param name="path">   The path.</param>
        /// <param name="content">The content.</param>
        public static void LogTaskFile(this TaskLoggingHelper log, FileInfo path, byte[] content)
        {
            if (content?.Length < 1)
            {
                return;
            }

            using FileStream fileStream = path.Open(path.Exists ? FileMode.Truncate : FileMode.CreateNew, FileAccess.Write, FileShare.None);
            fileStream.Write(content!, 0, content!.Length);
            log.LogTaskMessage(fileStream, MessageImportance.Normal);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">      Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            string message,
            params object?[] arguments)
        {
            log.LogMessage(message, arguments);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="textReader">The text reader.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, TextReader textReader, MessageImportance importance)
        {
            string? lineOfText = textReader.ReadLine();
            bool hasError = false;

            if (!log.LogsMessagesOfImportance(importance))
            {
                return false;
            }

            while (!string.IsNullOrEmpty(lineOfText))
            {
                if (log.LogTaskMessageText(lineOfText, importance))
                {
                    hasError = true;
                }

                lineOfText = textReader.ReadLine();
            }

            return hasError;
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, FileStream fileStream, MessageImportance importance)
        {
            if (!log.LogsMessagesOfImportance(importance))
            {
                return false;
            }

            using TextReader textReader = new StreamReader(fileStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: false);
            return log.LogTaskMessage(textReader, importance);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">     The log.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, string filePath)
        {
            return log.LogTaskMessage(new FileInfo(filePath), MessageImportance.Normal);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log"> The log.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, FileInfo path)
        {
            return log.LogTaskMessage(path, MessageImportance.Normal);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="filePath">  The file path.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, string filePath, MessageImportance importance)
        {
            return log.LogTaskMessage(new FileInfo(filePath), importance);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="path">      The path.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bool LogTaskMessage(this TaskLoggingHelper log, FileInfo path, MessageImportance importance)
        {
            if (!path.Exists)
            {
                Warning.LogTaskWarning(log, "Parameter 'path' with value '{0}' does not exist.", path.FullName);
                return false;
            }

            return log.LogTaskMessage(path.OpenText(), importance);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">       Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="importance">Specifies the <see cref="MessageImportance"/> of the message to log.</param>
        /// <param name="message">   Specifies the message to log.</param>
        /// <param name="arguments"> Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            MessageImportance importance,
            string message,
            params object?[] arguments)
        {
            if (!log.LogsMessagesOfImportance(importance))
            {
                return;
            }

            log.LogMessage(importance, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="subcategory">    
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="code">           Specifies the message code which is usually a resource identifier to multi-lingual string.</param>
        /// <param name="helpKeyword">    Specifies the help keyword. The default is <c>MSBuild</c>.</param>
        /// <param name="importance">     Specifies the <see cref="MessageImportance"/> of the message to log.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path for the message.</param>
        /// <param name="lineNumber">     Specifies the source beginning line number for the message.</param>
        /// <param name="columnNumber">   Specifies the source beginning column number for the message.</param>
        /// <param name="endLineNumber">  Specifies the source end line number for the message.</param>
        /// <param name="endColumnNumber">Specifies the source end column number for the message.</param>
        /// <param name="arguments">      Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            string subcategory,
            string code,
            string? helpKeyword,
            MessageImportance importance,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            if (!log.LogsMessagesOfImportance(importance))
            {
                return;
            }

            log.LogMessage(subcategory, code, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, importance, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">      Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="predicate">Specifies the predicate that must be true to log the message.</param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogMessage(message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">       Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="predicate"> Specifies the predicate that must be true to log the message.</param>
        /// <param name="importance">Specifies the <see cref="MessageImportance"/> of the message to log.</param>
        /// <param name="message">   Specifies the message to log.</param>
        /// <param name="arguments"> Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            MessageImportance importance,
            string message,
            params object?[] arguments)
        {
            if (predicate.Invoke() && log.LogsMessagesOfImportance(importance))
            {
                log.LogMessage(importance, message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivative task message.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> log.</param>
        /// <param name="predicate">      Specifies the predicate that must be true to log the message.</param>
        /// <param name="subcategory">    
        /// Specifies the subcategory. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is usually <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="code">           Specifies the message code which is usually a resource identifier to multi-lingual string.</param>
        /// <param name="helpKeyword">    Specifies the help keyword. The default is <c>MSBuild</c>.</param>
        /// <param name="importance">     Specifies the <see cref="MessageImportance"/> of the message to log.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path for the message.</param>
        /// <param name="lineNumber">     Specifies the source beginning line number for the message.</param>
        /// <param name="columnNumber">   Specifies the source beginning column number for the message.</param>
        /// <param name="endLineNumber">  Specifies the source end line number for the message.</param>
        /// <param name="endColumnNumber">Specifies the source end column number for the message.</param>
        /// <param name="arguments">      Specifies an <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskMessage(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string code,
            string? helpKeyword,
            MessageImportance importance,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke() && log.LogsMessagesOfImportance(importance))
            {
                log.LogMessage(subcategory, code, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, importance, message, arguments);
            }
        }

        /// <summary>
        /// Logs the task message text.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="lineOfText">The line of text.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bool LogTaskMessageText(this TaskLoggingHelper log, [AllowNull] string lineOfText, MessageImportance importance)
        {
            return !string.IsNullOrWhiteSpace(lineOfText)
                && log.LogsMessagesOfImportance(importance)
                && log.LogMessageFromText(lineOfText, importance);
        }

        /// <summary>
        /// Logs the task telemetry.
        /// </summary>
        /// <param name="log">       The log.</param>
        /// <param name="eventName"> Name of the event.</param>
        /// <param name="properties">The properties.</param>
        public static void LogTaskTelemetry(this TaskLoggingHelper log, string eventName, IDictionary<string, string> properties)
        {
            log.LogTelemetry(eventName, properties);
        }

        /// <summary>
        /// Logs the tool task command line.
        /// </summary>
        /// <param name="log">        The log.</param>
        /// <param name="commandLine">The command line.</param>
        public static void LogToolTaskCommandLine(this TaskLoggingHelper log, string commandLine)
        {
            log.LogToolTaskCommandLine(MessageImportance.Normal, commandLine);
        }

        /// <summary>
        /// Logs the tool task command line.
        /// </summary>
        /// <param name="log">        The log.</param>
        /// <param name="importance"> The importance.</param>
        /// <param name="commandLine">The command line.</param>
        public static void LogToolTaskCommandLine(this TaskLoggingHelper log, MessageImportance importance, string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine) || !log.LogsMessagesOfImportance(importance))
            {
                return;
            }

            log.LogCommandLine(importance, commandLine);
        }

        #endregion Public Methods
    }
}
