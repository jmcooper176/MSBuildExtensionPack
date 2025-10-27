namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class PatchObject : IMsiCom
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
                Instance = null;
                disposedValue = true;
            }
        }

        public Type? ComType { get; }
        public Guid IID { get; }
        public object? Instance { get; private set; }
        public string ProgId { get; }

        ~PatchObject()
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

        public override int GetHashCode()
        {
            return HashCode.Combine(ComType, IID, Instance, ProgId);
        }

        public override string ToString()
        {
            return Instance?.ToString() ?? string.Empty;
        }
    }
}
