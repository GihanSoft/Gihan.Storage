using System;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using WinIo = Windows.Storage;

namespace Gihan.Storage.UWP
{
    public class Folder : Base.StorageItem, IFolder
    {
        protected new WinIo.StorageFolder BaseStorageItem => (WinIo.StorageFolder)base.BaseStorageItem;

        public override StorageItemType Type => StorageItemType.Folder;
        //public override bool Exist => CheckExist(Path);

        public Folder(WinIo.StorageFolder folder) : base(folder)
        {
        }

        public Folder(string path) :
            base(WinIo.StorageFolder.GetFolderFromPathAsync(path).GetResults())
        {
        }

        public IReadOnlyList<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var files = new List<IFile>();
            var topLevelFiles = BaseStorageItem.GetFilesAsync().GetResults().
                Select(f => new File(f)).ToArray();
            files.AddRange(topLevelFiles);

            if (option == SearchOption.AllDirectories)
            {
                foreach (var folder in GetFolders())
                {
                    var innerFiles = folder.GetFiles(option);
                    files.AddRange(innerFiles);
                }
            }
            files.Sort((x, y) => String.Compare(x.Path, y.Path, StringComparison.Ordinal));
            return files;
        }

        public IReadOnlyList<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var folders = new List<IFolder>();

            var topLevelFolders = BaseStorageItem.GetFoldersAsync().GetResults().
                Select(f => new Folder(f)).ToArray();
            folders.AddRange(topLevelFolders);

            if (option == SearchOption.AllDirectories)
            {
                foreach (var folder in topLevelFolders)
                {
                    var innerFolders = folder.GetFolders(option);
                    folders.AddRange(innerFolders);
                }
            }
            folders.Sort((x, y) => String.Compare(x.Path, y.Path, StringComparison.Ordinal));
            return folders;
        }

        public bool IsEmpty(bool includeFolders = true)
        {
            throw new NotImplementedException();
        }

        public bool CheckExistFolder(string path)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var items = new List<IStorageItem>();

            var topLevelFolders = BaseStorageItem.GetFoldersAsync().GetResults().
                Select(f => new Folder(f)).ToArray();
            items.AddRange(topLevelFolders);
            var topLevelFiles = BaseStorageItem.GetFilesAsync().GetResults().
                Select(f => new File(f)).ToArray();
            items.AddRange(topLevelFiles);

            if (option == SearchOption.AllDirectories)
            {
                foreach (var folder in topLevelFolders)
                {
                    var innerItems = folder.GetItems(option);
                    items.AddRange(innerItems);
                }
            }

            items.Sort((x, y) => String.Compare(x.Path, y.Path, StringComparison.Ordinal));
            return items;
        }
    }
}
