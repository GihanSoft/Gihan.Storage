using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;

namespace Gihan.Storage.Core
{
    /// <inheritdoc />
    public interface IFile : IStorageItem
    {
        /// <summary>
        /// The name of the item with out the file name extension if there is one.
        /// </summary>
        string PureName { get; }

        /// <summary>
        /// The extension of current item. (intclude '.')
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Creates a copy of the file in the specified path
        ///     This method also specifies what to do if a file with the same name already exists.
        /// </summary>
        /// <param name="destinationFullPath">
        /// The destination path where the copy of the file is created.
        /// </param>
        /// <param name="option">
        /// One of the enum values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="destinationFullPath"/>" already exists.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file that path of it, is "<see cref="destinationFullPath"/>".
        /// </returns>
        IFile Copy(string destinationFullPath,
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
        IFile Copy(string destinationFolderPath, string desiredNewName,
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
        IFile Copy(IFolder destinationFolder,
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
        IFile Copy(IFolder destinationFolder, string desiredNewName,
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
        /// An enum value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        void Move(string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

        /// <summary>
        /// Moves the current file to <see cref="destinationFullPath"/>.
        /// This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destinationFullPath">
        /// new full path of file, including name and extension.
        /// </param>
        /// <param name="option">
        /// An enum value that determines how responds if a file with same path
        /// is exist in the destination folder.
        /// </param>
        void Move(string destinationFullPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        void Move(IFolder destinationFolder,
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
        /// An enum value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        void Move(IFolder destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

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
        void RenameIgnoreExtension(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);

        /// <summary>
        /// Check is there a file (and not folder) exist in gived path
        /// </summary>
        /// <param name="path">path to check for file</param>
        /// <returns>
        /// true if there is a file in <see cref="path"/>.
        /// fale if it's not exist
        /// </returns>
        bool CheckExistFile(string path);
    }
}
