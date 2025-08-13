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

namespace MSBuild.ExtensionPack
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.CommandLine.Parsing;
    using System.Diagnostics;

    /// <summary>
    /// ShellExecute.
    /// </summary>
    internal sealed class ShellWrapper
    {
        #region Private Fields

        private readonly Stack<string> stdError = new();
        private readonly Stack<string> stdOut = new();

        #endregion Private Fields

        #region Private Methods

        private void StandardErrorHandler(object sendingProcess, DataReceivedEventArgs lineReceived)
        {
            // Collect the error output.
            if (!string.IsNullOrEmpty(lineReceived.Data))
            {
                // Add the text to the collected errors.
                this.stdError.Push(lineReceived.Data);
            }

            if (this.ErrorDataReceived is not null)
            {
                this.ErrorDataReceived(sendingProcess, lineReceived);
            }
        }

        private void StandardOutHandler(object sendingProcess, DataReceivedEventArgs lineReceived)
        {
            // Collect the command output.
            if (!string.IsNullOrEmpty(lineReceived.Data))
            {
                // Add the text to the collected output.
                this.stdOut.Push(lineReceived.Data);
            }

            if (this.OutputDataReceived is not null)
            {
                this.OutputDataReceived(sendingProcess, lineReceived);
            }
        }

        #endregion Private Methods

        #region Public Constructors

        public ShellWrapper(string filePath, IEnumerable<string> arguments)
            : this(filePath)
        {
            this.ArgumentList = arguments;
        }

        public ShellWrapper(string filePath, string arguments)
            : this(filePath, CommandLineParser.SplitCommandLine(arguments))
        {
        }

        public ShellWrapper(string filePath)
        {
            this.FilePath = filePath;
        }

        #endregion Public Constructors

        #region Public Events

        /// <summary>
        /// A proxy for <see cref="Process.ErrorDataReceived"/>.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> ErrorDataReceived;

        /// <summary>
        /// A proxy for <see cref="Process.OutputDataReceived"/>.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> OutputDataReceived;

        #endregion Public Events

        #region Public Properties

        public IEnumerable<string> ArgumentList { get; }

        /// <summary>
        /// Sets the arguments.
        /// </summary>
        public string Arguments { get; set; }

        public StringDictionary? EnvironmentVariables
        {
            get
            {
                return Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process) as StringDictionary;
            }
        }

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Sets the FilePath.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the standard error.
        /// </summary>
        public string? StandardError
        {
            get
            {
                string accumulator = string.Empty;
                this.stdError.ToList().ForEach(e => accumulator = string.Concat(accumulator, e));
                return accumulator;
            }
        }

        /// <summary>
        /// Gets the standard output.
        /// </summary>
        public string StandardOutput
        {
            get
            {
                string accumulator = string.Empty;
                this.stdOut.ToList().ForEach(o => accumulator = string.Concat(accumulator, o));
                return accumulator;
            }
        }

        /// <summary>
        /// Sets the working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>int</returns>
        public int Execute()
        {
            ProcessStartInfo startInfo = new(this.FilePath, this.Arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = this.WorkingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            foreach (string key in this.EnvironmentVariables!.Keys)
            {
                startInfo.EnvironmentVariables[key] = this.EnvironmentVariables?[key];
            }

            using var process = Process.Start(startInfo);

            if (process is null)
            {
                return -1;
            }

            // Set event handlers to asynchronously read the output. We need to do this to avoid deadlock conditions.
            process.OutputDataReceived += this.StandardOutHandler;
            process.ErrorDataReceived += this.StandardErrorHandler;

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // wait for exit after reading the streams to avoid deadlock
            process.WaitForExit(int.MaxValue);

            // get the exit code and release the process handle
            if (!process.HasExited)
            {
                // not exited yet exceeding our timeout so kill the process
                process.Kill();
                process.WaitForExit();
            }

            this.ExitCode = process.ExitCode;
            return this.ExitCode;
        }

        #endregion Public Methods
    }
}
