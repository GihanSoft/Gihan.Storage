using Gihan.Storage.Core;
using Windows.Storage;
using Gihan.Storage.Core.Enums;

using SysPath = System.IO.Path;
using WinIo = Windows.Storage;
using NameCollisionOption = Gihan.Storage.Core.Enums.NameCollisionOption;

namespace Gihan.Storage.UWP
{
    public class File : Base.StorageItem, IFile
    {
        protected new StorageFile BaseStorageItem => (StorageFile)base.BaseStorageItem;

        public string PureName => SysPath.GetFileNameWithoutExtension(Name);

        public string Extension => BaseStorageItem.FileType; //check

        public override StorageItemType Type => StorageItemType.File;

        public File(StorageFile baseStorageItem) : base(baseStorageItem)
        { }

        public File(string path) : base(StorageFile.GetFileFromPathAsync(path).GetResults())
        { }

        public IFile Copy(string destinationFullPath, 
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var name = SysPath.GetFileName(destinationFullPath);
            var parentPath = SysPath.GetDirectoryName(destinationFullPath);
            return Copy(parentPath, name, option);
        }

        public IFile Copy(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var parent = new Folder(destinationFolderPath);
            return Copy(parent, desiredNewName, option);
        }

        public IFile Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            return Copy(destinationFolder, Name, option);
        }

        public IFile Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var baseFolder = StorageFolder.GetFolderFromPathAsync(destinationFolder.Path).GetResults();
            var baseResultFile = BaseStorageItem.CopyAsync
                (baseFolder, desiredNewName, (WinIo.NameCollisionOption)option).GetResults();
            return new File(baseResultFile);
        }

        public void Move(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var parent = new Folder(destinationFolderPath);
            Move(parent, desiredNewName, option);
        }

        public void Move(string destinationFullPath, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var parentPath = SysPath.GetDirectoryName(destinationFullPath);
            var name = SysPath.GetFileName(destinationFullPath);
            Move(parentPath, name, option);
        }

        public void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(destinationFolder, Name, option);
        }

        public void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var baseFolder = StorageFolder.GetFolderFromPathAsync(destinationFolder.Path).GetResults();
            BaseStorageItem.MoveAsync(baseFolder, desiredNewName, (WinIo.NameCollisionOption) option).GetResults();
        }

        public void RenameIgnoreExtension(string desiredName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Rename(desiredName + Extension);
        }

        public bool CheckExistFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
