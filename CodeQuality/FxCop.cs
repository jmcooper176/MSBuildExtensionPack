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

// Ignore Spelling: Ruleset Xsl Gac Fx

namespace CodeQuality
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The FxCop task provides a basic wrapper over FxCopCmd.exe. See http://msdn.microsoft.com/en-gb/library/bb429449(VS.80).aspx
    /// for more details.
    /// <para/>
    /// <b>Valid TaskActions are:</b>
    /// <para>
    /// <i>Analyze</i> ( <b>Required:</b> Project and / or Files, OutputFile <b>Optional:</b> DependencyDirectories, Imports, Rules,
    /// ShowSummary, UpdateProject, Verbose, UpdateProject, LogToConsole, Types, FxCopPath, ReportXsl, OutputFile, ConsoleXsl,
    /// Project, SearchGac, IgnoreInvalidTargets, Quiet, ForceOutput, AspNetOnly, IgnoreGeneratedCode, OverrideRuleVisibilities,
    /// FailOnMissingRules, SuccessFile, Dictionary, Ruleset, RulesetDirectory, References, AssemblyCompareMode <b>Output:</b>
    /// AnalysisFailed, OutputText, ExitCode)
    /// </para>
    /// <para><b>Remote Execution Support:</b> NA</para>
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
    ///<ItemGroup>
    ///<!--- Need to add to the dependencies because MSBuild.ExtensionPack.CodeQuality.StyleCop.dll references StyleCop -->
    ///<DependencyDirectories Include="c:\Program Files (x86)\MSBuild\Microsoft\StyleCop\v4.4"/>
    ///<!-- Define a bespoke set of rules to run. Prefix the Rules path with ! to treat warnings as errors -->
    ///<Rules Include="C:\Program Files (x86)\Microsoft Fxcop 10.0\Rules\DesignRules.dll"/>
    ///<Files Include="C:\Projects\MSBuildExtensionPack\Releases\4.0.1.0\Main\BuildBinaries\MSBuild.ExtensionPack.StyleCop.dll"/>
    ///</ItemGroup>
    ///<Target Name="Default">
    ///<!-- Call the task using a collection of files and all default rules -->
    ///<MSBuild.ExtensionPack.CodeQuality.FxCop TaskAction="Analyse" Files="@(Files)" OutputFile="c:\fxcoplog1.txt">
    ///<Output TaskParameter="AnalysisFailed" PropertyName="Result"/>
    ///</MSBuild.ExtensionPack.CodeQuality.FxCop>
    ///<Message Text="CA1 Failed: $(Result)"/>
    ///<!-- Call the task using a project file -->
    ///<MSBuild.ExtensionPack.CodeQuality.FxCop TaskAction="Analyse" Files="@(Files)" Project="C:\Projects\MSBuildExtensionPack\Releases\4.0.1.0\Main\Framework\XmlSamples\FXCop.FxCop" DependencyDirectories="@(DependencyDirectories)" OutputFile="c:\fxcoplog2.txt">
    ///<Output TaskParameter="AnalysisFailed" PropertyName="Result"/>
    ///</MSBuild.ExtensionPack.CodeQuality.FxCop>
    ///<Message Text="CA2 Failed: $(Result)"/>
    ///<!-- Call the task using a collection of files and bespoke rules. We can access the exact failure message using OutputText -->
    ///<MSBuild.ExtensionPack.CodeQuality.FxCop TaskAction="Analyse" Rules="@(Rules)" Files="@(Files)"  OutputFile="c:\fxcoplog3.txt" LogToConsole="true">
    ///<Output TaskParameter="AnalysisFailed" PropertyName="Result"/>
    ///<Output TaskParameter="OutputText" PropertyName="Text"/>
    ///</MSBuild.ExtensionPack.CodeQuality.FxCop>
    ///<Message Text="CA3 Failed: $(Result)"/>
    ///<Message Text="Failure Text: $(Text)" Condition="$(Result) == 'true'"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask"/>
    public class FxCop : BaseTask
    {
        private CompareMode assemblyCompareMode = CompareMode.StrongName;

        private void Analyze()
        {
            // if the output file exists, delete it.
            if (File.Exists(this.OutputFile))
            {
                File.Delete(this.OutputFile);
            }

            using Process proc = new();

            if (!string.IsNullOrEmpty(this.ReportXsl))
            {
                proc.StartInfo.ArgumentList.Add("/applyoutXsl /outXsl:\"" + this.ReportXsl + "\"");
            }

            if (this.LogToConsole)
            {
                proc.StartInfo.ArgumentList.Add(" /console");

                if (!string.IsNullOrEmpty(this.ConsoleXsl))
                {
                    proc.StartInfo.ArgumentList.Add(" /consoleXsl:\"" + this.ConsoleXsl + "\"");
                }
            }

            if (!string.IsNullOrEmpty(this.Ruleset))
            {
                proc.StartInfo.ArgumentList.Add(" /ruleset:\"" + this.Ruleset + "\"");
            }

            if (!string.IsNullOrEmpty(this.RulesetDirectory))
            {
                proc.StartInfo.ArgumentList.Add(" /rulesetdirectory:\"" + this.RulesetDirectory + "\"");
            }

            if (this.UpdateProject)
            {
                proc.StartInfo.ArgumentList.Add(" /update");
            }

            if (this.SearchGac)
            {
                proc.StartInfo.ArgumentList.Add(" /gac");
            }

            if (this.SuccessFile)
            {
                proc.StartInfo.ArgumentList.Add(" /successfile");
            }

            if (this.FailOnMissingRules)
            {
                proc.StartInfo.ArgumentList.Add(" /failonmissingrules");
            }

            if (this.IgnoreGeneratedCode)
            {
                proc.StartInfo.ArgumentList.Add(" /ignoregeneratedcode");
            }

            if (this.OverrideRuleVisibilities)
            {
                proc.StartInfo.ArgumentList.Add(" /overriderulevisibilities");
            }

            if (this.AspNetOnly)
            {
                proc.StartInfo.ArgumentList.Add(" /aspnet");
            }

            if (this.IgnoreInvalidTargets)
            {
                proc.StartInfo.ArgumentList.Add(" /ignoreinvalidtargets");
            }

            if (this.Timeout > 0)
            {
                proc.StartInfo.ArgumentList.Add(" /timeout:" + this.Timeout);
            }

            if (this.Quiet)
            {
                proc.StartInfo.ArgumentList.Add(" /quiet");
            }

            if (this.ForceOutput)
            {
                proc.StartInfo.ArgumentList.Add(" /forceoutput");
            }

            if (this.Dictionary is not null)
            {
                proc.StartInfo.ArgumentList.Add(" /dictionary:\"" + this.Dictionary.GetMetadata("FullPath") + "\"");
            }

            if (this.ShowSummary)
            {
                proc.StartInfo.ArgumentList.Add(" /summary");
            }

            if (this.Verbose)
            {
                proc.StartInfo.ArgumentList.Add(" /verbose");
            }

            if (this.assemblyCompareMode != CompareMode.StrongName)
            {
                proc.StartInfo.ArgumentList.Add(" /assemblyCompareMode:" + this.assemblyCompareMode.ToString());
            }

            if (!string.IsNullOrEmpty(this.Types))
            {
                proc.StartInfo.ArgumentList.Add(" /types:\"" + this.Types + "\"");
            }

            if (this.DependencyDirectories is not null)
            {
                foreach (ITaskItem i in this.DependencyDirectories)
                {
                    string path = i.ItemSpec;
                    if (path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase) || path.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                    {
                        path = path[..^1];
                    }

                    proc.StartInfo.ArgumentList.Add(" /directory:\"" + path + "\"");
                }
            }

            if (this.Imports is not null)
            {
                proc.StartInfo.ArgumentList.Add(this.Imports.Aggregate(string.Empty, (accumulator, i) => accumulator + (" /import:\"" + i.ItemSpec + "\"")));
            }

            if (this.Rules is not null)
            {
                proc.StartInfo.ArgumentList.Add(this.Rules.Aggregate(string.Empty, (accumulator, i) => accumulator + (" /rule:\"" + i.ItemSpec + "\"")));
            }

            if (string.IsNullOrEmpty(this.Project) && this.Files is null)
            {
                this.Log.LogError("A Project and / or Files collection must be passed.");
                return;
            }

            if (!string.IsNullOrEmpty(this.Project))
            {
                proc.StartInfo.ArgumentList.Add(" /project:\"" + this.Project + "\"");
            }

            if (this.Files is not null)
            {
                proc.StartInfo.ArgumentList.Add(this.Files.Aggregate(string.Empty, (accumulator, i) => accumulator + (" /file:\"" + i.ItemSpec + "\"")));
            }

            if (this.References is not null)
            {
                proc.StartInfo.ArgumentList.Add(this.References.Aggregate(string.Empty, (current, i) => current + (" /reference:\"" + i.ItemSpec + "\"")));
            }

            proc.StartInfo.ArgumentList.Add(" /out:\"" + this.OutputFile + "\"");

            proc.StartInfo.FileName = this.FxCopPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Running {0} {1}", proc.StartInfo.FileName, proc.StartInfo.Arguments);
            proc.Start();

            string outputStream = proc.StandardOutput.ReadToEnd();
            this.Log.LogTaskMessage(() => outputStream.Length > 0, MessageImportance.Normal, outputStream);

            if (outputStream.Length > 0)
            {
                this.OutputText = outputStream;
            }

            string errorStream = proc.StandardError.ReadToEnd();
            if (errorStream.Length > 0)
            {
                this.Log.LogTaskError(errorStream);
            }

            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                this.ExitCode = proc.ExitCode;
                this.Log.LogTaskError(proc.ExitCode.ToString(CultureInfo.CurrentCulture));
                this.AnalysisFailed = true;
                return;
            }

            this.AnalysisFailed = System.IO.File.Exists(this.OutputFile);
        }

        protected override void InternalExecute()
        {
            Initialize state = new(this.Log);

            if (state.IsLocalMachineOnly())
            {
                return;
            }

            if (string.IsNullOrEmpty(this.FxCopPath))
            {
                string? programFilePath = Environment.GetEnvironmentVariable("ProgramFiles");

                if (string.IsNullOrEmpty(programFilePath))
                {
                    this.Log.LogTaskError("Failed to read a value from the ProgramFiles Environment Variable");
                    return;
                }

                if (File.Exists(programFilePath + @"\Microsoft FxCop 1.36\FxCopCmd.exe"))
                {
                    this.FxCopPath = programFilePath + @"\Microsoft FxCop 1.36\FxCopCmd.exe";
                }
                else if (File.Exists(programFilePath + @"\Microsoft FxCop 10.0\FxCopCmd.exe"))
                {
                    this.FxCopPath = programFilePath + @"\Microsoft FxCop 10.0\FxCopCmd.exe";
                }
                else
                {
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "FxCopCmd.exe was not found in the default location. Use FxCopPath to specify it. Searched at: {0}", programFilePath + @"\Microsoft FxCop 1.36 and \Microsoft FxCop 10.0"));
                    return;
                }
            }

            switch (this.TaskAction)
            {
                case "Analyze":
                    this.Analyze();
                    break;

                default:
                    this.Log.LogTaskError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        public FxCop()
        {
            this.LogToConsole = true;
            this.ShowSummary = true;
        }

        /// <summary>
        /// Gets AnalysisFailed. True if FxCop logged Code Analysis errors to the Output file.
        /// </summary>
        [Output]
        public bool AnalysisFailed { get; set; }

        /// <summary>
        /// Set to true to analyze only ASP.NET-generated binaries and honor global suppressions in App_Code.dll for all assemblies
        /// under analysis. Default is false
        /// </summary>
        public bool AspNetOnly { get; set; }

        /// <summary>
        /// Set the assembly comparison mode. Supports None, StrongName, StrongNameIgnoringVersion. Default is StrongName.
        /// </summary>
        public string AssemblyCompareMode
        {
            get => this.assemblyCompareMode.ToString();
            set => this.assemblyCompareMode = Enum.Parse<CompareMode>(value, true);
        }

        /// <summary>
        /// Sets the ConsoleXsl (/consoleXsl option)
        /// </summary>
        public string ConsoleXsl { get; set; }

        /// <summary>
        /// Sets the DependencyDirectories :(/directory option)
        /// </summary>
        public IEnumerable<ITaskItem> DependencyDirectories { get; set; }

        /// <summary>
        /// Sets the custom dictionary used by spelling rules.Default is no custom dictionary
        /// </summary>
        public ITaskItem Dictionary { get; set; }

        /// <summary>
        /// The exit code returned from FxCop
        /// </summary>
        [Output]
        public int ExitCode { get; set; }

        /// <summary>
        /// Set to true to treat missing rules or rule sets as an error and halt execution. Default is false
        /// </summary>
        public bool FailOnMissingRules { get; set; }

        /// <summary>
        /// Sets the Item Collection of assemblies to analyse (/file option)
        /// </summary>
        public IEnumerable<ITaskItem> Files { get; set; }

        /// <summary>
        /// Set to true to write output XML and project files even in the case where no violations occurred. Default is false
        /// </summary>
        public bool ForceOutput { get; set; }

        /// <summary>
        /// Sets the path to FxCopCmd.exe. Default is [Program Files]\Microsoft FxCop 1.36\FxCopCmd.exe
        /// </summary>
        public string FxCopPath { get; set; }

        /// <summary>
        /// Set to true to suppress analysis results against generated code. Default is false
        /// </summary>
        public bool IgnoreGeneratedCode { get; set; }

        /// <summary>
        /// Set to true to silently ignore invalid target files. Default is false
        /// </summary>
        public bool IgnoreInvalidTargets { get; set; }

        /// <summary>
        /// Sets the name of an analysis report or project file to import (/import option)
        /// </summary>
        public IEnumerable<ITaskItem> Imports { get; set; }

        /// <summary>
        /// Set to true to direct analysis output to the console (/console option). Default is true
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Set the name of the file for the analysis report
        /// </summary>
        [Required]
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets the OutputText emitted during analysis
        /// </summary>
        [Output]
        public string OutputText { get; set; }

        /// <summary>
        /// Set to true to run all over-ridable rules against all targets. Default is false
        /// </summary>
        public bool OverrideRuleVisibilities { get; set; }

        /// <summary>
        /// Set the name of the .fxcop project to use
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Set to true to suppress all console output other than the reporting implied by /console or /consolexsl. Default is false
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// Sets the Item Collection of assemblies to reference (/reference option)
        /// </summary>
        public IEnumerable<ITaskItem> References { get; set; }

        /// <summary>
        /// Sets the ReportXsl (/outXsl: option)
        /// </summary>
        public string ReportXsl { get; set; }

        /// <summary>
        /// Sets the location of rule libraries to load (/rule option). Prefix the Rules path with ! to treat warnings as errors
        /// </summary>
        public IEnumerable<ITaskItem> Rules { get; set; }

        /// <summary>
        /// Specifies the Rule set to be used for the analysis. It can be a file path to the rule set file or the file name of a
        /// built-in rule set. '+' enables all rules in the rule set; '-' disables all rules in the rule set; '=' sets rules to
        /// match the rule set and disables all rules that are not enabled in the rule set
        /// </summary>
        public string Ruleset { get; set; }

        /// <summary>
        /// Specifies the directory to search for rule set files that are specified by the Ruleset switch or are included by one of
        /// the specified rule sets.
        /// </summary>
        public string RulesetDirectory { get; set; }

        /// <summary>
        /// Set to true to search the GAC for missing assembly references (/gac option). Default is false
        /// </summary>
        public bool SearchGac { get; set; }

        /// <summary>
        /// Set to true to display a summary (/summary option). Default is true
        /// </summary>
        public bool ShowSummary { get; set; }

        /// <summary>
        /// Set to true to create .lastcodeanalysissucceeded file in output report directory if no build-breaking messages occur
        /// during analysis. Default is false
        /// </summary>
        public bool SuccessFile { get; set; }

        /// <summary>
        /// Set the override timeout for analysis deadlock detection. Analysis will be aborted when analysis of a single item by a
        /// single rule exceeds the specified amount of time. Default is 0 to disable deadlock detection.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Specifies the types to analyze
        /// </summary>
        public string Types { get; set; }

        /// <summary>
        /// Saves the results of the analysis in the project file. This option is ignored if the /project option is not specified
        /// (/update option)
        /// </summary>
        public bool UpdateProject { get; set; }

        /// <summary>
        /// Set to true to output verbose information during analysis (/verbose option)
        /// </summary>
        public bool Verbose { get; set; }
    }
}
