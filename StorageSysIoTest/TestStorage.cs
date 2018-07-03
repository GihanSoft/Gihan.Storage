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
            var pastedFile = tempFile.Copy(new Folder(Environment.CurrentDirectory));
            Assert.True(SysIO.File.Exists(pastedFile.Path));
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
                pastedFile2 = tempFile.Copy(new Folder(Environment.CurrentDirectory), NameCollisionOption.FailIfExists);
            });
            Assert.Null(pastedFile2);
            if (pastedFile2 == null)
            {
                pastedFile2 = tempFile.Copy(new Folder(Environment.CurrentDirectory), NameCollisionOption.ReplaceExisting);
            }
            Assert.Equal(pastedFile.Name, pastedFile2.Name);
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
            Assert.True(SysIO.File.Exists(pastedFile.Path));
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
            if (SysIO.File.Exists(tempFile.Path))
                //////////////////
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
            Assert.Throws<SysIO.IOException>(() => {
                tempFile.Move(currentFolder, NameCollisionOption.FailIfExists);
            });
            tempFile.Move(currentFolder, NameCollisionOption.ReplaceExisting);
            Assert.False(SysIO.File.Exists(tempPath));
            Assert.True(SysIO.File.Exists(pastedFile.Path));
            SysIO.File.Copy(pastedFile.Path, tempPath);
            tempFile = new File(tempPath);
            tempFile.Move(currentFolder, NameCollisionOption.GenerateUniqueName);
            Assert.True(SysIO.File.Exists(pastedFile.Path));
            Assert.True(SysIO.File.Exists(tempFile.Path));
            Assert.NotEqual(pastedFile.Path, tempFile.Path);
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
        public void TestRename()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            tempFile.Rename("temp.file");
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
            tempFile.Rename("temp.file");
            tempFile.Rename("TeMp.file");
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
        public void TestReplace()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            //////////////////
            var tempFile = new File(tempPath);
            var tempPath2 = SysIO.Path.GetTempFileName();
            var tempFile2 = new File(tempPath2);
            tempFile2.Replace(tempFile);
            Assert.True(SysIO.File.Exists(tempPath));
            Assert.False(SysIO.File.Exists(tempPath2));
            //////////////////
            SysIO.File.Delete(tempPath);
        }
    }
}
