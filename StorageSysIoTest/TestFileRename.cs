using System;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO;
using Xunit;
//################################################################
using static System.IO.Path;
using IO = System.IO;

namespace StorageSysIoTest
{
    public class TestFileRename
    {
        [Fact]
        public void TestNormalRename_FailIfExists()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            var tempPathParent = GetDirectoryName(tempFilePath);

            string newName;
            string newPath;
            do
            {
                newName = Guid.NewGuid().ToString();
                newPath = Combine(tempPathParent, newName);
            } while (IO.File.Exists(newPath));

            var file = new File(tempFilePath);

            file.Rename(newName);

            Assert.False(IO.File.Exists(tempFilePath));
            Assert.True(IO.File.Exists(newPath));

            IO.File.Move(newPath, tempFilePath);
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestNormalRename_GenerateUniqueName()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            var tempPathParent = GetDirectoryName(tempFilePath);

            string newName;
            string newPath;
            do
            {
                newName = Guid.NewGuid().ToString();
                newPath = Combine(tempPathParent, newName);
            } while (IO.File.Exists(newPath));

            var file = new File(tempFilePath);

            file.Rename(newName, NameCollisionOption.GenerateUniqueName);

            Assert.False(IO.File.Exists(tempFilePath));
            Assert.True(IO.File.Exists(newPath));

            IO.File.Move(newPath, tempFilePath);
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestNormalRename_ReplaceExisting()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            var tempPathParent = GetDirectoryName(tempFilePath);

            string newName;
            string newPath;
            do
            {
                newName = Guid.NewGuid().ToString();
                newPath = Combine(tempPathParent, newName);
            } while (IO.File.Exists(newPath));

            var file = new File(tempFilePath);

            file.Rename(newName, NameCollisionOption.ReplaceExisting);

            Assert.False(IO.File.Exists(tempFilePath));
            Assert.True(IO.File.Exists(newPath));

            IO.File.Move(newPath, tempFilePath);
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        //=====================================================

        [Fact]
        public void TestCollisionExisting_FailIfExists()
        {
            var tempPath1 = GetTempFileName();
            var tempPath2 = GetTempFileName();
            //-------------------------------------

            var file1 = new File(tempPath1);
            var file2Name = GetFileName(tempPath2);

            Assert.ThrowsAny<Exception>(() => file1.Rename(file2Name));

            //-------------------------------------
            IO.File.Delete(tempPath1);
            IO.File.Delete(tempPath2);
        }

        [Fact]
        public void TestCollisionExisting_GenerateUniqueName()
        {
            var tempPath1 = GetTempFileName();
            var tempPath2 = GetTempFileName();
            //-------------------------------------

            var file1 = new File(tempPath1);
            var file2Name = GetFileName(tempPath2);

            file1.Rename(file2Name, NameCollisionOption.GenerateUniqueName);

            Assert.False(IO.File.Exists(tempPath1));
            Assert.True(IO.File.Exists(tempPath2));
            var tempPathNewName = Combine(GetTempPath(),
                GetFileNameWithoutExtension(tempPath2) + "(2)" + GetExtension(tempPath2));
            Assert.True(IO.File.Exists(tempPathNewName));

            //-------------------------------------
            //IO.File.Delete(tempPath1);
            IO.File.Delete(tempPath2);
            IO.File.Delete(tempPathNewName);
        }

        [Fact]
        public void TestCollisionExisting_ReplaceExisting()
        {
            var tempPath1 = GetTempFileName();
            var tempPath2 = GetTempFileName();
            //-------------------------------------

            var file1 = new File(tempPath1);
            var file1Name = GetFileName(tempPath1);
            var file2Name = GetFileName(tempPath2);

            file1.Rename(file2Name, NameCollisionOption.ReplaceExisting);

            Assert.False(IO.File.Exists(tempPath1));
            Assert.True(IO.File.Exists(tempPath2));

            file1.Rename(file1Name);

            //-------------------------------------
            IO.File.Delete(tempPath2);
        }

        [Fact]
        public void TestCollisionExisting_GenerateUniqueName_Multi()
        {
            var baseFile = GetTempFileName();
            var baseFileName = GetFileName(baseFile);
            var baseFilePureName = GetFileNameWithoutExtension(baseFile);
            var baseFileExtension = GetExtension(baseFile);
            //var tempFileArrray = new string[10];
            const int n = 10;
            for (var i = 0; i < n; i++)
            {
                var temp = GetTempFileName();
                var file = new File(temp);
                file.Rename(baseFileName, NameCollisionOption.GenerateUniqueName);
            }

            Assert.True(IO.File.Exists(baseFile));
            for (var index = 0; index < n; index++)
            {
                var filePath = Combine(GetTempPath(), $"{baseFilePureName}({index + 2}){baseFileExtension}");
                Assert.True(IO.File.Exists(filePath));
                IO.File.Delete(filePath);
            }

            IO.File.Delete(baseFile);
        }

        //=====================================================

        [Fact]
        public void TestIncludeDirectoryInName_FailIfExists()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string dirPath;
            do
            {
                dirPath = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), dirPath)));

            var newName = Combine(dirPath, GetFileName(tempFilePath));
            var newPath = Combine(GetTempPath(), newName);
            var file = new File(tempFilePath);
            Assert.ThrowsAny<ArgumentException>(() => file.Rename(newName));
            Assert.True(IO.File.Exists(tempFilePath));
            Assert.False(IO.File.Exists(newPath));
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestIncludeDirectoryInName_GenerateUniqueName()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string dirPath;
            do
            {
                dirPath = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), dirPath)));

            var newName = Combine(dirPath, GetFileName(tempFilePath));
            var newPath = Combine(GetTempPath(), newName);
            var file = new File(tempFilePath);
            Assert.ThrowsAny<ArgumentException>(() => file.Rename(newName,
                NameCollisionOption.GenerateUniqueName));
            Assert.True(IO.File.Exists(tempFilePath));
            Assert.False(IO.File.Exists(newPath));
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestIncludeDirectoryInName_ReplaceExisting()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string dirPath;
            do
            {
                dirPath = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), dirPath)));

            var newName = Combine(dirPath, GetFileName(tempFilePath));
            var newPath = Combine(GetTempPath(), newName);
            var file = new File(tempFilePath);
            Assert.ThrowsAny<ArgumentException>(() => file.Rename(newName,
                NameCollisionOption.ReplaceExisting));
            Assert.True(IO.File.Exists(tempFilePath));
            Assert.False(IO.File.Exists(newPath));
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        //=====================================================

        [Fact]
        public void TestInvalidChar_FailIfExists()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string newName;
            do
            {
                newName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), newName)));

            var file = new File(tempFilePath);
            foreach (var invalidFileNameChar in GetInvalidFileNameChars())
            {
                Assert.ThrowsAny<ArgumentException>(() => file.Rename(newName + invalidFileNameChar));
            }
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestInvalidChar_GenerateUniqueName()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string newName;
            do
            {
                newName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), newName)));

            var file = new File(tempFilePath);
            foreach (var invalidFileNameChar in GetInvalidFileNameChars())
            {
                Assert.ThrowsAny<ArgumentException>(() =>
                    file.Rename(newName + invalidFileNameChar, NameCollisionOption.GenerateUniqueName));
            }
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestInvalidChar_ReplaceExisting()
        {
            var tempFilePath = GetTempFileName();
            //-------------------------------------
            string newName;
            do
            {
                newName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(GetTempPath(), newName)));

            var file = new File(tempFilePath);
            foreach (var invalidFileNameChar in GetInvalidFileNameChars())
            {
                Assert.ThrowsAny<ArgumentException>(() =>
                    file.Rename(newName + invalidFileNameChar, NameCollisionOption.ReplaceExisting));
            }
            //-------------------------------------
            IO.File.Delete(tempFilePath);
        }

        //=====================================================

        [Fact]
        public void TestMissingSource_FailIfExists()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            string desiredFileName;
            do
            {
                desiredFileName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(tempDir, desiredFileName)));
            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() => file.Rename(desiredFileName));
        }

        [Fact]
        public void TestMissingSource_GenerateUniqueName()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            string desiredFileName;
            do
            {
                desiredFileName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(tempDir, desiredFileName)));
            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() =>
                file.Rename(desiredFileName, NameCollisionOption.GenerateUniqueName));
        }

        [Fact]
        public void TestMissingSource_ReplaceExisting()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            string desiredFileName;
            do
            {
                desiredFileName = Guid.NewGuid().ToString();
            } while (IO.File.Exists(Combine(tempDir, desiredFileName)));
            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() =>
                file.Rename(desiredFileName, NameCollisionOption.ReplaceExisting));
        }

        //=====================================================

        [Fact]
        public void TestMissingSourceCollision_FailIfExists()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            var tempFilePath = GetTempFileName();
            var desiredFileName = GetFileName(tempFilePath);

            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() => file.Rename(desiredFileName));
            Assert.True(IO.File.Exists(tempFilePath));
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestMissingSourceCollision_GenerateUniqueName()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            var tempFilePath = GetTempFileName();
            var desiredFileName = GetFileName(tempFilePath);

            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() =>
                file.Rename(desiredFileName, NameCollisionOption.GenerateUniqueName));
            Assert.True(IO.File.Exists(tempFilePath));
            IO.File.Delete(tempFilePath);
        }

        [Fact]
        public void TestMissingSourceCollision_ReplaceExisting()
        {
            var tempDir = GetTempPath();
            string missedPath;
            do
            {
                var missedFile = Guid.NewGuid().ToString();
                missedPath = Combine(tempDir, missedFile);
            } while (IO.File.Exists(missedPath));

            var tempFilePath = GetTempFileName();
            var desiredFileName = GetFileName(tempFilePath);

            var file = new File(missedPath);
            Assert.ThrowsAny<Exception>(() =>
                file.Rename(desiredFileName, NameCollisionOption.ReplaceExisting));
            Assert.True(IO.File.Exists(tempFilePath));
            IO.File.Delete(tempFilePath);
        }

        //todo تست همه موارد موجود برای تغییرنام بدون پسوند

    }
}
