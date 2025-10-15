namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    [Flags]
    public enum InstallContext : int
    {
        None = 0,

        UserManaged = 1,

        User = 2,

        Machine = 4,
    }
}
