//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlCmdWrapper.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.SqlServer
{
    using System.Collections.Specialized;
    using System.Diagnostics;

    internal sealed class SqlCmdWrapper
    {
        private readonly System.Text.StringBuilder stdOut = new System.Text.StringBuilder();
        private readonly System.Text.StringBuilder stdError = new System.Text.StringBuilder();

        internal SqlCmdWrapper(string executable, string arguments, string workingDirectory)
        {
            Arguments = arguments;
            Executable = executable;
            WorkingDirectory = workingDirectory;
        }

        /// <summary>
        /// Gets the standard output.
        /// </summary>
        internal string StandardOutput => stdOut.ToString();

        /// <summary>
        /// Gets the standard error.
        /// </summary>
        internal string StandardError => stdError.ToString();

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        internal int ExitCode { get; private set; }

        /// <summary>
        /// Sets the working directory.
        /// </summary>
        internal string WorkingDirectory { get; set; }

        /// <summary>
        /// Sets the Executable.
        /// </summary>
        internal string Executable { get; set; }

        /// <summary>
        /// Sets the arguments.
        /// </summary>
        internal string Arguments { get; set; }

        internal NameValueCollection EnvironmentVariables { get; } = new NameValueCollection();

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>int</returns>
        public int Execute()
        {
            using (Process sqlCmdProcess = new Process())
            {
                try
                {
                    var startInfo = new ProcessStartInfo(Executable, Arguments)
                                        {
                                            CreateNoWindow = true,
                                            RedirectStandardError = true,
                                            RedirectStandardOutput = true,
                                            UseShellExecute = false,
                                            WorkingDirectory = WorkingDirectory
                    };

                    foreach (string key in EnvironmentVariables)
                    {
                        startInfo.EnvironmentVariables[key] = EnvironmentVariables[key];
                    }

                    sqlCmdProcess.StartInfo = startInfo;

                    // Set our event handlers to asynchronously read the output and errors. If
                    // we use synchronous calls we may deadlock when the StandardOut/Error buffer
                    // gets filled (only 4k size) and the called app blocks until the buffer
                    // is flushed.  This stops the buffers from getting full and blocking.
                    sqlCmdProcess.OutputDataReceived += StandardOutHandler;
                    sqlCmdProcess.ErrorDataReceived += StandardErrorHandler;

                    sqlCmdProcess.Start();
                    sqlCmdProcess.BeginOutputReadLine();
                    sqlCmdProcess.BeginErrorReadLine();
                    sqlCmdProcess.WaitForExit(int.MaxValue);
                    ExitCode = sqlCmdProcess.ExitCode;
                }
                finally
                {
                    try
                    {
                        // get the exit code and release the process handle
                        if (!sqlCmdProcess.HasExited)
                        {
                            // not exited yet within our timeout so kill the process
                            sqlCmdProcess.Kill();

                            while (!sqlCmdProcess.HasExited)
                            {
                                System.Threading.Thread.Sleep(50);
                            }
                        }
                    }
                    catch (System.InvalidOperationException)
                    {
                        // lets assume the process terminated ok and swallow here.
                    }
                }
            }

            return ExitCode;
        }

        private void StandardErrorHandler(object sendingProcess, DataReceivedEventArgs lineReceived)
        {
            // Collect the error output.
            if (!string.IsNullOrEmpty(lineReceived.Data))
            {
                // Add the text to the collected errors.
                stdError.AppendLine(lineReceived.Data);
            }
        }

        private void StandardOutHandler(object sendingProcess, DataReceivedEventArgs lineReceived)
        {
            // Collect the command output.
            if (!string.IsNullOrEmpty(lineReceived.Data))
            {
                // Add the text to the collected output.
                stdOut.AppendLine(lineReceived.Data);
            }
        }
    }
}
