using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO.Base;

using SysPath = System.IO.Path;
using SysIo = System.IO;

namespace Gihan.Storage.SystemIO
{
    public class Folder : StorageItem, IFolder
    {
        protected new DirectoryInfo BaseStorageItem => (DirectoryInfo)base.BaseStorageItem;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public override StorageItemType Type => StorageItemType.Folder;

        public Folder(DirectoryInfo item) : base(item)
        {
            if (!item.Exists)
                throw new ArgumentException("Folder is not exist", nameof(item));
        }

        public Folder(string folderPath) : base(new DirectoryInfo(folderPath))
        {
            if (!Directory.Exists(folderPath))
                throw new ArgumentException("Folder is not exist", nameof(folderPath));
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
            (Core.Enums.SearchOption option = Core.Enums.SearchOption.TopDirectoryOnly)
        {
            var baseFiles = BaseStorageItem.EnumerateFiles("*.*", (SysIo.SearchOption)option);
            var files = baseFiles.Select(bf => new File(bf));
            return files.ToList().AsReadOnly();
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
            (Core.Enums.SearchOption option = Core.Enums.SearchOption.TopDirectoryOnly)
        {
            var baseFolders = BaseStorageItem.EnumerateDirectories("*", (SysIo.SearchOption)option);
            var folders = baseFolders.Select(bf => new Folder(bf));
            return folders.ToList().AsReadOnly();
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
            (Core.Enums.SearchOption option = Core.Enums.SearchOption.TopDirectoryOnly)
        {
            IEnumerable<IStorageItem> files = GetFiles(option);
            IEnumerable<IStorageItem> folders = GetFolders(option);
            return files.Concat(folders).ToList().AsReadOnly();   
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
            if (Directory.Exists(destFullPath))
            {
                item = new Folder(destFullPath);
            }
            else if (SysIo.File.Exists(destFullPath))
            {
                item = new File(destFullPath);
            }

            if (item != null)
            {
                switch (option)
                {
                    case NameCollisionOption.GenerateUniqueName:
                        var nextName = NextName(desiredName);
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
            }
            BaseStorageItem.MoveTo(destFullPath);
        }
    }
}
