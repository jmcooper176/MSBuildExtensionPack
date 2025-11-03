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
namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    internal partial class NativeMethod
    {
        /// <summary>
        /// Advertise product at <paramref name="packagePath"/>.
        /// </summary>
        /// <param name="packagePath">Specifies the package path.</param>
        /// <param name="scriptFilePath">Specifies the script file path.</param>
        /// <param name="transforms">Specifies on or more transforms separated by semi-colons.</param>
        /// <param name="language">Specifies the language language identifier.</param>
        /// <param name="platform">Specifies the platform.</param>
        /// <param name="options">Specifies the options.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseProductExW(
            [MarshalAs(UnmanagedType.LPWStr)] string packagePath,
            [MarshalAs(UnmanagedType.LPWStr)] string scriptFilePath,
            [MarshalAs(UnmanagedType.LPWStr)] string transforms,
            LangId language,
            UInt32 platform,
            UInt32 options);

        /// <summary>
        /// Msis the advertise product w.
        /// </summary>
        /// <param name="packagePath">The package path.</param>
        /// <param name="scriptFilePath">The script file path.</param>
        /// <param name="transforms">The transforms.</param>
        /// <param name="language">The language.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseProductW(
            [MarshalAs(UnmanagedType.LPWStr)] string packagePath,
            [MarshalAs(UnmanagedType.LPWStr)] string scriptFilePath,
            [MarshalAs(UnmanagedType.LPWStr)] string transforms, LangId language);

        /// <summary>
        /// Msis the advertise script w.
        /// </summary>
        /// <param name="scriptFile">The script file.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="registryKeyData">The registry key data.</param>
        /// <param name="removeItems">if set to <c>true</c> [remove items].</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseScriptW(
            [MarshalAs(UnmanagedType.LPWStr)] string scriptFile,
            UInt32 flags,
            IntPtr registryKeyData,
            [MarshalAs(UnmanagedType.Bool)] bool removeItems);

        /// <summary>
        /// Msis the apply multiple patches w.
        /// </summary>
        /// <param name="patchPackages">The patch packages.</param>
        /// <param name="productCode">The product code.</param>
        /// <param name="propertiesList">The properties list.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiApplyMultiplePatchesW(
            [MarshalAs(UnmanagedType.LPWStr)] string patchPackages,
            [MarshalAs(UnmanagedType.LPWStr)][Optional] string productCode,
            [MarshalAs(UnmanagedType.LPWStr)][Optional] string propertiesList);

        /// <summary>
        /// Msis the apply patch w.
        /// </summary>
        /// <param name="patchPackage">The patch package.</param>
        /// <param name="installPackage">The install package.</param>
        /// <param name="installType">Type of the install.</param>
        /// <param name="commandLine">The command line.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiApplyPatchW(
            [MarshalAs(UnmanagedType.LPWStr)] string patchPackage,
            [MarshalAs(UnmanagedType.LPWStr)] string installPackage,
            InstallType installType,
            [MarshalAs(UnmanagedType.LPWStr)] string commandLine);

        /// <summary>
        /// Msis the begin transaction w.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="transactionAttributes">The transaction attributes.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <param name="changeOfOwnerEvent">The change of owner event.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiBeginTransactionW(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            UInt32 transactionAttributes,
            [Out] IntPtr transactionHandle,
            [Out] IntPtr changeOfOwnerEvent);

        /// <summary>
        /// Msis the close all handles.
        /// </summary>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiCloseAllHandles();

        /// <summary>
        /// Msis the close handle.
        /// </summary>
        /// <param name="handleAny">The handle any.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiCloseHandle(IntPtr handleAny);

        /// <summary>
        /// Msis the collect user information w.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiCollectUserInfoW([MarshalAs(UnmanagedType.LPWStr)] string productCode);

        /// <summary>
        /// Msis the configure feature w.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="installState">State of the install.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureFeatureW(
            [MarshalAs(UnmanagedType.LPWStr)] string productCode,
            [MarshalAs(UnmanagedType.LPWStr)] string feature,
            InstallState installState);

        /// <summary>
        /// Msis the configure product ex w.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="installLevel">The install level.</param>
        /// <param name="installState">State of the install.</param>
        /// <param name="commandLine">The command line.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureProductExW(
            [MarshalAs(UnmanagedType.LPWStr)] string productCode,
            Int32 installLevel,
            InstallState installState,
            [MarshalAs(UnmanagedType.LPWStr)] string commandLine);

        /// <summary>
        /// Msis the configure product w.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="installLevel">The install level.</param>
        /// <param name="installState">State of the install.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureProductW(
            [MarshalAs(UnmanagedType.LPWStr)] string productCode,
            Int32 installLevel,
            InstallState installState);

        /// <summary>
        /// Msis the create record.
        /// </summary>
        /// <param name="countFields">The count fields.</param>
        /// <returns></returns>
        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial IntPtr MsiCreateRecord(UInt32 countFields);

        /// <summary>
        /// Msis the create transform summary information w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="databaseReference">The database reference.</param>
        /// <param name="transformFile">The transform file.</param>
        /// <param name="errorConditions">The error conditions.</param>
        /// <param name="validation">The validation.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiCreateTransformSummaryInfoW(
            IntPtr databaseHandle,
            IntPtr databaseReference,
            [MarshalAs(UnmanagedType.LPWStr)] string transformFile,
            Int32 errorConditions,
            Int32 validation);

        /// <summary>
        /// Msis the database apply transform w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="transformFile">The transform file.</param>
        /// <param name="errorConditions">The error conditions.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseApplyTransformW(
            IntPtr databaseHandle,
            [MarshalAs(UnmanagedType.LPWStr)] string transformFile,
            Int32 errorConditions);

        /// <summary>
        /// Msis the database commit.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiDatabaseCommit(IntPtr handle);

        /// <summary>
        /// Msis the database export w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseExportW(
            IntPtr databaseHandle,
            [MarshalAs(UnmanagedType.LPWStr)] string tableName,
            [MarshalAs(UnmanagedType.LPWStr)] string folderPath,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        /// <summary>
        /// Msis the database generate transform w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="databaseReference">The database reference.</param>
        /// <param name="transformFile">The transform file.</param>
        /// <param name="reserved1">The reserved1.</param>
        /// <param name="reserved2">The reserved2.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseGenerateTransformW(
            IntPtr databaseHandle,
            IntPtr databaseReference,
            [MarshalAs(UnmanagedType.LPWStr)] string transformFile,
            Int32 reserved1,
            Int32 reserved2);

        /// <summary>
        /// Msis the database get primary keys w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="recordHandle">The record handle.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseGetPrimaryKeysW(
            IntPtr databaseHandle,
            [MarshalAs(UnmanagedType.LPWStr)] string tableName,
            [Out] IntPtr recordHandle);

        /// <summary>
        /// Msis the database import w.
        /// </summary>
        /// <param name="databaseHandle">The database handle.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseImportW(
            IntPtr databaseHandle,
            [MarshalAs(UnmanagedType.LPWStr)] string folderPath,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        /// <summary>
        /// Msis the database open view.
        /// </summary>
        /// <param name="handleDatabase">The handle database.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="handleView">The handle view.</param>
        /// <returns>A <see cref="UInt32"/> representing the <c>HResult</c> for invoking the method.</returns>
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseOpenView(
            IntPtr handleDatabase,
            [MarshalAs(UnmanagedType.LPWStr)] string sql,
            [Out] IntPtr handleView);
    }
}
