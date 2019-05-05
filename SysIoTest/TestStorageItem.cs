using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO;
using NUnit.Framework;
using System;

using static System.IO.Path;
using IO = System.IO;

namespace SysIoTest
{
    public class TestStorageItem
    {
        [Test]
        public void TestFilePath()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            IStorageItem file = new File(tempPath);
            Assert.AreEqual(tempPath, file.Path);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }
        [Test]
        public void TestFolderPath()
        {
            var currentFolder = Environment.CurrentDirectory;
            IStorageItem folder = new Folder(currentFolder);
            Assert.AreEqual(currentFolder, folder.Path);
        }

        [Test]
        public void TestFileName()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            IStorageItem file = new File(tempPath);
            Assert.AreEqual(GetFileName(tempPath), file.Name);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }
        [Test]
        public void TestFolderName()
        {
            var currentFolder = Environment.CurrentDirectory;
            IStorageItem folder = new Folder(currentFolder);
            Assert.AreEqual(GetFileName(currentFolder), folder.Name);
        }

        [Test]
        public void TestFileParent()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            IStorageItem file = new File(tempPath);
            Assert.AreEqual(GetDirectoryName(tempPath), file.Parent.Path);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }
        [Test]
        public void TestFolderParent()
        {
            var currentFolder = Environment.CurrentDirectory;
            IStorageItem folder = new Folder(currentFolder);
            Assert.AreEqual(GetDirectoryName(currentFolder), folder.Parent.Path);
        }

        [Test]
        public void TestFileType()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            IStorageItem file = new File(tempPath);
            Assert.AreEqual(StorageItemType.File, file.Type);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }
        [Test]
        public void TestFolderType()
        {
            var currentFolder = Environment.CurrentDirectory;
            IStorageItem folder = new Folder(currentFolder);
            Assert.AreEqual(StorageItemType.Folder, folder.Type);
        }

        [Test]
        public void TestExist()
        {
            string path;
            do
            {
                path = @"C:\" + Guid.NewGuid();
            } while (IO.File.Exists(path));
            IStorageItem file = new File(path);
            IStorageItem folder = new Folder(path);
            Assert.False(file.Exist);
            Assert.False(folder.Exist);
        }

        [Test]
        public void TestFileDelete()
        {
            var tempPath = GetTempFileName();
            IStorageItem file = new File(tempPath);
            file.Delete();
            Assert.False(IO.File.Exists(tempPath));
        }
        [Test]
        public void TestFolderDelete()
        {
            var tempPathParent = GetTempPath();
            string tempPath;
            do
            {
                tempPath = Combine(tempPathParent, Guid.NewGuid().ToString());
            } while (IO.Directory.Exists(tempPath));

            IO.Directory.CreateDirectory(tempPath);

            IStorageItem folder = new Folder(tempPath);
            folder.Delete();

            Assert.False(IO.File.Exists(tempPath));
        }
    }
}
