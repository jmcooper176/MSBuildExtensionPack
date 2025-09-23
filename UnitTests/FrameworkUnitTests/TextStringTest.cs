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
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MSBuild.ExtensionPack.Framework;

    /// <summary>
    /// Unit Tests for TestString Task
    /// </summary>
    [TestClass]
    public class TextStringTest
    {
        #region Public Methods

        [TestMethod]
        public void TextStringSplitNoString1Test()
        {
            TextString target = new TextString();
            target.String1 = null;
            target.String2 = " ";
            target.TaskAction = "Split";
            target.BuildEngine = new MockBuildEngine();

            bool result = target.Execute();
            Assert.IsFalse(result);

            target.String1 = string.Empty;
            result = target.Execute();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TextStringSplitNoString2Test()
        {
            TextString target = new TextString();
            target.String1 = "The  quick  brown  fox  jumped  over  the  lazy  dog.";
            target.String2 = null;
            target.TaskAction = "Split";
            target.BuildEngine = new MockBuildEngine();

            bool result = target.Execute();
            Assert.IsFalse(result);

            target.String2 = string.Empty;
            result = target.Execute();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TextStringSplitWithoutSelectedIndexTest()
        {
            var input = "The  quick  brown  fox  jumped  over  the  lazy  dog.";
            var separator = " ";

            string[] expected = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            TextString target = new TextString();
            target.String1 = input;
            target.String2 = separator;
            target.TaskAction = "Split";
            target.StartIndex = -1;
            target.BuildEngine = new MockBuildEngine();

            bool result = target.Execute();
            Assert.IsTrue(result);
            Assert.IsNotNull(target.Strings);
            Assert.AreEqual(expected.Length, target.Strings.Length);
            Assert.AreEqual(0, expected.Except(target.Strings.Select(x => x.ItemSpec)).Count());
            Assert.IsNull(target.NewString);
        }

        [TestMethod]
        public void TextStringSplitWithSelectedIndexTest()
        {
            var input = "The  quick  brown  fox  jumped  over  the  lazy  dog.";
            var separator = " ";

            string[] expected = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            TextString target = new TextString();
            target.String1 = input;
            target.String2 = separator;
            target.TaskAction = "Split";
            target.StartIndex = 2;
            target.BuildEngine = new MockBuildEngine();

            bool result = target.Execute();
            Assert.IsTrue(result);
            Assert.IsNotNull(target.Strings);
            Assert.AreEqual(expected.Length, target.Strings.Length);
            Assert.AreEqual(0, expected.Except(target.Strings.Select(x => x.ItemSpec)).Count());
            Assert.AreEqual(expected[target.StartIndex], target.NewString);
        }

        #endregion Public Methods
    }
}
