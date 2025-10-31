namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class SessionObject : IMsiCom
    {
        private bool disposedValue;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~SessionObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        protected SessionObject()
        {
            Installer = new();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Installer?.Dispose();
                    Database?.Dispose();
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        public SessionObject(string packagePath, int options)
                    : this()
        {
            Instance = Installer?.OpenPackage(packagePath, options);
            ComType = Instance?.GetType();
            Database = ComType is not null ? (DatabaseObject?)ComUtility.InvokeComPropertyGet(ComType, "Database", Instance, null) : null;
        }

        public SessionObject(Guid productCode)
            : this()
        {
            Instance = Installer?.OpenProduct(productCode);
            ComType = Instance?.GetType();
            Database = ComType is not null ? (DatabaseObject?)ComUtility.InvokeComPropertyGet(ComType, "Database", Instance, null) : null;
        }

        public Type? ComType { get; private set; }
        public DatabaseObject? Database { get; private set; }
        public Guid IID => new("000C109E-0000-0000-C000-000000000046");
        public InstallerObject? Installer { get; private set; }
        public object? Instance { get; private set; }

        public RecordObject? LastError { get; private set; }
        public string ProgId => string.Empty;

        public string? SourcePath
        {
            get
            {
                if (ComType is not null)
                {
                    try
                    {
                        return (string?)ComUtility.InvokeComPropertyGet(ComType, "SourcePath", Instance, null);
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

        public string? TargetPath
        {
            get
            {
                if (ComType is not null)
                {
                    try
                    {
                        return (string?)ComUtility.InvokeComPropertyGet(ComType, "TargetPath", Instance, null);
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

            set
            {
                if (ComType is not null)
                {
                    try
                    {
                        ComUtility.InvokeComPropertySet(ComType, "TargetPath", Instance, [value], null);
                    }
                    finally
                    {
                        LastError = Installer?.LastErrorRecord();
                    }
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public ComponentRequestState GetComponentCurrentState(string name)
        {
            if (ComType is not null)
            {
                try
                {
                    return (ComponentRequestState?)ComUtility.InvokeComPropertyGet(ComType, "ComponentCurrentState", Instance, [name], null) ?? ComponentRequestState.None;
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return ComponentRequestState.None;
            }
        }

        public int GetFeatureCost(string name)
        {
            if (ComType is not null)
            {
                try
                {
                    return (int?)ComUtility.InvokeComPropertyGet(ComType, "FeatureCost", Instance, [name], null) ?? 0;
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return 0;
            }
        }

        public FeatureRequestState GetFeatureCurrentState(string name)
        {
            if (ComType is not null)
            {
                try
                {
                    return (FeatureRequestState?)ComUtility.InvokeComPropertyGet(ComType, "FeatureCurrentState", Instance, [name], null) ?? FeatureRequestState.InstallStateNone;
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return FeatureRequestState.InstallStateNone;
            }
        }

        public FeatureRequestState GetFeatureRequestState(string name, int installLevel)
        {
            if (ComType is not null)
            {
                try
                {
                    SetInstallLevel(installLevel);
                    return (FeatureRequestState?)ComUtility.InvokeComPropertyGet(ComType, "FeatureRequestState", Instance, [name], null) ?? FeatureRequestState.InstallStateNone;
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return FeatureRequestState.InstallStateNone;
            }
        }

        public int GetFeatureValidStates(string name)
        {
            if (ComType is not null)
            {
                try
                {
                    return (int?)ComUtility.InvokeComPropertyGet(ComType, "FeatureValidStates", Instance, [name], null) ?? 0;
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
            else
            {
                return 0;
            }
        }

        public void SetFeatureRequestState(string name, FeatureRequestState requestState, int installLevel)
        {
            if (ComType is not null)
            {
                switch (requestState)
                {
                    case FeatureRequestState.InstallStateAdvertised:
                    case FeatureRequestState.InstallStateAbsent:
                    case FeatureRequestState.InstallStateLocal:
                    case FeatureRequestState.InstallStateDefault:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(requestState), requestState, $"Parameter {nameof(requestState)} has an unsupported value '{requestState}' for setting Feature '{name}'.");
                }

                try
                {
                    SetInstallLevel(installLevel);
                    ComUtility.InvokeComPropertySet(ComType, "FeatureRequestState", Instance, [name, (int)requestState], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }

        public void SetInstallLevel(int installLevel)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(installLevel, nameof(installLevel));

            if (ComType is not null)
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType, "SetInstallLevel", Instance, [installLevel], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }
    }
}
