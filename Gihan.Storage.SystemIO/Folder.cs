using Gihan.Helpers.Linq;
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
        /// a list of the files in the current folder. 
        /// The list is of type <see cref="IReadOnlyCollection&lt;IFile&gt;"/>. 
        ///     Each file in the list is represented by a <see cref="IFile"/> object.
        /// </returns>
        public IReadOnlyCollection<IFile> GetFiles
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFiles = BaseStorageItem.EnumerateFiles("*", (SysIO.SearchOption)option);
            return baseFiles.Select(baseFile => new File(baseFile)).
                NaturalOrderBy(f => f.Path).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the sub-folders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the sub-folders
        ///     in the current folder. 
        ///     The list is of type <see cref="IReadOnlyList&lt;IFolder&gt;"/>
        ///     . Each folder in the list is represented by a <see cref="IFolder"/> object.
        /// </returns>
        public IReadOnlyCollection<IFolder> GetFolders
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var baseFolders = BaseStorageItem.EnumerateDirectories("*", (SysIO.SearchOption)option);
            return baseFolders.Select(baseFolder => new Folder(baseFolder)).
                NaturalOrderBy(f => f.Path).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the files and sub-folders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the files and folders
        ///     in the current folder. 
        ///     The list is of type <see cref="IReadOnlyList&lt;IStorageItem&gt;"/>. 
        ///     Each item in the list is represented by an <see cref="IStorageItem"/> object.
        /// </returns>
        public IReadOnlyCollection<IStorageItem> GetItems
            (SearchOption option = SearchOption.TopDirectoryOnly)
        {
            IEnumerable<IStorageItem> files = GetFiles(option);
            IEnumerable<IStorageItem> folders = GetFolders(option);
            return files.Concat(folders).NaturalOrderBy(i => i.Path).ToList().AsReadOnly();
        }

        /// <summary>
        /// Renames the current folder. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current folder.</param>
        /// <param name="option">
        /// The <see cref="Enum"/> value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current folder's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public override void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var invalidChar = SysPath.GetInvalidFileNameChars().FirstOrDefault(ch => desiredName.Contains(ch));
            if (invalidChar != char.MinValue)
            {
                throw new ArgumentException("Name contains invalid character:" + invalidChar, nameof(desiredName));
            }
            Move(Parent, desiredName, option);
        }

        public bool IsEmpty(bool countFolders = true)
        {
            var items = countFolders ? GetItems() : GetFiles(SearchOption.AllDirectories);
            return items.Count <= 0;
        }

        #region Copy
        public new IFolder Copy(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrWhiteSpace(destinationFullPath))
                throw new ArgumentNullException(nameof(destinationFullPath));
            if (!Exist)
                throw new SysIO.DirectoryNotFoundException("Source folder is not exist");
            Base.StorageHelper.Init();
            var item = Core.Base.StorageHelper.Creat().GetItem(destinationFullPath);
            if (item != null)
            {
                if (!string.Equals(Path, item.Path, StringComparison.OrdinalIgnoreCase))
                    switch (option)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            return Copy(item.Parent.Path, NextName.ProductNextName(item.Name), option);
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete();
                            break;
                        case NameCollisionOption.FailIfExists:
                            throw new SysIO.IOException($"A {item.Type} exist in {destinationFullPath}");
                        default:
                            throw new ArgumentException("invalid option", nameof(option));
                    }
                else
                {
                    throw new SysIO.IOException("Source and destination folders are same");
                }
            }

            var destFolder = new Folder(destinationFullPath);
            destFolder.Create();
            var files = GetFiles();
            foreach (var file in files)
                file.Copy(destFolder);
            var subFolders = GetFolders();
            foreach (var folder in subFolders)
                folder.Copy(destFolder);
            return destFolder;
        }

        public new IFolder Copy(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(SysPath.Combine(destinationFolderPath, desiredNewName), option);

        public new IFolder Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(destinationFolder.Path, desiredNewName, option);

        public new IFolder Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Copy(destinationFolder, Name, option);
        #endregion

        #region Move
        public override void Move(string destinationFullPath, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrWhiteSpace(destinationFullPath))
                throw new ArgumentNullException(nameof(destinationFullPath));
            if (!Exist)
                throw new SysIO.DirectoryNotFoundException("Source folder is not exist");
            Base.StorageHelper.Init();
            var item = Core.Base.StorageHelper.Creat().GetItem(destinationFullPath);
            if (item != null)
            {
                if (!string.Equals(Path, item.Path, StringComparison.OrdinalIgnoreCase))
                    switch (option)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            Move(item.Parent.Path, NextName.ProductNextName(item.Name), option);
                            return;
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete();
                            break;
                        case NameCollisionOption.FailIfExists:
                            throw new SysIO.IOException($"A {item.Type} exist in {destinationFullPath}");
                        default:
                            throw new ArgumentException("invalid option", nameof(option));
                    }
                else
                {
                    if (string.Equals(Path, item.Path, StringComparison.Ordinal))
                        throw new SysIO.IOException("Source and destination folders are same");
                    else
                    {
                        Move(item.Parent, NextName.ProductNextName(Name), NameCollisionOption.GenerateUniqueName);
                        Move(destinationFullPath);
                        return;
                    }
                }
            }

            try
            {
                BaseStorageItem.MoveTo(destinationFullPath);
            }
            catch (SysIO.IOException)
            {
                var newOne = Copy(destinationFullPath, option) as Folder;
                Delete();
                base.BaseStorageItem = newOne.BaseStorageItem;
            }
            ResetParent();
        }

        public override void Move(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Move(SysPath.Combine(destinationFolderPath, Path));

        public override void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Move(destinationFolder.Path, desiredNewName, option);

        public override void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => Move(destinationFolder, Name, option);
        #endregion Move

        public void Create()
        {
            BaseStorageItem.Create();
        }

        public IFolder CreateSubfolder(string name)
        {
            var invalidChar = SysPath.GetInvalidFileNameChars().FirstOrDefault(ch => name.Contains(ch));
            if (invalidChar != char.MinValue)
                throw new ArgumentException("Name contains invalid character:" + invalidChar, nameof(name));
            return new Folder(BaseStorageItem.CreateSubdirectory(name));
        }
    }
}
