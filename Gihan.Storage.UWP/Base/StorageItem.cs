using System;
using Windows.Storage;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Enums;

using SysPath = System.IO.Path;
using WinIO = Windows.Storage;
using NameCollisionOption = Gihan.Storage.Core.Enums.NameCollisionOption;

namespace Gihan.Storage.UWP.Base
{
    public abstract class StorageItem : Core.Base.IStorageItem
    {
        private IFolder _parent;

        protected IStorageItem2 BaseStorageItem { get; }

        public string Path
        {
            get
            {
                var path = BaseStorageItem.Path.
                    TrimEnd(SysPath.AltDirectorySeparatorChar, SysPath.DirectorySeparatorChar);
                if (!path.Contains(SysPath.DirectorySeparatorChar.ToString()))
                {
                    path += SysPath.DirectorySeparatorChar.ToString();
                }
                return path;
            }
        }

        public string Name => BaseStorageItem.Name;

        public IFolder Parent => _parent ?? SetParent();

        public abstract StorageItemType Type { get; }

        public bool Exist => CheckExist(Path);

        protected StorageItem(IStorageItem2 baseStorageItem)
        {
            BaseStorageItem = baseStorageItem;
        }

        public void Delete()
        {
            BaseStorageItem.DeleteAsync(StorageDeleteOption.PermanentDelete).GetResults();
        }

        public bool CheckExist(string path)
        {
            //todo I don't know wheather it works or not.
            try
            {
                StorageFolder.GetFolderFromPathAsync(path).GetResults();
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                StorageFile.GetFileFromPathAsync(path).GetResults();
                return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        public void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            BaseStorageItem.RenameAsync(desiredName, (WinIO.NameCollisionOption)option).GetResults();
        }

        protected IFolder SetParent()
        {
            return _parent = new Folder(BaseStorageItem.GetParentAsync().GetResults());
        }
    }
}
