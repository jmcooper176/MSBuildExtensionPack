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
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using File = FileSystem.Path.File;

    [TestClass]
    public class FileTest
    {
        private bool result;
        private File task;

        private object? methodResult;

        private string ConvertFromFileSystemRights(FileSystemRights[] rights)
        {
            return string.Join(',', rights.Select(r => r.ToString()));
        }

        private FileSystemAccessRule? GetFileSystemAccessRule(FileInfo path)
        {
            return path.GetAccessControl().GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier)).Cast<FileSystemAccessRule>().FirstOrDefault(r => (r.IdentityReference.Translate(typeof(NTAccount)) as NTAccount)?.Value == this.CurrentUser);
        }

        private void GivenAccessType(AccessControlType accessControlType)
        {
            this.task.AccessType = accessControlType.ToString();
        }

        private string GivenAFile()
        {
            return Path.GetTempFileName();
        }

        private void GivenFiles(string[] paths)
        {
            this.task.Files = new List<ITaskItem>(paths.Length);

            foreach (string path in paths)
            {
                this.task.Files.Add(new TaskItem(path));
            }
        }

        private void GivenPath(string path)
        {
            this.task.Path = new TaskItem(path);
        }

        private void GivenTaskAction(string action)
        {
            this.task.TaskAction = action;
        }

        private void GivenUser()
        {
            this.task.Users = [new TaskItem(this.CurrentUser)];
        }

        private void GivenUserPermissions(FileSystemRights[] rights)
        {
            this.task.Permission = this.ConvertFromFileSystemRights(rights);
        }

        private void GivenUsers(string[] users)
        {
            this.task.Users = new List<ITaskItem>(users.Length);

            foreach (string user in users)
            {
                this.task.Users.Add(new TaskItem(user));
            }
        }

        private void GivenUsersPermissions(FileSystemRights[] rights)
        {
            var permission = this.ConvertFromFileSystemRights(rights);

            foreach (ITaskItem userTaskItem in this.task.Users)
            {
                userTaskItem.SetMetadata("Permission", permission);
            }
        }

        private void ThenMethodReturnEqualsValue<TReturn>(TReturn value) where TReturn : struct, IEquatable<TReturn>
        {
            Assert.AreEqual(value, (TReturn?)methodResult);
        }

        private void ThenMethodReturnIsFalse(bool expected)
        {
            Assert.AreNotEqual(value, this.result);
        }

        private void ThenMethodReturnIsNotNull<TReturn>() where TReturn : class
        {
            Assert.IsNotNull((TReturn?)methodResult);
        }

        private void ThenMethodReturnIsNull<TReturn>() where TReturn : class
        {
            Assert.IsNull((TReturn?)methodResult);
        }

        private void ThenMethodReturnIsTrue(bool expected)
        {
            Assert.AreEqual(expected, this.result);
        }

        private void ThenMethodReturnNotEqualToValue<TReturn>(TReturn value) where TReturn : struct, IEquatable<TReturn>
        {
            Assert.AreNotEqual(value, (TReturn?)methodResult);
        }

        private void ThenPermissionsGetAdded(string[] paths, AccessControlType aclType, FileSystemRights[] rights)
        {
            this.ThenPermissionsGetSet(paths, true, aclType, rights);
        }

        private void ThenPermissionsGetRemoved(string[] paths, AccessControlType aclType, FileSystemRights[] rights)
        {
            this.ThenPermissionsGetSet(paths, false, aclType, rights);
        }

        private void ThenPermissionsGetSet(string[] paths, bool adding, AccessControlType aclType, FileSystemRights[] rights)
        {
            FileSystemRights expectedRights = 0;
            foreach (var right in rights)
            {
                expectedRights |= right;
            }

            if (aclType == AccessControlType.Allow)
            {
                expectedRights |= FileSystemRights.Synchronize;
            }

            foreach (string path in paths)
            {
                var rule = this.GetFileSystemAccessRule(new FileInfo(path));
                Assert.IsNotNull(rule);
                Assert.AreEqual(aclType, rule.AccessControlType);
                if (adding)
                {
                    Assert.IsTrue(rule.FileSystemRights.HasFlag(expectedRights));
                }
                else
                {
                    Assert.IsFalse(rule.FileSystemRights.HasFlag(expectedRights));
                }
            }
        }

        private void ThenTaskFailed()
        {
            Assert.IsFalse(this.result);
        }

        private void ThenTaskSucceeded()
        {
            Assert.IsTrue(this.result);
        }

        private void WhenGetCurrentRights(string path)
        {
            this.GetFileSystemAccessRule(new FileInfo(path));
        }

        private void WhenMethodCalled<TReturn>(Func<TReturn> predicate, string name)
        {
            this.methodResult = null;
            methodResult = predicate.Invoke();
        }

        private void WhenMethodCalled(Func<bool> predicate, string name)
        {
            this.result = false;
            Console.Error.WriteLine($"Calling method {name}");
            result = predicate.Invoke();
        }

        private void WhenTaskRuns()
        {
            this.WhenMethodCalled(() => this.task.Execute(), nameof(this.task.Execute));
        }

        public string CurrentUser
        {
            get { return WindowsIdentity.GetCurrent().Name; }
        }

        [TestMethod]
        public void AddingSecurity_GivenNoPathNoFilesPermissions_TaskFailed()
        {
            // Arrange
            this.task.Path = null;
            this.task.Files = null;
            this.GivenUserPermissions(new[] { FileSystemRights.Read });
            this.GivenAccessType(AccessControlType.Allow);
            this.GivenTaskAction("AddSecurity");

            // Act
            this.WhenTaskRuns();

            // Assert
            this.ThenTaskFailed();
        }

        [TestMethod]
        public void AddingSecurity_GivenNoUsersPermissions_TaskFailed()
        {
            // Arrange
            this.task.Users = null;
            this.GivenUserPermissions(new[] { FileSystemRights.Read });
            this.GivenAccessType(AccessControlType.Allow);

            // Act
            this.WhenAddingSecurity();

            // Assert
            this.ThenTaskFailed();
        }

        [TestMethod]
        public void AddingSecurity_GivenPathUserPermissions_PermissionsGetAdded()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUserPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Deny);

            // Act
            this.WhenAddingSecurity();

            // Assert
            this.ThenPermissionsGetAdded(paths, AccessControlType.Deny, rightsToAdd);
        }

        [TestMethod]
        public void AddingSecurity_GivenPathUserPermissions_TaskSucceeded()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUserPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Deny);

            // Act
            this.WhenAddingSecurity();

            // Assert
            this.ThenTaskSucceeded();
        }

        [TestMethod]
        public void RemovingSecurity_GivenPathUserPermissions_PermissionsGetRemoved()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUserPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Allow);
            this.WhenAddingSecurity();
            var rightsToRemove = new[] { FileSystemRights.Write };
            this.GivenUserPermissions(rightsToRemove);
            this.GivenAccessType(AccessControlType.Allow);

            // Act
            this.WhenRemovingSecurity();

            // Assert
            this.ThenPermissionsGetRemoved(paths, AccessControlType.Allow, rightsToRemove);
        }

        [TestMethod]
        public void RemovingSecurity_GivenPathUserPermissions_TaskSucceeded()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUserPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Allow);
            this.WhenAddingSecurity();
            var rightsToRemove = new[] { FileSystemRights.Write };
            this.GivenUserPermissions(rightsToRemove);
            this.GivenAccessType(AccessControlType.Allow);

            // Act
            this.WhenRemovingSecurity();

            // Assert
            this.ThenTaskSucceeded();
        }

        [TestMethod]
        public void RemovingSecurity_GivenPathUserPermissionsAddingSecurity_PermissionsGetRemoved()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUserPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Deny);
            this.WhenAddingSecurity();
            var rightsToRemove = new[] { FileSystemRights.Write };
            this.GivenUserPermissions(rightsToRemove);
            this.GivenAccessType(AccessControlType.Deny);

            // Act
            this.WhenRemovingSecurity();

            // Assert
            this.ThenPermissionsGetRemoved(paths, AccessControlType.Deny, rightsToRemove);
        }

        [TestMethod]
        public void RemovingSecurity_GivenPathUserPermissionsAddingSecurity_TaskSucceeded()
        {
            // Arrange
            var rightsToAdd = new[] { FileSystemRights.Read, FileSystemRights.Write };
            var paths = new[] { this.GivenAFile() };
            this.GivenPath(paths[0]);
            this.GivenUser();
            this.GivenUsersPermissions(rightsToAdd);
            this.GivenAccessType(AccessControlType.Deny);
            this.WhenAddingSecurity();
            var rightsToRemove = new[] { FileSystemRights.Write };
            this.GivenUserPermissions(rightsToRemove);
            this.GivenAccessType(AccessControlType.Deny);

            // Act
            this.WhenRemovingSecurity();

            // Assert
            this.ThenTaskSucceeded();
        }

        [TestInitialize]
        public void Setup()
        {
            this.task = new File();

            // this.task.Log = new TaskLoggingHelper(new MockBuildEngine(), "Full");
        }
    }
}
