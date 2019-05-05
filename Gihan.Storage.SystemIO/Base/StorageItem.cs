using System;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;

using static System.IO.Path;
using SysIO = System.IO;

namespace Gihan.Storage.SystemIO.Base
{
    public abstract class StorageItem : IStorageItem
    {
        private IFolder _parent;
        protected SysIO.FileSystemInfo BaseStorageItem { get; set; }

        /// <summary>
        /// The full path of the item, if the item has a path.
        /// </summary>
        public string Path
        {
            get
            {
                var path = BaseStorageItem.FullName.
                    TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);
                if (!path.Contains(DirectorySeparatorChar.ToString()))
                {
                    path += DirectorySeparatorChar;
                }
                return path;
            }
        }

        /// <summary>
        /// The parent folder of the current storage item.
        /// </summary>
        public IFolder Parent => _parent ?? ResetParent();

        /// <summary>
        /// The name of the item including the file name extension if there is one.
        /// </summary>
        public string Name => BaseStorageItem.Name;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public abstract StorageItemType Type { get; }

        /// <summary>
        /// Specifies whether the Item Exist or not
        /// </summary>
        public bool Exist => BaseStorageItem.Exists;

        protected StorageItem(SysIO.FileSystemInfo item)
        {
            BaseStorageItem = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        public void Delete()
        {
            BaseStorageItem.Delete();
        }

        /// <summary>
        /// Renames the current item. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <param name="option">
        /// The <see cref="Enum"/> value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public abstract void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

        public override string ToString()
        {
            return Path;
        }

        public IStorageItem Copy(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists){
            switch (this)
            {
                case File file:
                    return file.Copy(destinationFullPath, option);
                case Folder folder:
                    return folder.Copy(destinationFullPath, option);
                default:
                    throw new Exception();
            }
        }
        public IStorageItem Copy(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(Combine(destinationFolderPath, desiredNewName), option);
        public IStorageItem Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(destinationFolder.Path, desiredNewName, option);
        public IStorageItem Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(destinationFolder, Name, option);

        public abstract void Move(string destinationFullPath, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        public abstract void Move(string destinationFolderPath, string desiredNewName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        public abstract void Move(IFolder destinationFolder, string desiredNewName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        public abstract void Move(IFolder destinationFolder, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);

        #region InnerMethods
        protected IFolder ResetParent()
        {
            return _parent = new Folder(GetDirectoryName(Path));
        }
        #endregion
    }
}
