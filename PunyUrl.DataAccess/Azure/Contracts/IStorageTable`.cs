using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PunyUrl.DataAccess.Azure
{
    public interface IStorageTable<T> where T : TableEntity
    {
        ITableStorageQuery<T> Query();

        Task CreateIfNotExistsAsync();

        Task<T> InsertAsync(T entity);

        Task InsertBatchAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task<TableResult> InsertOrUpdateAsync(T entity);
    }
}