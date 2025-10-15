namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using MSBuild.ExtensionPack.COMTaskFactory;

    public class ComponentInfo : IMsiCom
    {
        #region Private Fields

        private bool disposedValue;

        #endregion Private Fields

        #region Public Properties

        public Type? ComType { get; private set; }
        public Guid IID => new("000C1099-0000-0000-C000-000000000046");

        public object? Instance { get; private set; }

        public string ProgId => string.Empty;

        #endregion Public Properties

        #region Public Constructors

        public ComponentInfo()
        {
        }

        #endregion Public Constructors

        #region Private Destructors

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ComponentInfo()
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

        #region Public Methods

        public object Clone() => throw new NotImplementedException();

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}
