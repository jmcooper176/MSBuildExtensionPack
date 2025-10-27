namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;
    using System.Security.Principal;

    using MSBuild.ExtensionPack.COMTaskFactory;

    public class ProductObject : IMsiCom
    {
        private bool disposedValue;

        ~ProductObject()
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

        public ProductObject(Guid productCode, SecurityIdentifier? userSid, InstallContext installContext)
        {
            ComType = ComUtility.GetTypeFromProgId(ProgId);
            ProductCode = productCode;
            UserSid = userSid;
            Context = installContext;
            Instance = (this.ComType?.CreateComInstance(ProductCode, userSid?.ToString(), (int)installContext));
        }

        public Type? ComType { get; private set; }
        public InstallContext Context { get; }
        public Guid IID => new("000C10A0-0000-0000-C000-000000000046");
        public object? Instance { get; private set; }
        public Guid ProductCode { get; }

        public string ProgId => "WindowsInstaller.Installer.Product";
        public SecurityIdentifier? UserSid { get; }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
