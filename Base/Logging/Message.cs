namespace MSBuild.ExtensionPack.Base.Logging
{
    using System.Resources;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public static class Message
    {
        #region Public Methods

        /// <summary>
        /// Extracts the message code.
        /// </summary>
        /// <param name="log">                     The log.</param>
        /// <param name="message">                 The message.</param>
        /// <param name="messageWithoutCodePrefix">The message without code prefix.</param>
        /// <returns></returns>
        public static string ExtractMessageCode(this TaskLoggingHelper log, string message, out string? messageWithoutCodePrefix)
        {
            return log.ExtractMessageCode(message, out messageWithoutCodePrefix);
        }

        /// <summary>
        /// Formats the specified unformatted.
        /// </summary>
        /// <param name="log">        The log.</param>
        /// <param name="unformatted">The unformatted.</param>
        /// <param name="arguments">  The arguments.</param>
        /// <returns></returns>
        public static string Format(this TaskLoggingHelper log, string unformatted, params object?[] arguments)
        {
            return log.FormatString(unformatted, arguments);
        }

        /// <summary>
        /// Formats the resource.
        /// </summary>
        /// <param name="log">         The log.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="arguments">   The arguments.</param>
        /// <returns></returns>
        public static string FormatResource(this TaskLoggingHelper log, string resourceName, params object?[] arguments)
        {
            return log.FormatResourceString(resourceName, arguments);
        }

        /// <summary>
        /// Gets the help keyword prefix.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static string GetHelpKeywordPrefix(this TaskLoggingHelper log)
        {
            return log.HelpKeywordPrefix;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="log">         The log.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetMessage(this TaskLoggingHelper log, string resourceName)
        {
            return log.GetResourceMessage(resourceName);
        }

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static ResourceManager GetResourceManager(this TaskLoggingHelper log)
        {
            return log.TaskResources;
        }

        /// <summary>
        /// Logs the task error.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskError(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogErrorFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="subcategory">        The subcategory.</param>
        /// <param name="errorCode">          The error code.</param>
        /// <param name="helpKeyword">        The help keyword.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="filePath">           The file path.</param>
        /// <param name="lineNumber">         The line number.</param>
        /// <param name="columnNumber">       The column number.</param>
        /// <param name="endLineNumber">      The end line number.</param>
        /// <param name="endColumnNumber">    The end column number.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskError(
            this TaskLoggingHelper log,
            string subcategory,
            string errorCode,
            string? helpKeyword,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.SetHelpKeywordPrefix(helpKeyword ?? "MSBuild");
            log.LogErrorFromResources(subcategory, errorCode, log.GetHelpKeywordPrefix(), filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error with code.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskErrorWithCode(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogErrorWithCodeFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task error with code.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="subcategory">        The subcategory.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="filePath">           The file path.</param>
        /// <param name="lineNumber">         The line number.</param>
        /// <param name="columnNumber">       The column number.</param>
        /// <param name="endLineNumber">      The end line number.</param>
        /// <param name="endColumnNumber">    The end column number.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskErrorWithCode(
            this TaskLoggingHelper log,
            string subcategory,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogErrorWithCodeFromResources(subcategory, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskMessage(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            TaskMessage.LogTaskMessage(log, MessageImportance.Normal, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task message.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="importance">         The importance.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskMessage(this TaskLoggingHelper log, MessageImportance importance, string messageResourceName, params object?[] arguments)
        {
            log.LogMessageFromResources(importance, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskWarning(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogWarningFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="subcategory">        The subcategory.</param>
        /// <param name="warningCode">        The warning code.</param>
        /// <param name="helpKeyword">        The help keyword.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="filePath">           The file path.</param>
        /// <param name="lineNumber">         The line number.</param>
        /// <param name="columnNumber">       The column number.</param>
        /// <param name="endLineNumber">      The end line number.</param>
        /// <param name="endColumnNumber">    The end column number.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            string? helpKeyword,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.SetHelpKeywordPrefix(helpKeyword ?? "MSBuild");
            log.LogWarningFromResources(subcategory, warningCode, log.GetHelpKeywordPrefix(), filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning with code.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskWarningWithCode(this TaskLoggingHelper log, string messageResourceName, params object?[] arguments)
        {
            log.LogWarningWithCodeFromResources(messageResourceName, arguments);
        }

        /// <summary>
        /// Logs the task warning with code.
        /// </summary>
        /// <param name="log">                The log.</param>
        /// <param name="subcategory">        The subcategory.</param>
        /// <param name="messageResourceName">Name of the message resource.</param>
        /// <param name="filePath">           The file path.</param>
        /// <param name="lineNumber">         The line number.</param>
        /// <param name="columnNumber">       The column number.</param>
        /// <param name="endLineNumber">      The end line number.</param>
        /// <param name="endColumnNumber">    The end column number.</param>
        /// <param name="arguments">          The arguments.</param>
        public static void LogTaskWarningWithCode(
            this TaskLoggingHelper log,
            string subcategory,
            string messageResourceName,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogWarningWithCodeFromResources(subcategory, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, messageResourceName, arguments);
        }

        /// <summary>
        /// Sets the help keyword prefix.
        /// </summary>
        /// <param name="log">              The log.</param>
        /// <param name="helpKeywordPrefix">The help keyword prefix.</param>
        public static void SetHelpKeywordPrefix(this TaskLoggingHelper log, string helpKeywordPrefix)
        {
            log.HelpKeywordPrefix = helpKeywordPrefix;
        }

        /// <summary>
        /// Sets the resource manager.
        /// </summary>
        /// <param name="log">         The log.</param>
        /// <param name="taskResource">The task resource.</param>
        public static void SetResourceManager(this TaskLoggingHelper log, ResourceManager taskResource)
        {
            log.TaskResources = taskResource;
        }

        #endregion Public Methods
    }
}
