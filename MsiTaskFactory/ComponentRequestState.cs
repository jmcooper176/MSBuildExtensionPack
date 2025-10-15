namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    public enum ComponentRequestState : int
    {
        None = 0,
        InstallStateAbsent = 2,
        InstallStateLocal = 3,
        InstallStateSource = 4,
        InstallStateDefault = 5,
    }
}
