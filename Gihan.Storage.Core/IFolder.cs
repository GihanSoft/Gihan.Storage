using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using System.Collections.Generic;

namespace Gihan.Storage.Core
{
    /// <inheritdoc />
    public interface IFolder : IStorageItem
    {
        /// <summary>
        /// Gets the files and subfolders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the files and folders
        ///     in the current folder. The list is of type <see cref="IReadOnlyList&lt;IStorageItem&gt;"/>. 
        ///     Each item in the list is represented by an <see cref="IStorageItem"/> object.
        /// </returns>
        IReadOnlyList<IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns>
        /// a list of the files in the
        ///     current folder. The list is of type <see cref="IReadOnlyList&lt;IFile &gt;"/>. 
        ///     Each file in the list is represented by a <see cref="IFile"/> object.
        /// </returns>
        IReadOnlyList<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Gets the subfolders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the subfolders
        ///     in the current folder. The list is of type <see cref="IReadOnlyList&lt;IFolder&gt;"/>
        ///     . Each folder in the list is represented by a <see cref="IFolder"/> object.
        /// </returns>
        IReadOnlyList<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly);

        /*
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
        IFolder Copy(string destinationFullPath,
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
        IFolder Copy(string destinationFolderPath, string desiredNewName,
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
        IFolder Copy(IFolder destinationFolder,
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
        IFolder Copy(IFolder destinationFolder, string desiredNewName,
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
        */

        /// <summary>
        /// Specifies is folder empty or not.
        /// This method also specifies should it include empty folder in it or not.
        /// </summary>
        /// <param name="includeFolders">
        /// If false, folder with empty folders in it is same as empty.
        /// </param>
        /// <returns></returns>
        bool IsEmpty(bool includeFolders = true);
    }
}
