using System;
using System.IO;

namespace Gihan.Helpers.Storage
{
    public static class FileSystemInfoEx
    {
        public static DirectoryInfo GetParent(this FileSystemInfo src)
        {
            switch (src)
            {
                case FileInfo file:
                    return file.Directory;
                case DirectoryInfo directory:
                    return directory.Parent;
                default:
                    throw new NotSupportedException("storage type not supported");
            }
        }
        public static void Rename(this FileSystemInfo src, string destName, 
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
        {
            src.MoveTo(Path.Combine(src.GetParent().FullName, destName), nameCollisionOption);
        }

        #region Copy
        /// <summary>
        /// Creates a copy of the file in the specified path
        ///     This method also specifies what to do if a file with the same name already exists.
        /// </summary>
        /// <param name="destFullName">
        /// The destination path where the copy of the file is created.
        /// </param>
        /// <param name="nameCollisionOption">
        /// One of the <see cref="Enum"/> values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="destinationFullPath"/>" already exists.
        /// </param>
        /// <returns>
        /// <see cref="FileSystemInfo"/> that represents the copy
        ///     of the FileSystemInfo that path of it is "<param name="destFullName"/>".
        /// </returns>
        public static FileSystemInfo CopyTo(this FileSystemInfo src, string destFullName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
        {
            switch (src)
            {
                case FileInfo file:
                    return file.CopyTo(destFullName, nameCollisionOption);
                case DirectoryInfo directory:
                    return directory.CopyTo(destFullName, nameCollisionOption);
                default:
                    throw new NotSupportedException("storage type not supported");
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
        /// One of the enumeration values that determines how to handle the collision if a file 
        ///     with the specified "<see cref="desiredNewName"/>" already exists in the destination folder.
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolderPath"/>".
        /// </returns>
        public static FileSystemInfo CopyTo(this FileSystemInfo src, string destinationFolderPath, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
            => src.CopyTo(Path.Combine(destinationFolderPath, desiredNewName), option);

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
        public static FileSystemInfo CopyTo(this FileSystemInfo src, DirectoryInfo destinationFolder, string desiredNewName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
            => src.CopyTo(destinationFolder.FullName, desiredNewName, option);

        /// <summary>
        /// Creates a copy of the file in the specified folder.
        /// This method also specifies what to do if 
        ///     a file with the same name already exists in the destination folder.
        /// </summary>
        /// <param name="destFolder">
        /// The destination folder where the copy of the file is created.
        /// </param>
        /// <param name="nameCollisionOption">
        /// One of the enumeration values that determines how to handle the collision if
        ///     a file with the same name already exists in the destination folder.
        ///     Default value is <see cref="NameCollisionOption.FailIfExists"/>
        /// </param>
        /// <returns>
        /// <see cref="IFile"/> that represents the copy
        ///     of the file created in the "<see cref="destinationFolder"/>".
        /// </returns>
        public static FileSystemInfo CopyTo(this FileSystemInfo src, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => src.CopyTo(destFolder.FullName, src.Name, nameCollisionOption);
        #endregion

        #region Move
        public static void MoveTo(this FileSystemInfo src, string destPath, 
            NameCollisionOption nameCollisionOption)
        {
            switch (src)
            {
                case FileInfo file:
                    file.MoveTo(destPath, nameCollisionOption);
                    return;
                case DirectoryInfo directory:
                    directory.MoveTo(destPath, nameCollisionOption);
                    return;
                default:
                    throw new NotSupportedException("storage type not supported");
            }
        }

        public static void MoveTo(this FileSystemInfo src, string destFolderPath, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => src.MoveTo(Path.Combine(destFolderPath, newName), nameCollisionOption);

        public static void MoveTo(this FileSystemInfo src, DirectoryInfo destFolder, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => src.MoveTo(destFolder.FullName, newName, nameCollisionOption);

        public static void MoveTo(this FileSystemInfo src, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
            => src.MoveTo(destFolder.FullName, src.Name, nameCollisionOption);
        #endregion

        public static FileSystemInfo GetFileSystemInfo(string path)
        {
            var file = new FileInfo(path);
            if (file.Attributes.HasFlag(FileAttributes.Directory))
                return new DirectoryInfo(path);
            return file;
        }
    }
}
