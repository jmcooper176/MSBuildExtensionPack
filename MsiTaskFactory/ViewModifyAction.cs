namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    public enum ViewModifyAction : int
    {
        Seek = -1,
        Refresh = 0,
        Insert = 1,
        Update = 2,
        Assign = 3,
        Replace = 4,
        Merge = 5,
        Delete = 6,
        InsertTemporary = 7,
        Validate = 8,
        ValidateNew = 9,
        ValidateField = 10,
        ValidateDelete = 11,
    }
}
