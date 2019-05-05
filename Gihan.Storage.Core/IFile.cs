using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using System;

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
        /// The extension of current item. (include '.')
        /// </summary>
        string Extension { get; }

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
        void RenameIgnoreExtension(string desiredName,
            NameCollisionOption option = NameCollisionOption.FailIfExists);
    }
}
