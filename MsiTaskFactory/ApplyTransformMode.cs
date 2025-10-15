namespace MSBuild.ExtensionPack.MsiTaskFactory
{
    using System;

    [Flags]
    public enum ApplyTransformMode : int
    {
        None = 0,
        ErrorAddExistingRow = 1,
        ErrorDeleteNonExistingRow = 2,
        ErrorAddExistingTable = 4,
        ErrorDeleteNonExistingTable = 8,
        ErrorUpdateNonExistingRow = 16,
        ErrorChangeCodePage = 32,
        ErrorViewTransform = 256,
    }
}
