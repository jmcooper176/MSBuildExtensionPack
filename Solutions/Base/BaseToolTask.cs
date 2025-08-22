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

// Ignore Spelling: Exe

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using MSBuild.ExtensionPack.Base.Validator;

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Security;
using System.Text;

namespace MSBuild.ExtensionPack.Base
{
    public abstract class BaseToolTask : ToolTask, IBaseTask, IDisposable
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseToolTask()
            : this(AssemblyResource.PrimaryResources, AssemblyResource.SharedResources, "MSBuild.")
        {
        }

        protected BaseToolTask(ResourceManager taskResources)
            : this(taskResources, taskResources, "MSBuild.")
        {
        }

        protected BaseToolTask(ResourceManager primaryResources, ResourceManager sharedResources, string helpKeywordPrefix)
        {
            LogPrivate = new TaskLoggingHelper(this)
            {
                TaskResources = primaryResources,
                HelpKeywordPrefix = helpKeywordPrefix,
            };

            LogShared = new TaskLoggingHelper(this)
            {
                TaskResources = sharedResources,
                HelpKeywordPrefix = helpKeywordPrefix,
            };

            foreach (DictionaryEntry item in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
            {
                if (!EnvironmentDictionary.TryAdd((string)item.Key, (string?)item.Value))
                {
                    this.LogPrivate.LogWarning("Failed to add Key {0} Value {1} to Environment Dictionary.", item.Key, item.Value);
                }
            }
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected new static TimeSpan TaskProcessTerminationTimeout => TimeSpan.FromSeconds(5.0);

        protected IDictionary<string, string?> EnvironmentDictionary { get; set; } = new Dictionary<string, string?>();

        /// <inheritdoc/>
        protected override bool HasLoggedErrors => base.HasLoggedErrors;

        protected TaskLoggingHelper LogPrivate { get; }

        protected TaskLoggingHelper LogShared { get; }

        /// <inheritdoc/>
        protected override Encoding ResponseFileEncoding => base.ResponseFileEncoding;

        /// <inheritdoc/>
        protected override Encoding StandardErrorEncoding => base.StandardErrorEncoding;

        /// <inheritdoc/>
        protected override MessageImportance StandardErrorLoggingImportance => base.StandardErrorLoggingImportance;

        /// <inheritdoc/>
        protected override Encoding StandardOutputEncoding => base.StandardOutputEncoding;

        /// <inheritdoc/>
        protected override MessageImportance StandardOutputLoggingImportance => base.StandardOutputLoggingImportance;

        protected CancellationToken ToolCanceled { get; private set; }

        /// <inheritdoc/>
        protected override string ToolName => Path.GetFileName(ToolPath ?? ToolExe);

        #endregion Protected Properties

        #region Protected Methods

        /// <inheritdoc/>
        protected override string AdjustCommandsForOperatingSystem(string input)
        {
            StringBuilder buffer = new(input);

            if (OperatingSystem.IsWindows())
            {
                return buffer.ToString();
            }
            else
            {
                int index = 0;

                foreach (var chunk in buffer.GetChunks())
                {
                    index = chunk[index..].ToString().IndexOf("\\\"", StringComparison.Ordinal);

                    if (index > -1)
                    {
                        buffer.Replace('\\', Path.DirectorySeparatorChar, index, 1);
                    }
                    else
                    {
                        buffer.Replace('\\', Path.DirectorySeparatorChar);
                        break;
                    }
                }

                return buffer.ToString();
            }
        }

        /// <inheritdoc/>
        protected override bool CallHostObjectToExecute()
        {
            return base.CallHostObjectToExecute();
        }

        protected void DeleteTempFile(FileInfo? path)
        {
            int retries = 6;

            if (path is not null && !PreserveTempFiles)
            {
                while (path.Exists)
                {
                    try
                    {
                        path.Delete();
                    }
                    catch (IOException)
                    {
                        if (retries-- > 0)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5.0));
                            continue;
                        }
                    }
                    catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
                    {
                        throw;
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Delete the temp file used for the response file.
                    if (ResponseFile is not null)
                    {
                        DeleteTempFile(ResponseFile);
                    }

                    // get the exit code and release the process handle
                    if (ToolTaskProcess?.HasExited == false)
                    {
                        if (!ToolTaskProcess.WaitForExit(TimeSpan.FromSeconds(5.0)))
                        {
                            ToolTaskProcess.Kill(entireProcessTree: true);
                        }
                    }

                    ExitCode = ToolTaskProcess?.ExitCode ?? -1;

                    ToolTimer?.Dispose();

                    if (ToolTaskProcess is not null)
                    {
                        ToolTaskProcess.OutputDataReceived -= ReceiveStandardOutputData;
                        ToolTaskProcess.ErrorDataReceived -= ReceiveStandardErrorData;
                        ToolTaskProcess.Dispose();
                    }

                    StandardOutputData?.Clear();
                    StandardErrorData?.Clear();

                    // If the tool exited cleanly, but logged errors then assign a failing exit code (-1)
                    if (ExitCode == 0 && HasLoggedErrors)
                    {
                        ExitCode = -1;
                    }
                }
            }

            // TODO: set large fields to null
            ToolTimer = null;
            ToolTaskProcess = null;
            StandardOutputData = null;
            StandardErrorData = null;
            disposedValue = true;
        }

        /// <inheritdoc/>
        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            ArgumentNullException.ThrowIfNull(ToolTaskProcess, nameof(ToolTaskProcess));

            ProcessInitialize(pathToTool, responseFileCommands, commandLineCommands);
            ToolTaskProcess = StartToolProcess(ToolTaskProcess);

            // Close the input stream. This is done to prevent commands from blocking the build waiting for input from the user.
            if (OperatingSystem.IsWindows())
            {
                ToolTaskProcess.StandardInput.Dispose();
            }

            this.ProcessStarted();

            while (!ToolTaskProcess.HasExited)
            {
                if (ToolCanceled.IsCancellationRequested)
                {
                    Cancel();
                }

                if (YieldDuringToolExecution)
                {
                    BuildEngine3.Yield();
                }
            }

            return ExitCode;
        }

        /// <inheritdoc/>
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder buffer = new(quoteHyphensOnCommandLine: false, useNewLineSeparator: false);
            CommandLineCommands = AdjustCommandsForOperatingSystem(buffer.ToString());
            return CommandLineCommands;
        }

        /// <inheritdoc/>
        protected override string GenerateFullPathToTool()
        {
            return ToolPath ?? ToolExe ?? Path.GetFullPath(ToolName);
        }

        /// <inheritdoc/>
        protected override string GenerateResponseFileCommands()
        {
            ResponseFileCommands = ResponseFileEscape(base.GenerateResponseFileCommands());
            return ResponseFileCommands;
        }

        /// <inheritdoc/>
        protected override ProcessStartInfo GetProcessStartInfo(string pathToTool, string commandLineCommands, string responseFileSwitch)
        {
            return base.GetProcessStartInfo(pathToTool, commandLineCommands, responseFileSwitch);
        }

        /// <inheritdoc/>
        protected override string GetResponseFileSwitch(string responseFilePath)
        {
            return base.GetResponseFileSwitch(responseFilePath);
        }

        protected virtual FileInfo? GetTemporaryResponseFile(string responseFileCommands, out string responsFileSwitch)
        {
        }

        /// <inheritdoc/>
        protected override string GetWorkingDirectory()
        {
            return base.GetWorkingDirectory();
        }

        /// <inheritdoc/>
        protected override bool HandleTaskExecutionErrors()
        {
            return base.HandleTaskExecutionErrors();
        }

        /// <inheritdoc/>
        protected override HostObjectInitializationStatus InitializeHostObject()
        {
            return base.InitializeHostObject();
        }

        protected virtual void ProcessInitialize(string pathToTool, string responseFileCommands, string commandLineCommands)
        {
            ArgumentNullException.ThrowIfNull(ToolTaskProcess, nameof(ToolTaskProcess));
            ArgumentNullException.ThrowIfNull(LogPrivate, nameof(LogPrivate));

            ExitCode = -1;

            if (!UseCommandProcessor)
            {
                LogPathToTool(ToolName, pathToTool);
            }

            LogToolCommand(commandLineCommands);

            ResponseFile = GetTemporaryResponseFile(responseFileCommands, out string responseFileSwitch);

            ToolTaskProcess.StartInfo = GetProcessStartInfo(pathToTool, commandLineCommands, responseFileSwitch);

            ToolTaskProcess.EnableRaisingEvents = true;
            ToolTaskProcess.Exited += ReceiveExitNotification;
            ToolTaskProcess.StartInfo.RedirectStandardError = true;
            ToolTaskProcess.ErrorDataReceived += ReceiveStandardErrorData;
            ToolTaskProcess.StartInfo.RedirectStandardOutput = true;
            ToolTaskProcess.OutputDataReceived += ReceiveStandardOutputData;
        }

        /// <inheritdoc/>
        protected override void ProcessStarted()
        {
            base.ProcessStarted();
            ToolTaskProcess?.BeginErrorReadLine();
            ToolTaskProcess?.BeginOutputReadLine();
            ToolTimer = new(ReceiveTimeoutNotification, ToolTaskProcess, Timeout, System.Threading.Timeout.Infinite);
        }

        protected virtual void ReceiveDataItem(object? sender, DataReceivedEventArgs? e, Queue<string> collection, Action<Queue<string>, string> append)
        {
            if (e?.Data is not null && collection is not null)
            {
                lock (collection.ToArray().SyncRoot)
                {
                    append.Invoke(collection, e.Data);
                }
            }
        }

        protected new void ReceiveExitNotification(object? sender, EventArgs? e)
        {
            try
            {
                ToolTaskProcess?.WaitForExit();

                while (ToolTaskProcess?.HasExited == false)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100L));
                }

                ToolTaskProcess?.CancelOutputRead();
                ToolTaskProcess?.CancelErrorRead();

                while (StandardErrorData.Count > 0)
                {
                    this.LogPrivate.LogMessageFromText(StandardErrorData.Dequeue(), StandardErrorLoggingImportance);
                }

                while (StandardOutputData.Count > 0)
                {
                    if (!SuppressTaskMessages)
                    {
                        this.LogPrivate.LogMessageFromText(StandardOutputData.Dequeue(), StandardOutputLoggingImportance);
                    }
                    else
                    {
                        StandardOutputData.Clear();
                    }
                }
            }
            finally
            {
                if (ToolTaskProcess is not null)
                {
                    ToolTaskProcess.Exited -= ReceiveExitNotification;
                }

                Dispose();
            }
        }

        protected new void ReceiveStandardErrorData(object? sender, DataReceivedEventArgs? e)
        {
            ReceiveDataItem(sender, e, StandardErrorData, (c, i) => c.Enqueue(i));
        }

        protected new void ReceiveStandardOutputData(object? sender, DataReceivedEventArgs? e)
        {
            ReceiveDataItem(sender, e, StandardOutputData, (c, i) => c.Enqueue(i));
        }

        protected virtual void ReceiveTimeoutNotification(object? state)
        {
            if (((Process?)state)?.HasExited == false)
            {
                Cancel();
            }
        }

        /// <inheritdoc/>
        protected override string ResponseFileEscape(string responseString)
        {
            return base.ResponseFileEscape(responseString);
        }

        /// <inheritdoc/>
        protected override bool SkipTaskExecution()
        {
            return base.SkipTaskExecution();
        }

        /// <inheritdoc/>
        protected override Process StartToolProcess(Process proc)
        {
            return Process.Start(proc.StartInfo)
                ?? throw new InvalidOperationException(
                    $"Tool Task {proc.ProcessName} failed to start.",
                    new Win32Exception($"Process {proc.Id} failed to start."));
        }

        /// <inheritdoc/>
        protected override bool ValidateParameters()
        {
            NullValidator defaultValidator = new($"Method {nameof(ValidateParameters)} failed validating one of the properties/arguments.");

            return defaultValidator.IsValid(this.canBeIncremental, nameof(this.canBeIncremental))
            && defaultValidator.IsValid(this.EchoOff, nameof(this.EchoOff))
            && defaultValidator.IsValid(this.EnvironmentVariables, nameof(this.EnvironmentVariables))
            && defaultValidator.IsValid(this.ErrorOnDeprecated, nameof(this.ErrorOnDeprecated))
            && defaultValidator.IsValid(this.FailIfNotIncremental, nameof(this.FailIfNotIncremental))
            && defaultValidator.IsValid(this.HelpKeywordPrefix, nameof(this.HelpKeywordPrefix))
            && defaultValidator.IsValid(this.HostObject, nameof(this.HostObject))
            && defaultValidator.IsValid(this.LogExceptionDetail, nameof(this.LogExceptionDetail))
            && defaultValidator.IsValid(this.LogExceptionStackTrace, nameof(this.LogExceptionStackTrace))
            && defaultValidator.IsValid(this.LogStandardErrorAsError, nameof(this.LogStandardErrorAsError))
            && defaultValidator.IsValid(this.StandardErrorImportance, nameof(this.StandardErrorImportance))
            && defaultValidator.IsValid(this.StandardOutputImportance, nameof(this.StandardOutputImportance))
            && defaultValidator.IsValid(this.TaskAction, nameof(this.TaskAction))
            && defaultValidator.IsValid(this.TaskProcessTerminationTimeout, nameof(this.TaskProcessTerminationTimeout))
            && defaultValidator.IsValid(this.TaskResources, nameof(this.TaskResources))
            && defaultValidator.IsValid(this.Timeout, nameof(this.Timeout))
            && defaultValidator.IsValid(this.ToolExe, nameof(this.ToolExe))
            && defaultValidator.IsValid(this.ToolPath, nameof(this.ToolPath))
            && defaultValidator.IsValid(this.UseCommandProcessor, nameof(this.UseCommandProcessor))
            && defaultValidator.IsValid(this.UseUtf8Encoding, nameof(this.UseUtf8Encoding))
            && defaultValidator.IsValid(this.YieldDuringToolExecution, nameof(this.YieldDuringToolExecution))
            && base.ValidateParameters();
        }

        #endregion Protected Methods

        #region Internal Properties

        internal string CommandLineCommands { get; set; }
        internal bool EventsDisposed { get; set; }
        internal FileInfo? ResponseFile { get; private set; }
        internal string ResponseFileCommands { get; set; }
        internal Queue<string>? StandardErrorData { get; set; } = new();
        internal Queue<string>? StandardOutputData { get; set; } = new();
        internal FileInfo? TemporaryBatchFile { get; set; }
        internal bool TerminatedTool { get; set; }
        internal Process? ToolTaskProcess { get; set; } = new();
        internal Timer? ToolTimer { get; set; }

        #endregion Internal Properties

        #region Public Properties

        public new IEnumerable<string> EnvironmentVariables { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether set <see cref="TaskLoggingHelper.HasLoggedErrors"/> if the task has been
        /// deprecated; otherwise, a warning will be logged.
        /// </summary>
        public bool ErrorOnDeprecated { get; set; }

        public new int ExitCode { get; private set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether a trapped <see cref="Exception"/> will log details.
        /// </summary>
        public bool LogExceptionDetail { get; set; } = Logging.GetLogExceptionDetail();

        /// <summary>
        /// Gets or sets a value indicating whether a trapped <see cref="Exception"/> will log the full stack trace.
        /// </summary>
        public bool LogExceptionStackTrace { get; set; } = Logging.GetLogExceptionStackTrace();

        public bool PreserveTempFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to suppress all Message logging by tasks; otherwise, all messages will be logged.
        /// </summary>
        /// <remarks>Errors and Warnings are never affected.</remarks>
        public bool SuppressTaskMessages { get; set; } = Logging.GetSuppressTaskMessages();

        /// <summary>
        /// Gets or sets a value indicating the task action string.
        /// </summary>
        public virtual string? TaskAction { get; set; }

        /// <inheritdoc/>
        public override int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <inheritdoc/>
        public override string ToolExe
        {
            get => base.ToolExe ?? Path.GetFullPath(ToolName);
            set => base.ToolExe = value;
        }

        #endregion Public Properties

        #region Public Methods

        /// <inheritdoc/>
        public override void Cancel()
        {
            if (!ToolCanceled.IsCancellationRequested)
            {
                using CancellationTokenSource source = new();
                source.Cancel();
                ToolCanceled = source.Token;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override bool Execute()
        {
            // Let the tool validate its parameters.
            if (!ValidateParameters())
            {
                return false;
            }

            if (EnvironmentVariables is not null)
            {
                foreach (string entry in EnvironmentVariables)
                {
                    string[] nameValuePair = entry.Split("=", 2);

                    if (nameValuePair.Length == 1 || (nameValuePair.Length == 2 && nameValuePair[0].Length == 0))
                    {
                        LogPrivate.LogErrorWithCodeFromResources("ToolTask.InvalidEnvironmentParameter", nameValuePair[0]);
                        return false;
                    }

                    if (!EnvironmentDictionary.TryAdd(nameValuePair[0], nameValuePair[1]))
                    {
                        EnvironmentDictionary[nameValuePair[0]] = nameValuePair[1];
                    }
                }
            }

            // Assign standard stream logging importance
            if (!AssignStandardStreamLoggingImportance())
            {
                return false;
            }

            try
            {
                if (SkipTaskExecution())
                {
                    // the task has said there's no command-line that we need to run, so return true to indicate this task completed
                    // successfully (without doing any actual work).
                    return true;
                }
                else if (canBeIncremental && FailIfNotIncremental)
                {
                    LogPrivate.LogErrorWithCodeFromResources("ToolTask.NotUpToDate");
                    return false;
                }

                string commandLineCommands = GenerateCommandLineCommands();
                // If there are response file commands, then we need a response file later.
                string batchFileContents = commandLineCommands;
                string responseFileCommands = GenerateResponseFileCommands();

                if (UseCommandProcessor)
                {
                    if (OperatingSystem.IsWindows())
                    {
                        ToolExe = "cmd.exe";
                        // Generate the temporary batch file May throw IO-related exceptions
                        TemporaryBatchFile = FileUtilities.GetTemporaryFile(".cmd");
                    }
                    else
                    {
                        ToolExe = "/bin/sh";
                        // Generate the temporary batch file May throw IO-related exceptions
                        TemporaryBatchFile = FileUtilities.GetTemporaryFile(".sh");
                    }

                    using StreamWriter sw = TemporaryBatchFile.AppendText();

                    if (!OperatingSystem.IsWindows())
                    {
                        // Use sh rather than bash, as not all 'nix systems necessarily have Bash installed
                        sw.WriteLine("#/bin/sh");
                        sw.Write(AdjustCommandsForOperatingSystem(commandLineCommands));

                        commandLineCommands = $"\"{TemporaryBatchFile.FullName}\"";
                    }
                    else
                    {
                        Encoding encoding = Console.OutputEncoding;

                        if (encoding.CodePage != Encoding.ASCII.CodePage)
                        {
                            // cmd.exe reads the first line in the console CP, which for a new console (as here) is OEMCP this
                            // string should ideally always be ASCII and the same in any OEMCP.
                            sw.WriteLine($@"%SystemRoot%\System32\chcp.com {encoding.CodePage}>nul", Console.OutputEncoding);
                        }

                        sw.Write(commandLineCommands, encoding);

                        string batchFileForCommandLine = TemporaryBatchFile.FullName;

                        // /D: Do not load AutoRun configuration from the registry (perf)
                        commandLineCommands = $"/D /C \"{batchFileForCommandLine}\"";

                        if (EchoOff)
                        {
                            commandLineCommands = $"/Q {commandLineCommands}";
                        }
                    }
                }

                // ensure the command line arguments string is not null
                if (string.IsNullOrEmpty(commandLineCommands))
                {
                    commandLineCommands = string.Empty;
                }
                // add a leading space to the command line arguments (if any) to separate them from the tool path
                else
                {
                    commandLineCommands = $" {commandLineCommands}";
                }

                // Initialize the host object. At this point, the task may elect to not proceed. Compiler tasks do this for purposes
                // of up-to-date checking in the IDE.
                HostObjectInitializationStatus nextAction = InitializeHostObject();

                if (nextAction == HostObjectInitializationStatus.NoActionReturnSuccess)
                {
                    return true;
                }
                else if (nextAction == HostObjectInitializationStatus.NoActionReturnFailure)
                {
                    ExitCode = 1;
                    return HandleTaskExecutionErrors();
                }

                string pathToTool = ComputePathToTool();

                if (pathToTool == null)
                {
                    // An appropriate error should have been logged already.
                    return false;
                }

                // Log the environment. We do this up here, rather than later where the environment is set, so that it appears
                // before the command line is logged.
                bool alreadyLoggedEnvironmentHeader = false;

                // New style environment overrides
                if (EnvironmentDictionary is not null)
                {
                    foreach (var variable in EnvironmentDictionary)
                    {
                        alreadyLoggedEnvironmentHeader = LogEnvironmentVariable(alreadyLoggedEnvironmentHeader, variable.Key, variable.Value);
                    }
                }

                commandLineCommands = AdjustCommandsForOperatingSystem(commandLineCommands);
                responseFileCommands = AdjustCommandsForOperatingSystem(responseFileCommands);

                if (UseCommandProcessor)
                {
                    // Log that we are about to invoke the specified command.
                    LogToolCommand($"{pathToTool}{commandLineCommands}");
                    LogToolCommand(batchFileContents);
                }
                else
                {
                    // Log that we are about to invoke the specified command.
                    LogToolCommand($"{pathToTool}{commandLineCommands} {responseFileCommands}");
                }

                ExitCode = 0;

                if (nextAction == HostObjectInitializationStatus.UseHostObjectToExecute)
                {
                    // The hosting IDE passed in a host object to this task. Give the task a chance to call this host object to do
                    // the actual work.
                    try
                    {
                        if (!CallHostObjectToExecute())
                        {
                            ExitCode = 1;
                        }
                    }
                    catch (Exception e)
                    {
                        LogPrivate.LogErrorFromException(e);
                        return false;
                    }
                }
                else if (nextAction == HostObjectInitializationStatus.UseAlternateToolToExecute)
                {
                    throw new InvalidOperationException("Invalid return status");
                }
                else
                {
                    // No host object was provided, or at least not one that supports all of the switches/parameters we need. So
                    // shell out to the command-line tool.
                    ExitCode = ExecuteTool(pathToTool, responseFileCommands, commandLineCommands);
                }

                // Raise a comment event to notify that the process completed
                if (ToolTaskProcess?.HasExited == true)
                {
                    return false;
                }
                else if (ExitCode != 0)
                {
                    return HandleTaskExecutionErrors();
                }
                else
                {
                    return true;
                }
            }
            catch (ArgumentException e)
            {
                if (ToolTaskProcess?.HasExited == false)
                {
                    LogPrivate.LogErrorWithCodeFromResources("General.InvalidToolSwitch", ToolExe, e.ToString());
                }
                return false;
            }
            catch (Exception e) when (e is Win32Exception || e is IOException || e is UnauthorizedAccessException)
            {
                if (ToolTaskProcess?.HasExited == false)
                {
                    LogPrivate.LogErrorWithCodeFromResources("ToolTask.CouldNotStartToolExecutable", ToolExe, e.ToString());
                }
                return false;
            }
            finally
            {
                // Clean up after ourselves.
                if (TemporaryBatchFile != null && TemporaryBatchFile.Exists)
                {
                    DeleteTempFile(TemporaryBatchFile);
                }
            }
        }

        #endregion Public Methods
    }
}
