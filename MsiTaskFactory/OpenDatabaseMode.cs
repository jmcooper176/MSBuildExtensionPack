namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    public enum OpenDatabaseMode : int
    {
        ReadOnly = 0,

        Transact = 1,

        Direct = 2,

        Create = 3,

        CreateDirect = 4,

        ListScript = 5,

        PatchFile = 32,
    }
}
