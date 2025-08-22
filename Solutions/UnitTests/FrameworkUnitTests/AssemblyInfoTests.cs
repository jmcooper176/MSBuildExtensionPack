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

namespace MSBuild.ExtensionPack.Framework.Tests
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;

    [TestClass]
    public class AssemblyInfoTests
    {
        #region Public Methods

        [TestMethod]
        public void Can_update_attribute()
        {
            string tempFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_normal_spacing.Temp.cs");
            try
            {
                System.IO.File.Copy(System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_normal_spacing.cs"), tempFile, overwrite: true);
                var assemblyInfoTask = new AssemblyInfo
                {
                    BuildEngine = new MockBuildEngine(),
                    AssemblyCompany = "Foo Bar Ltd.",
                    AssemblyInfoFiles = new ITaskItem[] { new TaskItem(tempFile), }
                };

                Assert.IsTrue(assemblyInfoTask.Execute());
            }
            finally
            {
                System.IO.File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void Can_update_attribute_when_single_quotes_appear_in_attribute_constructor()
        {
            string tempFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_mixed_spacing.Temp.cs");
            try
            {
                System.IO.File.Copy(System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_mixed_spacing.cs"), tempFile, overwrite: true);
                var assemblyInfoTask = new AssemblyInfo
                {
                    BuildEngine = new MockBuildEngine(),
                    AssemblyDescription = "Foo Bar Description.",
                    AssemblyInfoFiles = new ITaskItem[] { new TaskItem(tempFile), }
                };

                Assert.IsTrue(assemblyInfoTask.Execute());
            }
            finally
            {
                System.IO.File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void Can_update_attribute_when_spaces_appear_after_assembly_keyword()
        {
            string tempFile = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_mixed_spacing.Temp.cs");
            try
            {
                System.IO.File.Copy(System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "AssemblyInfo_mixed_spacing.cs"), tempFile, overwrite: true);
                var assemblyInfoTask = new AssemblyInfo
                {
                    BuildEngine = new MockBuildEngine(),
                    AssemblyCompany = "Foo Bar Ltd.",
                    AssemblyInfoFiles = new ITaskItem[] { new TaskItem(tempFile), }
                };

                Assert.IsTrue(assemblyInfoTask.Execute());
            }
            finally
            {
                System.IO.File.Delete(tempFile);
            }
        }

        #endregion Public Methods
    }
}
