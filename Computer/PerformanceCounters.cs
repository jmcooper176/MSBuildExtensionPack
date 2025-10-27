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
namespace Computer
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// <b>Valid TaskActions are:</b>
    /// <para><i>Add</i> ( <b>Required:</b> CategoryName, CounterList, CategoryHelp <b>Optional:</b> MultiInstance, KeepExistingCounters)</para>
    /// <para><i>CheckCategoryExists</i> ( <b>Required:</b> CategoryName <b>Optional:</b> MachineName)</para>
    /// <para><i>CheckCounterExists</i> ( <b>Required:</b> CategoryName, CounterName <b>Optional:</b> MachineName)</para>
    /// <para><i>GetValue</i> ( <b>Required:</b> CategoryName, CounterName <b>Output:</b> Value, MachineName)</para>
    /// <para><i>Remove</i> ( <b>Required:</b> CategoryName)</para>
    /// <para><b>Remote Execution Support:</b> Partial</para>
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
    ///<ItemGroup>
    ///<!-- Configure some perf counters -->
    ///<CounterList Include="foobar.A">
    ///<CounterName>ACounter</CounterName>
    ///<CounterHelp>A Custom Counter</CounterHelp>
    ///<CounterType>CounterTimer</CounterType>
    ///</CounterList>
    ///<CounterList Include="foobar.A">
    ///<CounterName>AnotherCounter</CounterName>
    ///<CounterHelp>Another Custom Counter</CounterHelp>
    ///<CounterType>CounterTimer</CounterType>
    ///</CounterList>
    ///</ItemGroup>
    ///<!-- Add a Performance Counter -->
    ///<MSBuild.ExtensionPack.Computer.PerformanceCounters TaskAction="Add" CategoryName="YourCustomCategory" CategoryHelp="This is a custom performance counter category" CounterList="@(CounterList)" MultiInstance="true" />
    ///<!-- Check whether a Category Exists -->
    ///<MSBuild.ExtensionPack.Computer.PerformanceCounters TaskAction="CheckCategoryExists" CategoryName="aYourCustomCategory">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.PerformanceCounters>
    ///<Message Text="aYourCustomCategory - $(DoesExist)"/>
    ///<!-- Check whether a Counter Exists -->
    ///<MSBuild.ExtensionPack.Computer.PerformanceCounters TaskAction="CheckCounterExists" CategoryName="aYourCustomCategory" CounterName="AnotherCounter">
    ///<Output TaskParameter="Exists" PropertyName="DoesExist"/>
    ///</MSBuild.ExtensionPack.Computer.PerformanceCounters>
    ///<Message Text="AnotherCounter - $(DoesExist)"/>
    ///<!-- Remove a Performance Counter -->
    ///<MSBuild.ExtensionPack.Computer.PerformanceCounters TaskAction="Remove" CategoryName="YourCustomCategory"/>
    ///<!-- Get a Performance Counter value-->
    ///<MSBuild.ExtensionPack.Computer.PerformanceCounters TaskAction="GetValue" CategoryName="Memory" CounterName="Available MBytes">
    ///<Output PropertyName="TheValue" TaskParameter="Value"/>
    ///</MSBuild.ExtensionPack.Computer.PerformanceCounters>
    ///<Message Text="Available MBytes: $(TheValue)"/>
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    /// <seealso cref="BaseTask" />
    public class PerformanceCounters : BaseTask
    {
        private const string AddTaskAction = "Add";
        private const string CheckCategoryExistsTaskAction = "CheckCategoryExists";
        private const string CheckCounterExistsTaskAction = "CheckCounterExists";
        private const string GetValueTaskAction = "GetValue";
        private const string RemoveTaskAction = "Remove";

        private static bool IsCounterAlreadyIncluded(ref CounterCreationDataCollection colCounterCreationData, string counterName)
        {
            return colCounterCreationData.Cast<CounterCreationData>().Any(objCreateCounter => objCreateCounter.CounterName == counterName);
        }

        private void Add()
        {
            this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Adding Performance Counter Category: {0}", this.CategoryName);
            CounterCreationDataCollection colCounterCreationData = new();
            colCounterCreationData.Clear();
            if (PerformanceCounterCategory.Exists(this.CategoryName))
            {
                if (this.KeepExistingCounters)
                {
                    this.IncludeExistingCounters(ref colCounterCreationData);
                }

                this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Removing Performance Counter Category: {0}", this.CategoryName);
                PerformanceCounterCategory.Delete(this.CategoryName);
            }

            foreach (ITaskItem counter in this.CounterList)
            {
                string counterName = counter.GetMetadata("CounterName");
                string counterHelp = counter.GetMetadata("CounterHelp");
                PerformanceCounterType counterType = Enum.Parse<PerformanceCounterType>(counter.GetMetadata("CounterType"));
                this.Log.LogTaskMessage(() => true, MessageImportance.Low, "Adding Performance Counter: {0}", counterName);
                CounterCreationData objCreateCounter = new CounterCreationData(counterName, counterHelp, counterType);
                bool includeCounter = true;
                if (this.KeepExistingCounters)
                {
                    includeCounter = !IsCounterAlreadyIncluded(ref colCounterCreationData, counterName);
                }

                if (includeCounter)
                {
                    colCounterCreationData.Add(objCreateCounter);
                }
            }

            if (colCounterCreationData.Count > 0)
            {
                PerformanceCounterCategoryType categoryType = PerformanceCounterCategoryType.SingleInstance;

                if (this.MultiInstance)
                {
                    categoryType = PerformanceCounterCategoryType.MultiInstance;
                }

                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Creating Performance Counter Category: {0}", this.CategoryName);
                PerformanceCounterCategory.Create(this.CategoryName, this.CategoryHelp, categoryType, colCounterCreationData);
            }
        }

        private bool CheckCounterExists()
        {
            if (string.IsNullOrEmpty(this.CounterName))
            {
                this.Log.LogError("CounterName is required");
                return false;
            }

            if (!PerformanceCounterCategory.Exists(this.CategoryName, this.MachineName))
            {
                this.Log.LogTaskWarning("Performance Counter Category not found: {0}", this.CategoryName);
                return false;
            }

            PerformanceCounterCategory cat = new PerformanceCounterCategory(this.CategoryName, this.MachineName);
            PerformanceCounter[] counters = cat.GetCounters();
            return counters.Any(c => c.CounterName == this.CounterName);
        }

        private void GetValue()
        {
            if (PerformanceCounterCategory.Exists(this.CategoryName, this.MachineName))
            {
                this.Log.LogTaskMessage(
                    predicate: () => true,
                    messageImportance: MessageImportance.Low,
                    message: "Getting Performance Counter: {0} from: {1} on: {2}",
                    arguments: [this.CounterName, this.CategoryName, this.MachineName]);
                using PerformanceCounter pc = new(this.CategoryName, this.CounterName, null, this.MachineName);
                this.Value = pc.NextValue().ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Performance Counter Category not found: {0}", this.CategoryName);
            }
        }

        private void IncludeExistingCounters(ref CounterCreationDataCollection colCounterCreationData)
        {
            var category = PerformanceCounterCategory.GetCategories().FirstOrDefault(x => x.CategoryName == this.CategoryName);
            if (category is null)
            {
                return;
            }

            foreach (CounterCreationData objCreateCounter in category.GetCounters().Select(counter => new CounterCreationData(counter.CounterName, counter.CounterHelp, counter.CounterType)))
            {
                colCounterCreationData.Add(objCreateCounter);
            }
        }

        private void Remove()
        {
            if (PerformanceCounterCategory.Exists(this.CategoryName))
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Removing Performance Counter Category: {0}", this.CategoryName);
                PerformanceCounterCategory.Delete(this.CategoryName);
            }
            else
            {
                this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Performance Counter Category not found: {0}", this.CategoryName);
            }
        }

        /// <summary>
        /// Performs the action of this task.
        /// </summary>
        protected override void InternalExecute()
        {
            switch (this.TaskAction)
            {
                case AddTaskAction:
                    this.Add();
                    break;

                case RemoveTaskAction:
                    this.Remove();
                    break;

                case GetValueTaskAction:
                    this.GetValue();
                    break;

                case CheckCategoryExistsTaskAction:
                    this.Log.LogTaskMessage(() => true, MessageImportance.Normal, "Checking whether Performance Counter Category: {0} exists on : {1}", this.CategoryName, this.MachineName);
                    this.Exists = PerformanceCounterCategory.Exists(this.CategoryName, this.MachineName);
                    break;

                case CheckCounterExistsTaskAction:
                    this.Exists = this.CheckCounterExists();
                    break;

                default:
                    this.Log.LogError(string.Format(CultureInfo.CurrentCulture, "Invalid TaskAction passed: {0}", this.TaskAction));
                    return;
            }
        }

        /// <summary>
        /// Sets the description of the custom category.
        /// </summary>
        public string CategoryHelp { get; set; }

        /// <summary>
        /// Sets the CategoryName
        /// </summary>
        [Required]
        public string CategoryName { get; set; }

        /// <summary>
        /// Sets the TaskItem[] that specifies the counters to create as part of the new category.
        /// </summary>
        public IEnumerable<ITaskItem> CounterList { get; set; }

        /// <summary>
        /// Sets the name of the counter.
        /// </summary>
        public string CounterName { get; set; }

        /// <summary>
        /// Gets whether the item exists
        /// </summary>
        [Output]
        public bool Exists { get; set; }

        /// <summary>
        /// Sets a value whether existing performance counters of the given category should be preserved when adding new ones.
        /// </summary>
        public bool KeepExistingCounters { get; set; }

        /// <summary>
        /// Sets a value indicating whether to create a multiple instance performance counter. Default is false
        /// </summary>
        public bool MultiInstance { get; set; }

        /// <summary>
        /// Gets the value of the counter
        /// </summary>
        [Output]
        public string Value { get; set; }
    }
}
