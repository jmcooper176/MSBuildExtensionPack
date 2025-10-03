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
namespace MSBuild.ExtensionPack.Base.SecureFile
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Versioning;
    using System.Security;
    using System.Security.AccessControl;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.PlatformUI;

    /// <summary>
    /// Implements secure temporary file <see cref="FileInfo"/> generation.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    /// <seealso cref="IAsyncDisposable"/>
    /// <seealso cref="IEqualityComparer{TempFile}"/>
    /// <seealso cref="IEqualityComparer{FileInfo}"/>
    /// <seealso cref="IEqualityComparer{String}"/>
    public partial class TempFile : IDisposable, IAsyncDisposable, IEqualityComparer<TempFile>, IEqualityComparer<FileInfo>, IEqualityComparer<string>
    {
        #region Private Fields

        /// <summary>
        /// If <see langref="true"/>, this instance has been disposed by <see cref="Dispose(bool)"/>; otherwise, this instance has
        /// not been disposed.
        /// </summary>
        private bool disposedValue;

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// File base name regular expression to ensure template is in correct form.
        /// </summary>
        /// <returns>Returns the <see cref="Regex"/> regular expression.</returns>
        [GeneratedRegex(@"^[A-Za-z0-9\-_].+(?:X){18}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
        private static partial Regex BaseNameTemplateRegex();

        #endregion Private Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DeleteTempFile(TempFilePath);
                    TempDirectory.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!disposedValue)
            {
                DeleteTempFile(TempFilePath);
                await TempDirectory.DisposeAsync();
            }

            disposedValue = true;
        }

        #endregion Protected Methods

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="directoryName">Specifies the parent directory string to use.</param>
        /// <param name="leaf">         specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempFile(string? directoryName, string leaf = ".tmp")
            : this(TempDirectory.NormalizeDirectory(directoryName), leaf)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="directory">Specifies the parent directory to use.</param>
        /// <param name="leaf">     specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempFile(DirectoryInfo directory, string leaf = ".tmp")
        {
            this.TempDirectory = new(directory, leaf);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempDirectory"/> class.
        /// </summary>
        /// <param name="leaf">specifies the name of the leaf node for <see cref="ParentDirectory"/>.</param>
        public TempFile(string leaf = ".tmp")
            : this(Path.GetTempPath() ?? Environment.GetEnvironmentVariable("TEMP") ?? Environment.GetEnvironmentVariable("TMP") ?? Environment.GetEnvironmentVariable("TMPDIR"), leaf)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indicating the <see cref="FileAccess"/> mode for the created file.
        /// </summary>
        public virtual FileAccess AccessMode => TempDirectory.AccessMode;

        /// <summary>
        /// Gets or sets a value indicating the <c>Windows</c> buffer size for the created file. The default is <c>4096</c> bytes.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual int BufferSize { get; set; } = 4096;

        public string DefaultTemplate => string.Concat("temp", "XXXXXXXXXXXXXXXXXX");

        /// <summary>
        /// Gets a value indicating the <see cref="FileAttributes"/> attributes to use for the created file.
        /// </summary>
        public virtual FileAttributes FileAttributes => TempDirectory.FileAttributes;

        /// <summary>
        /// Gets a value indicating the <c>Unix</c> file mode bitmap for the created file.
        /// </summary>
        /// <remarks>0600</remarks>
        public virtual UnixFileMode Mode => TempDirectory.Mode;

        /// <summary>
        /// Gets a value indicating the <see cref="FileMode"/> for the created file.
        /// </summary>
        public virtual FileMode OpenMode => TempDirectory.OpenMode;

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="FileOptions"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual FileOptions Options => TempDirectory.Options;

        /// <summary>
        /// Gets a value indicating the <see cref="FileShare"/> mode to use for the secure created file.
        /// </summary>
        public virtual FileShare ShareMode => TempDirectory.ShareMode;

        public TempDirectory TempDirectory { get; }

        public FileInfo? TempFilePath { get; }

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="AccessControlSections"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual AccessControlSections WindowsAccessControl => TempDirectory.WindowsAccessControl;

        /// <summary>
        /// Gets a value indicating the <c>Windows</c><see cref="FileSystemRights"/> to use for the created file.
        /// </summary>
        [SupportedOSPlatform("Windows")]
        public virtual FileSystemRights WindowsFileSystemRights => TempDirectory.WindowsFileSystemRights;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Combines parent directory <see cref="DirectoryInfo"/><paramref name="directory"/> with <paramref name="template"/> and
        /// <paramref name="extension"/> to form a new <see cref="FileInfo"/> path.
        /// </summary>
        /// <param name="directory">Specifies the parent directory <see cref="DirectoryInfo"/>.</param>
        /// <param name="baseName"> Specifies the file base name.</param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>
        /// A <see cref="FileInfo"/> representing the combining of <paramref name="directory"/>, <paramref name="template"/>, and
        /// <paramref name="extension"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        public static FileInfo Combine(DirectoryInfo directory, string baseName, string? extension)
        {
            return TempFile.Combine(directory, string.Concat(baseName, extension ?? string.Empty));
        }

        public static FileInfo Combine(DirectoryInfo directory, string fileName)
        {
            return new(Path.Combine(directory.FullName, fileName));
        }

        /// <summary>
        /// Combines parent directory <see cref="DirectoryInfo"/><paramref name="directory"/> with <paramref name="template"/> and
        /// <paramref name="extension"/> to form a new <see cref="FileInfo"/> path.
        /// </summary>
        /// <param name="directory">Specifies the parent directory <see cref="DirectoryInfo"/>.</param>
        /// <param name="template"> 
        /// Specifies the file template base name to be formatted with <see cref="TempDirectory.FormatFileTemplate(string, IFormatProvider?)"/>.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>
        /// A <see cref="FileInfo"/> representing the combining of <paramref name="directory"/>, <paramref name="template"/>, and
        /// <paramref name="extension"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        public static FileInfo CombineTemp(DirectoryInfo directory, string template, string? extension)
        {
            return TempFile.Combine(directory, TempFile.FormatFileTemplate(template), extension);
        }

        /// <summary>
        /// Format a file template <paramref name="template"/> as a pseudo-random file name.
        /// </summary>
        /// <param name="template">
        /// Specifies the file name template. The template must end in 'XXXXXXXXXXXXXXXXXX' and start with a letter, digit, dash, or underscore.
        /// </param>
        /// <param name="provider">
        /// Specifies the <see cref="IFormatProvider"/> provider. If null, <see cref="CultureInfo.InvariantCulture"/> will be used.
        /// </param>
        /// <returns>Returns a formatted file base name.</returns>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        public static string FormatFileTemplate(string template, IFormatProvider? provider = null)
        {
            Regex pattern = BaseNameTemplateRegex();

            if (!pattern.IsMatch(template))
            {
                throw new ArgumentException($"Parameter {nameof(template)} must always end with XXXXXXXXXXXXXXXXXX", nameof(template));
            }

            return template.Replace("XXXXXXXXXXXXXXXXXX", DateTime.UtcNow.TimeOfDay.Ticks.ToString("D:18", provider ?? CultureInfo.InvariantCulture));
        }

        public static FileInfo NormalizePath(string path)
        {
            FileInfo temp = new(PathUtil.Normalize(path));

            return temp.Name.StartsWith('-')
                ? TempFile.Combine(temp.Directory ?? TempDirectory.GetCurrentDirectory(), string.Concat('.', Path.DirectorySeparatorChar, temp.Name))
                : temp;
        }

        /// <summary>
        /// Create a file combining <see cref="DirectoryName"/>, <paramref name="baseName"/>, and <paramref name="extension"/> to
        /// create the path.
        /// </summary>
        /// <param name="baseName"> Specifies the base name.</param>
        /// <param name="extension">Specifies the extension.</param>
        /// <returns>A <see cref="FileInfo"/> with the resulting path.</returns>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        /// <remarks>See <a href="https://man7.org/linux/man-pages/man3/mktemp.3.html"/> on why you should never do this.</remarks>
        public FileInfo CreateFile(string? baseName, string? extension)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseName) || !string.IsNullOrWhiteSpace(extension), $"Parameters {nameof(baseName)} and {nameof(extension)} cannot both be null, empty, or all whitespace");

            Console.Error.WriteLine($"WARNING:  No restrictive security attributes and full control for {baseName}.{extension}.  This method is INSECURE.");

            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith('.'))
            {
                extension = extension.Insert(0, ".");
            }

            var fileName = string.Concat(baseName ?? string.Empty, extension ?? string.Empty);

            FileInfo path = new(Path.Combine(TempDirectory.TemporaryDirectory.FullName, fileName));

            if (path.Exists && IsFileInfoOpen(path))
            {
                throw new SecurityException($"Path {path.FullName} should not both exist and be open", new IOException($"Path {path.FullName} is already open"));
            }
            else if (path.Exists)
            {
                Console.Error.WriteLine($"WARNING: Path {path.FullName} exists but is not open.  Attempting to delete.");
                path.Delete();
            }

            return OperatingSystem.IsWindows()
                ? CreateWindowsFile(path, FileMode.Create, FileSystemRights.FullControl)
                : CreateOtherFile(path, FileMode.Create, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute | UnixFileMode.GroupRead | UnixFileMode.OtherRead);
        }

        /// <summary>
        /// Creates a new, secure <see cref="FileInfo"/> path from <paramref name="template"/><paramref name="extension"/>.
        /// </summary>
        /// <param name="baseName"> 
        /// Specifies the base name of the file. NOTE: No attempt in this method is made to avoid collision or prevent brute force
        /// attacks except that the file name is already created and secure attributes are applied. Collision will throw a <c>SecurityException</c>.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>A <see cref="FileInfo"/> instance representing a secure temporary file path ready for opening.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        public FileInfo CreateNewSecureFile(string? baseName, string? extension)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseName) || !string.IsNullOrWhiteSpace(extension), $"Parameters {nameof(baseName)} and {nameof(extension)} cannot both be null, empty, or all whitespace");

            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith('.'))
            {
                extension = extension.Insert(0, ".");
            }

            var fileName = string.Concat(baseName ?? string.Empty, extension ?? string.Empty);

            FileInfo path = new(Path.Combine(TempDirectory.TemporaryDirectory.FullName, fileName));

            if (path.Exists && IsFileInfoOpen(path))
            {
                throw new SecurityException($"Path {path.FullName} should not both exist and be open", new IOException($"Path {path.FullName} is already open"));
            }
            else if (path.Exists)
            {
                Console.Error.WriteLine($"WARNING: Path {path.FullName} exists but is not open.  Attempting to delete.");
                path.Delete();
            }

            return OperatingSystem.IsWindows() ? CreateWindowsFile(path) : CreateOtherFile(path);
        }

        /// <summary>
        /// Creates a new, secure <see cref="FileInfo"/> path from <paramref name="template"/><paramref name="extension"/>.
        /// </summary>
        /// <param name="template"> 
        /// Specifies the file template base name to be formatted with <see cref="TempDirectory.FormatFileTemplate(string, IFormatProvider?)"/>.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>A <see cref="FileInfo"/> instance representing a secure temporary file path ready for opening.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        public FileInfo CreateNewSecureTempFile(string template, string? extension)
        {
            return CreateNewSecureFile(TempFile.FormatFileTemplate(template, CultureInfo.InvariantCulture), extension ?? string.Empty);
        }

        /// <summary>
        /// Create a new non- <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/>, and the value of
        /// properties <see cref="OpenMode"/> and <see cref="Mode"/>.
        /// </summary>
        /// <param name="path">Specifies the <see cref="FileInfo"/> path of the path to create.</param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        [UnsupportedOSPlatform("Windows")]
        public FileInfo CreateOtherFile(FileInfo path)
        {
            return CreateOtherFile(path, OpenMode, Mode);
        }

        /// <summary>
        /// Create a new non- <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/>, and the value of
        /// properties <see cref="OpenMode"/> and <see cref="Mode"/>.
        /// </summary>
        /// <param name="template"> 
        /// Specifies the file template base name to be formatted with <see cref="TempDirectory.FormatFileTemplate(string, IFormatProvider?)"/>.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        [UnsupportedOSPlatform("Windows")]
        public FileInfo CreateOtherFile(string template, string? extension)
        {
            FileInfo path = new(Path.Combine(TempDirectory.TemporaryDirectory.FullName, string.Concat(TempFile.FormatFileTemplate(template), extension ?? string.Empty)));
            return CreateOtherFile(path, OpenMode, Mode);
        }

        /// <summary>
        /// Create a new non- <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/>, and the value of
        /// properties <see cref="OpenMode"/> and <see cref="Mode"/>.
        /// </summary>
        /// <param name="path">    Specifies the <see cref="FileInfo"/> path of the path to create.</param>
        /// <param name="openMode">Specifies the <see cref="FileMode"/> to use for the path created from <paramref name="path"/>.</param>
        /// <param name="mode">    Specifies the <see cref="UnixFileMode"/> to use for the path created from <paramref name="path"/>.</param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        [UnsupportedOSPlatform("Windows")]
        public FileInfo CreateOtherFile(FileInfo path, FileMode openMode, UnixFileMode mode)
        {
            if (path.Exists && IsFileInfoOpen(path))
            {
                throw new SecurityException($"Path {path.FullName} should not both exist and be open", new IOException($"Path {path.FullName} is already open"));
            }
            else if (path.Exists)
            {
                Console.Error.WriteLine($"WARNING: Path {path.FullName} exists but is not open.  Attempting to delete.");
                path.Delete();
            }

            path.Open(openMode, AccessMode, ShareMode);
            path.UnixFileMode = mode;
            path.Attributes = this.FileAttributes;
            return path;
        }

        /// <summary>
        /// Create a new non- <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/>, and the value of
        /// properties <see cref="OpenMode"/> and <see cref="Mode"/>.
        /// </summary>
        /// <param name="template"> 
        /// Specifies the file template base name to be formatted with <see cref="TempDirectory.FormatFileTemplate(string, IFormatProvider?)"/>.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <param name="openMode"> Specifies the <see cref="FileMode"/> to use for the path created from <paramref name="path"/>.</param>
        /// <param name="mode">     Specifies the <see cref="UnixFileMode"/> to use for the path created from <paramref name="path"/>.</param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <remarks>See man page for <a href="https://man7.org/linux/man-pages/man3/mkstemp.3.html"/>.</remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        [UnsupportedOSPlatform("Windows")]
        public FileInfo CreateOtherFile(string template, string? extension, FileMode openMode, UnixFileMode mode)
        {
            FileInfo path = new(
                Path.Combine(
                    TempDirectory.TemporaryDirectory.FullName,
                    string.Concat(TempFile.FormatFileTemplate(template), extension ?? string.Empty)));
            return CreateOtherFile(path, openMode, mode);
        }

        /// <summary>
        /// Creates a new, insecure <see cref="FileInfo"/> path from <paramref name="template"/><paramref name="extension"/>.
        /// </summary>
        /// <param name="template"> 
        /// Specifies the file template base name to be formatted with <see cref="TempFile.FormatFileTemplate(string,
        /// IFormatProvider?)"/>. NOTE: This avoids collision and most brute force attacks, but no security attributes are applied.
        /// </param>
        /// <param name="extension">Specifies the optional extension with a leading '.'; or null for no extension.</param>
        /// <returns>A <see cref="FileInfo"/> instance representing an insecure temporary file path ready for opening.</returns>
        /// <remarks>
        /// See <a href="https://man7.org/linux/man-pages/man3/mktemp.3.html"/> on why you should never use this method in the wild.
        /// </remarks>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        /// <exception cref="ArgumentException">Throws if the last 18 characters of <paramref name="template"/> are not 'X'.</exception>
        public FileInfo CreateTempFile(string template, string? extension)
        {
            return CreateFile(TempFile.FormatFileTemplate(template), extension ?? string.Empty);
        }

        /// <summary>
        /// Create new <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/> and the value of properties
        /// <see cref="OpenMode"/> and <see cref="WindowsFileSystemRights"/>.
        /// </summary>
        /// <param name="path">Specifies the <see cref="FileInfo"/> path of the path to create.</param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        [SupportedOSPlatform("Windows")]
        public FileInfo CreateWindowsFile(FileInfo path)
        {
            return CreateWindowsFile(path, OpenMode, WindowsFileSystemRights);
        }

        /// <summary>
        /// Create new <see cref="OperatingSystem.IsWindows"/> file path from <paramref name="path"/>, <paramref name="openMode"/>,
        /// and <paramref name="systemRights"/>, and the value of properties <see cref="ShareMode"/>, <see cref="BufferSize"/>, and
        /// <see cref="Options"/>.
        /// </summary>
        /// <param name="path">        Specifies the <see cref="FileInfo"/> path of the path to create.</param>
        /// <param name="openMode">    Specifies the <see cref="FileMode"/> to use for the path created from <paramref name="path"/>.</param>
        /// <param name="systemRights">
        /// Specifies the <see cref="FileSystemRights"/> to use for the path created from <paramref name="path"/>.
        /// </param>
        /// <returns>Returns a created <see cref="FileInfo"/> instance ready to open.</returns>
        /// <exception cref="SecurityException">Throws if file name both exists and is open.</exception>
        [SupportedOSPlatform("Windows")]
        public FileInfo CreateWindowsFile(FileInfo path, FileMode openMode, FileSystemRights systemRights)
        {
            if (path.Exists && IsFileInfoOpen(path))
            {
                throw new SecurityException($"Path {path.FullName} should not both exist and be open", new IOException($"Path {path.FullName} is already open"));
            }
            else if (path.Exists)
            {
                Console.Error.WriteLine($"WARNING: Path {path.FullName} exists but is not open.  Attempting to delete.");
                path.Delete();
            }

            using var _ = path.Create(openMode, systemRights, ShareMode, BufferSize, Options, new FileSecurity(path.FullName, AccessControlSections.None));
            path.Attributes = this.FileAttributes;
            return path;
        }

        public void DeleteTempFile(FileInfo? path)
        {
            int retries = 6;

            while (path is not null && path.Exists)
            {
                try
                {
                    path.Delete();
                }
                catch (IOException)
                {
                    if (retries-- > 0)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5.0));
                        continue;
                    }
                }
                catch (Exception ex) when (ex is SecurityException or UnauthorizedAccessException)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            // Perform async cleanup.
            await DisposeAsyncCore().ConfigureAwait(false);

            // Dispose of unmanaged resources.
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        public bool Equals(FileInfo? x, FileInfo? y)
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

        public bool Equals(TempFile? x, TempFile? y)
        {
            return this.Equals(x?.TempFilePath?.FullName, y?.TempFilePath?.FullName);
        }

        public int GetHashCode([DisallowNull] TempFile obj) => HashCode.Combine(obj.TempDirectory, obj.TempFilePath);

        public int GetHashCode([DisallowNull] FileInfo obj) => GetHashCode(obj);

        public int GetHashCode([DisallowNull] string obj) => GetHashCode(obj);

        /// <summary>
        /// Determine whether a <see cref="FileInfo"/> is open by attempting to open and processing any <see cref="Exception"/>.
        /// </summary>
        /// <param name="path">Specifies the <see cref="FileInfo"/> to process</param>
        /// <returns><see langref="true"/> if the file is open; otherwise, <see langref="false"/>.</returns>
        public bool IsFileInfoOpen(FileInfo path)
        {
            if (!path.Exists)
            {
                Console.Error.WriteLine($"Path {path.FullName} does not exist");
                return false;
            }
            else if (path.Attributes.HasFlag(FileAttributes.Directory))
            {
                Console.Error.WriteLine($"Path {path.FullName} is a directory");
                return false;
            }
            else if (path.IsReadOnly)
            {
                Console.Error.WriteLine($"Path {path.FullName} is read-only");
                return false;
            }
            else
            {
                try
                {
                    using FileStream stream = path.Open(FileMode.Open, AccessMode, ShareMode);
                    Console.Error.WriteLine($"Path {path.FullName} is not already open");
                    return false;
                }
                catch (Exception ex) when (ex is SecurityException || ex is IOException)
                {
                    Console.Error.WriteLine(ex.ToString());
                    return ex is IOException;
                }
            }
        }

        /// <summary>
        /// Determine whether a <see cref="FileInfo"/> is open by attempting to open and processing any <see cref="Exception"/> asynchronously.
        /// </summary>
        /// <param name="path">Specifies the <see cref="FileInfo"/> to process</param>
        /// <returns><see langref="true"/> if the file is open; otherwise, <see langref="false"/>.</returns>
        public async Task<bool> IsFileInfoOpenAsync(FileInfo path)
        {
            if (!path.Exists)
            {
                await Console.Error.WriteLineAsync($"Path {path.FullName} does not exist").ConfigureAwait(false);
                return false;
            }
            else if (path.Attributes.HasFlag(FileAttributes.Directory))
            {
                await Console.Error.WriteLineAsync($"Path {path.FullName} is a directory").ConfigureAwait(false);
                return false;
            }
            else if (path.IsReadOnly)
            {
                await Console.Error.WriteLineAsync($"Path {path.FullName} is read-only").ConfigureAwait(false);
                return false;
            }
            else
            {
                try
                {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                    await using FileStream stream = path.Open(FileMode.Open, AccessMode, ShareMode);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                    await Console.Error.WriteLineAsync($"Path {path.FullName} is not already open").ConfigureAwait(false);
                    return false;
                }
                catch (Exception ex) when (ex is SecurityException || ex is IOException)
                {
                    await Console.Error.WriteLineAsync(ex.ToString());
                    return ex is IOException;
                }
            }
        }

        public FileInfo MakeRelative(DirectoryInfo directory, FileInfo fileName)
        {
            if (this.Equals(directory.FullName, fileName.DirectoryName))
            {
                return TempFile.Combine(directory, fileName.Name);
            }
            else if (TempDirectory.Equals(directory.Root, fileName.Directory!.Root))
            {
                return new(Path.GetRelativePath(TempDirectory.NormalizeDirectory(directory.FullName).FullName, PathUtil.Normalize(fileName.FullName)));
            }
            else
            {
                return fileName;
            }
        }

        #endregion Public Methods
    }
}
