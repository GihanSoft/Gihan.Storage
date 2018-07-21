using Gihan.Storage.Core;
using Gihan.Storage.Core.Enums;

using SysPath = System.IO.Path;

namespace Gihan.Storage.SystemIO.Base
{
    public abstract class StorageItem : Core.Base.IStorageItem
    {
        private IFolder _parent;
        protected System.IO.FileSystemInfo BaseStorageItem { get; }

        /// <summary>
        /// The full path of the item, if the item has a path.
        /// </summary>
        public string Path
        {
            get
            {
                var path = BaseStorageItem.FullName.
                    TrimEnd(SysPath.AltDirectorySeparatorChar, SysPath.DirectorySeparatorChar);
                if (!path.Contains(SysPath.DirectorySeparatorChar.ToString()))
                {
                    path += SysPath.DirectorySeparatorChar.ToString();
                }
                return path;
            }
        }

        /// <summary>
        /// The parent folder of the current storage item.
        /// </summary>
        public IFolder Parent => _parent ?? (_parent = new Folder(SysPath.GetDirectoryName(Path)));

        /// <summary>
        /// The name of the item including the file name extension if there is one.
        /// </summary>
        public string Name => SysPath.GetFileName(Path);

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public abstract StorageItemType Type { get; }

        /// <summary>
        /// Specifies whether the Item Exist or not
        /// </summary>
        public bool Exist => BaseStorageItem.Exists;

        protected StorageItem(System.IO.FileSystemInfo item)
        {
            BaseStorageItem = item;
        }

        /// <summary>
        /// Deletes the current item.
        /// </summary>
        public void Delete()
        {
            BaseStorageItem.Delete();
        }

        /// <summary>
        /// Check is a storage item exist in gived path
        /// </summary>
        /// <param name="path">path to check for storage item</param>
        /// <returns>
        /// true if a storage item exist.
        /// fale if it's not exist
        /// </returns>
        public abstract bool CheckExist(string path);

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
        public abstract void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

        //protected string NextName(string currentName)
        //{
        //    currentName = new string(currentName.ToCharArray());
        //    var currentNum = -1;
        //    if (currentName.Last() != ')')
        //        currentNum = 2;

        //    string currentNumStr = null;
        //    var i = 0;
        //    if (currentNum == -1)
        //        for (i = currentName.Length - 2; i > 0 && char.IsDigit(currentName[i]); i--)
        //        {
        //            currentNumStr += currentName[i].ToString();
        //        }

        //    if (currentNum == -1 && currentNumStr != null && currentName[i] == '(')
        //        currentNum = int.Parse(currentNumStr);

        //    if (currentNum == -1) currentNum = 2;


        //    currentName = currentName + $"({currentNum})";

        //    return currentName;
        //}

        public override string ToString()
        {
            return Path;
        }
    }
}
