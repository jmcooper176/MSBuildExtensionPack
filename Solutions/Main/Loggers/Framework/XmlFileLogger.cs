// This file is part of CycloneDX CLI Tool
//
// Licensed under the Apache License, Version 2.0 (the “License”); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an “AS IS”
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language
// governing permissions and limitations under the License.
//
// SPDX-License-Identifier: Apache-2.0 Copyright (c) OWASP Foundation. All Rights Reserved. Ignore Spelling: cyclonedx Cli
namespace MSBuild.ExtensionPack.Loggers
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// <para>This logger can be used to log in xml format</para>
    /// <para><b>Syntax:</b></para>
    /// <para>/l:XmlFileLogger,MSBuild.ExtensionPack.Loggers.dll;logfile=YOURLOGFILE;verbosity=YOURVERBOSITY;encoding=YOURENCODING</para>
    /// <para><b>Parameters:</b></para>
    /// <para>Logfile: A optional parameter that specifies the file in which to store the log information. Defaults to msbuild.xml</para>
    /// <para>
    /// Verbosity: An optional parameter that overrides the global verbosity setting for this file logger only. This enables you to
    ///            log to several loggers, each with a different verbosity. The verbosity setting is case sensitive.
    /// </para>
    /// <para>Encoding: An optional parameter that specifies the encoding for the file, for example, UTF-8.</para>
    /// </summary>
    public class XmlFileLogger : Logger
    {
        private static readonly char[] FileLoggerParameterDelimiters = [';'];
        private static readonly char[] FileLoggerParameterValueSplitCharacter = ['='];
        private Encoding encoding;
        private int errors;
        private string logFileName;
        private DateTime startTime;
        private int warnings;
        private XmlTextWriter xmlWriter;

        private static bool NotExpectedException(Exception e)
        {
            return e is not UnauthorizedAccessException && e is not ArgumentNullException && (e is not PathTooLongException && e is not DirectoryNotFoundException) && (e is not NotSupportedException && e is not ArgumentException && (e is not SecurityException && e is not IOException));
        }

        private void ApplyFileLoggerParameter(string parameterName, string parameterValue)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "LOGFILE":
                    this.logFileName = parameterValue;
                    break;

                case "VERBOSITY":
                    this.Verbosity = Enum.Parse<LoggerVerbosity>(parameterValue);
                    break;

                case "ENCODING":
                    try
                    {
                        this.encoding = Encoding.GetEncoding(parameterValue);
                    }
                    catch (ArgumentException exception)
                    {
                        throw new LoggerException(exception.Message, exception.InnerException, "MSB4128", null);
                    }

                    break;
            }
        }

        private void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            this.xmlWriter.WriteStartElement("warnings");
            this.xmlWriter.WriteValue(this.warnings);
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteStartElement("errors");
            this.xmlWriter.WriteValue(this.errors);
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteStartElement("starttime");
            this.xmlWriter.WriteValue(this.startTime.ToString(CultureInfo.CurrentCulture));
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteStartElement("endtime");
            this.xmlWriter.WriteValue(DateTime.UtcNow.ToString(CultureInfo.CurrentCulture));
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteStartElement("timeelapsed");
            TimeSpan s = DateTime.UtcNow - this.startTime;
            this.xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0}", s));
            this.xmlWriter.WriteEndElement();
            this.LogFinished();
        }

        private void BuildStarted(object sender, BuildStartedEventArgs e)
        {
            this.startTime = DateTime.UtcNow;
            this.LogStarted("build", string.Empty, string.Empty);
        }

        private void CustomBuildEventRaised(object sender, CustomBuildEventArgs e)
        {
            this.LogMessage("custom", e.Message, MessageImportance.Normal);
        }

        private void ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            this.errors++;
            this.LogErrorOrWarning("error", e.Message, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory);
        }

        private void InitializeFileLogger()
        {
            this.ParseFileLoggerParameters();
            try
            {
                this.xmlWriter = new XmlTextWriter(this.logFileName, this.encoding) { Formatting = Formatting.Indented };
                this.xmlWriter.WriteStartDocument();
                this.xmlWriter.WriteStartElement("build");
                this.xmlWriter.Flush();
            }
            catch (Exception exception) when (!NotExpectedException(exception))
            {
                string message = string.Format(CultureInfo.InvariantCulture, "Invalid File Logger File {0}. {1}", this.logFileName, exception.Message);
                this.xmlWriter?.Close();

                throw new LoggerException(message, exception.InnerException);
            }
        }

        private void LogErrorOrWarning(string messageType, string message, string code, string file, int line, int column, string subcategory)
        {
            this.xmlWriter.WriteStartElement(messageType);
            this.SetAttribute("code", code);
            this.SetAttribute("file", file);
            this.SetAttribute("line", line);
            this.SetAttribute("column", column);
            this.SetAttribute("subcategory", subcategory);
            this.SetAttribute("started", DateTime.UtcNow);
            this.WriteMessage(message, code != "Properties");
            this.xmlWriter.WriteEndElement();
        }

        private void LogFinished()
        {
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.Flush();
        }

        private void LogMessage(string messageType, string message, MessageImportance importance)
        {
            if (importance == MessageImportance.Low && this.Verbosity != LoggerVerbosity.Detailed && this.Verbosity != LoggerVerbosity.Diagnostic)
            {
                return;
            }

            if (importance == MessageImportance.Normal && (this.Verbosity == LoggerVerbosity.Minimal || this.Verbosity == LoggerVerbosity.Quiet))
            {
                return;
            }

            this.xmlWriter.WriteStartElement(messageType);
            this.SetAttribute("importance", importance);
            this.SetAttribute("started", DateTime.UtcNow);
            this.WriteMessage(message, false);
            this.xmlWriter.WriteEndElement();
        }

        private void LogStarted(string elementName, string stageName, string file)
        {
            if (elementName != "build")
            {
                this.xmlWriter.WriteStartElement(elementName);
            }

            this.SetAttribute(elementName == "project" ? "targets" : "name", stageName);
            this.SetAttribute("file", file);
            this.SetAttribute("started", DateTime.UtcNow);
            this.xmlWriter.Flush();
        }

        private void MessageRaised(object sender, BuildMessageEventArgs e)
        {
            this.LogMessage("message", e.Message, e.Importance);
        }

        private void ParseFileLoggerParameters()
        {
            if (this.Parameters != null)
            {
                string[] strArray = this.Parameters.Split(FileLoggerParameterDelimiters);
                foreach (string[] strArray2 in from t in strArray where t.Length > 0 select t.Split(FileLoggerParameterValueSplitCharacter))
                {
                    this.ApplyFileLoggerParameter(strArray2[0], strArray2.Length > 1 ? strArray2[1] : null);
                }
            }
        }

        private void ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            this.LogFinished();
        }

        private void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            this.LogStarted("project", e.TargetNames, e.ProjectFile);
            if (this.IsVerbosityAtLeast(LoggerVerbosity.Diagnostic))
            {
                this.xmlWriter.WriteStartElement("InitialProperties");
                SortedDictionary<string, string> sortedProperties = [];
                foreach (DictionaryEntry k in e.Properties.Cast<DictionaryEntry>())
                {
                    sortedProperties.Add(k.Key.ToString(), k.Value.ToString());
                }

                foreach (var p in sortedProperties)
                {
                    this.xmlWriter.WriteStartElement(p.Key);
                    this.xmlWriter.WriteCData(p.Value);
                    this.xmlWriter.WriteEndElement();
                }

                this.xmlWriter.WriteEndElement();
            }
        }

        private void SetAttribute(string name, object value)
        {
            if (value == null)
            {
                return;
            }

            Type t = value.GetType();
            if (t == typeof(int))
            {
                if (int.TryParse(value.ToString(), out int number))
                {
                    this.xmlWriter.WriteAttributeString(name, number.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (t == typeof(bool))
            {
                this.xmlWriter.WriteAttributeString(name, value.ToString());
            }
            else if (t == typeof(MessageImportance))
            {
                MessageImportance importance = (MessageImportance)value;
                this.xmlWriter.WriteAttributeString(name, importance.ToString());
            }
            else
            {
                string text = value.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    this.xmlWriter.WriteAttributeString(name, text);
                }
            }
        }

        private void TargetFinished(object sender, TargetFinishedEventArgs e)
        {
            this.LogFinished();
        }

        private void TargetStarted(object sender, TargetStartedEventArgs e)
        {
            this.LogStarted("target", e.TargetName, string.Empty);
        }

        private void TaskFinished(object sender, TaskFinishedEventArgs e)
        {
            this.LogFinished();
        }

        private void TaskStarted(object sender, TaskStartedEventArgs e)
        {
            this.LogStarted("task", e.TaskName, e.ProjectFile);
        }

        private void WarningRaised(object sender, BuildWarningEventArgs e)
        {
            this.warnings++;
            this.LogErrorOrWarning("warning", e.Message, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory);
        }

        private void WriteMessage(string message, bool escape)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            // Avoid CDATA in CDATA
            message = message.Replace("<![CDATA[", string.Empty);
            message = message.Replace("]]>", string.Empty);

            message = message.Replace("&", "&amp;");
            if (escape)
            {
                message = message.Replace("<", "&lt;");
                message = message.Replace(">", "&gt;");
            }

            this.xmlWriter.WriteCData(message);
        }

        /// <summary>
        /// Initialize Override
        /// </summary>
        /// <param name="eventSource">IEventSource</param>
        public override void Initialize(IEventSource eventSource)
        {
            this.logFileName = "msbuild.xml";
            this.encoding = Encoding.Default;

            this.InitializeFileLogger();

            eventSource.BuildFinished += this.BuildFinished;
            eventSource.BuildStarted += this.BuildStarted;
            eventSource.ErrorRaised += this.ErrorRaised;
            eventSource.WarningRaised += this.WarningRaised;

            if (this.Verbosity != LoggerVerbosity.Quiet)
            {
                eventSource.MessageRaised += this.MessageRaised;
                eventSource.CustomEventRaised += this.CustomBuildEventRaised;
                eventSource.ProjectStarted += this.ProjectStarted;
                eventSource.ProjectFinished += this.ProjectFinished;
            }

            if (this.IsVerbosityAtLeast(LoggerVerbosity.Normal))
            {
                eventSource.TargetStarted += this.TargetStarted;
                eventSource.TargetFinished += this.TargetFinished;
            }

            if (this.IsVerbosityAtLeast(LoggerVerbosity.Detailed))
            {
                eventSource.TaskStarted += this.TaskStarted;
                eventSource.TaskFinished += this.TaskFinished;
            }
        }

        /// <summary>
        /// Shutdown() is guaranteed to be called by MSBuild at the end of the build, after all events have been raised.
        /// </summary>
        public override void Shutdown()
        {
            this.xmlWriter?.Close();
        }
    }
}
