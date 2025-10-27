namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class SummaryInfoObject : IMsiCom
    {
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        public Type? ComType { get; private set; }
        public Guid IID => new("000C109B-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public string ProgId => string.Empty;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~SummaryInfoObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
