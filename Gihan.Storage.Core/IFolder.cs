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
    }
}
