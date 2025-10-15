namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    public enum FeatureRequestState : int
    {
        InstallStateUnknown = -1,
        InstallStateNone = 0,
        InstallStateAdvertised = 1,
        InstallStateAbsent = 2,
        InstallStateLocal = 3,
        InstallStateSource = 4,
        InstallStateDefault = 5,
    }
}
