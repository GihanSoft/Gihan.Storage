using System;
using System.IO;

using static Gihan.Helpers.String.NextName;

namespace Gihan.Helpers.Storage
{
    public static class DirectoryInfoEx
    {
        public static bool IsEmpty(this DirectoryInfo srcDir)
            => srcDir.EnumerateFileSystemInfos().GetEnumerator().MoveNext();

        #region Copy
        public static DirectoryInfo CopyTo(this DirectoryInfo srcDir, string destPath,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath));
            if (!srcDir.Exists)
                throw new DirectoryNotFoundException("Source folder is not exist");
            var item = FileSystemInfoEx.GetFileSystemInfo(destPath);
            if (item.Exists)
            {
                if (!string.Equals(srcDir.FullName, item.FullName, StringComparison.OrdinalIgnoreCase))
                    switch (option)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            return srcDir.CopyTo(item.GetParent().FullName, ProductNextName(item.Name), option);
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete();
                            break;
                        case NameCollisionOption.FailIfExists:
                            var type = item is DirectoryInfo ? "srcDir" : "file";
                            throw new IOException($"A {type} exists in {destPath}");
                        default:
                            throw new ArgumentException("invalid option", nameof(option));
                    }
                else
                {
                    throw new IOException("Source and destination folders are same");
                }
            }

            var destFolder = new DirectoryInfo(destPath);
            destFolder.Create();
            var files = srcDir.EnumerateFiles();
            foreach (var file in files)
                file.CopyTo(destFolder);
            var subFolders = srcDir.EnumerateDirectories();
            foreach (var subFolder in subFolders)
                subFolder.CopyTo(destFolder);
            return destFolder;
        }

        public static DirectoryInfo CopyTo(this DirectoryInfo srcDir, string destFolderPath, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
                => srcDir.CopyTo(Path.Combine(destFolderPath, newName), nameCollisionOption);

        public static DirectoryInfo CopyTo(this DirectoryInfo srcDir, DirectoryInfo destFolder, string newName,
            NameCollisionOption option = NameCollisionOption.FailIfExists)
                => srcDir.CopyTo(destFolder.FullName, newName, option);

        public static DirectoryInfo CopyTo(this DirectoryInfo srcDir, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
                => srcDir.CopyTo(destFolder.FullName, srcDir.Name, nameCollisionOption);
        #endregion

        #region Move
        public static void Move(this DirectoryInfo srcDir, string destPath, 
            NameCollisionOption nameCollisionOption)
        {
            if (string.IsNullOrWhiteSpace(destPath))
                throw new ArgumentNullException(nameof(destPath));
            if (!srcDir.Exists)
                throw new DirectoryNotFoundException("Source folder is not exist");
            var item = FileSystemInfoEx.GetFileSystemInfo(destPath);
            if (item.Exists)
            {
                if (!string.Equals(srcDir.FullName, item.FullName, StringComparison.OrdinalIgnoreCase))
                    switch (nameCollisionOption)
                    {
                        case NameCollisionOption.GenerateUniqueName:
                            srcDir.Move(item.GetParent().FullName, 
                                ProductNextName(item.Name), nameCollisionOption);
                            return;
                        case NameCollisionOption.ReplaceExisting:
                            item.Delete();
                            break;
                        case NameCollisionOption.FailIfExists:
                            var type = item is DirectoryInfo ? "srcDir" : "file";
                            throw new IOException($"A {type} exists in {destPath}");
                        default:
                            throw new ArgumentException("invalid option", nameof(nameCollisionOption));
                    }
                else
                {
                    if (string.Equals(srcDir.FullName, item.FullName, StringComparison.Ordinal))
                        throw new IOException("Source and destination folders are same");
                    else
                    {
                        srcDir.Move(item.GetParent(), ProductNextName(srcDir.Name),
                            NameCollisionOption.GenerateUniqueName);
                        srcDir.Move(destPath, nameCollisionOption);
                        return;
                    }
                }
            }

            try
            {
                srcDir.MoveTo(destPath);
            }
            catch (IOException)
            {
                srcDir.CopyTo(destPath, nameCollisionOption);
                srcDir.Delete();
            }
        }

        public static void Move(this DirectoryInfo srcDir, string destFolderPath, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
                => srcDir.Move(Path.Combine(destFolderPath, newName), nameCollisionOption);

        public static void Move(this DirectoryInfo srcDir, DirectoryInfo destFolder, string newName,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
                => srcDir.Move(destFolder.FullName, newName, nameCollisionOption);

        public static void Move(this DirectoryInfo srcDir, DirectoryInfo destFolder,
            NameCollisionOption nameCollisionOption = NameCollisionOption.FailIfExists)
                => srcDir.Move(destFolder.FullName, srcDir.Name, nameCollisionOption);
        #endregion Move
    }
}
