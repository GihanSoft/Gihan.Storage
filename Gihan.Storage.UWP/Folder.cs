using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using System;
using System.Collections.Generic;
using WinIo = Windows.Storage;

namespace Gihan.Storage.UWP
{
    public class Folder : Base.StorageItem, IFolder
    {
        private WinIo.StorageFolder BaseFolder { get; }

        public IReadOnlyList<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            throw new NotImplementedException();
        }
    }
}
