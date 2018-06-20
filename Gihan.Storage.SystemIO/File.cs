using System;
using System.IO;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO.Base;

using SysIo = System.IO;
using SysPath = System.IO.Path;

namespace Gihan.Storage.SystemIO
{
    public class File : Base.StorageItem, IFile
    {
        //protected FileInfo BaseFile => (FileInfo)BaseStorageItem;
        protected new FileInfo BaseStorageItem => (FileInfo)base.BaseStorageItem;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public override StorageItemType Type => StorageItemType.File;

        /// <summary>
        /// The name of the item with out the file name extension if there is one.
        /// </summary>
        public string PureName => SysPath.GetFileNameWithoutExtension(Name);

        /// <summary>
        /// The extension of current item. (intclude '.')
        /// </summary>
        public string Extension => SysPath.GetExtension(Name);

        public File(FileInfo item) : base(item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!item.Exists)
                throw new ArgumentException("file is not exist", nameof(item));
            if (Directory.Exists(item.FullName))
                throw new ArgumentException("this is a folder", nameof(item));
        }

        public File(string filePath) : this(new FileInfo(filePath))
        {
        }

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
        public override void Rename(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(Parent, desiredName, option);
        }

        /// <summary>
        /// Renames the current item but ignore extension.
        ///     This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">
        /// The desired, new name of the current item.
        /// </param>
        /// <param name="option">
        /// The enum value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public void RenameIgnoreExtension(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Rename(desiredName + Extension, option);
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// This method also specifies what to do if 
        ///     a file with the same name already exists in the destination folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the copy of the file is created.
        /// </param>
        /// <param name="option">
        /// One of the enumeration values that determines how to handle the collision if
        ///     a file with the same name already exists in the destination folder.
        ///     Default value is <see cref="NameCollisionOption.FailIfExists"/>
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public IFile Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            return Copy(destinationFolder, Name, option);
        }

        /// <summary>
        /// Creates a copy of the file in the specified folder and renames the copy. This
        ///     method also specifies what to do if a file with the same name already exists
        ///     in the destination folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the copy of the file is created.
        /// </param>
        /// <param name="desiredNewName">
        /// The new name for the copy of the file created in the "<see cref="destinationFolder"/>".
        /// </param>
        /// <param name="option">
        /// One of the enumeration values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public IFile Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var destFullPath = SysPath.Combine(destinationFolder.Path, desiredNewName);

            switch (option)
            {
                case NameCollisionOption.GenerateUniqueName:
                    if (Directory.Exists(destFullPath) || SysIo.File.Exists(destFullPath))
                    {
                        var pureName = SysPath.GetFileNameWithoutExtension(desiredNewName);
                        var ex = SysPath.GetExtension(desiredNewName);
                        pureName = NextName(pureName);
                        return Copy(destinationFolder, pureName + ex, option);
                    }
                    else
                        return new File(BaseStorageItem.CopyTo(destFullPath));
                case NameCollisionOption.ReplaceExisting:
                    return new File(BaseStorageItem.CopyTo(destFullPath, true));
                case NameCollisionOption.FailIfExists:
                    return new File(BaseStorageItem.CopyTo(destFullPath, false));
                default:
                    throw new ArgumentException("invalid option", nameof(option));
            }
        }

        /// <summary>
        /// Moves the current file to the specified folder. This method also specifies 
        ///     what to do if a file with the same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="option">
        /// An enum value that determines how responds if the name of current file is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(destinationFolder, Name, option);
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according
        ///     to the desired name. This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="desiredNewName">
        /// The desired name of the file after it is moved.
        /// </param>
        /// <param name="option">
        /// An enum value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            var destFullPath = SysPath.Combine(destinationFolder.Path, desiredNewName);

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
                        var pureName = SysPath.GetFileNameWithoutExtension(desiredNewName);
                        var ex = SysPath.GetExtension(desiredNewName);
                        pureName = NextName(pureName);
                        Move(destinationFolder, pureName + ex, option);
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

        /// <summary>
        /// Replaces the specified file with a copy of the current file.
        /// </summary>
        /// <param name="fileToReplace">
        /// The file to replace.
        /// </param>
        public void Replace(IFile fileToReplace)
        {
            Move(fileToReplace.Parent, fileToReplace.Name, NameCollisionOption.ReplaceExisting);
        }
    }
}