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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.PlatformUI;

using MSBuild.ExtensionPack.Base.Extension;
using MSBuild.ExtensionPack.Base.Iterator;

namespace MSBuild.ExtensionPack.Base.SecureFile
{
    /// <summary>
    /// Class implement creation and removal of a temporary directory rooted on <see cref="tempInfo"/> and named <see cref="tempDirName"/>.
    /// </summary>
    public partial class TempDirectory : IDisposable, IAsyncDisposable, IEqualityComparer<TempDirectory>, IEqualityComparer<DirectoryInfo>, IEqualityComparer<string>
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="directoryName">Specifies the parent directory string to use.</param>
        /// <param name="leaf">         specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempDirectory(string? directoryName, string leaf = ".tmp")
            : this(NormalizeDirectory(directoryName), leaf)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="directory">Specifies the parent directory to use.</param>
        /// <param name="leaf">     specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempDirectory(DirectoryInfo directory, string leaf = ".tmp")
        {
            this.TemporaryDirectory = directory;

            if (this.TemporaryDirectory.Parent is null)
            {
                this.ParentDirectory = OperatingSystem.IsWindows() ? new(TempDirectory.GetCurrentDrive().Name) : new(new string(Path.DirectorySeparatorChar, 1));
            }
            else
            {
                this.ParentDirectory = this.TemporaryDirectory.Parent;
            }

            this.TemporaryDirectory = this.TemporaryDirectory.CreateSubdirectory(Guid.NewGuid().ToString()).CreateSubdirectory(leaf);

            if (!OperatingSystem.IsWindows())
            {
                this.TemporaryDirectory.UnixFileMode = Mode;
            }
            else
            {
                DirectorySecurity directorySecurity = this.TemporaryDirectory.GetAccessControl();
                directorySecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
                AddAdministratorsFullControlAce(directorySecurity);
                AddUserFullControlAce(directorySecurity);
                AddGroupFullControlDenyAce(directorySecurity);
                AddOtherFullControlDenyAce(directorySecurity);
                this.TemporaryDirectory.SetAccessControl(directorySecurity);
            }

            CurrentDirectoryStack = new();
            PushLocation(this.TemporaryDirectory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="leaf">specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempDirectory(string leaf = ".tmp")
            : this(Path.GetTempPath() ?? Environment.GetEnvironmentVariable("TEMP") ?? Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TMPDIR"), leaf)
        {
        }

        [SupportedOSPlatform("Windows")]
        public static void AddAdministratorsFullControlAce(DirectorySecurity directorySecurity)
        {
            AddFileSystemAce(directorySecurity, "Administrators", FileSystemRights.FullControl, AccessControlType.Deny);
            AddFileSystemAce(directorySecurity, "Administrators", FileSystemRights.FullControl, AccessControlType.Allow);
            AddFileSystemAce(directorySecurity, "Administrators", FileSystemRights.ExecuteFile, AccessControlType.Deny);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddFileSystemAce(DirectorySecurity directorySecurity, string identity, FileSystemRights fileSystemRight, AccessControlType type)
        {
            FileSystemAccessRule rule = new(identity, fileSystemRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, type);
            directorySecurity.AddAccessRule(rule);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddFileSystemAce(DirectorySecurity directorySecurity, IdentityReference identity, FileSystemRights fileSystemRight, AccessControlType type)
        {
            FileSystemAccessRule rule = new(identity, fileSystemRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, type);
            directorySecurity.AddAccessRule(rule);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddFileSystemAce(DirectorySecurity directorySecurity, IdentityReference identity, FileSystemRights fileSystemRight, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
        {
            FileSystemAccessRule rule = new(identity, fileSystemRight, inheritanceFlags, propagationFlags, type);
            directorySecurity.AddAccessRule(rule);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddFileSystemAce(DirectorySecurity directorySecurity, string identity, FileSystemRights fileSystemRight, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
        {
            FileSystemAccessRule rule = new(identity, fileSystemRight, inheritanceFlags, propagationFlags, type);
            directorySecurity.AddAccessRule(rule);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddGroupFullControlDenyAce(DirectorySecurity directorySecurity)
        {
            using LocalGroupEnumerator localGroupEnumerator = new(e => !e.Name.Equals("Administrators", StringComparison.CurrentCultureIgnoreCase), Environment.MachineName);

            foreach (var localGroup in localGroupEnumerator)
            {
                using LocalGroupMemberEnumerator localGroupMemberEnumerator = new(localGroup.Name);

                foreach (var member in localGroupMemberEnumerator)
                {
                    if (member.Name.Contains(Environment.UserName))
                    {
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.FullControl, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.ExecuteFile, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.Read, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.Write, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.CreateDirectories, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.CreateFiles, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.ListDirectory, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.Traverse, AccessControlType.Deny);
                        AddFileSystemAce(directorySecurity, localGroup.Name, FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Deny);
                    }
                }
            }
        }

        [SupportedOSPlatform("Windows")]
        public static void AddOtherFullControlDenyAce(DirectorySecurity directorySecurity)
        {
            AddFileSystemAce(directorySecurity, "Everybody", FileSystemRights.FullControl, AccessControlType.Deny);
        }

        [SupportedOSPlatform("Windows")]
        public static void AddUserFullControlAce(DirectorySecurity directorySecurity)
        {
            AddFileSystemAce(directorySecurity, Environment.UserName, FileSystemRights.FullControl, AccessControlType.Deny);
            AddFileSystemAce(directorySecurity, Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow);
            AddFileSystemAce(directorySecurity, Environment.UserName, FileSystemRights.ExecuteFile, AccessControlType.Deny);
        }

        [SupportedOSPlatform("Windows")]
        public static IEnumerator<string> EnumerateLocalMachineGroups()
        {
            using DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
            var filtered = localMachine.Children
                .Cast<DirectoryEntry>()
                .Where(e => e.SchemaClassName.Equals("Group", StringComparison.Ordinal)
                    && e.Name.Equals("Administrators", StringComparison.CurrentCultureIgnoreCase));

            foreach (var entry in filtered)
            {
                yield return entry.Name;
            }
        }

        [SupportedOSPlatform("Windows")]
        public static IEnumerator<string> EnumerateLocalMachineGroups(string excludeGroup)
        {
            using DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
            var filtered = localMachine.Children
                .Cast<DirectoryEntry>()
                .Where(e => e.SchemaClassName.Equals("Group", StringComparison.Ordinal)
                    && excludeGroup.Equals(e.Name, StringComparison.CurrentCultureIgnoreCase));

            foreach (var entry in filtered)
            {
                yield return entry.Name;
            }
        }

        [SupportedOSPlatform("Windows")]
        public static IEnumerator<string> EnumerateLocalMachineGroups(IEnumerable<string> excludeGroupList)
        {
            using DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
            var filtered = localMachine.Children.Cast<DirectoryEntry>().Where(e => e.SchemaClassName == "Group" && excludeGroupList.Any(x => x.Equals(e.Name, StringComparison.CurrentCultureIgnoreCase)));

            foreach (var entry in filtered)
            {
                yield return entry.Name;
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating the full name string for <see cref="TemporaryDirectory"/>.
        /// </summary>
        public string DirectoryName => TemporaryDirectory.FullName;

        /// <summary>
        /// Gets a value indicate the parent <see cref="DirectoryInfo"/> of the leaf.
        /// </summary>
        public DirectoryInfo? ParentDirectory { get; }

        /// <summary>
        /// Gets a value indicating the string absolute path name of the <see cref="ParentDirectory"/>.
        /// </summary>
        public string? ParentName => ParentDirectory?.FullName;

        /// <summary>
        /// Gets a value indicating the <see cref="TempDirectory"/> directory path.
        /// </summary>
        public DirectoryInfo TemporaryDirectory { get; }

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets a value indicating the <see cref="FileAccess"/> mode for the created file.
        /// </summary>
        public virtual FileAccess AccessMode => FileAccess.ReadWrite;

        /// <summary>
        /// Gets or sets a value indicating the <c>Windows</c> buffer size for the created file. The default is <c>4096</c> bytes.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual int BufferSize { get; set; } = 4096;

        /// <summary>
        /// Gets a value indicating the <see cref="FileAttributes"/> attributes to use for the created file.
        /// </summary>
        public virtual FileAttributes FileAttributes => FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.NotContentIndexed;

        /// <summary>
        /// Gets a value indicating the <c>Unix</c> file mode bitmap for the created file.
        /// </summary>
        /// <remarks>0600</remarks>
        public virtual UnixFileMode Mode => UnixFileMode.UserRead | UnixFileMode.UserWrite;

        /// <summary>
        /// Gets a value indicating the <see cref="FileMode"/> for the created file.
        /// </summary>
        public virtual FileMode OpenMode => FileMode.CreateNew;

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="FileOptions"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual FileOptions Options => FileOptions.DeleteOnClose | FileOptions.SequentialScan | FileOptions.Asynchronous;

        /// <summary>
        /// Gets a value indicating the <see cref="FileShare"/> mode to use for the secure created file.
        /// </summary>
        public virtual FileShare ShareMode => FileShare.None;

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="AccessControlSections"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual AccessControlSections WindowsAccessControl => AccessControlSections.Owner;

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="FileSystemRights"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual FileSystemRights WindowsFileSystemRights => FileSystemRights.Delete | FileSystemRights.AppendData | FileSystemRights.WriteData | FileSystemRights.Read;

        #endregion Protected Properties

        #region Public Methods

        public static bool Equals(DirectoryInfo? x, FileInfo? y)
        {
            if (OperatingSystem.IsWindows())
            {
                return string.Equals(x?.FullName, y?.DirectoryName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Equals(x?.FullName, y?.DirectoryName, StringComparison.Ordinal);
            }
        }

        public static bool Equals(FileInfo? x, DirectoryInfo? y)
        {
            if (OperatingSystem.IsWindows())
            {
                return string.Equals(x?.DirectoryName, y?.FullName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Equals(x?.DirectoryName, y?.FullName, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Get the current directory as a <see cref="DirectoryInfo"/> instance.
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GetCurrentDirectory()
        {
            return new(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Gets the <see cref="DriveInfo"/> for <see cref="Directory.GetCurrentDirectory"/>.
        /// </summary>
        /// <returns>Returns a <see cref="DriveInfo"/> instance for the current drive.</returns>
        [SupportedOSPlatform("Windows")]
        public static DriveInfo GetCurrentDrive()
        {
            return new(Directory.GetCurrentDirectory());
        }

        public static DirectoryInfo GetPathRoot(DirectoryInfo directory)
        {
            return directory.Root;
        }

        public static DirectoryInfo? GetPathRoot(FileInfo path)
        {
            return path.Directory?.Root;
        }

        public static implicit operator DirectoryInfo?(TempDirectory temp)
        {
            return temp.ParentDirectory;
        }

        public static implicit operator TempDirectory(DirectoryInfo directory)
        {
            return new(directory);
        }

        public static DirectoryInfo NormalizeDirectory(string directoryName)
        {
            var temp = PathUtil.Normalize(directoryName);

            if (!Path.EndsInDirectorySeparator(temp))
            {
                temp += Path.DirectorySeparatorChar;
            }

            return new(temp);
        }

        /// <summary>
        /// Set the current directory using <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">Specifies the <see cref="DirectoryInfo"/> instance of the directory to make the current directory.</param>
        public static void SetCurrentDirectory(DirectoryInfo directory)
        {
            Directory.SetCurrentDirectory(directory.FullName);
        }

        /// <summary>
        /// Public method that calls <see cref="Dispose(bool)"/> with a parameter value of <see langref="true"/>.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            // Perform async cleanup.
            await DisposeAsyncCore().ConfigureAwait(false);

            // Dispose of unmanaged resources.
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        public bool Equals(DirectoryInfo? x, DirectoryInfo? y)
        {
            if (OperatingSystem.IsWindows())
            {
                return string.Equals(x?.FullName, y?.FullName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Equals(x?.FullName, y?.FullName, StringComparison.Ordinal);
            }
        }

        public bool Equals(string? x, string? y)
        {
            return PathUtil.ArePathsEqual(PathUtil.Normalize(x), PathUtil.Normalize(y));
        }

        public bool Equals(TempDirectory? x, TempDirectory? y)
        {
            return this.Equals(x?.DirectoryName, y?.DirectoryName);
        }

        public int GetHashCode([DisallowNull] DirectoryInfo obj)
        {
            return HashCode.Combine(obj.Exists, obj.Extension, obj.FullName, obj.LinkTarget, obj.Name, obj.Parent, obj.Root);
        }

        public int GetHashCode([DisallowNull] FileInfo obj)
        {
            return HashCode.Combine(obj.Directory, obj.DirectoryName, obj.Exists, obj.Extension, obj.FullName, obj.Length, obj.LinkTarget, obj.Name);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return obj.GetHashCode();
        }

        public int GetHashCode([DisallowNull] TempDirectory obj)
        {
            return HashCode.Combine(obj.AccessMode, obj.DirectoryName, obj.FileAttributes, obj.Mode, obj.OpenMode, obj.ParentDirectory, obj.ParentName);
        }

        /// <summary>
        /// Peeks for the current directory on the directory stack.
        /// </summary>
        /// <returns>
        /// Returns the <see cref="DirectoryInfo"/> instance off the stack without removing it; otherwise, <see langref="null"/> if
        /// the stack is empty.
        /// </returns>
        public DirectoryInfo? PeekLocation()
        {
            return CurrentDirectoryStack.TryPeek(out DirectoryInfo? peek) ? peek : null;
        }

        /// <summary>
        /// Pops the current directory off the directory stack and sets it as the new current directory.
        /// </summary>
        /// <returns>Returns the <see cref="DirectoryInfo"/> instance off the stack and removes it.</returns>
        public void PopLocation()
        {
            TempDirectory.SetCurrentDirectory(CurrentDirectoryStack.TryPop(out DirectoryInfo? pop) ? pop : TempDirectory.GetCurrentDirectory());
        }

        /// <summary>
        /// Push the current directory and set the current directory to either <paramref name="current"/> or the current directory
        /// if <paramref name="current"/> is <see langref="null"/>.
        /// </summary>
        /// <param name="current">Specifies the new <see cref="DirectoryInfo"/> current directory</param>
        public void PushLocation(DirectoryInfo? current)
        {
            CurrentDirectoryStack.Push(TempDirectory.GetCurrentDirectory());
            TempDirectory.SetCurrentDirectory(current ?? TempDirectory.GetCurrentDirectory());
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Protected method disposing of <see cref="DirectoryInfo"/> if <paramref name="disposing"/> is <see langref="true"/>.
        /// </summary>
        /// <param name="disposing">
        /// If <see langref="true"/>, both managed and unmanaged resources will be disposed; otherwise only unmanaged resources will
        /// be disposing.
        /// </param>
        /// <exception cref="ObjectDisposedException">
        /// Throws if <see cref="TemporaryDirectory"/> has already been deleted before <see cref="Dispose(bool)"/> has been called.
        /// </exception>
        /// <exception cref="IOException">Throws if one or more files is open.</exception>
        /// <exception cref="SecurityException">
        /// Throws if the caller of <see cref="Dispose(bool)"/> lacks the permissions to delete <see cref="TemporaryDirectory"/>.
        /// </exception>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (PeekLocation() is not null)
                    {
                        PopLocation();
                    }

                    try
                    {
                        ParentDirectory?.Delete(recursive: true);
                    }
                    catch (UnauthorizedAccessException uaex)
                    {
                        Console.Error.WriteLine(uaex.ToString());

                        int count = 0;

                        if (ParentDirectory is not null)
                        {
                            foreach (var fi in ParentDirectory.EnumerateFiles("*", new EnumerationOptions() { AttributesToSkip = FileAttributes.ReadOnly, RecurseSubdirectories = true }))
                            {
                                if (!fi.Attributes.HasFlag(FileAttributes.ReadOnly))
                                {
                                    fi.Delete();
                                }
                                else
                                {
                                    count++;
                                    Console.Error.WriteLine($"File {fi.FullName} is read-only");
                                }
                            }

                            Debug.WriteLineIf(count > 0, $"Directory {ParentName} still has {count} undeleted files");

                            count = 0;

                            foreach (var di in this.ParentDirectory.EnumerateDirectories("*", SearchOption.AllDirectories))
                            {
                                if (!di.EnumerateFiles().Any())
                                {
                                    di.Delete();
                                }
                                else
                                {
                                    count++;
                                    Console.Error.WriteLine($"Directory {di.FullName} contains one or more read-only files");
                                }
                            }

                            Debug.WriteLineIf(count > 0, $"Directory {ParentName} still has {count} undeleted sub-directories");
                        }
                    }
                    catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException || ex is SecurityException)
                    {
                        Console.Error.WriteLine(ex.ToString());
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Protected asynchronous method disposing of <see cref="DirectoryInfo"/> if <paramref name="disposing"/> is <see langref="true"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Throws if <see cref="TemporaryDirectory"/> has already been deleted before <see cref="DisposeAsyncCore"/> has been called.
        /// </exception>
        /// <exception cref="IOException">Throws if one or more files is open.</exception>
        /// <exception cref="SecurityException">
        /// Throws if the caller of <see cref="DisposeAsyncCore"/> lacks the permissions to delete <see cref="TemporaryDirectory"/>.
        /// </exception>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!disposedValue)
            {
                PopLocation();

                try
                {
                    ParentDirectory?.Delete(recursive: true);
                }
                catch (UnauthorizedAccessException uaex)
                {
                    await Console.Error.WriteLineAsync(uaex.ToString()).ConfigureAwait(false);

                    int count = 0;

                    if (ParentDirectory is not null)
                    {
                        await foreach (var fi in ListAsyncEnumerable.GetAsyncEnumerable(ParentDirectory.EnumerateFiles("*", new EnumerationOptions() { AttributesToSkip = FileAttributes.ReadOnly, RecurseSubdirectories = true })).ConfigureAwait(false))
                        {
                            if (!fi.Attributes.HasFlag(FileAttributes.ReadOnly))
                            {
                                fi.Delete();
                            }
                            else
                            {
                                count++;
                                await Console.Error.WriteLineAsync($"File {fi.FullName} is read-only").ConfigureAwait(false);
                            }
                        }

                        Debug.WriteLineIf(count > 0, $"Directory {ParentName} still has {count} undeleted files");

                        count = 0;

                        await foreach (var di in ListAsyncEnumerable.GetAsyncEnumerable(this.ParentDirectory.EnumerateDirectories("*", SearchOption.AllDirectories)).ConfigureAwait(false))
                        {
                            if (!IsNullOrEmpty(di.EnumerateFiles()))
                            {
                                di.Delete();
                            }
                            else
                            {
                                count++;
                                await Console.Error.WriteLineAsync($"Directory {di.FullName} contains one or more read-only files").ConfigureAwait(false);
                            }
                        }

                        Debug.WriteLineIf(count > 0, $"Directory {ParentName} still has {count} undeleted sub-directories");
                    }
                }
                catch (DirectoryNotFoundException dnfex)
                {
                    await Console.Error.WriteLineAsync($"Directory {DirectoryName} no longer exists").ConfigureAwait(false);
                    throw new ObjectDisposedException(this.TemporaryDirectory.GetType().Name, dnfex);
                }
                catch (IOException ioex)
                {
                    await Console.Error.WriteLineAsync($"Directory {DirectoryName} is the application directory for {Path.GetDirectoryName(Assembly.GetAssembly(typeof(TempDirectory))?.Location) ?? Directory.GetCurrentDirectory()}").ConfigureAwait(false);
                }
                catch (SecurityException sex)
                {
                    await Console.Error.WriteLineAsync($"Caller {Environment.UserName} does not have the permissions to delete {DirectoryName}").ConfigureAwait(false);
                }
            }

            disposedValue = true;
        }

        #endregion Protected Methods

        #region Private Fields

        /// <summary>
        /// Contains a stack of <see cref="DirectoryInfo"/> for pushing and popping the current directory.
        /// </summary>
        private readonly Stack<DirectoryInfo> CurrentDirectoryStack;

        /// <summary>
        /// Semaphore signaling whether <see cref="Dispose(bool)"/> has already been called.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// File base name regular expression to ensure template is in correct form.
        /// </summary>
        /// <returns>Returns the <see cref="Regex"/> regular expression.</returns>
        [GeneratedRegex(@"^[A-Za-z0-9\-_].+(?:X){18}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
        private static partial Regex BaseNameTemplateRegex();

        #endregion Private Fields
    }
}
