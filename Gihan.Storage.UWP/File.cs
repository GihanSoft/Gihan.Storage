using System;
using Gihan.Storage.Core;
using Windows.Storage;
using NameCollisionOption = Gihan.Storage.Core.Enums.NameCollisionOption;

namespace Gihan.Storage.UWP
{
    public class File : Base.StorageItem, IFile
    {
        StorageFile BaseFile { get; }

        public string PureName => throw new NotImplementedException();

        public string Extension => throw new NotImplementedException();

        public IFile Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public IFile Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void RenameIgnoreExtension(string desiredName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }

        public void Replace(IFile fileToReplace)
        {
            throw new NotImplementedException();
        }
    }
}
