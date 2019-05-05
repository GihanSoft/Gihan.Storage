using Gihan.Storage.Core.Base;
using Gihan.Storage.Core.Enums;
using Gihan.Storage.SystemIO;
using Gihan.Storage.SystemIO.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using StorageHelper = Gihan.Storage.SystemIO.Base.StorageHelper;
using SysIO = System.IO;

namespace SysIoTest
{
    public class TestStorageHelper
    {
        [SetUp]
        public void Setup()
        {
            StorageHelper.Init();
        }

        [Test]
        public void TestGetItemFile()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            //----------------------------------------------------------------------
            var file = helper.GetItem(tempPath);
            Assert.AreEqual(StorageItemType.File, file.Type);
            Assert.IsTrue(file.Exist);
            //----------------------------------------------------------------------
            SysIO.File.Delete(tempPath);
        }
        [Test]
        public void TestGetItemFolder()
        {
            var tempPath = SysIO.Path.GetTempPath();
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            //----------------------------------------------------------------------
            var folder = helper.GetItem(tempPath);
            Assert.AreEqual(StorageItemType.Folder, folder.Type);
            Assert.IsTrue(folder.Exist);
            //----------------------------------------------------------------------
        }

        [Test]
        public void TestExistFile()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            //----------------------------------------------------------------------
            Assert.IsTrue(helper.Exist(tempPath));
            SysIO.File.Delete(tempPath);
            Assert.IsFalse(helper.Exist(tempPath));
            //----------------------------------------------------------------------
        }
        [Test]
        public void TestExistFolder()
        {
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            var tempPathParent = SysIO.Path.GetTempPath();
            string tempPath;
            do
            {
                tempPath = SysIO.Path.Combine(tempPathParent, Guid.NewGuid().ToString());
            } while (SysIO.Directory.Exists(tempPath));
            //----------------------------------------------------------------------
            SysIO.Directory.CreateDirectory(tempPath);
            Assert.IsTrue(helper.Exist(tempPath));
            SysIO.Directory.Delete(tempPath);
            Assert.IsFalse(helper.Exist(tempPath));
            //----------------------------------------------------------------------
        }

        [Test]
        public void TestFileExist()
        {
            var tempPath = SysIO.Path.GetTempFileName();
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            //----------------------------------------------------------------------
            Assert.IsTrue(helper.FileExist(tempPath));
            Assert.IsFalse(helper.FolderExist(tempPath));
            SysIO.File.Delete(tempPath);
            Assert.IsFalse(helper.FileExist(tempPath));
            //----------------------------------------------------------------------
        }
        [Test]
        public void TestFolderExist()
        {
            var helper = Gihan.Storage.Core.Base.StorageHelper.Creat();
            var tempPathParent = SysIO.Path.GetTempPath();
            string tempPath;
            do
            {
                tempPath = SysIO.Path.Combine(tempPathParent, Guid.NewGuid().ToString());
            } while (SysIO.Directory.Exists(tempPath));
            //----------------------------------------------------------------------
            SysIO.Directory.CreateDirectory(tempPath);
            Assert.IsTrue(helper.FolderExist(tempPath));
            Assert.IsFalse(helper.FileExist(tempPath));
            SysIO.Directory.Delete(tempPath);
            Assert.IsFalse(helper.FolderExist(tempPath));
            //----------------------------------------------------------------------
        }
    }
}
