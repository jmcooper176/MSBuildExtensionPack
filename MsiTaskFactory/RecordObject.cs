namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class RecordObject : IMsiCom
    {
        private bool disposedValue;

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RecordObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        protected InstallerObject? Installer { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Installer?.Dispose();
                }

                ComUtility.Release(Instance);
                Instance = null;
                disposedValue = true;
            }
        }

        public const int MAX_COUNT = 65535;

        public RecordObject(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MAX_COUNT, nameof(count));

            Installer = new InstallerObject();
            Instance = Installer.CreateRecord(count);
        }

        public Type? ComType { get; private set; }
        public Guid IID => new("000C1093-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public RecordObject? LastError { get; private set; }
        public string ProgId => string.Empty;

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void SetStream(int field, string file)
        {
            if (ComType is not null)
            {
                try
                {
                    ComUtility.InvokeComVoidMethod(ComType, "SetStream", Instance, [field, file], null);
                }
                finally
                {
                    LastError = Installer?.LastErrorRecord();
                }
            }
        }
    }
}
