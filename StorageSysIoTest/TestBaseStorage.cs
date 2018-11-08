using System;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO;
using Xunit;
//##########################################################################
using static System.IO.Path;
using IO = System.IO;

namespace StorageSysIoTest
{
    public class TestBaseStorage
    {
        [Fact]
        public void TestFilePath()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            var file = new File(tempPath);
            Assert.Equal(tempPath, file.Path);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }

        [Fact]
        public void TestFolderPath()
        {
            var currentFolder = Environment.CurrentDirectory;
            var folder = new Folder(currentFolder);
            Assert.Equal(currentFolder, folder.Path);
        }

        [Fact]
        public void TestFileName()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            var file = new File(tempPath);
            Assert.Equal(GetFileName(tempPath), file.Name);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }

        [Fact]
        public void TestFolderName()
        {
            var currentFolder = Environment.CurrentDirectory;
            var folder = new Folder(currentFolder);
            Assert.Equal(GetFileName(currentFolder), folder.Name);
        }

        [Fact]
        public void TestFileParent()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            var file = new File(tempPath);
            Assert.Equal(GetDirectoryName(tempPath), file.Parent.Path);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }

        [Fact]
        public void TestFolderParent()
        {
            var currentFolder = Environment.CurrentDirectory;
            var folder = new Folder(currentFolder);
            Assert.Equal(GetDirectoryName(currentFolder), folder.Parent.Path);
        }

        [Fact]
        public void TestFileType()
        {
            var tempPath = GetTempFileName();
            //-------------------------------------
            var file = new File(tempPath);
            Assert.Equal(StorageItemType.File, file.Type);
            //-------------------------------------
            IO.File.Delete(tempPath);
        }

        [Fact]
        public void TestFolderType()
        {
            var currentFolder = Environment.CurrentDirectory;
            var folder = new Folder(currentFolder);
            Assert.Equal(StorageItemType.Folder, folder.Type);
        }

        [Fact]
        public void TestExist()
        {
            string path;
            do
            {
                path = @"C:\" + Guid.NewGuid();
            } while (IO.File.Exists(path));
            var file = new File(path);
            var folder = new Folder(path);
            Assert.False(file.Exist);
            Assert.False(folder.Exist);
        }

        //[Fact]
        //public void TestCheckExist()
        //{
        //    string path;
        //    do
        //    {
        //        path = @"C:\" + Guid.NewGuid();
        //    } while (IO.File.Exists(path));
        //    Assert.False(new File(path).CheckExist(path));
        //}

        [Fact]
        public void TestFileDelete()
        {
            var tempPath = GetTempFileName();
            var file = new File(tempPath);
            file.Delete();
            Assert.False(IO.File.Exists(tempPath));
        }

        [Fact]
        public void TestFolderDelete()
        {
            var tempPathParent = GetTempPath();
            string tempPath;
            do
            {
                tempPath = Combine(tempPathParent, Guid.NewGuid().ToString());
            } while (IO.Directory.Exists(tempPath));

            IO.Directory.CreateDirectory(tempPath);

            var folder = new Folder(tempPath);
            folder.Delete();

            Assert.False(IO.File.Exists(tempPath));
        }
    }
}
