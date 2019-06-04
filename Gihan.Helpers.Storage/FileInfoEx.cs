using Gihan.Helpers.String;
using System;
using System.IO;

using static Gihan.Helpers.String.NextName;

namespace Gihan.Helpers.Storage
{
    public static class FileInfoEx
    {
        public static string GetNameWithoutExtension(this FileInfo srcFile)
            => Path.GetFileNameWithoutExtension(srcFile.Name);
        public static string GetPureName(this FileInfo srcFile)
            => Path.GetFileNameWithoutExtension(srcFile.Name);
        public static void RenameIgnoreExtension(this FileInfo srcFile, string destName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => srcFile.Rename(destName + srcFile.Extension, nameCollisionOption);

        #region Copy
        /// <summary>
        /// Creates a copy of the srcFile in the specified path
        ///     This method also specifies what to do if a srcFile with the same name already exists.
        /// </summary>
        /// <param name="destPath">
        /// The destination path where the copy of the srcFile is created.
        /// </param>
        /// <param name="nameCollisionOption">
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if a srcFile 
        ///     with the specified "<param name="destPath"/>" already exists.
        /// </param>
        /// <returns>
        /// <see cref="FileInfo"/> that represents the copy
        ///     of the srcFile that path of it is "<param name="destPath">".
        /// </returns>
        public static FileInfo CopyTo(this FileInfo srcFile, string destPath,
            NameCollisionOption nameCollisionOption)
        {
            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath));

            switch (nameCollisionOption)
            {
                case NameCollisionOption.GenerateUniqueName:
                    var item = FileSystemInfoEx.GetFileSystemInfo(destPath);
                    if (item.Exists)
                    {
                        return srcFile.CopyTo(item.GetParent().FullName,
                                         ProductNextName(srcFile.GetPureName()) + srcFile.Extension,
                                         nameCollisionOption);
                    }
                    else
                        return srcFile.CopyTo(destPath);
                case NameCollisionOption.ReplaceExisting:
                    return srcFile.CopyTo(destPath, true);
                case NameCollisionOption.FailIfExists:
                    return srcFile.CopyTo(destPath, false);
                default:
                    throw new ArgumentException("invalid option", nameof(nameCollisionOption));
            }
        }

        /// <summary>
        /// Creates a copy of the srcFile in the specified path and rename the copy
        ///     This method also specifies what to do if a srcFile with the same name already exists.
        /// </summary>
        /// <param name="destFolderPath">
        /// The destination folder path where the copy of the srcFile is created.
        /// </param>
        /// <param name="newName">
        /// The new name for the copy of the srcFile created in the "<param name="destFolderPath">".
        /// </param>
        /// <param name="nameCollisionOption">
        /// One of the enumeration values that determines how to handle the collision if a srcFile 
        ///     with the specified "<param name="newName">" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="FileInfo"/> that represents the copy
        ///     of the srcFile created in the "<param name="destFolderPath">".
        /// </returns>
        public static FileInfo CopyTo(this FileInfo srcFile, string destFolderPath, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => srcFile.CopyTo(Path.Combine(destFolderPath, newName), nameCollisionOption);

        /// <summary>
        /// Creates a copy of the srcFile in the specified folder and renames the copy. This
        ///     method also specifies what to do if a srcFile with the same name already exists
        ///     in the destination folder.
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder where the copy of the srcFile is created.
        /// </param>
        /// <param name="desiredNewName">
        /// The new name for the copy of the srcFile created in the "<see cref="destinationFolder"/>".
        /// </param>
        /// <param name="option">
        /// One of the enumeration values that determines how to handle the collision if a srcFile 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the srcFile created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public static FileInfo ToCopy(this FileInfo srcFile, DirectoryInfo destFolder, string newName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
            => srcFile.CopyTo(destFolder.FullName, newName, option);

        /// <summary>
        /// Creates a copy of the srcFile in the specified folder.
        /// This method also specifies what to do if 
        ///     a srcFile with the same name already exists in the destination folder.
        /// </summary>
        /// <param name="destFolder">
        /// The destination folder where the copy of the srcFile is created.
        /// </param>
        /// <param name="nameCollisionOption">
        /// One of the enumeration values that determines how to handle the collision if
        ///     a srcFile with the same name already exists in the destination folder.
        ///     Default value is <see cref="NameCollisionOption.FailIfExists"/>
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the srcFile created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public static FileInfo CopyTo(this FileInfo srcFile, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => srcFile.CopyTo(destFolder.FullName, srcFile.Name, nameCollisionOption);
        #endregion

        #region Move
        /// <summary>
        /// Moves the current file to <see cref="destinationFullPath"/>.
        /// This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destPath">
        /// new full path of file, including name and extension.
        /// </param>
        /// <param name="option">
        /// An <see cref="Enum"/> value that determines how responds if a file with same path
        /// is exist in the destination folder.
        /// </param>
        public static void MoveTo(this FileInfo file, string destPath,
            NameCollisionOption nameCollisionOption)
        {
            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath));
            if (!file.Exists)
                // like this, we will have a real exception XD. (FileNotFoundException)
                file.MoveTo(destPath);

            var item = FileSystemInfoEx.GetFileSystemInfo(destPath);

            if (item.Exists)
            {
                if (!string.Equals(file.FullName, item.FullName, StringComparison.OrdinalIgnoreCase))
                    switch (nameCollisionOption)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            file.MoveTo(item.GetParent(),
                                ProductNextName(file.GetPureName()) + file.Extension, nameCollisionOption);
                            return;
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete(); //No problem. Existence of source checked in start of method.
                            break;
                        case NameCollisionOption.FailIfExists:
                            // System.IO default option. .net will throw exception
                            break;
                        default:
                            throw new ArgumentException("invalid option", nameof(nameCollisionOption));
                    }
            }
            file.MoveTo(destPath);
        }

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according
        ///     to the desired name. This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destFolderPath">
        /// The destination folderPath where the file is moved.
        /// </param>
        /// <param name="newName">
        /// The desired name of the file after it is moved.
        /// </param>
        /// <param name="nameCollisionOption">
        /// An <see cref="Enum"/> value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public static void MoveTo(this FileInfo file, string destFolderPath, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => file.MoveTo(Path.Combine(destFolderPath, newName), nameCollisionOption);

        /// <summary>
        /// Moves the current file to the specified folder and renames the file according
        ///     to the desired name. This method also specifies what to do if a file with the
        ///     same name already exists in the specified folder.
        /// </summary>
        /// <param name="destFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="newName">
        /// The desired name of the file after it is moved.
        /// </param>
        /// <param name="option">
        /// An <see cref="Enum"/> value that determines how responds if the "<see cref="desiredNewName"/>" is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public static void MoveTo(this FileInfo file, DirectoryInfo destFolder, string newName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
            => file.MoveTo(destFolder.FullName, newName, option);

        /// <summary>
        /// Moves the current file to the specified folder. This method also specifies 
        ///     what to do if a file with the same name already exists in the specified folder.
        /// </summary>
        /// <param name="destFolder">
        /// The destination folder where the file is moved.
        /// </param>
        /// <param name="nameCollisionOption">
        /// An <see cref="Enum"/> value that determines how responds if the name of current file is
        ///     the same as the name of an existing file in the destination folder.
        /// </param>
        public static void MoveTo(this FileInfo file, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => file.MoveTo(destFolder.FullName, file.Name, nameCollisionOption);
        #endregion
    }
}
