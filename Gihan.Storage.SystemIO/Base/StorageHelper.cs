using Gihan.Storage.Core.Base;

namespace Gihan.Storage.SystemIO.Base
{
    public class StorageHelper : Core.Base.StorageHelper
    {
        static StorageHelper()
        {
            Init();
        }
        public static void Init()
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
            if (!Exist(path)) return null;
            return FolderExist(path) ? new Folder(path) : new File(path) as IStorageItem;
        }
    }
}
