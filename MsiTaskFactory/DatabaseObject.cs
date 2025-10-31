namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using MSBuild.ExtensionPack.COMTaskFactory;

    /// <summary>
    /// Implements a wrapper around the Windows Installer Database object.
    /// </summary>
    /// <seealso cref="MSBuild.ExtensionPack.MsiTaskFactory.IMsiCom"/>
    public class DatabaseObject : IMsiCom
    {
        /// <summary>
        /// If <see langref="true"/>, this instance has called <see cref="Dispose(bool)"/>; otherwise, <see langref="false"/>.
        /// </summary>
        private bool disposedValue;

        ~DatabaseObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Throws if <see cref="Installer"/> is <see longref="null"/> on exit from the constructor.
        /// </exception>
        protected DatabaseObject()
        {
            Installer = new InstallerObject();

            ArgumentNullException.ThrowIfNull(Installer, nameof(Installer));
        }

        protected InstallerObject? Installer { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Installer?.Dispose();
                    LastError?.Dispose();
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <param name="name">Specifies the file path name to the <c>MSI</c>.</param>
        /// <param name="mode">
        /// Specifies the <see cref="OpenDatabaseMode"/> mode to open the <c>MSI</c> with. Defaults to <see
        /// cref="OpenDatabaseMode.ReadOnly"/> which is the most common case.
        /// </param>
        /// <exception cref="FileNotFoundException">Parameter <paramref name="name"/> file name path does not exist.</exception>
        public DatabaseObject(string? name, OpenDatabaseMode mode = OpenDatabaseMode.ReadOnly)
            : this()
        {
            FilePath = string.IsNullOrEmpty(name)
                ? null
                : !File.Exists(name) ? throw new FileNotFoundException($"Parameter {nameof(name)} file path does not exist.", name) : new(name);

            Instance = Installer?.Instance.IsInstanceOfCom(Installer.ComType) == true ? Installer?.OpenDatabase(FilePath?.FullName, mode) : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObject"/> class.
        /// </summary>
        /// <param name="name">   Specifies the file path name to the <c>MSI</c>.</param>
        /// <param name="newName">Specifies the new file path name to persist the <c>MSI</c> to.</param>
        public DatabaseObject(string? name, string newName)
            : this()
        {
            Instance = Installer?.Instance.IsInstanceOfCom(Installer.ComType) == true && ValidInstance.IsValidPath(newName) ? Installer?.OpenDatabase(name, newName) : null;
        }

        public Type? ComType { get; private set; }
        public FileInfo? FilePath { get; }
        public Guid IID => new("000C109D-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }
        public string ProgId => string.Empty;

        public SummaryInfoObject? SummaryInformation
        {
            get
            {
                if (Instance.IsInstanceOfCom(ComType))
                {
                    try
                    {
                        return (SummaryInfoObject?)ComUtility.InvokeComPropertyGet(ComType!, "SummaryInformation", Instance, null);
                    }
                    finally
                    {
                        LastError = Installer?.LastErrorRecord();
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public void ApplyTransform(string storage, ApplyTransformMode errorConditions)
        {
            if (Instance.IsInstanceOfCom(ComType) && ValidInstance.IsValidPath(storage))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "ApplyTransform", Instance, [storage, (int)errorConditions], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public object Clone() => throw new NotImplementedException();

        public void Commit()
        {
            if (Instance.IsInstanceOfCom(ComType))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "Commit", Instance, null, null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Export(string table, string path, string file)
        {
            if (Instance.IsInstanceOfCom(ComType) && ValidInstance.IsValidMsiIdentifier(table) && ValidInstance.IsValidDirectory(path) && ValidInstance.IsValidFileName(file))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "Export", Instance, [table, path, file], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public void GenerateTransform(DatabaseObject reference, string? storage)
        {
            if (Instance.IsInstanceOfCom(ComType) && reference.Instance.IsInstanceOfCom(ComType) && ValidInstance.IsValidPath(storage))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "GenerateTransform", Instance, [reference?.Instance, storage], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public void Import(string path, string file)
        {
            if (Instance.IsInstanceOfCom(ComType) && Directory.Exists(path) && File.Exists(Path.Combine(path, file)))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "Import", Instance, [path, file], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public void Merge(DatabaseObject database, string? errorTable)
        {
            if (Instance.IsInstanceOfCom(ComType) && database.Instance.IsInstanceOfCom(ComType) && ValidInstance.IsValidMsiIdentifier(errorTable))
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType!, "Merge", Instance, [database?.Instance, errorTable], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public ViewObject? OpenView(string sql)
        {
            if (Instance.IsInstanceOfCom(ComType) && !string.IsNullOrWhiteSpace(sql))
            {
                try
                {
                    return (ViewObject?)ComUtility.InvokeComMethod(ComType!, "OpenView", Instance, [sql], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
