namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    internal partial class NativeMethod
    {
        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseProductExW([MarshalAs(UnmanagedType.LPWStr)] string packagePath, [MarshalAs(UnmanagedType.LPWStr)] string scriptFilePath, [MarshalAs(UnmanagedType.LPWStr)] string transforms, LangId language, UInt32 platform, UInt32 options);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseProductW([MarshalAs(UnmanagedType.LPWStr)] string packagePath, [MarshalAs(UnmanagedType.LPWStr)] string scriptFilePath, [MarshalAs(UnmanagedType.LPWStr)] string transforms, LangId language);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiAdvertiseScriptW([MarshalAs(UnmanagedType.LPWStr)] string scriptFile, UInt32 flags, IntPtr registryKeyData, [MarshalAs(UnmanagedType.Bool)] bool removeItems);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiApplyMultiplePatchesW([MarshalAs(UnmanagedType.LPWStr)] string patchPackages, [MarshalAs(UnmanagedType.LPWStr)][Optional] string productCode, [Optional][MarshalAs(UnmanagedType.LPWStr)] string propertiesList);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiApplyPatchW([MarshalAs(UnmanagedType.LPWStr)] string patchPackage, [MarshalAs(UnmanagedType.LPWStr)] string installPackage, InstallType installType, [MarshalAs(UnmanagedType.LPWStr)] string commandLine);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiBeginTransactionW([MarshalAs(UnmanagedType.LPWStr)] string name, UInt32 transactionAttributes, [Out] IntPtr transactionHandle, [Out] IntPtr changeOfOwnerEvent);

        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiCloseAllHandles();

        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiCloseHandle(IntPtr handleAny);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiCollectUserInfoW([MarshalAs(UnmanagedType.LPWStr)] string productCode);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureFeatureW([MarshalAs(UnmanagedType.LPWStr)] string productCode, [MarshalAs(UnmanagedType.LPWStr)] string feature, InstallState installState);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureProductExW([MarshalAs(UnmanagedType.LPWStr)] string productCode, Int32 installLevel, InstallState installState, [MarshalAs(UnmanagedType.LPWStr)] string commandLine);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiConfigureProductW([MarshalAs(UnmanagedType.LPWStr)] string productCode, Int32 installLevel, InstallState installState);

        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial IntPtr MsiCreateRecord(UInt32 countFields);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiCreateTransformSummaryInfoW(IntPtr databaseHandle, IntPtr databaseReference, [MarshalAs(UnmanagedType.LPWStr)] string transformFile, Int32 errorConditions, Int32 validation);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseApplyTransformW(IntPtr databaseHandle, [MarshalAs(UnmanagedType.LPWStr)] string transformFile, Int32 errorConditions);

        [LibraryImport("msi.dll", SetLastError = true)]
        internal static partial UInt32 MsiDatabaseCommit(IntPtr handle);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseExportW(IntPtr databaseHandle, [MarshalAs(UnmanagedType.LPWStr)] string tableName, [MarshalAs(UnmanagedType.LPWStr)] string folderPath, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseGenerateTransformW(IntPtr databaseHandle, IntPtr databaseReference, [MarshalAs(UnmanagedType.LPWStr)] string transformFile, Int32 reserved1, Int32 reserved2);\

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseGetPrimaryKeysW(IntPtr databaseHandle, [MarshalAs(UnmanagedType.LPWStr)] string tableName, [Out] IntPtr recordHandle);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseImportW(IntPtr databaseHandle, [MarshalAs(UnmanagedType.LPWStr)] string folderPath, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [LibraryImport("msi.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial UInt32 MsiDatabaseOpenView(IntPtr handleDatabase, [MarshalAs(UnmanagedType.LPWStr)] string sql, out IntPtr handleView);
    }
}
