using System;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Enums;

namespace Gihan.Storage.UWP.Base
{
    public abstract class StorageItem : Core.Base.IStorageItem
    {
        public string Path => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public IFolder Parent => throw new NotImplementedException();

        public StorageItemType Type => throw new NotImplementedException();

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Rename(string desiredName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            throw new NotImplementedException();
        }
    }
}
