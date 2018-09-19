using Gihan.Storage.Core.Enums;

namespace Gihan.Storage.Core.Base
{
    public interface IStorageItem
    {
        /// <summary>
        /// The full path of the item, if the item has a path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The name of the item including the file name extension if there is one.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The parent folder of the current storage item.
        /// </summary>
        IFolder Parent { get; }

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        StorageItemType Type { get; }

        /// <summary>
        /// Specifies whether the Item Exist or not
        /// </summary>
        bool Exist { get; }

        /// <summary>
        /// Check is a storage item exist in gived path
        /// </summary>
        /// <param name="path">path to check for storage item</param>
        /// <returns>
        /// true if a storage item exist.
        /// fale if it's not exist
        /// </returns>
        bool CheckExist(string path);

        /// <summary>
        /// Renames the current item. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <param name="option">
        /// The enum value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        void Rename(string desiredName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        
        /// <summary>
        /// Deletes the current item.
        /// </summary>
        void Delete();
    }
}
