using Gihan.Storage.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gihan.Storage.SystemIO.Base
{
    public class StorageHelper : Core.Base.StorageHelper
    {
        static StorageHelper()
        {
            Ctor = () => new StorageHelper();
        }

        public override bool Exist(string path)
        {
            return System.IO.File.Exists(path) || System.IO.Directory.Exists(path);
        }

        public override bool FileExist(string path)
        {
            return System.IO.File.Exists(path) && !System.IO.Directory.Exists(path);
        }

        public override bool FolderExist(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public override IStorageItem GetItem(string path)
        {
            return FolderExist(path) ? new Folder(path) as IStorageItem : new File(path);
        }
    }
}
