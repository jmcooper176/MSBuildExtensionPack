namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class StringList : IMsiCom, ICollection<string>
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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Public Properties

        public Type? ComType { get; }
        public int Count { get; }
        public Guid IID { get; }
        public object? Instance { get; }
        public bool IsReadOnly { get; }
        public string ProgId { get; }

        #endregion Public Properties

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources ~StringList() { // Do
        // not change this code. Put cleanup code in 'Dispose(bool disposing)' method Dispose(disposing: false); }

        #region Public Methods

        public void Add(string item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(string item) => throw new NotImplementedException();

        public void CopyTo(string[] array, int arrayIndex) => throw new NotImplementedException();

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<string> GetEnumerator() => throw new NotImplementedException();

        public bool Remove(string item) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion Public Methods
    }
}
