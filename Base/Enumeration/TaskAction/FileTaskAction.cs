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
namespace MSBuild.ExtensionPack.Base.Enumeration.TaskAction
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using MSBuild.ExtensionPack.ErrorMessage.AttributeAccess;

    /// <summary>
    /// The <see cref="File"/> task action enumeration.
    /// </summary>
    public enum FileTaskAction
    {
        /// <summary>
        /// An unknown file task action.
        /// </summary>
        [Display(Name = "Unknown File Task Action", ShortName = "Unknown")]
        [Description("No action has been specified.")]
        Unknown = 0,

        /// <summary>
        /// Creates one or more directories task action.
        /// </summary>
        [Display(Name = "Create Directory File Task Action", ShortName = "CreateDirectory")]
        [Description("Creates one or more directories.")]
        CreateDirectory,

        /// <summary>
        /// Creates one or more sub-directories of a directory task action.
        /// </summary>
        [Display(Name = "Create SubDirectory File Task Action", ShortName = "CreateSubDirectory")]
        [Description("Creates one or more sub-directories of a directory.")]
        CreateSubDirectory,

        /// <summary>
        /// Reads a directory and outputs the sub-directory names task action.
        /// </summary>
        [Display(Name = "ReadDirectory File Task Action", ShortName = "ReadDirectory")]
        [Description("Read directory and outputs the sub-directory names.")]
        ReadDirectory,

        /// <summary>
        /// Represents an action that reads a directory and returns the names of the files it contains.
        /// </summary>
        [Display(Name = "Read Directory File Task Action", ShortName = "ReadDirectoryFile")]
        [Description("Read directory and outputs the file names only.")]
        ReadDirectoryFile,

        /// <summary>
        /// Represents an action that reads the parent directory of the specified directory and outputs the parent directory full name.
        /// </summary>
        [Display(Name = "Read Directory Parent File Task Action", ShortName = "ReadDirectoryParent")]
        [Description("Reads the parent directory of the specified directory and outputs the parent directory full name.")]
        ReadDirectoryParent,

        /// <summary>
        /// Represents an action that reads the root directory of the specified directory and outputs the root directory full name.
        /// </summary>
        [Display(Name = "Read Directory Root File Task Action", ShortName = "ReadDirectoryRoot")]
        [Description("Reads the root directory of the specified directory and outputs the root directory full name.")]
        ReadDirectoryRoot,

        /// <summary>
        /// Enumerates all subdirectory names within a specified directory, including those in nested subdirectories.
        /// </summary>
        [Display(Name = "Enumerate Directory File Task Action", ShortName = "EnumerateDirectory")]
        [Description("Enumerates directory recursively and output the sub-directory names.")]
        EnumerateDirectory,

        /// <summary>
        /// Enumerates all files and subdirectories within a directory recursively.
        /// </summary>
        /// <remarks>
        /// Use this action to retrieve the full list of file and directory names contained in a directory and all of its
        /// subdirectories. This is useful for operations that require processing or displaying the entire directory tree.
        /// </remarks>
        [Display(Name = "Enumerate Directory All File Task Action", ShortName = "EnumerateDirectoryAll")]
        [Description("Enumerates directory recursively and output all sub-directory and file names.")]
        EnumerateDirectoryAll,

        /// <summary>
        /// Enumerates all file names within a specified directory, including those in nested subdirectories.
        /// </summary>
        [Display(Name = "Enumerate Directory Files File Task Action", ShortName = "EnumerateDirectoryFile")]
        [Description("Enumerates directory recursively and output the file names only.")]
        EnumerateDirectoryFile,

        /// <summary>
        /// Represents an action that deletes one or more directories.
        /// </summary>
        /// <remarks>
        /// Deletion will fail if any subdirectories or files are present unless additional options are specified. If the
        /// 'recursive' option is enabled, all remaining subdirectories and files are deleted recursively.
        /// </remarks>
        [Display(Name = "Delete Directory File Task Action", ShortName = "DeleteDirectory")]
        [Description("Deletes one or more directories.  Deletion will fail if any sub-directories or files are present.  If 'recursive' is specified, all remaining subdirectories and files will deleted recursively.")]
        DeleteDirectory,

        /// <summary>
        /// Moves a directory to a new location task action.
        /// </summary>
        [Display(Name = "Move Directory File Task Action", ShortName = "MoveDirectory")]
        [Description("Moves a directory to a new location.")]
        MoveDirectory,

        /// <summary>
        /// Specifies an action that tests whether one or more directories exist.
        /// </summary>
        [Display(Name = "Test Directory Exists File Task Action", ShortName = "TestDirectoryExists")]
        [Description("Tests whether one or more directories exist.")]
        TestDirectoryExists,

        /// <summary>
        /// Specifies an action that creates one or more binary files as part of a task.
        /// </summary>
        [Display(Name = "Create Binary File Task Action", ShortName = "CreateFile")]
        [Description("Creates one or more binary files.")]
        CreateFile,

        /// <summary>
        /// Specifies an action that creates one or more text files as part of a task.
        /// </summary>
        [Display(Name = "Create Text File Task Action", ShortName = "CreateTextFile")]
        [Description("Creates one or more text files.")]
        CreateTextFile,

        /// <summary>
        /// Specifies an action that reads one or more binary files and outputs the byte content as a hexadecimal string.
        /// </summary>
        [Display(Name = "Read Binary File Task Action", ShortName = "ReadFile")]
        [Description("Reads one or more files and outputs the byte content as a hexadecimal string.")]
        ReadFile,

        /// <summary>
        /// Reads one or more text files and outputs the content as an array of strings, with each string representing a line in the file.
        /// </summary>
        [Display(Name = "Read Text File Task Action", ShortName = "ReadTextFile")]
        [Description("Reads one or more files and outputs the content.")]
        ReadTextFile,

        /// <summary>
        /// Specifies an action that updates one or more binary files with the translated content of a hexadecimal string.
        /// </summary>
        [Display(Name = "Update File Task Action", ShortName = "UpdateFile")]
        [Description("Updates one or more binary files with the transalated content of a hexadecimal string.")]
        UpdateFile,

        /// <summary>
        /// Represents an action that updates the contents of one or more text files.
        /// </summary>
        [Display(Name = "Unknown File Task Action", ShortName = "UpdateTextFile")]
        [Description("Updates one or more files.")]
        UpdateTextFile,

        /// <summary>
        /// Represents an action that deletes one or more files.
        /// </summary>
        [Display(Name = "Delete File Task Action", ShortName = "DeleteFile")]
        [Description("Deletes one or more files.")]
        DeleteFile,

        /// <summary>
        /// Represents an action that copies one or more files to a new location; or one file to a new location with a new name.
        /// </summary>
        [Display(Name = "Copy File File Task Action", ShortName = "CopyFile")]
        [Description("Copies one or more files to a new location; or one file to a new location with a new name.")]
        CopyFile,

        /// <summary>
        /// Represents an action that copies one or more files to a new location; or one file to a new location with a new name,
        /// ensuring that the destination file is not read-only and that the copy operation is retried up to three times if it fails
        /// due to a transient error such as a file lock, and ensures the integrity of the copied content.
        /// </summary>
        [Display(Name = "Copy File Safe File Task Action", ShortName = "CopyFileSafe")]
        [Description("Copies one or more files to a new location; or one file to a new location with a new name, ensuring that the destination file is not read-only and that the copy operation is retried up to three times if it fails due to a transient error such as a file lock, and ensures the integrity of the copied content.")]
        CopyFileSafe,

        /// <summary>
        /// Represents an action that decrypts one or more files using the Windows Data Protection API (DPAPI).
        /// </summary>
        [Display(Name = "Decrypt File Task Action", ShortName = "DecryptFile")]
        [Description("Decrypts one or more files using the Windows Data Protection API (DPAPI).")]
        DecryptFile,

        /// <summary>
        /// Represents an action that encrypts one or more files using the Windows Data Protection API (DPAPI).
        /// </summary>
        [Display(Name = "Encrypt File Task Action", ShortName = "EncryptFile")]
        [Description("Encrypts one or more files using the Windows Data Protection API (DPAPI).")]
        EncryptFile,

        /// <summary>
        /// Represents an action that moves one or more files to a new location.
        /// </summary>
        [Display(Name = "Move File Task Action", ShortName = "MoveFile")]
        [Description("Moves a one or more file to a new location.")]
        MoveFile,

        /// <summary>
        /// Represents an action that moves one or more files to a new location, ensuring that the destination file is not read-only
        /// and that the move operation is retried up to three times if it fails due to a transient error such as a file lock, and
        /// ensures the integrity of the moved content.
        /// </summary>
        [Display(Name = "Move File Safe File Task Action", ShortName = "MoveFileSafe")]
        [Description("Moves a one or more file to a new location, ensuring that the destination file is not read-only and that the move operation is retried up to three times if it fails due to a transient error such as a file lock, and ensures the integrity of the moved content.")]
        MoveFileSafe,

        /// <summary>
        /// Specifies an action that reads one or more files and outputs the directory name.
        /// </summary>
        [Display(Name = "Read File Directory File Task Action", ShortName = "ReadFileDirectory")]
        [Description("Reads one or more files and outputs the directory name.")]
        ReadFileDirectory,

        /// <summary>
        /// Represents an action that reads one or more files and outputs the file length in bytes.
        /// </summary>
        [Display(Name = "Read File Length File Task Action", ShortName = "ReadFileLength")]
        [Description("Reads one or more files and outputs the file length in bytes.")]
        ReadFileLength,

        /// <summary>
        /// Represents an action that reads one or more files and outputs their file names.
        /// </summary>
        [Display(Name = "Read File Name File Task Action", ShortName = "ReadFileName")]
        [Description("Reads one or more files and outputs the file names.")]
        ReadFileName,

        /// <summary>
        /// Specifies an action that overwrites a binary file with the contents of another specified binary file.
        /// </summary>
        [Display(Name = "Replace File Task Action", ShortName = "ReplaceFile")]
        [Description("Overwrites a binary file with the contents of another specified binary file.")]
        ReplaceFile,

        /// <summary>
        /// Specifies an action that overwrites a text file with the contents of another specified text file.
        /// </summary>
        [Display(Name = "Replace Text File Task Action", ShortName = "ReplaceTextFile")]
        [Description("Overwrites a text file with the contents of another specified text file.")]
        ReplaceTextFile,

        /// <summary>
        /// Represents an action that tests whether one or more specified files exist, returning a value indicating the existence of
        /// each file.
        /// </summary>
        [Display(Name = "Test File Exists File Task Action", ShortName = "TestFileExits")]
        [Description("Tests whether one or more files exist and outputs 'true' for each file that exists; otherwise, 'false'.")]
        TestFileExists,

        /// <summary>
        /// Represents an action that tests whether one or more files are archive files.
        /// </summary>
        [Display(Name = "Test File Is Archive File Task Action", ShortName = "TestFileIsArchive")]
        [Description("Tests whether one or more files are archive files and outputs 'true' for each file that is an archive; otherwise, 'false'.")]
        TestFileIsArchive,

        /// <summary>
        /// Represents an action that tests whether one or more files are compressed files.
        /// </summary>
        [Display(Name = "Test File Is Compressed File Task Action", ShortName = "TestFileIsCompressed")]
        [Description("Tests whether one or more files are compressed files and outputs 'true' for each file that is compressed; otherwise, 'false'.")]
        TestFileIsCompressed,

        /// <summary>
        /// Represents an action that tests whether one or more files are a device file.
        /// </summary>
        [Display(Name = "Test File Is Device File Task Action", ShortName = "TestFileIsDevice")]
        [Description("Tests whether one or more files are a device file and outputs 'true' for each file that is a device; otherwise, 'false'.")]
        TestFileIsDevice,

        /// <summary>
        /// Represents an action that tests whether one or more files are a directory.
        /// </summary>
        [Display(Name = "Test File Is Directory File Task Action", ShortName = "TestFileIsDirectory")]
        [Description("Tests whether one or more files are a directory and outputs 'true' for each file that is a directory; otherwise, 'false'.")]
        TestFileIsDirectory,

        /// <summary>
        /// Represents an action that tests whether one or more files are encrypted files.
        /// </summary>
        [Display(Name = "Test File Is Encrypted File Task Action", ShortName = "TestFileIsEncrypted")]
        [Description("Tests whether one or more files are encrypted and outputs 'true' for each file that is encrypted; otherwise, 'false'.")]
        TestFileIsEncrypted,

        /// <summary>
        /// Represents an action that tests whether one or more files are hidden.
        /// </summary>
        [Display(Name = "Test File Is Hidden File Task Action", ShortName = "TestFileIsHidden")]
        [Description("Tests whether one or more files are hidden and outputs 'true' for each file that is hidden; otherwise, 'false'.")]
        TestFileIsHidden,

        /// <summary>
        /// Represents an action that tests whether one or more files are an Integrity Stream.
        /// </summary>
        [Display(Name = "Test File Is Integrity Stream File Task Action", ShortName = "TestFileIsIntegrityStream")]
        [Description("Tests whether one or more files are integrity streams and outputs 'true' for each file that is an integrity stream; otherwise, 'false'.")]
        TestFileIsIntegrityStream,

        /// <summary>
        /// Represents an action that tests whether on or more files are normal.
        /// </summary>
        [Display(Name = "Test File Is Normal File Task Action", ShortName = "TestFileIsNormal")]
        [Description("Tests whether one or more files are normal files and outputs 'true' for each file that is a normal file; otherwise, 'false'.")]
        TestFileIsNormal,

        /// <summary>
        /// Represents an action that tests whether one or more files are not scrub data.
        /// </summary>
        [Display(Name = "Test File Is No Scrub Data File Task Action", ShortName = "TestFileIsNoScrubData")]
        [Description("Tests whether one or more files are excluded from data integrity support and outputs 'true' for each file that is offline; otherwise, 'false'.")]
        TestFileIsNoScrubData,

        /// <summary>
        /// Represents an action that tests whether one or more files are not content indexed.
        /// </summary>
        [Display(Name = "Test File Is Not Content Indexed File Task Action", ShortName = "TestFileIsNotContentIndexed")]
        [Description("Tests whether one or more files are not content indexed and outputs 'true' for each file that is not content indexed; otherwise, 'false'.")]
        TestFileIsNotContentIndexed,

        /// <summary>
        /// Represents and action that tests whether one or more files are offline.
        /// </summary>
        [Display(Name = "Test File Is Offline File Task Action", ShortName = "TestFileIsOffline")]
        [Description("Tests whether one or more files are offline and outputs 'true' for each file that is offline; otherwise, 'false'.")]
        TestFileIsOffline,

        /// <summary>
        /// Represents an action that determines whether one or more files are marked as read-only.
        /// </summary>
        [Display(Name = "Test File Is Read Only File Task Action", ShortName = "TestFileIsReadOnly")]
        [Description("Tests whether one or more files are read-only and outputs 'true' for each file that is read-only; otherwise, 'false'.")]
        TestFileIsReadOnly,

        /// <summary>
        /// Specifies an action that tests whether one or more files are reparse points, returning <see langword="true"/> for each
        /// file that is a reparse point; otherwise, <see langword="false"/>.
        /// </summary>
        [Display(Name = "Test File Is Reparse Point File Task Action", ShortName = "TestFileIsReparsePoint")]
        [Description("Tests whether one or more files are a reparse point and outputs 'true' for each file that is a reparse point; otherwise, 'false'.")]
        TestFileIsReparsePoint,

        /// <summary>
        /// Represents an action that tests whether one or more files are sparse files and outputs <see langword="true"/> for each
        /// file that is a sparse file; otherwise, <see langword="false"/>.
        /// </summary>
        [Display(Name = "Test File Is Sparse File Task Action", ShortName = "TestFileIsSparseFile")]
        [Description("Tests whether one or more files are a sparse file and outputs 'true' for each file that is a sparse file; otherwise, 'false'.")]
        TestFileIsSparseFile,

        /// <summary>
        /// Represents an action that tests whether one or more files are system files.
        /// </summary>
        [Display(Name = "Test File Is System File Task Action", ShortName = "TestFileIsSystem")]
        [Description("Tests whether one or more files are a system file and outputs 'true' for each file that is a system file; otherwise, 'false'.")]
        TestFileIsSystem,

        /// <summary>
        /// Specifies an action that tests whether one or more files are temporary files.
        /// </summary>
        [Display(Name = "Test File Is Temporary File Task Action", ShortName = "TestFileIsTemporary")]
        [Description("Tests whether one or more files are a temporary file and outputs 'true' for each file that is a temporary file; otherwise, 'false'.")]
        TestFileIsTemporary,

        /// <summary>
        /// Represents an action that updates the last write time of one or more files to the current date and time, creating a new
        /// binary file if it does not already exist.
        /// </summary>
        [Display(Name = "Touch File Task Action", ShortName = "TouchFile")]
        [Description("Updates the last write time of one or more files to the current date and time, creating a binary file if it does not already exist.")]
        TouchFile,

        /// <summary>
        /// Specifies an action that sets one or more files to the given attributes, replacing any existing file attributes.
        /// </summary>
        /// <remarks>
        /// Use this action to ensure that the specified files have only the provided attributes. Any attributes previously set on
        /// the files will be removed before applying the new ones.
        /// </remarks>
        [Display(Name = "Create File Attribute File Task Action", ShortName = "CreateFileAttribute")]
        [Description("Set one or more files with the specified attributes, removing any existing file attributes.")]
        CreateFileAttribute,

        /// <summary>
        /// Specifies an action that reads one or more files and outputs their file attributes.
        /// </summary>
        [Display(Name = "Read File Attribute File Task Action", ShortName = "ReadFileAttribute")]
        [Description("Reads one or more files and outputs the file attributes.")]
        ReadFileAttribute,

        /// <summary>
        /// Specifies an action that adds the given file attributes to one or more files.
        /// </summary>
        [Display(Name = "Update File Attribute File Task Action", ShortName = "UpdateFileAttribute")]
        [Description("Add the specified file attributes to the file(s).")]
        UpdateFileAttribute,

        /// <summary>
        /// Specifies an action that removes the specified attributes from one or more files.
        /// </summary>
        [Display(Name = "Delete File Attribute File Task Action", ShortName = "DeleteFileAttribute")]
        [Description("Remove the specified attributes from the file(s).")]
        DeleteFileAttribute,

        /// <summary>
        /// Specifies an action that creates one or more text files with the provided content.
        /// </summary>
        [Display(Name = "Create File Content File Task Action", ShortName = "CreateFileContent")]
        [Description("Creates one or more text files with the specified text content.")]
        CreateFileContent,

        /// <summary>
        /// Represents an action that reads the contents of one or more text files and outputs their content.
        /// </summary>
        [Display(Name = "Read File Content File Task Action", ShortName = "ReadFileContent")]
        [Description("Read one or more text files and output the contents.")]
        ReadFileContent,

        /// <summary>
        /// Specifies an action that concatenates the contents of one or more text files into a single target text file.
        /// </summary>
        [Display(Name = "Concatenate File Content File Task Action", ShortName = "ConcatenateFileContent")]
        [Description("Concatenates one or more text files into a single target text file.")]
        ConcatenateFileContent,

        /// <summary>
        /// Represents an action that counts characters, words, or lines in one or more files and outputs the total count.
        /// </summary>
        [Display(Name = "Measure File Content File Task Action", ShortName = "MeasureFileContent")]
        [Description("Counts characters, words, or lines in one or more files and outputs the total count.")]
        MeasureFileContent,

        /// <summary>
        /// Represents an action that deletes the contents of one or more files without removing the files themselves.
        /// </summary>
        /// <remarks>
        /// Use this action to truncate files, clearing their contents while preserving their existence and metadata. This is useful
        /// when you need to reset file contents without deleting the files from the file system.
        /// </remarks>
        [Display(Name = "Delete File Content File Task Action", ShortName = "DeleteFileContent")]
        [Description("Truncates one or more files.")]
        DeleteFileContent,

        /// <summary>
        /// Replaces the contents of a specified text file with the contents of another text file.
        /// </summary>
        [Display(Name = "Replace File Content File Task Action", ShortName = "ReplaceFileContent")]
        [Description("Replaces the contents of one text file with the contents of another text file.")]
        ReplaceFileContent,

        /// <summary>
        /// Specifies an action that overwrites the contents of a text file with the provided array of strings.
        /// </summary>
        [Display(Name = "Overwrite File Content File Task Action", ShortName = "OverwriteFileContent")]
        [Description("Overwrite the text file contents with the specified array of strings.")]
        OverwriteFileContent,

        /// <summary>
        /// Appends the specified text content to the end of a text file as part of a file task action.
        /// </summary>
        [Display(Name = "Append File Content File Task Action", ShortName = "AppendFileContent")]
        [Description("Appends the specified text content to the end of the text file.")]
        AppendFileContent,

        /// <summary>
        /// Specifies an action that computes a cryptographic hash (SHA256, SHA384, SHA512, or other supported
        /// algorithm) for the specified file and outputs the result.
        /// </summary>
        /// <remarks>
        /// Use this action to generate a hash value for a file, which can be used for integrity verification or comparison
        /// purposes. The specific hash algorithm used may depend on additional configuration or parameters.
        /// </remarks>
        [Display(Name = "Create File Hash File Task Action", ShortName = "CreateFileHash")]
        [Description("Computes an SHA256, SHA384, SHA512, or Cryptographic Hash for the specified file and outputs it.")]
        CreateFileHash,

        /// <summary>
        /// Represents an action that reads a previously computed hash value for a specified file and compares it to the current
        /// file contents to determine if the hashes match.
        /// </summary>
        /// <remarks>
        /// This action is typically used to verify file integrity by checking whether the file has changed since the hash was last
        /// computed. It outputs <see langword="true"/> if the current file hash matches the stored hash; otherwise, it outputs <see langword="false"/>.
        /// </remarks>
        [Display(Name = "Read File Hash File Task Action", ShortName = "ReadFileHash")]
        [Description("Reads the previously computed hash value for the specified file and compares it to the current file contents outputing 'true' if the file hashes match; otherwise, 'false'.")]
        ReadFileHash,

        /// <summary>
        /// Represents an action that reads the previously computed hash value for a specified file and outputs an updated hash if
        /// the file has changed.
        /// </summary>
        /// <remarks>
        /// Use this action to detect changes in a file by comparing its current hash with a previously stored value. If the file
        /// contents have changed, the updated hash is provided; otherwise, the original hash is retained.
        /// </remarks>
        [Display(Name = "Update File Hash File Task Action", ShortName = "UpdateFileHash")]
        [Description("Reads the previously computed hash value for the specified file and outputs the updated hash if the file has changed.")]
        UpdateFileHash,

        /// <summary>
        /// Creates a new file system access control rule for the specified file or files, replacing any existing rules.
        /// </summary>
        /// <remarks>
        /// Use this action to define a specific access control rule and remove all previous rules from the target file or files.
        /// This operation is typically used to enforce a strict security policy by ensuring only the newly specified rule is applied.
        /// </remarks>
        [Display(Name = "Create FileSystem Security Rule File Task Action", ShortName = "CreateFileSystemSecurityRule")]
        [Description("Creatses a new file system access control rule for the specified file(s) and clears all other rules.")]
        CreateFileSystemSecurityRule,

        /// <summary>
        /// Represents an action that reads the access control list (ACL) entries for one or more specified files.
        /// </summary>
        /// <remarks>
        /// Use this action to retrieve security information, such as permissions and access rules, associated with the target
        /// file(s). This can be useful for auditing or managing file system security settings.
        /// </remarks>
        [Display(Name = "Read FileSystem Security Rule File Task Action", ShortName = "ReadFileSystemSecurityRule")]
        [Description("Reads the access control list (ACL) entries for the specified file(s).")]
        ReadFileSystemSecurityRule,

        /// <summary>
        /// Specifies an action that adds access control list (ACL) entries to one or more files, updating their file system
        /// security rules.
        /// </summary>
        /// <remarks>
        /// Use this action to grant or modify permissions for specified users or groups on the target files. The changes apply only
        /// to the files specified and do not affect parent directories unless explicitly included.
        /// </remarks>
        [Display(Name = "Update FileSystem Security File Rule File Task Action", ShortName = "UpdateFileSystemSecurityRule")]
        [Description("Adds specified access control list (ACL) entries to the file(s).")]
        UpdateFileSystemSecurityRule,

        /// <summary>
        /// Removes specified access control list (ACL) entries from one or more files.
        /// </summary>
        /// <remarks>
        /// Use this action to delete file system security rules, such as user or group permissions, from the selected files.
        /// Removing ACL entries may affect access for users or applications that rely on these permissions. Ensure that you have
        /// appropriate privileges before performing this action.
        /// </remarks>
        [Display(Name = "Delete FileSystem Security  Rule File Task Action", ShortName = "DeleteFileSystemSecurityRule")]
        [Description("Removes specified access control list (ACL) entries from the file(s).")]
        DeleteFileSystemSecurityRule,

        /// <summary>
        /// Specifies an action that creates a secure temporary file in a secure directory and outputs its full path.
        /// </summary>
        /// <remarks>
        /// The temporary file is created with appropriate file attributes and security rules to help prevent unauthorized access.
        /// Use this action when a temporary file is needed for sensitive data or operations requiring enhanced security.
        /// </remarks>
        [Display(Name = "Create Secure Temp Path File Task Action", ShortName = "CreateSecureTempPath")]
        [Description("Creates a secure temporary file with appropriate file attributes and file security rules and in a secure path and outputs the path full name.")]
        CreateSecureTempPath,

        /// <summary>
        /// Represents an action that reads a secure temporary file and outputs its contents as either an array of strings or a
        /// hexadecimal string.
        /// </summary>
        /// <remarks>
        /// Use this action to retrieve the contents of a secure temporary file for further processing. The output format depends on
        /// the context in which the action is used.
        /// </remarks>
        [Display(Name = "Read Secure Temp Path File Task Action", ShortName = "ReadSecureTempPath")]
        [Description("Reads the secure temporary file and outputs the contents as either an array of strings or as a hexadecimal string.")]
        ReadSecureTempPath,

        /// <summary>
        /// Represents an action that updates a secure temporary file with the specified content, either as an array of strings or
        /// as a hexadecimal string.
        /// </summary>
        [Display(Name = "Update Secure Temp Path File Task Action", ShortName = "UpdateSecureTempPath")]
        [Description("Updates the secure temporary file with the specified content, either as an array of strings or as a hexadecimal string.")]
        UpdateSecureTempPath,

        /// <summary>
        /// Represents an action that deletes a secure temporary file.
        /// </summary>
        [Display(Name = "Delete Secure Temp Path File Task Action", ShortName = "DeleteSecureTempPath")]
        [Description("Deletes the secure temporary file.")]
        DeleteSecureTempPath,

        /// <summary>
        /// Specifies an action that creates a temporary file in an unsecured location without applying special file attributes or
        /// security rules.
        /// </summary>
        /// <remarks>
        /// The created temporary file will have a unique name and will not include any custom file permissions or security
        /// settings. Use this action when a simple, unsecured temporary file is sufficient for your scenario.
        /// </remarks>
        [Display(Name = "Create Temp File File Task Action", ShortName = "CreateTempFile")]
        [Description("Creates a temporary file without any special file attributes or file security rules and in and unsecure path and outputs the name of the file.")]
        CreateTempFile,

        /// <summary>
        /// Represents an action that reads a temporary file and outputs its contents as either an array of strings or a hexadecimal string.
        /// </summary>
        /// <remarks>
        /// Use this action to retrieve the contents of a temporary file for further processing or inspection. The output format may
        /// vary depending on the context in which the action is used.
        /// </remarks>
        [Display(Name = "Read Temp File File Task Action", ShortName = "ReadTempFile")]
        [Description("Reads the temporary file and outputs the contents as either an array of strings or as a hexadecimal string.")]
        ReadTempFile,

        /// <summary>
        /// Represents an action that updates a temporary file with the specified content.
        /// </summary>
        /// <remarks>
        /// This action can update the temporary file using either an array of strings or a hexadecimal string as the content. The
        /// method of update depends on the context in which the action is invoked.
        /// </remarks>
        [Display(Name = "Update Temp File File Task Action", ShortName = "UpdateTempFile")]
        [Description("Updates the temporary file with the specified content, either as an array of strings or as a hexadecimal string.")]
        UpdateTempFile,

        /// <summary>
        /// Specifies an action that deletes a temporary file.
        /// </summary>
        [Display(Name = "Delete Temp File File Task Action", ShortName = "DeleteTempFile")]
        [Description("Deletes the temporary file.")]
        DeleteTempFile,
    }

    public static class FileTaskActionExtensions
    {
        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DescriptionAttribute"/>.</returns>
        public static string? GetDescription(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDescriptionAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the description text of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetDescription2(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the description string from the <see cref="DescriptionAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DescriptionAttribute"/> or <see langref="null"/> if no <see cref="DescriptionAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DescriptionAttribute? GetDescriptionAttribute(this FileTaskAction value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DescriptionAttribute, FileTaskAction>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the <see cref="DisplayAttribute"/> on an <see cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>
        /// A <see cref="DisplayAttribute"/> or <see langref="null"/> if no <see cref="DisplayAttribute"/> on <paramref
        /// name="value"/> was found.
        /// </returns>
        public static DisplayAttribute? GetDisplayAttribute(this FileTaskAction value, bool inherit = false)
        {
            return CustomAttribute.GetCustomAttribute<DisplayAttribute, FileTaskAction>(value, inherit);
        }

        /// <summary>
        /// Extension method to recover the group name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the group name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetGroupName(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.GroupName;
        }

        /// <summary>
        /// Extension method to recover the name string from the <see cref="DisplayAttribute"/> on an <see cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetName(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Description;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>An <see cref="int"/> or <see langref="null"/> representing the order property of the <see cref="DisplayAttribute"/>.</returns>
        public static int? GetOrder(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Order;
        }

        /// <summary>
        /// Extension method to recover the order property from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the prompt for the <see cref="DisplayAttribute"/>.</returns>
        public static string? GetPrompt(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.Prompt;
        }

        /// <summary>
        /// Extension method to recover the resource <see cref="Type"/> from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="Type"/> or <see langref="null"/> representing the resource <see cref="Type"/> of the <see cref="DisplayAttribute"/>.</returns>
        public static Type? GetResourceType(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ResourceType;
        }

        /// <summary>
        /// Extension method to recover the short name string from the <see cref="DisplayAttribute"/> on an <see
        /// cref="FileTaskAction"/> field.
        /// </summary>
        /// <param name="value">  Specifies the <see cref="FileTaskAction"/> field.</param>
        /// <param name="inherit">
        /// If <see langref="true"/>, inherited custom attributes will be considered; otherwise, only current custom attributes will
        /// be considered.
        /// </param>
        /// <returns>A <see cref="string"/> or <see langref="null"/> representing the short name of the <see cref="DisplayAttribute"/>.</returns>
        public static string? ShortName(this FileTaskAction value, bool inherit = false)
        {
            return value.GetDisplayAttribute(inherit)?.ShortName;
        }
    }
}
