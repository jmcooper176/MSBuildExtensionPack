namespace MSBuild.ExtensionPack.ErrorMessage.Code
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum NtStatus : uint
    {
        /// <summary>
        /// Indicates that the operation completed successfully.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Success = 0x00000000,

        /// <summary>
        /// Indicates that no wait operation is performed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Wait0 = 0x00000000,

        /// <summary>
        /// Specifies that the wait operation should wait for the first event to be signaled.
        /// </summary>
        /// <remarks>
        /// Use this value when you want the wait operation to complete as soon as any one of the specified events is signaled,
        /// rather than waiting for all events. This is commonly used in scenarios where multiple resources may become available and
        /// you want to proceed as soon as any one is ready.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Wait1 = 0x00000001,

        /// <summary>
        /// Specifies that the operation should wait for a secondary condition before proceeding.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Wait2 = 0x00000002,

        /// <summary>
        /// Specifies the wait operation type with a value of 0x00000003.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Wait3 = 0x00000003,

        /// <summary>
        /// Specifies the wait operation with an identifier of 63.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Wait63 = 0x0000003f,

        /// <summary>
        /// Indicates that the item has been abandoned and is no longer in use.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Abandoned = 0x00000080,

        /// <summary>
        /// Indicates that a wait operation has returned because the specified object was abandoned.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by wait functions when a mutex was not properly released by the owning thread before it
        /// terminated. Handling this result may be necessary to ensure application stability and resource integrity.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AbandonedWait0 = 0x00000080,

        /// <summary>
        /// Indicates that a wait operation was abandoned, typically due to a thread exiting without releasing a mutex.
        /// </summary>
        /// <remarks>
        /// This value is commonly returned by wait functions when a mutex was not properly released by the owning thread.
        /// Applications should handle this result to ensure resource integrity and avoid potential deadlocks.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AbandonedWait1 = 0x00000081,

        /// <summary>
        /// Indicates that a wait operation was abandoned and returned with the status code 0x00000082.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a synchronization object, such as a mutex, was not properly released by the
        /// owning thread before the wait operation completed. Handling this status may be necessary to ensure application stability
        /// and resource integrity.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AbandonedWait2 = 0x00000082,

        /// <summary>
        /// Indicates that a wait operation has been abandoned. This value is returned when a wait function detects that the
        /// specified object was abandoned.
        /// </summary>
        /// <remarks>
        /// An abandoned wait typically occurs when a thread exits without releasing a synchronization object, such as a mutex. This
        /// can indicate a programming error and may require corrective action.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AbandonedWait3 = 0x00000083,

        /// <summary>
        /// Indicates that a wait operation was abandoned, corresponding to the status code 0x000000bf.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a synchronization object, such as a mutex, was not properly released by the
        /// owning thread before the wait operation completed. Handling this status may require additional cleanup or error handling
        /// to ensure application stability.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AbandonedWait63 = 0x000000bf,

        /// <summary>
        /// Indicates that the thread is in a user-mode asynchronous procedure call (APC) state.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify or check thread states related to user-mode APCs in thread management or
        /// synchronization scenarios.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UserApc = 0x000000c0,

        /// <summary>
        /// Indicates that the thread is in a kernel asynchronous procedure call (APC) state.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify threads that are executing kernel-mode APC routines. Kernel APCs are used by
        /// the operating system to perform asynchronous operations in the context of a specific thread.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        KernelApc = 0x00000100,

        /// <summary>
        /// Indicates that the item is in an alerted state.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Alerted = 0x00000101,

        /// <summary>
        /// Indicates that the operation timed out before completion.
        /// </summary>
        /// <remarks>
        /// Use this value to represent scenarios where an operation exceeds its allotted time and does not finish successfully.
        /// This is commonly used in error handling to distinguish timeout conditions from other types of failures.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Timeout = 0x00000102,

        /// <summary>
        /// Indicates that the operation or request is pending and has not yet been completed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Pending = 0x00000103,

        /// <summary>
        /// Indicates that the file or directory has a reparse point, such as a symbolic link or mount point.
        /// </summary>
        /// <remarks>
        /// A reparse point is a file system object with special metadata that can be used to implement features like symbolic
        /// links, junctions, or custom file system behaviors. This value is typically used when working with file attributes to
        /// identify objects that require special handling.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Reparse = 0x00000104,

        /// <summary>
        /// Indicates that additional entries are available beyond those currently provided.
        /// </summary>
        /// <remarks>
        /// Use this value to signal that the result set is incomplete and more data can be retrieved, such as in paginated or
        /// batched operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MoreEntries = 0x00000105,

        /// <summary>
        /// Indicates that not all required assignments have been completed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotAllAssigned = 0x00000106,

        /// <summary>
        /// Represents a value that is not mapped to a database column.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SomeNotMapped = 0x00000107,

        /// <summary>
        /// Indicates that an opportunistic lock (oplock) break is currently in progress.
        /// </summary>
        /// <remarks>
        /// This value is typically used in file system or network protocols to signal that a client or server is handling an oplock
        /// break request. An oplock break occurs when another process requests access to a file that is currently locked for
        /// exclusive or cached access, requiring the holder to release or downgrade the lock.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        OpLockBreakInProgress = 0x00000108,

        /// <summary>
        /// Indicates that the device is mounted as a volume.
        /// </summary>
        /// <remarks>
        /// Use this value to identify devices that are currently accessible as mounted volumes. This can be useful when determining
        /// available storage or when performing operations that require a mounted file system.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        VolumeMounted = 0x00000109,

        /// <summary>
        /// Indicates that the prescription action has been committed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RxActCommitted = 0x0000010a,

        /// <summary>
        /// Indicates that a notification to perform cleanup should be sent.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotifyCleanup = 0x0000010b,

        /// <summary>
        /// Specifies that changes to the directory, such as file creation, deletion, or renaming, should be reported by the
        /// notification system.
        /// </summary>
        /// <remarks>
        /// Use this value to monitor directory-level changes rather than changes to individual files. This is typically used with
        /// file system notification APIs to receive alerts when the contents of a directory are modified.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotifyEnumDir = 0x0000010c,

        /// <summary>
        /// Indicates that no quotas are defined for the specified account.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoQuotasForAccount = 0x0000010d,

        /// <summary>
        /// Indicates that a connection attempt to the primary transport has failed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PrimaryTransportConnectFailed = 0x0000010e,

        /// <summary>
        /// Indicates a transition caused by a page fault, typically when a memory page is not present and must be loaded.
        /// </summary>
        /// <remarks>
        /// This value is commonly used in memory management or diagnostic scenarios to identify transitions related to page faults.
        /// The specific meaning may vary depending on the context in which it is used.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PageFaultTransition = 0x00000110,

        /// <summary>
        /// Indicates a page fault caused by a demand-zero operation, where a memory page is allocated and initialized to zero upon
        /// first access.
        /// </summary>
        /// <remarks>
        /// This value is typically used in memory management scenarios to identify page faults that occur when a process accesses a
        /// page that has not been previously allocated, resulting in the operating system providing a zero-initialized page.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PageFaultDemandZero = 0x00000111,

        /// <summary>
        /// Indicates that a page fault occurred due to a copy-on-write operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify memory management events where a process attempts to write to a shared memory
        /// page, causing the operating system to create a private copy for the process. It is commonly encountered in diagnostics
        /// or low-level memory monitoring scenarios.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PageFaultCopyOnWrite = 0x00000112,

        /// <summary>
        /// Indicates a page fault guard page, typically used to detect or handle memory access violations.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PageFaultGuardPage = 0x00000113,

        /// <summary>
        /// Indicates that a page fault occurred when accessing data in the paging file.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PageFaultPagingFile = 0x00000114,

        /// <summary>
        /// Indicates that a crash dump event has occurred.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CrashDump = 0x00000116,

        /// <summary>
        /// Specifies a reparse object file attribute, typically used to indicate that a file or directory has associated reparse
        /// point data.
        /// </summary>
        /// <remarks>
        /// Reparse objects are commonly used in file systems to support features such as symbolic links, mount points, or other
        /// extended file system behaviors. The exact meaning and usage of this attribute may depend on the underlying file system
        /// and platform.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ReparseObject = 0x00000118,

        /// <summary>
        /// Indicates that there is no process or object to terminate.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NothingToTerminate = 0x00000122,

        /// <summary>
        /// Indicates that the process is not part of a job object.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProcessNotInJob = 0x00000123,

        /// <summary>
        /// Specifies that the process should be executed within a job object.
        /// </summary>
        /// <remarks>
        /// Use this value to indicate that the process will run as part of a job, which may impose resource limits or group
        /// management. This is typically used in environments where process isolation or control is required.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProcessInJob = 0x00000124,

        /// <summary>
        /// Indicates that a process has been cloned.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProcessCloned = 0x00000129,

        /// <summary>
        /// Indicates that the file is locked, but only by readers. No writers currently hold a lock on the file.
        /// </summary>
        /// <remarks>
        /// This value can be used to determine whether write operations are permitted on the file. When set, read access is
        /// allowed, but attempts to acquire a write lock may fail until all reader locks are released.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileLockedWithOnlyReaders = 0x0000012a,

        /// <summary>
        /// Indicates that the file is locked due to active writer processes.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when an attempt to access or modify a file fails because one or more processes are
        /// currently writing to it. Applications should wait until all writer processes have released the file before retrying the operation.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileLockedWithWriters = 0x0000012b,

        /// <summary>
        /// Specifies that the member provides informational data, such as metadata or descriptive details, rather than functional behavior.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Informational = 0x40000000,

        /// <summary>
        /// Indicates that the object name already exists.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectNameExists = 0x40000000,

        /// <summary>
        /// Indicates that the thread was suspended.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ThreadWasSuspended = 0x40000001,

        /// <summary>
        /// Specifies a limit range for the working set of a process.
        /// </summary>
        /// <remarks>
        /// This value can be used to indicate that a working set limit range is being set or queried, typically in process
        /// management or resource control scenarios.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        WorkingSetLimitRange = 0x40000002,

        /// <summary>
        /// Indicates that the image is not loaded at its preferred base address.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that an executable or library has been relocated in memory, which may affect
        /// address-dependent operations or optimizations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ImageNotAtBase = 0x40000003,

        /// <summary>
        /// Indicates that the registry has been successfully recovered after a failure or corruption.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RegistryRecovered = 0x40000009,

        /// <summary>
        /// Specifies a warning status code, typically used to indicate a non-critical issue that does not prevent operation.
        /// </summary>
        /// <remarks>
        /// This value can be used to represent conditions where an operation completes with warnings rather than errors. The
        /// specific meaning of the warning depends on the context in which it is used.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Warning = 0x80000000,

        /// <summary>
        /// Indicates that a guard page violation has occurred, typically when a process attempts to access memory beyond a
        /// protected boundary.
        /// </summary>
        /// <remarks>
        /// A guard page violation is commonly used in memory management to detect stack overflows or unauthorized memory access.
        /// This value may be returned by system-level APIs or exception handlers when such a violation is detected.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        GuardPageViolation = 0x80000001,

        /// <summary>
        /// Indicates an error caused by a misalignment of data types during an operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent a specific error code in scenarios where a data type mismatch occurs, such as
        /// when reading or writing data with incompatible types. It may be encountered in interop or low-level data access operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DatatypeMisalignment = 0x80000002,

        /// <summary>
        /// Represents a breakpoint exception code, typically used to indicate that a breakpoint has been reached during program execution.
        /// </summary>
        /// <remarks>
        /// This value is commonly encountered in debugging scenarios when execution is paused at a breakpoint. It may be used to
        /// identify or handle breakpoint exceptions in exception handling logic.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Breakpoint = 0x80000003,

        /// <summary>
        /// Indicates that the operation should be performed in single-step mode.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SingleStep = 0x80000004,

        /// <summary>
        /// Indicates that a buffer overflow error has occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that an operation has exceeded the allocated buffer size, resulting in data loss
        /// or corruption. Handling this error may require increasing the buffer size or validating input data to prevent overflow conditions.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BufferOverflow = 0x80000005,

        /// <summary>
        /// Indicates that there are no more files available to process or enumerate.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoMoreFiles = 0x80000006,

        /// <summary>
        /// Indicates that the operation handles closed resources.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        HandlesClosed = 0x8000000a,

        /// <summary>
        /// Indicates that a partial copy operation is requested.
        /// </summary>
        /// <remarks>
        /// Use this value to specify that only a portion of the data should be copied, rather than the entire content. The exact
        /// behavior may depend on the context in which this value is used.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PartialCopy = 0x8000000d,

        /// <summary>
        /// Indicates that the device is currently busy and cannot process the requested operation.
        /// </summary>
        /// <remarks>
        /// This value may be returned by device-related APIs when the device is occupied with another task. Callers should wait and
        /// retry the operation if appropriate.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DeviceBusy = 0x80000011,

        /// <summary>
        /// Indicates that the specified extended attribute name is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidEaName = 0x80000013,

        /// <summary>
        /// Indicates that the extended attribute list is inconsistent.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when the system detects a mismatch or corruption in the extended attribute list
        /// associated with a file or object. Handling this error may require validating or repairing the extended attribute data.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EaListInconsistent = 0x80000014,

        /// <summary>
        /// Indicates that there are no more entries available in the collection or enumeration.
        /// </summary>
        /// <remarks>
        /// This value is typically used as a status or error code to signal the end of a sequence when iterating through items. It
        /// can be returned by methods or APIs that enumerate resources, such as files, records, or directory entries.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoMoreEntries = 0x8000001a,

        /// <summary>
        /// Represents the long jump event in the enumeration.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LongJump = 0x80000026,

        /// <summary>
        /// Indicates that a DLL may be insecure due to potential vulnerabilities or unsafe configurations.
        /// </summary>
        /// <remarks>
        /// This value can be used to signal a warning or error condition when evaluating the security of a DLL file. It is
        /// typically returned by security checks or validation routines to inform callers that further investigation or remediation
        /// may be required.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DllMightBeInsecure = 0x8000002b,

        /// <summary>
        /// Indicates that an error has occurred during the operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Error = 0xc0000000,

        /// <summary>
        /// Indicates that the operation was unsuccessful.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Unsuccessful = 0xc0000001,

        /// <summary>
        /// Indicates that the requested operation is not implemented.
        /// </summary>
        /// <remarks>
        /// Use this value to represent an error condition where a feature or method is not available or has not been provided by
        /// the implementation.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotImplemented = 0xc0000002,

        /// <summary>
        /// Represents an error code indicating that the provided information is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidInfoClass = 0xc0000003,

        /// <summary>
        /// Indicates that the length of the provided information does not match the expected length.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InfoLengthMismatch = 0xc0000004,

        /// <summary>
        /// Indicates an access violation error, which occurs when a process attempts to read from or write to a protected memory location.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an access violation exception, such as when invalid memory is accessed. It
        /// corresponds to the standard Windows error code 0xC0000005.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AccessViolation = 0xc0000005,

        /// <summary>
        /// Indicates that a page error occurred during a memory access operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where the system cannot read or write to a memory page due
        /// to hardware failure or other critical issues. It is commonly encountered in low-level system programming or when
        /// handling native error codes.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InPageError = 0xc0000006,

        /// <summary>
        /// Indicates that the quota limit for the system pagefile has been exceeded.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition related to insufficient pagefile resources. It may be
        /// returned by system APIs when an operation cannot be completed due to pagefile quota restrictions.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PagefileQuota = 0xc0000007,

        /// <summary>
        /// Indicates that an invalid handle was specified.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system operations when a handle provided to a function is not valid or has been
        /// closed. Handle validity should be verified before passing to APIs that require it.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidHandle = 0xc0000008,

        /// <summary>
        /// Indicates that the initial stack provided is invalid or corrupted.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadInitialStack = 0xc0000009,

        /// <summary>
        /// Indicates that the initial program counter (PC) value is invalid.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where the starting address for execution is not valid. It
        /// may be returned by APIs or components that validate program counters during initialization or loading.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadInitialPc = 0xc000000a,

        /// <summary>
        /// Represents an error condition indicating that the provided CID (Connection Identifier) is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidCid = 0xc000000b,

        /// <summary>
        /// Indicates that the timer was not canceled and completed as scheduled.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TimerNotCanceled = 0xc000000c,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter = 0xc000000d,

        /// <summary>
        /// Indicates that a device specified in an operation does not exist.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchDevice = 0xc000000e,

        /// <summary>
        /// Indicates that a file specified in an operation could not be found.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when a requested file does not exist at the specified path.
        /// It may be returned by file system operations or APIs that access files.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchFile = 0xc000000f,

        /// <summary>
        /// Indicates that the requested operation cannot be performed because the device does not support the request.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by device drivers or system APIs when an operation is not valid for the specified
        /// device. It may be used to signal unsupported features or commands.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidDeviceRequest = 0xc0000010,

        /// <summary>
        /// Indicates that the end of a file has been reached.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EndOfFile = 0xc0000011,

        /// <summary>
        /// Indicates that the specified volume is incorrect or does not match the expected volume.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        WrongVolume = 0xc0000012,

        /// <summary>
        /// Indicates that no media is present in the device.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoMediaInDevice = 0xc0000013,

        /// <summary>
        /// Indicates that a memory allocation failed due to insufficient system resources.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system operations when the requested memory cannot be allocated. It may occur during
        /// high memory usage or when system limits are reached.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoMemory = 0xc0000017,

        /// <summary>
        /// Represents a view that is not mapped to a database table or entity.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotMappedView = 0xc0000019,

        /// <summary>
        /// Indicates that the operation failed because the virtual memory could not be freed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UnableToFreeVm = 0xc000001a,

        /// <summary>
        /// Indicates that a section could not be deleted due to an error.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UnableToDeleteSection = 0xc000001b,

        /// <summary>
        /// Represents an error condition indicating that an illegal instruction was encountered during program execution.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that the processor attempted to execute an instruction that is not recognized or
        /// permitted. It may occur due to corrupted code, unsupported instructions, or hardware faults.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IllegalInstruction = 0xc000001d,

        /// <summary>
        /// Indicates that the operation could not be completed because the transaction has already been committed.
        /// </summary>
        /// <remarks>
        /// Use this value to detect scenarios where further changes or rollbacks are not possible due to the transaction's
        /// committed state.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AlreadyCommitted = 0xc0000021,

        /// <summary>
        /// Indicates that access to the requested resource is denied.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AccessDenied = 0xc0000022,

        /// <summary>
        /// Indicates that the buffer provided to an operation is too small to hold the required data.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by methods or APIs that require a buffer of sufficient size to complete the operation.
        /// Callers should allocate a larger buffer and retry the operation if this value is encountered.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BufferTooSmall = 0xc0000023,

        /// <summary>
        /// Indicates that an operation failed due to a mismatch between the expected and actual object types.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectTypeMismatch = 0xc0000024,

        /// <summary>
        /// Indicates an error condition where the application cannot continue execution after the exception is thrown.
        /// </summary>
        /// <remarks>
        /// This value corresponds to the Windows NTSTATUS code 0xC0000025, which represents a non-continuable exception. Typically,
        /// this exception is raised when an operation results in a state from which recovery is not possible, and the process must terminate.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NonContinuableException = 0xc0000025,

        /// <summary>
        /// Indicates that a stack is corrupted or invalid.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where the stack cannot be used due to corruption. It may be
        /// returned by APIs or set in error codes to signal stack-related failures.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadStack = 0xc0000028,

        /// <summary>
        /// Indicates that the resource is not locked.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotLocked = 0xc000002a,

        /// <summary>
        /// Indicates that the operation or transaction has not been committed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotCommitted = 0xc000002d,

        /// <summary>
        /// Indicates that an invalid combination of parameters was provided.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameterMix = 0xc0000030,

        /// <summary>
        /// Indicates that the specified object name is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectNameInvalid = 0xc0000033,

        /// <summary>
        /// Indicates that the specified object name was not found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectNameNotFound = 0xc0000034,

        /// <summary>
        /// Indicates that a name collision occurred when attempting to create or open an object, such as a file or directory,
        /// because an object with the specified name already exists.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that an operation failed due to the presence of an existing object with the same
        /// name. It may be returned by system APIs or error handling routines when a unique name is required for the operation to succeed.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectNameCollision = 0xc0000035,

        /// <summary>
        /// Indicates that the specified object path is invalid.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when a requested file, directory, or object path does not
        /// conform to expected format or does not exist. It may be returned by APIs that validate or access file system or resource paths.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectPathInvalid = 0xc0000039,

        /// <summary>
        /// Indicates that the specified object path was not found.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when a requested file, directory, or resource path does not
        /// exist. It may be returned by APIs that perform file system or resource lookups.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectPathNotFound = 0xc000003a,

        /// <summary>
        /// Indicates that the object path syntax is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectPathSyntaxBad = 0xc000003b,

        /// <summary>
        /// Indicates that a data overrun error has occurred, typically when incoming data exceeds the buffer capacity.
        /// </summary>
        /// <remarks>
        /// This value is commonly used to signal that received data could not be processed because it was larger than the allocated
        /// buffer. Handling this error may require increasing buffer size or managing data flow to prevent loss.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DataOverrun = 0xc000003c,

        /// <summary>
        /// Indicates that data was received later than expected.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DataLate = 0xc000003d,

        /// <summary>
        /// Indicates that a data error has occurred.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DataError = 0xc000003e,

        /// <summary>
        /// Indicates that a cyclic redundancy check (CRC) error has occurred during an operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal data corruption or transmission errors detected by CRC validation. It may be
        /// returned by methods or APIs that perform integrity checks on data streams or files.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CrcError = 0xc000003f,

        /// <summary>
        /// Indicates that a section exceeds the allowed size limit.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error when processing data structures or files that contain sections larger
        /// than permitted by the format specification. The exact size limit may vary depending on the context in which this value
        /// is used.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SectionTooBig = 0xc0000040,

        /// <summary>
        /// Indicates that a connection attempt to the specified port was refused by the remote host.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PortConnectionRefused = 0xc0000041,

        /// <summary>
        /// Indicates that the specified port handle is invalid.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system or API calls when an operation fails due to an invalid or unrecognized port
        /// handle. Check that the handle being used is valid and has been properly initialized before invoking operations that
        /// require it.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidPortHandle = 0xc0000042,

        /// <summary>
        /// Indicates that a sharing violation has occurred, typically when a file or resource is being accessed by another process
        /// and cannot be opened or modified.
        /// </summary>
        /// <remarks>
        /// This value is commonly returned by file system operations when an attempt is made to access a file that is already in
        /// use by another process. To resolve a sharing violation, ensure that no other process is locking the resource before
        /// retrying the operation.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SharingViolation = 0xc0000043,

        /// <summary>
        /// Indicates that a quota limit has been exceeded.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        QuotaExceeded = 0xc0000044,

        /// <summary>
        /// Indicates that a page protection error has occurred due to invalid memory access or protection settings.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidPageProtection = 0xc0000045,

        /// <summary>
        /// Indicates that an attempt to access a mutant object failed because the caller does not own the mutant.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MutantNotOwned = 0xc0000046,

        /// <summary>
        /// Indicates that a semaphore limit has been exceeded.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that an operation could not be completed because the maximum number of allowed
        /// concurrent accesses has been reached. It may be returned by APIs that enforce semaphore limits to prevent resource overuse.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SemaphoreLimitExceeded = 0xc0000047,

        /// <summary>
        /// Indicates that the port has already been set and cannot be modified.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PortAlreadySet = 0xc0000048,

        /// <summary>
        /// Indicates that the section does not contain image data.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SectionNotImage = 0xc0000049,

        /// <summary>
        /// Indicates that the maximum number of allowed thread suspensions has been exceeded.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SuspendCountExceeded = 0xc000004a,

        /// <summary>
        /// Indicates that a thread is terminating as part of process shutdown.
        /// </summary>
        /// <remarks>
        /// This value is typically used in error handling to identify situations where a thread is ending due to the process
        /// exiting. It may be returned by system calls or APIs that interact with thread or process lifecycles.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ThreadIsTerminating = 0xc000004b,

        /// <summary>
        /// Indicates that a process has exceeded its working set limit.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify an error condition where a process's memory usage surpasses the allowed
        /// threshold. It may be returned by system APIs or error reporting mechanisms related to process resource management.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadWorkingSetLimit = 0xc000004c,

        /// <summary>
        /// Indicates that a file mapping operation failed due to incompatible file formats.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IncompatibleFileMap = 0xc000004d,

        /// <summary>
        /// Indicates that the section is protected and access is restricted according to the specified protection level.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SectionProtection = 0xc000004e,

        /// <summary>
        /// Indicates that the requested operation is not supported by the Encrypting File System (EFS) or the system's encryption capabilities.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EasNotSupported = 0xc000004f,

        /// <summary>
        /// Indicates that an extended attribute (EA) is too large to be processed.
        /// </summary>
        /// <remarks>
        /// This value is typically used in error handling to signal that the size of an extended attribute exceeds the allowable
        /// limit. It may be returned by file system operations that manipulate extended attributes.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EaTooLarge = 0xc0000050,

        /// <summary>
        /// Indicates that a requested extended attribute entry does not exist.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NonExistentEaEntry = 0xc0000051,

        /// <summary>
        /// Indicates that no End-User Assignment Sheet (EAS) is on file for the specified entity.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoEasOnFile = 0xc0000052,

        /// <summary>
        /// Indicates that an extended attribute (EA) is corrupt.
        /// </summary>
        /// <remarks>
        /// This error code is typically returned by file system operations when an extended attribute cannot be read or processed
        /// due to corruption. Handling this error may require repairing or removing the affected attribute.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EaCorruptError = 0xc0000053,

        /// <summary>
        /// Indicates that a file lock conflict has occurred, preventing access to the file.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when an operation cannot proceed because the file is locked by another process or user.
        /// Handling this status may require retrying the operation or releasing the conflicting lock before proceeding.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileLockConflict = 0xc0000054,

        /// <summary>
        /// Indicates that a requested lock could not be granted due to a conflicting lock held by another process.
        /// </summary>
        /// <remarks>
        /// This value is typically used in scenarios involving file or resource locking, where exclusive access cannot be obtained
        /// because another process currently holds a conflicting lock. Applications should handle this status by retrying the
        /// operation, notifying the user, or taking other appropriate action.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LockNotGranted = 0xc0000055,

        /// <summary>
        /// Indicates that the object is marked for deletion but the deletion has not yet been completed.
        /// </summary>
        /// <remarks>
        /// This status can be used to identify resources that are pending removal and may not be accessible for normal operations.
        /// The object may still exist until the deletion process is finalized.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DeletePending = 0xc0000056,

        /// <summary>
        /// Indicates that the specified file type is not supported by the operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CtlFileNotSupported = 0xc0000057,

        /// <summary>
        /// Represents an error code indicating that the revision of the requested item is unknown.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UnknownRevision = 0xc0000058,

        /// <summary>
        /// Indicates that a revision mismatch has occurred, typically when the expected version does not match the actual version encountered.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RevisionMismatch = 0xc0000059,

        /// <summary>
        /// Indicates that the operation failed due to an invalid owner.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidOwner = 0xc000005a,

        /// <summary>
        /// Indicates that the specified primary group is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidPrimaryGroup = 0xc000005b,

        /// <summary>
        /// Indicates that no impersonation token is present.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent a security state where a process or thread is not impersonating another user.
        /// It may be returned by security-related APIs to signal the absence of an impersonation context.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoImpersonationToken = 0xc000005c,

        /// <summary>
        /// Indicates that a mandatory setting or feature cannot be disabled.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantDisableMandatory = 0xc000005d,

        /// <summary>
        /// Indicates that no logon servers are currently available to service the logon request.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoLogonServers = 0xc000005e,

        /// <summary>
        /// Indicates that the specified logon session does not exist.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by authentication or security-related APIs when a requested logon session cannot be
        /// found. It may occur if the session has expired or was never established.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchLogonSession = 0xc000005f,

        /// <summary>
        /// Indicates that the requested operation failed because the user does not have the required privilege.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchPrivilege = 0xc0000060,

        /// <summary>
        /// Indicates that the required privilege is not held by the user or process.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by security-related operations when the caller lacks the necessary permissions to
        /// perform the requested action.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PrivilegeNotHeld = 0xc0000061,

        /// <summary>
        /// Indicates that the specified account name is not valid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidAccountName = 0xc0000062,

        /// <summary>
        /// Indicates that the specified user account exists.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        UserExists = 0xc0000063,

        /// <summary>
        /// Indicates that the specified user does not exist in the directory or authentication system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchUser = 0xc0000064,

        /// <summary>
        /// Indicates that the specified group exists.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        GroupExists = 0xc0000065,

        /// <summary>
        /// Indicates that the specified group does not exist.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoSuchGroup = 0xc0000066,

        /// <summary>
        /// Indicates that the user is a member of the group.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MemberInGroup = 0xc0000067,

        /// <summary>
        /// Indicates that the specified member does not belong to the group.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MemberNotInGroup = 0xc0000068,

        /// <summary>
        /// Represents an error code indicating that the last administrator account has been removed or disabled.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LastAdmin = 0xc0000069,

        /// <summary>
        /// Indicates that the specified password is incorrect.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        WrongPassword = 0xc000006a,

        /// <summary>
        /// Indicates that the specified password is not valid according to the system's password requirements.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IllFormedPassword = 0xc000006b,

        /// <summary>
        /// Indicates that the password provided does not meet the required restrictions.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PasswordRestriction = 0xc000006c,

        /// <summary>
        /// Indicates that the logon attempt failed due to an incorrect username or password.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by authentication systems when user credentials do not match any valid account. It can
        /// be used to identify authentication failures and prompt users to re-enter their credentials.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LogonFailure = 0xc000006d,

        /// <summary>
        /// Indicates that the account is restricted and cannot be used for authentication.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by authentication systems when an account has been disabled or restricted due to policy
        /// violations or administrative actions. Applications should handle this status by preventing login or access for the
        /// affected account.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AccountRestriction = 0xc000006e,

        /// <summary>
        /// Indicates that the logon attempt failed because the user's logon hours are restricted.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by authentication systems when a user tries to log on outside of their permitted logon
        /// hours. Applications can use this code to inform users about logon restrictions or to implement custom handling for
        /// restricted access times.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidLogonHours = 0xc000006f,

        /// <summary>
        /// Indicates that the logon attempt failed because the workstation is not authorized to access the requested resource.
        /// </summary>
        /// <remarks>
        /// This value is typically used in authentication scenarios to signal that access is denied due to workstation
        /// restrictions. It may be returned by security-related APIs when a user attempts to log on from a computer that is not
        /// permitted by policy.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidWorkstation = 0xc0000070,

        /// <summary>
        /// Indicates that the user's password has expired and must be changed before authentication can proceed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PasswordExpired = 0xc0000071,

        /// <summary>
        /// Indicates that the user account is disabled.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an authentication or access failure due to the account being disabled.
        /// Applications can use this code to provide specific error handling or messaging when a disabled account is encountered.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AccountDisabled = 0xc0000072,

        /// <summary>
        /// Indicates that no mapping exists for the specified value.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoneMapped = 0xc0000073,

        /// <summary>
        /// Indicates that too many locally unique identifiers (LUIDs) were requested.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TooManyLuidsRequested = 0xc0000074,

        /// <summary>
        /// Indicates that no more locally unique identifiers (LUIDs) are available.
        /// </summary>
        /// <remarks>
        /// This error code is typically returned by system functions that allocate LUIDs when the pool of available identifiers has
        /// been exhausted. LUIDs are used to uniquely identify resources or sessions within the system.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LuidsExhausted = 0xc0000075,

        /// <summary>
        /// Indicates that the specified sub-authority value is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidSubAuthority = 0xc0000076,

        /// <summary>
        /// Indicates that an invalid access control list (ACL) was encountered.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidAcl = 0xc0000077,

        /// <summary>
        /// Indicates that the security identifier (SID) provided is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidSid = 0xc0000078,

        /// <summary>
        /// Indicates that a security descriptor is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidSecurityDescr = 0xc0000079,

        /// <summary>
        /// Indicates that the specified procedure could not be found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProcedureNotFound = 0xc000007a,

        /// <summary>
        /// Indicates that the image file format is invalid or not supported.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidImageFormat = 0xc000007b,

        /// <summary>
        /// Indicates that no token is available or present.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoToken = 0xc000007c,

        /// <summary>
        /// Indicates that an access control list (ACL) contains an invalid inheritance configuration.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when processing security descriptors or ACLs. It may be
        /// returned by APIs that validate or interpret ACL structures.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadInheritanceAcl = 0xc000007d,

        /// <summary>
        /// Indicates that the requested range is not locked and can be accessed or modified.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RangeNotLocked = 0xc000007e,

        /// <summary>
        /// Indicates that an operation failed because the disk is full.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DiskFull = 0xc000007f,

        /// <summary>
        /// Indicates that the server is disabled and cannot process requests.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ServerDisabled = 0xc0000080,

        /// <summary>
        /// Indicates that the server is not disabled.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ServerNotDisabled = 0xc0000081,

        /// <summary>
        /// Indicates that the requested number of GUIDs exceeds the allowed limit.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TooManyGuidsRequested = 0xc0000082,

        /// <summary>
        /// Indicates that no more globally unique identifiers (GUIDs) are available for allocation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error condition when a system or service has exhausted its pool of available
        /// GUIDs. Applications encountering this status should not attempt further GUID allocations until additional GUIDs become
        /// available or the underlying issue is resolved.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        GuidsExhausted = 0xc0000083,

        /// <summary>
        /// Indicates that the security authority specified for an identifier is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidIdAuthority = 0xc0000084,

        /// <summary>
        /// Indicates that all available agents have been exhausted and no further agents are available to process the request.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        AgentsExhausted = 0xc0000085,

        /// <summary>
        /// Indicates that the specified volume label is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidVolumeLabel = 0xc0000086,

        /// <summary>
        /// Indicates that the section of the request was not extended. This value is typically used to represent the
        /// STATUS_SECTION_NOT_EXTENDED error code.
        /// </summary>
        /// <remarks>
        /// This value corresponds to the Windows error code 0xc0000087, which may be returned by system APIs when a request for a
        /// section extension fails. Use this value to identify and handle this specific error condition in your application.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SectionNotExtended = 0xc0000087,

        /// <summary>
        /// Represents a value that indicates data not mapped to a database column.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotMappedData = 0xc0000088,

        /// <summary>
        /// Indicates that the requested resource data could not be found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ResourceDataNotFound = 0xc0000089,

        /// <summary>
        /// Indicates that the specified resource type was not found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ResourceTypeNotFound = 0xc000008a,

        /// <summary>
        /// Indicates that the specified resource name could not be found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ResourceNameNotFound = 0xc000008b,

        /// <summary>
        /// Indicates that an attempt was made to access an array element outside its valid bounds.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ArrayBoundsExceeded = 0xc000008c,

        /// <summary>
        /// Indicates that a floating-point denormal operand exception has occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where a floating-point operation encounters a denormal
        /// operand, which may affect the precision or performance of calculations. It is commonly used in exception handling or
        /// status reporting for floating-point operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatDenormalOperand = 0xc000008d,

        /// <summary>
        /// Indicates that a floating-point division by zero error has occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition resulting from an attempt to divide a floating-point number
        /// by zero. It may be returned by system APIs or used in exception handling to identify this specific error.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatDivideByZero = 0xc000008e,

        /// <summary>
        /// Indicates that a floating-point operation resulted in an inexact value, such as rounding or loss of precision.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a floating-point calculation did not produce an exact result, which may
        /// occur during arithmetic operations that cannot be represented precisely in binary form.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatInexactResult = 0xc000008f,

        /// <summary>
        /// Indicates that a floating-point invalid operation exception has occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition resulting from an invalid operation in floating-point
        /// arithmetic, such as dividing zero by zero or taking the square root of a negative number.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatInvalidOperation = 0xc0000090,

        /// <summary>
        /// Indicates that a floating-point overflow error has occurred.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatOverflow = 0xc0000091,

        /// <summary>
        /// Represents an error code indicating a floating-point stack check failure.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify exceptions or error conditions related to floating-point stack integrity, such
        /// as stack overflows or underflows during floating-point operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatStackCheck = 0xc0000092,

        /// <summary>
        /// Indicates that a floating-point underflow error has occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where a floating-point operation produces a result too
        /// small to be represented by the destination type. Handling this error may require special consideration depending on the
        /// application's numerical requirements.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FloatUnderflow = 0xc0000093,

        /// <summary>
        /// Represents an error code indicating that an integer division by zero occurred.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify exceptions or error conditions resulting from attempts to divide an integer
        /// value by zero. It may be returned by system calls or APIs that report low-level error codes.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IntegerDivideByZero = 0xc0000094,

        /// <summary>
        /// Indicates that an integer overflow occurred during an operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IntegerOverflow = 0xc0000095,

        /// <summary>
        /// Represents an error code indicating that a privileged instruction was executed.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an access violation or illegal operation when a process attempts to execute a CPU
        /// instruction that requires elevated privileges. It is commonly encountered in low-level system programming or when
        /// handling exceptions related to processor instructions.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PrivilegedInstruction = 0xc0000096,

        /// <summary>
        /// Indicates that the system has encountered too many paging files, preventing further paging file creation or expansion.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition related to virtual memory management. It may be returned by
        /// system APIs or components when the limit for paging files has been reached.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TooManyPagingFiles = 0xc0000097,

        /// <summary>
        /// Indicates that a file is invalid or corrupted.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileInvalid = 0xc0000098,

        /// <summary>
        /// Indicates that the requested instance is not available.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InstanceNotAvailable = 0xc00000ab,

        /// <summary>
        /// Indicates that a named pipe is not available for use.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeNotAvailable = 0xc00000ac,

        /// <summary>
        /// Indicates that an operation failed because the pipe is in an invalid state.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidPipeState = 0xc00000ad,

        /// <summary>
        /// Indicates that the pipe is busy and cannot process the requested operation.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system calls or APIs interacting with named pipes when the pipe is currently in use.
        /// Callers should wait and retry the operation if appropriate.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeBusy = 0xc00000ae,

        /// <summary>
        /// Represents an error code indicating that an illegal function was called.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IllegalFunction = 0xc00000af,

        /// <summary>
        /// Indicates that a named pipe has been disconnected.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeDisconnected = 0xc00000b0,

        /// <summary>
        /// Indicates that the pipe is being closed and no further operations can be performed.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a named pipe or similar resource is no longer available for communication.
        /// Attempting to read from or write to a pipe in this state will result in an error.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeClosing = 0xc00000b1,

        /// <summary>
        /// Indicates that a pipe is connected and ready for communication.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeConnected = 0xc00000b2,

        /// <summary>
        /// Indicates that a named pipe is currently in a listening state and ready to accept incoming connections.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeListening = 0xc00000b3,

        /// <summary>
        /// Indicates that an invalid read mode was encountered.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidReadMode = 0xc00000b4,

        /// <summary>
        /// Indicates that an I/O operation has timed out.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IoTimeout = 0xc00000b5,

        /// <summary>
        /// Indicates that a file was forcibly closed by the system.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where a file handle is closed unexpectedly, such as by an
        /// external process or system action. Applications encountering this status should handle potential data loss or incomplete operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileForcedClosed = 0xc00000b6,

        /// <summary>
        /// Indicates that profiling has not been started.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProfilingNotStarted = 0xc00000b7,

        /// <summary>
        /// Indicates that profiling has not been stopped for the process or application.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent a status or error code in diagnostic or profiling operations. It may be
        /// returned by APIs or components that monitor application performance to signal that profiling is still active.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProfilingNotStopped = 0xc00000b8,

        /// <summary>
        /// Indicates that the operation failed because the source and target are not on the same device.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when attempting actions such as moving or renaming files across different devices or
        /// volumes, which is not supported by the underlying system.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NotSameDevice = 0xc00000d4,

        /// <summary>
        /// Indicates that a file has been renamed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileRenamed = 0xc00000d5,

        /// <summary>
        /// Indicates that the operation cannot wait and must proceed immediately.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantWait = 0xc00000d8,

        /// <summary>
        /// Indicates that a pipe operation failed because the pipe is empty.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PipeEmpty = 0xc00000d9,

        /// <summary>
        /// Indicates that a process cannot terminate itself. Typically used to represent an error code returned when a process
        /// attempts to terminate its own execution, which is not permitted.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantTerminateSelf = 0xc00000db,

        /// <summary>
        /// Indicates that an internal error has occurred within the system or application.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent unexpected failures that do not fall under more specific error categories. It
        /// may be returned when the underlying cause of the error is unknown or cannot be determined.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InternalError = 0xc00000e5,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter1 = 0xc00000ef,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter2 = 0xc00000f0,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter3 = 0xc00000f1,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter4 = 0xc00000f2,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter5 = 0xc00000f3,

        /// <summary>
        /// Indicates that an operation failed due to an invalid parameter being provided.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter6 = 0xc00000f4,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter7 = 0xc00000f5,

        /// <summary>
        /// Indicates that an operation failed due to an invalid parameter being provided.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter8 = 0xc00000f6,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter9 = 0xc00000f7,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter10 = 0xc00000f8,

        /// <summary>
        /// Indicates that a parameter provided to a method or operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter11 = 0xc00000f9,

        /// <summary>
        /// Indicates that a parameter provided to an operation is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidParameter12 = 0xc00000fa,

        /// <summary>
        /// Indicates that the size of the mapped file is zero.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when attempting to map a file with no data. Ensure that the
        /// file being mapped contains data before using operations that require a non-zero file size.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MappedFileSizeZero = 0xc000011e,

        /// <summary>
        /// Indicates that the operation failed because too many files are open.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by file system operations when the process or system has reached its limit for
        /// simultaneously opened files. To resolve this error, close unused files before attempting to open new ones.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TooManyOpenedFiles = 0xc000011f,

        /// <summary>
        /// Indicates that the operation was cancelled before completion.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        Cancelled = 0xc0000120,

        /// <summary>
        /// Indicates that the operation cannot be completed because the target object cannot be deleted.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CannotDelete = 0xc0000121,

        /// <summary>
        /// Indicates that the specified computer name is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidComputerName = 0xc0000122,

        /// <summary>
        /// Indicates that a file has been deleted.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileDeleted = 0xc0000123,

        /// <summary>
        /// Represents a special account type used for system-level operations or reserved purposes.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SpecialAccount = 0xc0000124,

        /// <summary>
        /// Represents a special group identifier used to distinguish system-defined or reserved groups.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify groups that have special significance within the system or application. It may
        /// be reserved for internal use or for groups that require unique handling compared to regular user-defined groups.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SpecialGroup = 0xc0000125,

        /// <summary>
        /// Represents a user with special privileges or status within the system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SpecialUser = 0xc0000126,

        /// <summary>
        /// Represents the identifier for the primary group of members.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MembersPrimaryGroup = 0xc0000127,

        /// <summary>
        /// Indicates that the operation failed because the file is closed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileClosed = 0xc0000128,

        /// <summary>
        /// Indicates that a thread creation request failed because the system has reached the maximum number of allowable threads.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system APIs when no additional threads can be created due to resource limits.
        /// Applications should handle this condition by reducing thread usage or retrying the operation after some time.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TooManyThreads = 0xc0000129,

        /// <summary>
        /// Indicates that the thread is not in the process.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ThreadNotInProcess = 0xc000012a,

        /// <summary>
        /// Indicates that the specified token is already in use.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TokenAlreadyInUse = 0xc000012b,

        /// <summary>
        /// Indicates that the system's pagefile quota has been exceeded.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system operations when there is insufficient pagefile space to complete the
        /// requested action. Applications encountering this status should consider freeing up resources or increasing the pagefile size.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PagefileQuotaExceeded = 0xc000012c,

        /// <summary>
        /// Indicates that a system resource allocation request failed due to exceeding a commitment limit.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where the requested operation cannot be completed because
        /// the system has reached its maximum allowed commitment of resources, such as memory or handles. Handling this value may
        /// require freeing resources or adjusting system limits before retrying the operation.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CommitmentLimit = 0xc000012d,

        /// <summary>
        /// Indicates that the image file is in an invalid LE (Little Endian) format.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidImageLeFormat = 0xc000012e,

        /// <summary>
        /// Indicates that the image file is invalid and not a valid MZ executable format.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidImageNotMz = 0xc000012f,

        /// <summary>
        /// Indicates that image protection is invalid, typically representing an error condition related to image security or integrity.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidImageProtect = 0xc0000130,

        /// <summary>
        /// Indicates that an invalid image format was encountered for a Win16 application.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidImageWin16 = 0xc0000131,

        /// <summary>
        /// Indicates that the logon server is unavailable or cannot be contacted.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when authentication or logon operations fail due to the
        /// inability to reach the domain controller or logon server. It may be returned by authentication APIs or system calls that
        /// require access to a logon server.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LogonServer = 0xc0000132,

        /// <summary>
        /// Indicates that there is a time difference between the client and domain controller, which may affect authentication or
        /// access operations.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system operations that rely on time synchronization with a domain controller.
        /// Applications encountering this value should ensure that the system clock is synchronized with the domain controller to
        /// avoid authentication failures.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DifferenceAtDc = 0xc0000133,

        /// <summary>
        /// Indicates that synchronization with another resource is required before the operation can proceed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        SynchronizationRequired = 0xc0000134,

        /// <summary>
        /// Indicates that a dynamic-link library (DLL) required for execution could not be found.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when a process or application attempts to load a DLL that
        /// is missing or unavailable. It may be returned by system calls or APIs that depend on external libraries.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DllNotFound = 0xc0000135,

        /// <summary>
        /// Indicates that an I/O privilege operation has failed due to insufficient privileges.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IoPrivilegeFailed = 0xc0000137,

        /// <summary>
        /// Indicates that the specified ordinal was not found.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when an operation attempts to locate an ordinal value that
        /// does not exist. The numeric value corresponds to the system error code 0xC0000138.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        OrdinalNotFound = 0xc0000138,

        /// <summary>
        /// Indicates that a specified entry point could not be found in a dynamic-link library (DLL).
        /// </summary>
        /// <remarks>
        /// This error code is typically returned when attempting to load a function from a DLL and the function name does not exist
        /// in the library. It is commonly encountered in scenarios involving platform invocation (P/Invoke) or dynamic loading of
        /// native code.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EntryPointNotFound = 0xc0000139,

        /// <summary>
        /// Represents the exit code indicating that a process was terminated by a Control+C (Ctrl+C) signal.
        /// </summary>
        /// <remarks>
        /// This exit code is typically returned when a process is interrupted by the user pressing Ctrl+C in the console. It can be
        /// used to detect user-initiated termination and handle cleanup or logging accordingly.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ControlCExit = 0xc000013a,

        /// <summary>
        /// Indicates that the port has not been set.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PortNotSet = 0xc0000353,

        /// <summary>
        /// Indicates that the debugger is inactive or not currently attached.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DebuggerInactive = 0xc0000354,

        /// <summary>
        /// Indicates that a callback bypass condition has occurred.
        /// </summary>
        /// <remarks>
        /// This value may be used to identify specific error or status codes related to callback bypass scenarios. The exact
        /// meaning and usage depend on the context in which it is applied.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CallbackBypass = 0xc0000503,

        /// <summary>
        /// Indicates that the operation failed because the port is closed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        PortClosed = 0xc0000700,

        /// <summary>
        /// Indicates that a message was lost during transmission or processing.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MessageLost = 0xc0000701,

        /// <summary>
        /// Represents an error code indicating that a message is invalid or cannot be processed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidMessage = 0xc0000702,

        /// <summary>
        /// Indicates that the request was canceled before completion.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RequestCanceled = 0xc0000703,

        /// <summary>
        /// Indicates an error caused by a recursive dispatch operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RecursiveDispatch = 0xc0000704,

        /// <summary>
        /// Indicates that the expected receive buffer for a Local Procedure Call (LPC) was not provided.
        /// </summary>
        /// <remarks>
        /// This value is typically used in error handling scenarios to identify issues related to LPC communication where the
        /// receive buffer is missing or not as expected. It may be returned by system APIs or components that rely on LPC for
        /// inter-process communication.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LpcReceiveBufferExpected = 0xc0000705,

        /// <summary>
        /// Indicates that an invalid connection usage occurred during a Local Procedure Call (LPC) operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition where a connection was used in an unsupported or incorrect
        /// manner in LPC-based communication. It may be returned by system APIs or components that interact with LPC mechanisms.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LpcInvalidConnectionUsage = 0xc0000706,

        /// <summary>
        /// Indicates that local procedure call (LPC) requests are not allowed. This value is typically used to represent an error
        /// condition where an LPC operation is denied by the system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LpcRequestsNotAllowed = 0xc0000707,

        /// <summary>
        /// Indicates that the requested resource is currently in use and cannot be accessed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ResourceInUse = 0xc0000708,

        /// <summary>
        /// Indicates that the process is protected and cannot be accessed or modified by standard operations.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system calls or APIs when an attempt is made to interact with a protected process.
        /// Access to protected processes may require elevated privileges or may be restricted entirely depending on system security policies.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ProcessIsProtected = 0xc0000712,

        /// <summary>
        /// Indicates that the volume is marked as dirty, typically due to file system corruption or pending changes that require verification.
        /// </summary>
        /// <remarks>
        /// This value is commonly used in error handling to signal that a storage volume may need to be checked or repaired before
        /// further operations can proceed.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        VolumeDirty = 0xc0000806,

        /// <summary>
        /// Indicates that the file is currently checked out and may be locked for editing or exclusive access.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent a file state in systems that support check-in and check-out functionality,
        /// such as version control or document management systems. When a file is checked out, other users may be prevented from
        /// making changes until it is checked in.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileCheckedOut = 0xc0000901,

        /// <summary>
        /// Indicates that a check-out operation is required before the item can be modified.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CheckOutRequired = 0xc0000902,

        /// <summary>
        /// Indicates that the file type is invalid or not supported.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        BadFileType = 0xc0000903,

        /// <summary>
        /// Indicates that the file exceeds the maximum allowed size for the operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileTooLarge = 0xc0000904,

        /// <summary>
        /// Indicates that forms-based authentication is required for access.
        /// </summary>
        /// <remarks>
        /// Use this value to specify that a resource or operation must be accessed by a user authenticated through forms
        /// authentication. This is typically used in scenarios where credentials are collected via a web form rather than
        /// integrated authentication mechanisms.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FormsAuthRequired = 0xc0000905,

        /// <summary>
        /// Indicates that the file or resource is infected with a virus.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by system operations that detect virus infections in files or resources. Applications
        /// should handle this status by preventing access to the infected item and notifying the user or system administrator as appropriate.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        VirusInfected = 0xc0000906,

        /// <summary>
        /// Indicates that a virus has been deleted as a result of an operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        VirusDeleted = 0xc0000907,

        /// <summary>
        /// Indicates that a transactional conflict has occurred, typically as a result of concurrent operations that cannot be
        /// resolved automatically.
        /// </summary>
        /// <remarks>
        /// This value is commonly used to signal errors in systems that support transactions, such as databases or distributed
        /// services. Handling this status may require the caller to retry the operation or resolve the conflict manually.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionalConflict = 0xc0190001,

        /// <summary>
        /// Represents an error code indicating that the transaction is invalid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        InvalidTransaction = 0xc0190002,

        /// <summary>
        /// Indicates that the transaction is not active.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionNotActive = 0xc0190003,

        /// <summary>
        /// Indicates that the transaction manager failed to initialize.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TmInitializationFailed = 0xc0190004,

        /// <summary>
        /// Indicates that the resource manager is not active.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RmNotActive = 0xc0190005,

        /// <summary>
        /// Indicates that the resource manager metadata is corrupt.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when interacting with resource managers or transaction
        /// systems. It may be returned by APIs that detect corruption in resource manager metadata, signaling that recovery or
        /// repair actions may be necessary.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RmMetadataCorrupt = 0xc0190006,

        /// <summary>
        /// Indicates that an operation was attempted without joining a transaction.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a required transaction context is missing when performing transactional
        /// operations. Ensure that a transaction is properly joined before invoking methods that require transactional participation.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionNotJoined = 0xc0190007,

        /// <summary>
        /// Indicates that a directory could not be removed because it is not empty.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when attempting to delete a directory that contains files
        /// or subdirectories. It may be returned by file system operations that enforce directory emptiness before removal.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        DirectoryNotRm = 0xc0190008,

        /// <summary>
        /// Indicates that the operation failed because the system could not resize the log file.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CouldNotResizeLog = 0xc0190009,

        /// <summary>
        /// Indicates that remote transactions are not supported by the target system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionsUnsupportedRemote = 0xc019000a,

        /// <summary>
        /// Indicates that a log resize operation failed due to an invalid size parameter.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LogResizeInvalidSize = 0xc019000b,

        /// <summary>
        /// Indicates that the version of the remote file does not match the expected version.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal a version conflict when accessing or synchronizing files across remote systems.
        /// Applications can use this status to prompt users to resolve version discrepancies or to retry operations with the
        /// correct file version.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RemoteFileVersionMismatch = 0xc019000c,

        /// <summary>
        /// Indicates that a CRM protocol with the specified identifier already exists.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CrmProtocolAlreadyExists = 0xc019000f,

        /// <summary>
        /// Indicates that a transaction propagation has failed.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error condition when a transaction cannot be propagated as expected. It may be
        /// returned by transaction management APIs or logged for diagnostic purposes.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionPropagationFailed = 0xc0190010,

        /// <summary>
        /// Indicates that the specified protocol was not found in the Certificate Request Management (CRM) subsystem.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CrmProtocolNotFound = 0xc0190011,

        /// <summary>
        /// Indicates that a transaction superior exists for the current operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used in transaction management scenarios to signal that a parent or superior transaction is
        /// present. It may be returned by transaction-related APIs to inform the caller of hierarchical transaction relationships.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionSuperiorExists = 0xc0190012,

        /// <summary>
        /// Indicates that the transaction request is not valid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionRequestNotValid = 0xc0190013,

        /// <summary>
        /// Indicates that a transaction was not requested for the operation.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionNotRequested = 0xc0190014,

        /// <summary>
        /// Indicates that the transaction has already been aborted and cannot be completed.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by transaction management APIs when an operation is attempted on a transaction that is
        /// no longer active due to a prior abort. Applications should check for this status to avoid performing further actions on
        /// aborted transactions.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionAlreadyAborted = 0xc0190015,

        /// <summary>
        /// Indicates that the transaction has already been committed and cannot be modified or committed again.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionAlreadyCommitted = 0xc0190016,

        /// <summary>
        /// Indicates that a transaction failed due to an invalid marshalling buffer.
        /// </summary>
        /// <remarks>
        /// This value is typically used to identify errors related to improper serialization or deserialization of transaction
        /// data. It may be returned by APIs that process transaction buffers when the buffer format does not meet expected requirements.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionInvalidMarshallBuffer = 0xc0190017,

        /// <summary>
        /// Indicates that the current transaction is not valid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CurrentTransactionNotValid = 0xc0190018,

        /// <summary>
        /// Indicates that a log growth operation has failed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LogGrowthFailed = 0xc0190019,

        /// <summary>
        /// Indicates that the referenced object no longer exists.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        ObjectNoLongerExists = 0xc0190021,

        /// <summary>
        /// Indicates that the requested stream miniversion was not found.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        StreamMiniversionNotFound = 0xc0190022,

        /// <summary>
        /// Indicates that the stream miniversion is not valid.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        StreamMiniversionNotValid = 0xc0190023,

        /// <summary>
        /// Indicates that a miniversion is inaccessible from the specified transaction.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error condition when attempting to access a miniversion that is not available
        /// within the context of the current transaction. It may be returned by file system or transactional APIs that support miniversioning.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,

        /// <summary>
        /// Indicates that a miniversion cannot be opened with modify intent due to access restrictions or file system limitations.
        /// </summary>
        /// <remarks>
        /// This error code is typically returned by file system operations when attempting to open a miniversion for modification,
        /// but the operation is not permitted. Miniversions are read-only snapshots and cannot be modified directly.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantOpenMiniversionWithModifyIntent = 0xc0190025,

        /// <summary>
        /// Indicates that no additional stream miniversions can be created because the limit has been reached.
        /// </summary>
        /// <remarks>
        /// This value is typically returned by file system operations that attempt to create a new stream miniversion when the
        /// maximum number allowed has already been created. Stream miniversions are used to maintain multiple versions of a stream
        /// within a file, commonly in transactional file systems.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantCreateMoreStreamMiniversions = 0xc0190026,

        /// <summary>
        /// Indicates that the handle is no longer valid.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that an operation failed because a previously obtained handle has become invalid,
        /// such as after the resource it refers to has been closed or released. Callers should ensure that handles are still valid
        /// before using them to avoid this error.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        HandleNoLongerValid = 0xc0190028,

        /// <summary>
        /// Indicates that the file or volume does not have transaction metadata associated with it.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        NoTxfMetadata = 0xc0190029,

        /// <summary>
        /// Indicates that corruption has been detected in the log file.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal a log file integrity issue, which may require recovery or repair operations.
        /// Applications encountering this value should avoid further operations on the affected log until the corruption is resolved.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        LogCorruptionDetected = 0xc0190030,

        /// <summary>
        /// Indicates that recovery is not possible because a handle to the resource is open.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error condition when an operation cannot proceed due to an open handle. Ensure
        /// that all handles to the resource are closed before attempting recovery.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantRecoverWithHandleOpen = 0xc0190031,

        /// <summary>
        /// Indicates that the resource manager has been disconnected from the transaction manager.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RmDisconnected = 0xc0190032,

        /// <summary>
        /// Indicates that the enlistment operation failed because the specified enlistment is not a superior transaction.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EnlistmentNotSuperior = 0xc0190033,

        /// <summary>
        /// Indicates that recovery is not required for the operation or resource.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RecoveryNotNeeded = 0xc0190034,

        /// <summary>
        /// Indicates that the resource manager has already been started.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RmAlreadyStarted = 0xc0190035,

        /// <summary>
        /// Indicates that the file identity is not persistent and may change over time.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a file's unique identifier cannot be relied upon for long-term tracking or
        /// referencing. Applications should avoid using such identities for persistent storage or cross-session operations.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        FileIdentityNotPersistent = 0xc0190036,

        /// <summary>
        /// Indicates that the operation cannot proceed because it would break a transactional dependency.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantBreakTransactionalDependency = 0xc0190037,

        /// <summary>
        /// Indicates that the operation cannot cross a resource manager boundary.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CantCrossRmBoundary = 0xc0190038,

        /// <summary>
        /// Indicates that the directory targeted by the transaction is not empty.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TxfDirNotEmpty = 0xc0190039,

        /// <summary>
        /// Indicates that one or more indoubt transactions exist on the resource manager.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that the resource manager has transactions whose outcome is uncertain, often due
        /// to a failure during the commit or rollback process. Applications may need to resolve these transactions manually or wait
        /// for automatic resolution, depending on the resource manager's capabilities.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        IndoubtTransactionsExist = 0xc019003a,

        /// <summary>
        /// Represents a transaction manager object that is volatile and does not persist state across system restarts.
        /// </summary>
        /// <remarks>
        /// Use this value when specifying a transaction manager that should not retain information after a reboot. Volatile
        /// transaction managers are typically used for temporary or in-memory transactions where durability is not required.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TmVolatile = 0xc019003b,

        /// <summary>
        /// Indicates that a rollback timer has expired during a transaction or operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a rollback was triggered because the allotted time for the operation was
        /// exceeded. It may be returned by transaction management systems or APIs to indicate a timeout condition requiring rollback.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        RollbackTimerExpired = 0xc019003c,

        /// <summary>
        /// Indicates that a file or directory operation failed because the transaction metadata is corrupt.
        /// </summary>
        /// <remarks>
        /// This error code is returned when a transactional file system operation cannot proceed due to corruption in the
        /// associated transaction metadata. This typically requires manual intervention to repair or restore the affected data.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TxfAttributeCorrupt = 0xc019003d,

        /// <summary>
        /// Indicates that the operation is not allowed because it involves the Encrypting File System (EFS) within a transaction.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when attempting to perform an EFS-related action as part of a transactional operation,
        /// which is not supported.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        EfsNotAllowedInTransaction = 0xc019003e,

        /// <summary>
        /// Indicates that a transactional open operation is not allowed.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionalOpenNotAllowed = 0xc019003f,

        /// <summary>
        /// Indicates that a transacted mapping operation is not supported on a remote file system.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactedMappingUnsupportedRemote = 0xc0190040,

        /// <summary>
        /// Indicates that the specified file already contains Transactional NTFS (TxF) metadata.
        /// </summary>
        /// <remarks>
        /// This value is typically returned when an operation attempts to add TxF metadata to a file that already has it. TxF is
        /// used to provide transactional support for file operations on NTFS volumes.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TxfMetadataAlreadyPresent = 0xc0190041,

        /// <summary>
        /// Indicates that transaction scope callbacks have not been set for the operation.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal an error condition when required transaction scope callbacks are missing. Ensure
        /// that all necessary callbacks are configured before initiating a transaction that depends on them.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionScopeCallbacksNotSet = 0xc0190042,

        /// <summary>
        /// Indicates that the operation requires a transaction promotion to complete successfully.
        /// </summary>
        /// <remarks>
        /// This value is typically used to signal that a distributed transaction must be promoted to a higher level, such as
        /// escalating from a local to a distributed transaction, in order to proceed. It is commonly encountered in transaction
        /// management scenarios where resource managers require promotion for consistency.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionRequiredPromotion = 0xc0190043,

        /// <summary>
        /// Indicates that a file cannot be executed within a transaction.
        /// </summary>
        /// <remarks>
        /// This value is typically used to represent an error condition when attempting to execute a file operation in the context
        /// of a transaction. It may be returned by APIs that enforce transactional integrity and do not support executing files as
        /// part of a transaction.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        CannotExecuteFileInTransaction = 0xc0190044,

        /// <summary>
        /// Indicates that transactions are not frozen and can be processed normally.
        /// </summary>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        TransactionsNotFrozen = 0xc0190045,

        /// <summary>
        /// Represents the maximum value for an NTSTATUS code.
        /// </summary>
        /// <remarks>
        /// This value is typically used to indicate an undefined or invalid NTSTATUS. It may be used as a sentinel value in error
        /// handling scenarios.
        /// </remarks>
        [Description("")]
        [Display(Name = "", ShortName = "")]
        MaximumNtStatus = 0xffffffff,
    }
}
