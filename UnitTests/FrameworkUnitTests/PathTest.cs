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
namespace MSBuild.ExtensionPack.Framework.Tests
{
    using MSBuild.ExtensionPack;

    [TestClass]
    public class PathTest
    {
        #region Public Methods

        [TestMethod]
        public void Path_CantExecuteRemote()
        {
            // arrange
            Path target = new Path();
            target.Filepath = @"C:\myfile.myex";
            target.MachineName = "Another";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetExtension";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value is null);
        }

        [TestMethod]
        public void Path_ChangeExtension()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile.myex";
            target.Extension = "log";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "ChangeExtension";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == @"C:\myfile.log");
        }

        [TestMethod]
        public void Path_Combine()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile";
            target.Filepath2 = @"log.txt";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "Combine";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == @"C:\myfile\log.txt");
        }

        [TestMethod]
        public void Path_GetDirectoryName()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\mydir\myfile.txt";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetDirectoryName";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == @"C:\mydir");
        }

        [TestMethod]
        public void Path_GetExtension()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetExtension";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == ".myex");
        }

        [TestMethod]
        public void Path_GetFileName()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetFileName";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == "myfile.myex");
        }

        [TestMethod]
        public void Path_GetFileNameWithoutExtension()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetFileNameWithoutExtension";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == "myfile");
        }

        [TestMethod]
        public void Path_GetFullPath()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetFullPath";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == @"C:\myfile.myex");
        }

        [TestMethod]
        public void Path_GetPathRoot()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\mypath\mypath2\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetPathRoot";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == @"C:\");
        }

        [TestMethod]
        public void Path_GetTempPath()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "GetTempPath";

            // act
            target.Execute();

            // assert
            Assert.IsTrue(target.Value == System.IO.Path.GetTempPath());
        }

        [TestMethod]
        public void Path_HasExtensionFalse()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\mypath\mypath2\myfile";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "HasExtension";

            // act
            target.Execute();

            // assert
            Assert.AreEqual(target.Value, "False");
        }

        [TestMethod]
        public void Path_HasExtensionTrue()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"C:\mypath\mypath2\myfile.myex";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "HasExtension";

            // act
            target.Execute();

            // assert
            Assert.AreEqual(target.Value, "True");
        }

        [TestMethod]
        public void Path_InvalidTaskAction()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "NotValid";

            // act
            bool result = target.Execute();

            // assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void Path_IsPathRootedFalse()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"..\myfile.txt";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "IsPathRooted";

            // act
            target.Execute();

            // assert
            Assert.AreEqual(target.Value, "False");
        }

        [TestMethod]
        public void Path_IsPathRootedTrue()
        {
            // arrange
            Path target = new MSBuild.ExtensionPack.Framework.Path();
            target.Filepath = @"c:\myfile.txt";
            target.BuildEngine = new MockBuildEngine();
            target.TaskAction = "IsPathRooted";

            // act
            target.Execute();

            // assert
            Assert.AreEqual(target.Value, "True");
        }

        #endregion Public Methods
    }
}
