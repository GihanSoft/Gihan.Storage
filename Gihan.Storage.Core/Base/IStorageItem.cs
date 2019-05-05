using Gihan.Storage.Core.Enums;
using System;

namespace Gihan.Storage.Core.Base
{
    public interface IStorageItem
    {
        /// <summary>
        /// The full path of the item, if the item has a path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The name of the item including the file name extension if there is one.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The parent folder of the current storage item.
        /// </summary>
        IFolder Parent { get; }

        /// <summary>
        /// The <see cref="StorageItemType"/> of this item.
        /// </summary>
        StorageItemType Type { get; }

        /// <summary>
        /// Specifies whether the Item Exist or not
        /// </summary>
        bool Exist { get; }

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
        void Rename(string desiredName, 
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        
        /// <summary>
        /// Deletes the current item.
        /// </summary>
        void Delete();

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
        IStorageItem Copy(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        /// One of the enumeration values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolderPath"/>".
        /// </returns>
        IStorageItem Copy(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        IStorageItem Copy(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        IStorageItem Copy(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);
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
        void Move(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        void Move(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        void Move(IFolder destinationFolder,
            NameCollisionOption option = NameCollisionOption.FailIfExists);
        #endregion
    }
}
