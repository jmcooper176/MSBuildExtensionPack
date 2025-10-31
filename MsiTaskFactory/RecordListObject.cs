namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class RecordListObject : IMsiCom, ICollection<RecordObject>
    {
        private bool disposedValue;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RecordListObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

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
        public int Count { get; }
        public Guid IID => new("000C1096-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public bool IsReadOnly { get; }
        public string ProgId => string.Empty;

        public void Add(RecordObject item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(RecordObject item) => throw new NotImplementedException();

        public void CopyTo(RecordObject[] array, int arrayIndex) => throw new NotImplementedException();

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<RecordObject> GetEnumerator() => throw new NotImplementedException();

        public bool Remove(RecordObject item) => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
