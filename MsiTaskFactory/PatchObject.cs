namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class PatchObject : IMsiCom
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Protected Methods

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

        #endregion Protected Methods

        #region Public Properties

        public Type? ComType { get; }
        public Guid IID { get; }
        public object? Instance { get; private set; }
        public string ProgId { get; }

        #endregion Public Properties

        #region Private Destructors

        ~PatchObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        #endregion Private Destructors

        #region Public Methods

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

        #endregion Public Methods
    }
}
