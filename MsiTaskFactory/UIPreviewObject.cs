namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class UIPreviewObject : IMsiCom
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Private Destructors

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~UIPreviewObject()
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
                    // TODO: dispose managed state (managed objects)
                }

                ComUtility.Release(Instance);
                ComType = null;
                Instance = null;
                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public Type? ComType { get; private set; }
        public Guid IID => new("000C109A-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public string ProgId => string.Empty;

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}
