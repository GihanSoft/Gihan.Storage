using System;
using Gihan.Storage.SystemIO;
using Xunit;
//#############################################################
using static System.IO.Path;
using IO = System.IO;

namespace StorageSysIoTest
{
    public class TestFile
    {
        [Fact]
        public void TestPureName()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------------------
            var file = new File(tempFilePath);
            Assert.Equal(GetFileNameWithoutExtension(tempFilePath), file.PureName);
            //-------------------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestExtension()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------------------
            var file = new File(tempFilePath);
            Assert.Equal(GetExtension(tempFilePath), file.Extension);
            //-------------------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestExtension_FileHasNotExtension()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------------------
            var withoutExtensionFile = Combine(GetTempPath(), GetFileNameWithoutExtension(tempFilePath));
            IO.File.Move(tempFilePath, withoutExtensionFile);
            var file = new File(withoutExtensionFile);
            Assert.Equal("", file.Extension);
            IO.File.Move(withoutExtensionFile, tempFilePath);
            //-------------------------------------------------
            IO.File.Delete(tempFilePath);
        }

        //todo چک فایل بدون پسوند

        //[Fact]
        //public void TestCheckExistFile()
        //{
        //    var tempFilePath = GetTempFileName();
        //    //-------------------------------------------------
        //    var file = new File(tempFilePath);
        //    Assert.True(file.CheckExistFile(tempFilePath));
        //    IO.File.Delete(tempFilePath);
        //    Assert.False(file.CheckExistFile(tempFilePath));
        //}

        //[Fact]
        //public void TestCheckExistFile_FolderPathInput()
        //{
        //    string notExistPath;
        //    do
        //    {
        //        notExistPath = Combine(GetTempPath(), Guid.NewGuid().ToString());
        //    } while (IO.File.Exists(notExistPath));

        //    var file = new File(notExistPath);
        //    Assert.False(file.CheckExistFile(GetTempPath()));
        //    Assert.False(file.CheckExistFile(notExistPath));

        //}
    }
}
