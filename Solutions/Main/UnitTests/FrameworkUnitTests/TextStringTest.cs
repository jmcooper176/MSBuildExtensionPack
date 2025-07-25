﻿//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TextStringTest.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
        #region Test Setup and Teardown
        #endregion

        #region Test Methods
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
        #endregion

        #region Test Support Methods and Properties
        #endregion
    }
}
