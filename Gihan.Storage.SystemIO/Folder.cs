using Gihan.Helpers.String;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using SearchOption = Gihan.Storage.Core.Enums.SearchOption;
using SysIO = System.IO;
using SysPath = System.IO.Path;

namespace Gihan.Storage.SystemIO
{
    public class Folder : StorageItem, IFolder
    {
        protected new SysIO.DirectoryInfo BaseStorageItem => (SysIO.DirectoryInfo)base.BaseStorageItem;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public override StorageItemType Type => StorageItemType.Folder;

        public Folder(SysIO.DirectoryInfo item) : base(item)
        {
        }

        public Folder(string folderPath) : base(new SysIO.DirectoryInfo(folderPath))
        {
        }

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns>
        /// a list of the files in the
        ///     current folder. The list is of type <see cref="IReadOnlyList&lt;IFile &gt;"/>. 
        ///     Each file in the list is represented by a <see cref="IFile"/> object.
        /// </returns>
        public IReadOnlyList<IFile> GetFiles
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFiles = BaseStorageItem.EnumerateFiles("*", (SysIO.SearchOption)option);
            var files = baseFiles.Select(bf => new File(bf)).ToList();
            files.Sort(NaturalStringComparer<StorageItem>.Default);
            return files.AsReadOnly();
        }

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the subfolders
        ///     in the current folder. 
        ///     The list is of type <see cref="IReadOnlyList&lt;IFolder&gt;"/>
        ///     . Each folder in the list is represented by a <see cref="IFolder"/> object.
        /// </returns>
        public IReadOnlyList<IFolder> GetFolders
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFolders = BaseStorageItem.EnumerateDirectories("*", (SysIO.SearchOption)option);
            var folders = baseFolders.Select(bf => new Folder(bf)).ToList();
            folders.Sort(NaturalStringComparer<IStorageItem>.Default);
            return folders.AsReadOnly();
        }

        /// <summary>
        /// Gets the files and subfolders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the files and folders
        ///     in the current folder. 
        ///     The list is of type <see cref="IReadOnlyList&lt;IStorageItem&gt;"/>. 
        ///     Each item in the list is represented by an <see cref="IStorageItem"/> object.
        /// </returns>
        public IReadOnlyList<IStorageItem> GetItems
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            IEnumerable<IStorageItem> files = GetFiles(option);
            IEnumerable<IStorageItem> folders = GetFolders(option);
            var items = files.Concat(folders).ToList();
            items.Sort(NaturalStringComparer<IStorageItem>.Default);
            return items.AsReadOnly();
        }

        public bool CheckExistFolder(string path)
        {
            return SysIO.Directory.Exists(path);
        }

        /// <summary>
        /// Renames the current folder. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current folder.</param>
        /// <param name="option">
        /// The enum value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current folder's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public override void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var destFullPath = SysPath.Combine(Parent.Path, desiredName);

            StorageItem item = null;
            if (SysIO.Directory.Exists(destFullPath))
            {
                item = new Folder(destFullPath);
            }
            else if (SysIO.File.Exists(destFullPath))
            {
                item = new File(destFullPath);
            }

            if (item != null)
            {
                switch (option)
                {
                    case NameCollisionOption.GenerateUniqueName:
                        var nextName = NextName.ProductNextName(desiredName);
                        Rename(nextName, option);
                        return;
                    case NameCollisionOption.ReplaceExisting:
                        item.Delete();
                        break;
                    case NameCollisionOption.FailIfExists:
                        // System.IO default option. .net will throw exception
                        break;
                    default:
                        throw new ArgumentException("invalid option", nameof(option));
                }
                if (item.Parent.Path == Parent.Path && item.Name != Name && item.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
                    Rename(Name, NameCollisionOption.GenerateUniqueName);
            }
            BaseStorageItem.MoveTo(destFullPath);
        }

        public bool IsEmpty(bool includeFolders = true)
        {
            var items = GetItems(includeFolders ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            return items.Count <= 0;
        }
    }
}
