using System;

namespace Gihan.Storage.Core.Enums
{
    /// <summary>
    /// Describes whether an item that implements the IStorageItem interface is a file or a folder.
    /// </summary>
    [Flags]
    public enum StorageItemType
    {
        /// <summary>
        /// A storage item that is neither a file nor a folder!!!
        /// </summary>
        None = 0,
        File = 1,
        Folder = 2,
    }
}
