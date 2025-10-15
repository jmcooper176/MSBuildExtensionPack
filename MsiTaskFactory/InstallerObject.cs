namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class InstallerObject : IMsiCom
    {
        #region Public Fields

        private bool disposedValue;
        public Guid IID => new(" 000C1090-0000-0000-C000-000000000046");
        public string ProgId => "WindowsInstaller.Installer";

        #endregion Public Fields

        #region Public Constructors

        public InstallerObject()
        {
            ComType = ComUtility.GetTypeFromProgId(ProgId);
            Instance = ComUtility.CreateComInstance(ProgId);
        }

        #endregion Public Constructors

        #region Public Properties

        public Type? ComType { get; private set; }
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }

        #endregion Public Properties

        #region Private Destructors

        ~InstallerObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        #endregion Private Destructors

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LastError?.Dispose();
                }

                ComUtility.Release(Instance);
                LastError?.Dispose();
                Instance = null;
                ComType = null;
                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Public Methods

        public RecordObject? CreateRecord(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, RecordObject.MAX_COUNT, nameof(count));

            return ComType is not null && Instance is not null ? (RecordObject?)ComUtility.InvokeComMethod(ComType, "CreateRecord", Instance, [count], null) : null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public RecordObject? LastErrorRecord()
        {
            return ComType is not null && Instance is not null ? (RecordObject?)ComUtility.InvokeComMethod(ComType, "LastErrorRecord", Instance, null, null) : null;
        }

        public DatabaseObject? OpenDatabase(string? name, OpenDatabaseMode mode)
        {
            try
            {
                return ComType is not null && Instance is not null ? (DatabaseObject?)ComUtility.InvokeComMethod(ComType, "OpenDatabase", Instance, [name, (int)mode], null) : null;
            }
            finally
            {
                LastError = LastErrorRecord();
            }
        }

        public DatabaseObject? OpenDatabase(string? name, string? newName)
        {
            try
            {
                return ComType is not null && Instance is not null ? (DatabaseObject?)ComUtility.InvokeComMethod(ComType, "OpenDatabase", Instance, [name, newName], null) : null;
            }
            finally
            {
                LastError = LastErrorRecord();
            }
        }

        public SessionObject? OpenPackage(string packagePath, int options)
        {
            return ComType is not null && Instance is not null ? (SessionObject?)ComUtility.InvokeComMethod(ComType, "OpenPackage", Instance, [packagePath, options], null) : null;
        }

        public SessionObject? OpenProduct(Guid productCode)
        {
            return ComType is not null && Instance is not null ? (SessionObject?)ComUtility.InvokeComMethod(ComType, "OpenProduct", Instance, [productCode.ToString()], null) : null;
        }

        #endregion Public Methods

        public static object? ToInstance(InstallerObject? thick) => thick?.Instance;

        public static InstallerObject? ToObject(object? thin) => new(thin);
    }
}
