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
namespace FileSystem.Path
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using MSBuild.ExtensionPack.Base;
    using MSBuild.ExtensionPack.ErrorMessage.Message;
    using MSBuild.ExtensionPack.FileSystem.Version;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>Increment</i> ( <b>Required:</b> File <b>Optional:</b> Increment <b>Output:</b> Value)</para>
    /// <para><i>Reset</i> ( <b>Required:</b> File <b>Optional:</b> Value <b>Output:</b> Value)</para>
    /// <para><b>Remote Execution Support:</b> No</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" DefaultTargets="Default" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<Target Name="Default">
    ///<!-- Perform a default increment of 1 -->
    ///<MSBuild.ExtensionPack.FileSystem.FileVersion TaskAction="Increment" File="C:\a\MyVersionfile.txt">
    ///<Output TaskParameter="Value" PropertyName="NewValue"/>
    ///</MSBuild.ExtensionPack.FileSystem.FileVersion>
    ///<Message Text="$(NewValue)"/>
    ///<!-- Perform an increment of 5 -->
    ///<MSBuild.ExtensionPack.FileSystem.FileVersion TaskAction="Increment" File="C:\a\MyVersionfile2.txt" Increment="5">
    ///<Output TaskParameter="Value" PropertyName="NewValue"/>
    ///</MSBuild.ExtensionPack.FileSystem.FileVersion>
    ///<Message Text="$(NewValue)"/>
    ///<!-- Reset a file value -->
    ///<MSBuild.ExtensionPack.FileSystem.FileVersion TaskAction="Reset" File="C:\a\MyVersionfile3.txt" Value="10">
    ///<Output TaskParameter="Value" PropertyName="NewValue"/>
    ///</MSBuild.ExtensionPack.FileSystem.FileVersion>
    ///<Message Text="$(NewValue)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class FileVersion : BaseTask
    {
        private string taskAction;

        /// <summary>
        /// The file to store the incrementing version in.
        /// </summary>
        [Required]
        public ITaskItem File { get; set; }

        /// <summary>
        /// Value to increment by. Default is '0.0.0.1' which has the effect of incrementing the Revision part of the version number
        /// by 1.
        /// </summary>
        public string Increment { get; set; } = "0.0.0.1";

        [Required]
        public override string TaskAction
        {
            get
            {
                return taskAction;
            }

            set
            {
                taskAction = ValidateTaskAction(value) ? value : taskAction;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the value read from the file, or used to reset the value in the file. Default is '0.0.0.0'.
        /// </summary>
        [Output]
        public string Value { get; set; }

        public static void ClearSemanticVersion(TaskLoggingHelper log, FileInfo versionFile, Encoding encoding)
        {
            WriteSemanticVersion(log, versionFile, new SemanticVersion(0, 0, 0), encoding);
        }

        public static void ClearVersion(TaskLoggingHelper log, FileInfo versionFile, Encoding encoding)
        {
            WriteVersion(log, versionFile, new Version(0, 0, 0, 0), encoding);
        }

        public static Encoding GetEncoding(TaskLoggingHelper log, FileInfo versionFile, Encoding encoding)
        {
            try
            {
                using StreamReader streamReader = new(versionFile.FullName, encoding, detectEncodingFromByteOrderMarks: true);
                return streamReader.CurrentEncoding;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is NotSupportedException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, versionFile.FullName);
                return encoding;
            }
        }

        public static SemanticVersion GetSemanticVersion(TaskLoggingHelper log, FileInfo versionFile)
        {
            try
            {
                using StreamReader streamReader = new(versionFile.FullName, GetEncoding(log, versionFile, Encoding.UTF8));
                return SemanticVersion.TryParse(streamReader.ReadLine(), out SemanticVersion? result) ? result : new(0, 0, 0);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is NotSupportedException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, versionFile.FullName);
                return new(0, 0, 0);
            }
        }

        public static Version GetVersion(TaskLoggingHelper log, FileInfo versionFile)
        {
            try
            {
                using StreamReader streamReader = new(versionFile.FullName, GetEncoding(log, versionFile, Encoding.UTF8));
                return Version.TryParse(streamReader.ReadLine(), out Version? result) ? result : new(0, 0, 0, 0);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is NotSupportedException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, versionFile.FullName);
                return new(0, 0, 0, 0);
            }
        }

        public static SemanticVersion IncrementSemanticVersion(TaskLoggingHelper log, FileInfo versionFile, Version increment)
        {
            SemanticVersion v1 = GetSemanticVersion(log, versionFile);
            var v2 = new SemanticVersion(increment);

            return new(v1.Major + v2.Major, v1.Minor + v2.Minor, v1.Patch + v2.Patch, v1.PreReleaseLabel, v1.BuildLabel);
        }

        public static Version IncrementVersion(TaskLoggingHelper log, FileInfo versionFile, Version increment)
        {
            Version currentValue = GetVersion(log, versionFile);
            var v1 = new QuickFixEngineering(currentValue);
            var v2 = new QuickFixEngineering(increment);

            return v1 + v2;
        }

        public static void ResetVersion(TaskLoggingHelper log, FileInfo versionFile, Encoding encoding, Version defaultVersion)
        {
            WriteVersion(log, versionFile, defaultVersion, encoding);
        }

        public static void SetReadOnly(TaskLoggingHelper log, FileInfo file)
        {
            log.LogTaskMessage(() => true, MessageImportance.Low, "Making File Read-Only:  {0}", file.FullName);

            try
            {
                file.IsReadOnly = file.Exists ? !file.IsReadOnly || true : throw new FileNotFoundException($"Parameter {nameof(file)} not found.", file.FullName);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, file.FullName);
            }
        }

        public static void SetWriteable(TaskLoggingHelper log, FileInfo file)
        {
            log.LogTaskMessage(() => true, MessageImportance.Low, "Making File Writeable:  {0}", file.FullName);

            try
            {
                file.IsReadOnly = file.Exists ? !file.IsReadOnly && false : throw new FileNotFoundException($"Parameter {nameof(file)} not found.", file.FullName);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, file.FullName);
            }
        }

        public static FileInfo TouchFile(TaskLoggingHelper log, FileInfo targetFile)
        {
            if (!targetFile.Exists)
            {
                try
                {
                    targetFile.Create().Flush();
                }
                catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
                {
                    log.LogTaskError(ex, showStackTrace: false, showDetail: true, targetFile.FullName);
                    throw;
                }
            }
            else
            {
                try
                {
                    targetFile.LastWriteTime = DateTime.UtcNow;
                }
                catch (Exception ex) when (ex is IOException || ex is PlatformNotSupportedException || ex is ArgumentOutOfRangeException)
                {
                    log.LogTaskError(ex, showStackTrace: false, showDetail: true, targetFile.FullName);
                }
            }

            return targetFile;
        }

        public static void WriteSemanticVersion(TaskLoggingHelper log, FileInfo versionFile, SemanticVersion value, Encoding encoding)
        {
            using StreamWriter streamWriter = new(versionFile.FullName, append: false, encoding);

            try
            {
                streamWriter.Write(value.ToString());
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is NotSupportedException || ex is IOException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, versionFile.FullName);
            }
        }

        public static void WriteVersion(TaskLoggingHelper log, FileInfo versionFile, Version value, Encoding encoding)
        {
            using StreamWriter streamWriter = new(versionFile.FullName, append: false, encoding);

            try
            {
                streamWriter.Write(value.ToString());
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is NotSupportedException || ex is IOException)
            {
                log.LogTaskError(ex, showStackTrace: false, showDetail: true, versionFile.FullName);
            }
        }

        public override void TaskActionRouter([CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            FileInfo versionFile = new(this.File.ItemSpec);
            TouchFile(this.Log, versionFile);
            SetWriteable(this.Log, versionFile);
            Encoding fileEncoding = GetEncoding(this.Log, versionFile, Encoding.UTF8);

            switch (this.TaskAction.ToUpperInvariant())
            {
                case "INCREMENTTASKACTION":
                    Version increment = Version.TryParse(this.Increment, out Version? inc) ? inc : new Version(0, 0, 0, 1);
                    Version newValue = IncrementVersion(this.Log, versionFile, increment);
                    WriteVersion(this.Log, versionFile, newValue, fileEncoding);
                    this.Value = newValue.ToString();
                    break;

                case "CLEARTASKACTION":
                    ClearVersion(this.Log, versionFile, fileEncoding);
                    this.Value = string.Empty;
                    break;

                case "RESETTASKACTION":
                    Version resetValue = Version.TryParse(this.Value, out Version? val) ? val : new Version(0, 0, 0, 0);
                    ResetVersion(this.Log, versionFile, fileEncoding, resetValue);
                    this.Value = resetValue.ToString();
                    break;
            }

            SetReadOnly(this.Log, versionFile);
        }

        public override bool ValidateTaskAction(string taskAction)
        {
            return !string.IsNullOrWhiteSpace(taskAction)
                && taskAction.ToUpperInvariant() switch
                {
                    "INCREMENTTASKACTION" or "CLEARTASKACTION" or "RESETTASKACTION" => true,
                    _ => false,
                };
        }
    }
}
