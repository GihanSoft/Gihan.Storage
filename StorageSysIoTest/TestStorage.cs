using System;
using SysIO = System.IO;
using Xunit;
using Gihan.Storage.SystemIO;
using Gihan.Storage.Core;
using Gihan.Storage.Core.Enums;

namespace StorageSysIoTest
{
    public class TestStorage
    {
        [Fact]
        public void TestCopy()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            var pastedFile = tempFile.Copy(new Folder(Environment.CurrentDirectory), NameCollisionOption.ReplaceExisting);
            Assert.True(SysIO.File.Exists(SysIO.Path.Combine(Environment.CurrentDirectory, SysIO.Path.GetFileName(tempPath))));
            SysIO.File.Delete(pastedFile.Path);
            //////////////////
            SysIO.File.Delete(tempPath);
        }
        [Fact]
        public void TestCopyExistConflict()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            var pastedFile = tempFile.Copy(new Folder(Environment.CurrentDirectory));
            IFile pastedFile2 = null;

            Assert.Throws<SysIO.IOException>(() =>
            {
                pastedFile2 = tempFile.Copy(new Folder(Environment.CurrentDirectory));
            });
            Assert.Null(pastedFile2);
            pastedFile2 = tempFile.Copy(new Folder(Environment.CurrentDirectory), NameCollisionOption.ReplaceExisting);
            Assert.Equal(pastedFile.Path, pastedFile2.Path);
            pastedFile2 = tempFile.Copy(new Folder(Environment.CurrentDirectory), NameCollisionOption.GenerateUniqueName);

            SysIO.File.Delete(pastedFile.Path);
            if (pastedFile2 != null)
                SysIO.File.Delete(pastedFile2.Path);
            //////////////////
            SysIO.File.Delete(tempPath);
        }
        [Fact]
        public void TestCopyNewName()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            var pastedFile = tempFile.Copy(new Folder(Environment.CurrentDirectory), "temp.file");
            Assert.True(SysIO.File.Exists(SysIO.Path.Combine(Environment.CurrentDirectory, "temp.file")));
            SysIO.File.Delete(pastedFile.Path);
            //////////////////
            SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void TestDelete()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Delete();
            Assert.False(SysIO.File.Exists(tempFile.Path));
            //////////////////
            if (SysIO.File.Exists(tempFile.Path))
                SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void TestMove()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Move(new Folder(Environment.CurrentDirectory));
            Assert.False(SysIO.File.Exists(tempPath));
            Assert.True(SysIO.File.Exists(SysIO.Path.Combine(Environment.CurrentDirectory, SysIO.Path.GetFileName(tempPath))));
            SysIO.File.Delete(tempFile.Path);
            //////////////////
            //SysIO.File.Delete(tempPath);
        }
        [Fact]
        public void TestMoveExistConflict()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            var currentFolder = new Folder(Environment.CurrentDirectory);
            var pastedFile = tempFile.Copy(currentFolder);
            Assert.Throws<SysIO.IOException>(() =>
            {
                tempFile.Move(currentFolder);
            });
            tempFile.Move(currentFolder, NameCollisionOption.ReplaceExisting);
            Assert.False(SysIO.File.Exists(tempPath));
            Assert.True(SysIO.File.Exists(SysIO.Path.Combine(Environment.CurrentDirectory, SysIO.Path.GetFileName(tempPath))));
            SysIO.File.Copy(pastedFile.Path, tempPath); //make source temp file
            tempFile = new File(tempPath);
            tempFile.Move(currentFolder, NameCollisionOption.GenerateUniqueName);
            Assert.True(SysIO.File.Exists(pastedFile.Path));
            Assert.True(SysIO.File.Exists(tempFile.Path));
            Assert.NotEqual(tempFile.Path, pastedFile.Path);
            Assert.Equal(pastedFile.Parent.Path, tempFile.Parent.Path);
            pastedFile.Delete();
            tempFile.Delete();
            //////////////////
            //SysIO.File.Delete(tempPath);
        }
        [Fact]
        public void TestMoveNewName()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Move(new Folder(Environment.CurrentDirectory), "temp.file");
            Assert.True(SysIO.File.Exists(SysIO.Path.Combine(Environment.CurrentDirectory, "temp.file")));
            Assert.False(SysIO.File.Exists(tempPath));
            SysIO.File.Delete(tempFile.Path);
            //////////////////
            //SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void TestMoveSourceNotExist()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            SysIO.File.Delete(tempPath);
            Assert.Throws<SysIO.FileNotFoundException>(() => tempFile.Move(Environment.CurrentDirectory, "temp.file"));
        }

        [Fact]
        public void TestRename()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Rename("temp.file", NameCollisionOption.ReplaceExisting);
            Assert.False(SysIO.File.Exists(tempPath));
            var newName = SysIO.Path.Combine(SysIO.Path.GetDirectoryName(tempPath), "temp.file");
            Assert.True(SysIO.File.Exists(newName));
            tempPath = tempFile.Path;
            //////////////////
            SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void TestCaseSensetiveRename()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Rename("temp.file", NameCollisionOption.ReplaceExisting);
            tempFile.Rename("TeMp.file", NameCollisionOption.ReplaceExisting);
            Assert.False(SysIO.File.Exists(tempPath));
            var newName = SysIO.Path.Combine(SysIO.Path.GetDirectoryName(tempPath), "TeMp.file");
            Assert.True(SysIO.File.Exists(newName));
            tempPath = tempFile.Path;
            //////////////////
            SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void TestRenameIgnoreExtension()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.RenameIgnoreExtension("temp.file");
            Assert.False(SysIO.File.Exists(tempPath));
            var newName = SysIO.Path.Combine(SysIO.Path.GetDirectoryName(tempPath), "temp.file" + ".tmp");
            Assert.True(SysIO.File.Exists(newName));
            tempPath = tempFile.Path;
            //////////////////
            SysIO.File.Delete(tempPath);
        }

        [Fact]
        public void ChangingPath()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var file = new File(tempPath);
            var path = SysIO.Path.Combine(Environment.CurrentDirectory, "temp.tmp");
            file.Move(path, NameCollisionOption.ReplaceExisting);
            Assert.False(SysIO.File.Exists(tempPath));
            Assert.True(SysIO.File.Exists(path));
            SysIO.File.Delete(path);
        }
    }
}
