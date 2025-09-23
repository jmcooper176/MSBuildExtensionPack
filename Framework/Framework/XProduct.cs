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
namespace MSBuild.ExtensionPack.Framework
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// This task creates a cross product of up to 10 ItemGroups
    /// <para><b>Remote Execution Support:</b> NA</para>
    /// </summary>
    /// <example>
    /// <code lang="xml">
    ///<![CDATA[
    ///<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///<PropertyGroup>
    ///<TPath>$(MSBuildProjectDirectory)\..\MSBuild.ExtensionPack.tasks</TPath>
    ///<TPath Condition="Exists('$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks')">$(MSBuildProjectDirectory)\..\..\Common\MSBuild.ExtensionPack.tasks</TPath>
    ///</PropertyGroup>
    ///<Import Project="$(TPath)"/>
    ///<ItemGroup>
    ///<AllConfigurations Include="Release">
    ///<Name>Release</Name>
    ///<Framework>net-3.5</Framework>
    ///<OutputDirectory>net-3.5\bin\release\</OutputDirectory>
    ///</AllConfigurations>
    ///<AllConfigurations Include="Debug">
    ///<Name>Debug</Name>
    ///<Framework>net-3.5</Framework>
    ///<OutputDirectory>net-3.5\bin\debug\</OutputDirectory>
    ///</AllConfigurations>
    ///<AllPlatforms Include="x86">
    ///<Use32Bit>True</Use32Bit>
    ///</AllPlatforms>
    ///<AllPlatforms Include="x64">
    ///<Use32Bit>False</Use32Bit>
    ///</AllPlatforms>
    ///<AllDatabaseSystems Include="SqlServerLocal"  Condition="'true' == 'true'">
    ///<DataSource>localhost\.</DataSource>
    ///<DatabaseDirectory>C:\Databases\.</DatabaseDirectory>
    ///</AllDatabaseSystems>
    ///<AllDatabaseSystems Include="SqlServer2005">
    ///<DataSource>localhost\MSSQL2005</DataSource>
    ///<DatabaseDirectory>C:\Databases\MsSql2005</DatabaseDirectory>
    ///</AllDatabaseSystems>
    ///</ItemGroup>
    ///<Target Name="Default">
    ///<MSBuild.ExtensionPack.Framework.XProduct IdentityFormat="{0}-{1}-{2}" Group1="@(AllConfigurations)" Group2="@(AllPlatforms)" Group3="@(AllDatabaseSystems)" >
    ///<Output ItemName="NewList" TaskParameter="Result" />
    ///<Output PropertyName="CountX" TaskParameter="Count" />
    ///</MSBuild.ExtensionPack.Framework.XProduct>
    ///<Message Text="Got $(CountX) configurations" />
    ///<Message Text="%(NewList.Identity)
    ///%(NewList.Name)
    ///%(NewList.Framework)
    ///%(NewList.OutputDirectory)
    ///%(NewList.Use32Bit)
    ///%(NewList.DataSource)
    ///%(NewList.DataBaseDirectory)" />
    ///</Target>
    ///</Project>
    ///]]>
    /// </code>
    /// </example>
    public class XProduct : Task
    {
        #region Private Methods

        private static void DoIdentity(ITaskItem item, string identityFormat, int number)
        {
            var replacements = item.ItemSpec.Split(';').Select((t, i) => new { Old = "{" + i + "}", New = t }).ToDictionary(x => x.Old, x => x.New);
            replacements["{0}"] = number.ToString(CultureInfo.InvariantCulture);

            var regex = new Regex(string.Join("|", replacements.Keys.Select(Regex.Escape)));
            item.ItemSpec = regex.Replace(identityFormat, m => replacements[m.Value]);
        }

        private static IEnumerable<ITaskItem> DoXProduct(IEnumerable<ITaskItem> group1, ITaskItem[] group2, int group2Number, bool addOriginalIdentityUsingGroupNumberSuffix)
        {
            foreach (var item1 in group1)
            {
                foreach (var item2 in group2)
                {
                    var newItem = new TaskItem(item1.ItemSpec + ";" + item2.ItemSpec);
                    item1.CopyMetadataTo(newItem);
                    item2.CopyMetadataTo(newItem);
                    if (addOriginalIdentityUsingGroupNumberSuffix)
                    {
                        newItem.SetMetadata("Identity" + group2Number, item2.ItemSpec);
                    }

                    yield return newItem;
                }
            }
        }

        private IEnumerable<ITaskItem[]> CreateDataArrays()
        {
            var allProperties = typeof(XProduct).GetProperties();
            var dataProperties = Enumerable.Range(1, 10).Select(i => allProperties.SingleOrDefault(p => p.Name == "Group" + i)).TakeWhile(x => x is not null);
            var datas = dataProperties.Select(p => p.GetValue(this, null)).Cast<ITaskItem[]>().TakeWhile(x => x is not null).ToArray();
            return datas;
        }

        #endregion Private Methods

        #region Public Properties

        /// <summary>
        /// Copies original Identity metadata to result item as well - suffixed by the group number, i.e. you can use <c>%(ResultList.Identity1)</c>.
        /// </summary>
        public bool AddOriginalIdentityUsingGroupNumberSuffix { get; set; }

        /// <summary>
        /// The number of items produced by the cross-product
        /// </summary>
        [Output]
        public int Count { get; set; }

        /// <summary>
        /// ItemGroup1
        /// </summary>
        public ITaskItem[] Group1 { get; set; }

        /// <summary>
        /// ItemGroup10
        /// </summary>
        public ITaskItem[] Group10 { get; set; }

        /// <summary>
        /// ItemGroup2
        /// </summary>
        public ITaskItem[] Group2 { get; set; }

        /// <summary>
        /// ItemGroup3
        /// </summary>
        public ITaskItem[] Group3 { get; set; }

        /// <summary>
        /// ItemGroup4
        /// </summary>
        public ITaskItem[] Group4 { get; set; }

        /// <summary>
        /// ItemGroup5
        /// </summary>
        public ITaskItem[] Group5 { get; set; }

        /// <summary>
        /// ItemGroup6
        /// </summary>
        public ITaskItem[] Group6 { get; set; }

        /// <summary>
        /// ItemGroup7
        /// </summary>
        public ITaskItem[] Group7 { get; set; }

        /// <summary>
        /// ItemGroup8
        /// </summary>
        public ITaskItem[] Group8 { get; set; }

        /// <summary>
        /// ItemGroup9
        /// </summary>
        public ITaskItem[] Group9 { get; set; }

        /// <summary>
        /// Specifies the format to use for the new ItemGroup names
        /// </summary>
        public string IdentityFormat { get; set; }

        /// <summary>
        /// The cross-product result output.
        /// </summary>
        [Output]
        public ITaskItem[] Result { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Execute()
        {
            var groups = this.CreateDataArrays().ToList();

            this.Result = new ITaskItem[] { new TaskItem() };
            for (var i = 0; i < groups.Count; ++i)
            {
                this.Result = DoXProduct(this.Result, groups[i], i + 1, this.AddOriginalIdentityUsingGroupNumberSuffix).ToArray();
            }

            this.Count = this.Result.Length;

            for (var i = 0; i < this.Result.Length; i++)
            {
                DoIdentity(this.Result[i], this.IdentityFormat ?? "{0}", i);
            }

            return !this.Log.HasLoggedErrors;
        }

        #endregion Public Methods
    }
}
