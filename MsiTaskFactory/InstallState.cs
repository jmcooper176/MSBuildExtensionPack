namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    public enum InstallState : int
    {
        NotUsed = -7,
        BadConfig = -6,
        Incomplete = -5,
        SourceAbsent = -4,
        MoreInfo = -3,
        InvalidArg = -2,
        Unknown = -1,
        Broken = 0,
        Advertised = 1,
        Absent = 2,
        Local = 3,
        Source = 4,
        Default = 5,
    }
}
