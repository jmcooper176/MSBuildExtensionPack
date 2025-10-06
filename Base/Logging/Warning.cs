namespace MSBuild.ExtensionPack.Base.Logging
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base.Interface;

    public static class Warning
    {
        #region Public Methods

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string message,
            params object?[] arguments)
        {
            log.LogWarning(message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string..
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            [AllowNull] string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies the <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string.
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            string subcategory,
            string warningCode,
            [AllowNull] string helpKeyword,
            string? helpLink,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="message">  Specifies the message to log.</param>
        /// <param name="arguments">Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string message,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogWarning(message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">      
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string..
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string warningCode,
            [AllowNull] string? helpKeyword,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">            Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="predicate">      
        /// Specifies a <see cref="Func{TResult}"/> that returns <see langword="true"/> if the warning should be logged.
        /// </param>
        /// <param name="subcategory">    
        /// Specifies the warning subcategory to log. For <see cref="BaseTask"/> or <see cref="BaseToolTask"/> derivatives, this is
        /// usually the same as <see cref="IBaseTask.TaskAction"/>.
        /// </param>
        /// <param name="warningCode">    
        /// Specifies the warning code to log which is usually the same as the resource identifier for a multi-lingual string.
        /// </param>
        /// <param name="helpKeyword">    Specifies the help keyword to log. The default is <c>MsBuild</c>.</param>
        /// <param name="helpLink">       Specifies the help link to log. The default is <see langword="null"/>.</param>
        /// <param name="message">        Specifies the message to log.</param>
        /// <param name="filePath">       Specifies the source file path where the warning to be logged occurred.</param>
        /// <param name="lineNumber">     
        /// Specifies the source beginning line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="columnNumber">   
        /// Specifies the source beginning column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endLineNumber">  
        /// Specifies the source ending line number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="endColumnNumber">
        /// Specifies the source ending column number in <paramref name="filePath"/> where the warning to be logged occurred.
        /// </param>
        /// <param name="arguments">      
        /// Specifies and <see cref="Array"/> of zero or more arguments to instantiate <paramref name="message"/>.
        /// </param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> predicate,
            string subcategory,
            string warningCode,
            [AllowNull] string helpKeyword,
            string? helpLink,
            string message,
            [CallerFilePath] string? filePath = null,
            [CallerLineNumber] int lineNumber = 0,
            int columnNumber = 0,
            int endLineNumber = 0,
            int endColumnNumber = 0,
            params object?[] arguments)
        {
            if (predicate.Invoke())
            {
                log.LogWarning(subcategory, warningCode, helpKeyword ?? "MSBuild", helpLink, filePath, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, arguments);
            }
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">      Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="exception">Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception)
        {
            log.LogWarningFromException(exception);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Exception exception,
            bool showStackTrace)
        {
            log.LogWarningFromException(exception, showStackTrace);
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">          Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="postPredicate">Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
        /// <param name="exception">    Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> postPredicate,
            Exception exception)
        {
            log.LogWarningFromException(exception);
            Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
        }

        /// <summary>
        /// Logs the <see cref="BaseTask"/> or <see cref="BaseToolTask"/>, and derived classes, task warnings.
        /// </summary>
        /// <param name="log">           Specifies he <see cref="TaskLoggingHelper"/> instance to use.</param>
        /// <param name="postPredicate"> Specifies the post-condition, which if <see langref="false"/> will throw <see cref="ArgumentException"/>.</param>
        /// <param name="exception">     Specifies the <see cref="Exception"/> to log as an warning.</param>
        /// <param name="showStackTrace">If <see langref="true"/>, the <see cref="Exception.StackTrace"/> will be logged.</param>
        /// <remarks>
        /// For <see cref="BaseTask"/>, use the <see cref="TaskLoggingHelper"/><c>Log</c> property. For <see cref="BaseToolTask"/>,
        /// use the <see cref="TaskLoggingHelper"/><c>LogPrivate</c> or <c>LogShared</c> property.
        /// </remarks>
        public static void LogTaskWarning(
            this TaskLoggingHelper log,
            Func<bool> postPredicate,
            Exception exception,
            bool showStackTrace)
        {
            log.LogWarningFromException(exception, showStackTrace);
            Contract.EnsuresOnThrow<ArgumentException>(!postPredicate.Invoke(), $"Parameter {nameof(postPredicate)} failed.");
        }
    }

    #endregion Public Methods
}
