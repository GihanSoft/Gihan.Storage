namespace Gihan.Storage.Core.Base
{
    public abstract class StorageHelper
    {
        protected delegate StorageHelper StorageHelperCreator();
        protected static StorageHelperCreator Ctor { get; set; }

        public abstract bool Exist(string path);
        public abstract bool FileExist(string path);
        public abstract bool FolderExist(string path);

        public abstract IStorageItem GetItem(string path);

        public static StorageHelper Creat()
        {
            return Ctor.Invoke();
        }
    }
}
