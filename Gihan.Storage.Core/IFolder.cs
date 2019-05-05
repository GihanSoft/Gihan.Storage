using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using System;
using System.Collections.Generic;

namespace Gihan.Storage.Core
{
    /// <inheritdoc />
    public interface IFolder : IStorageItem
    {
        /// <summary>
        /// Gets the files and sub-folders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the files and folders
        ///     in the current folder. The list is of type <see cref="IReadOnlyList&lt;IStorageItem&gt;"/>. 
        ///     Each item in the list is represented by an <see cref="IStorageItem"/> object.
        /// </returns>
        IReadOnlyCollection<IStorageItem> GetItems(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns>
        /// a list of the files in the
        ///     current folder. The list is of type <see cref="IReadOnlyList&lt;IFile &gt;"/>. 
        ///     Each file in the list is represented by a <see cref="IFile"/> object.
        /// </returns>
        IReadOnlyCollection<IFile> GetFiles(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Gets the sub-folders in the current folder.
        /// </summary>
        /// <returns>
        /// A list of the sub-folders
        ///     in the current folder. The list is of type <see cref="IReadOnlyList&lt;IFolder&gt;"/>
        ///     . Each folder in the list is represented by a <see cref="IFolder"/> object.
        /// </returns>
        IReadOnlyCollection<IFolder> GetFolders(SearchOption option = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Specifies is folder empty or not.
        /// This method also specifies should it include empty folder in it or not.
        /// </summary>
        /// <param name="includeFolders">
        /// If false, folder with empty folders in it is same as empty.
        /// </param>
        /// <returns></returns>
        bool IsEmpty(bool includeFolders = true);

        /// <summary>
        /// Creates directory.
        /// </summary>
        void Create();

        /// <summary>
        /// Creates a new sub-folder with the specified name in the current folder.
        /// </summary>
        /// <param name="name">The name of the new sub folder to create in the current folder.</param>
        /// <returns>Created Folder</returns>
        IFolder CreateSubfolder(string name);
    }
}
