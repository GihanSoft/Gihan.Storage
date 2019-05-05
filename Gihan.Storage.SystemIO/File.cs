using Gihan.Helpers.String;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO.Base;
using System;
using System.Linq;
using SysIO = System.IO;
using SysPath = System.IO.Path;

namespace Gihan.Storage.SystemIO
{
    public class File : StorageItem, IFile
    {
        protected new SysIO.FileInfo BaseStorageItem => (SysIO.FileInfo)base.BaseStorageItem;

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        public override StorageItemType Type => StorageItemType.File;

        /// <summary>
        /// The name of the item with out the file name extension if there is one.
        /// </summary>
        public string PureName => SysPath.GetFileNameWithoutExtension(Name);

        /// <summary>
        /// The extension of current item. (include '.')
        /// </summary>
        public string Extension => BaseStorageItem.Extension;

        public File(SysIO.FileInfo item) : base(item)
        {
            if (SysIO.Directory.Exists(item.FullName))
                throw new ArgumentException("this is a folder", nameof(item));
        }

        public File(string filePath) : this(new SysIO.FileInfo(filePath))
        {
        }

        /// <summary>
        /// Renames the current item. This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">The desired, new name of the current item.</param>
        /// <param name="option">
        /// The <see cref="Enum"/> value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
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

        /// <summary>
        /// Renames the current item but ignore extension.
        ///     This method also specifies what to do if an existing
        ///     item in the current item's location has the same name.
        /// </summary>
        /// <param name="desiredName">
        /// The desired, new name of the current item.
        /// </param>
        /// <param name="option">
        /// The <see cref="Enum"/> value that determines how responds if the <see cref="desiredName"/> is the
        ///     same as the name of an existing item in the current item's location.
        ///     Default value is "<see cref="NameCollisionOption.FailIfExists"/>".
        /// </param>
        public void RenameIgnoreExtension(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Rename(desiredName + Extension, option);
        }

        #region Copy
        /// <summary>
        /// Creates a copy of the file in the specified path
        ///     This method also specifies what to do if a file with the same name already exists.
        /// </summary>
        /// <param name="destinationFullPath">
        /// The destination path where the copy of the file is created.
        /// </param>
        /// <param name="option">
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="destinationFullPath"/>" already exists.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file that path of it, is "<see cref="destinationFullPath"/>".
        /// </returns>
        public new IFile Copy(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrWhiteSpace(destinationFullPath))
                throw new ArgumentNullException(nameof(destinationFullPath));

            switch (option)
            {
                case NameCollisionOption.GenerateUniqueName:
                    Base.StorageHelper.Init();
                    var item = Core.Base.StorageHelper.Creat().GetItem(destinationFullPath);
                    if (item != null)
                        return Copy(item.Parent, NextName.ProductNextName(PureName) + Extension, option);
                    else
                        return new File(BaseStorageItem.CopyTo(destinationFullPath));
                case NameCollisionOption.ReplaceExisting:
                    return new File(BaseStorageItem.CopyTo(destinationFullPath, true));
                case NameCollisionOption.FailIfExists:
                    return new File(BaseStorageItem.CopyTo(destinationFullPath, false));
                default:
                    throw new ArgumentException("invalid option", nameof(option));
            }
        }

        /// <summary>
        /// Creates a copy of the file in the specified path and rename the copy
        ///     This method also specifies what to do if a file with the same name already exists.
        /// </summary>
        /// <param name="destinationFolderPath">
        /// The destination folder path where the copy of the file is created.
        /// </param>
        /// <param name="desiredNewName">
        /// The new name for the copy of the file created in the "<see cref="destinationFolderPath"/>".
        /// </param>
        /// <param name="option">
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolderPath"/>".
        /// </returns>
        public new IFile Copy(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            return Copy(SysPath.Combine(destinationFolderPath, desiredNewName), option);
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
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if
        ///     a file with the same name already exists in the destination folder.
        ///     Default value is <see cref="NameCollisionOption.FailIfExists"/>
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public new IFile Copy(IFolder destinationFolder,
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
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public new IFile Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (destinationFolder == null) throw
                new ArgumentNullException(nameof(destinationFolder));
            if (string.IsNullOrWhiteSpace(desiredNewName))
                throw new ArgumentNullException(nameof(desiredNewName));

            var destFullPath = SysPath.Combine(destinationFolder.Path, desiredNewName);
            return Copy(destFullPath, option);
        }
        #endregion

        #region Move
        /// <summary>
        /// Moves the current file to <see cref="destinationFullPath"/>.
        /// This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFullPath">
        /// new full path of file, including name and extension.
        /// </param>
        /// <param name="option">
        /// An <see cref="Enum"/> value that determines how responds if a file with same path
        /// is exist in the destination folder.
        /// </param>
        public override void Move(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrWhiteSpace(destinationFullPath))
                throw new ArgumentNullException(nameof(destinationFullPath));
            if (!Exist)
                // like this, we will have a real exception XD. (FileNotFoundException)
                BaseStorageItem.MoveTo(destinationFullPath);

            Base.StorageHelper.Init();
            var item = Core.Base.StorageHelper.Creat().GetItem(destinationFullPath);

            if (item != null)
            {
                if (!string.Equals(Path, item.Path, StringComparison.OrdinalIgnoreCase))
                    switch (option)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            Move(item.Parent, NextName.ProductNextName(Name), option);
                            return;
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete(); //No problem. Existence of source checked in start of method.
                            break;
                        case NameCollisionOption.FailIfExists:
                            // System.IO default option. .net will throw exception
                            break;
                        default:
                            throw new ArgumentException("invalid option", nameof(option));
                    }
            }
            BaseStorageItem.MoveTo(destinationFullPath);
            ResetParent();
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according
        ///     to the desired name. This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolderPath">
        /// The destination folderPath where the file is moved.
        /// </param>
        /// <param name="desiredNewName">
        /// The desired name of the file after it is moved.
        /// </param>
        /// <param name="option">
        /// An <see cref="Enum"/> value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public override void Move(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(SysPath.Combine(destinationFolderPath, desiredNewName), option);
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
        /// An <see cref="Enum"/> value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public override void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(destinationFolder.Path, desiredNewName, option);
        }

        /// <summary>
        /// Moves the current file to the specified folder. This method also specifies 
        ///     what to do if a file with the same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="option">
        /// An <see cref="Enum"/> value that determines how responds if the name of current file is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public override void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            Move(destinationFolder, Name, option);
        }
        #endregion
    }
}