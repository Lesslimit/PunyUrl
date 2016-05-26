using Microsoft.WindowsAzure.Storage.Table;

namespace PunyUrl.DataAccess.Azure
{
    public interface ITableStorage
    {
        IStorageTable<T> Table<T>(string name = null) where T : TableEntity, new();
    }
}