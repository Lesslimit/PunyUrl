using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PunyUrl.DataAccess.Azure
{
    public class StorageTable<T> : IStorageTable<T> where T : TableEntity, new()
    {
        private readonly CloudTable cloudTable;

        public StorageTable(CloudTable cloudTable)
        {
            this.cloudTable = cloudTable;
        }

        public ITableStorageQuery<T> Query()
        {
            var tableQuery = cloudTable.CreateQuery<T>();

            return new TableStorageQuery<T>(tableQuery);
        }

        public async Task CreateIfNotExistsAsync()
        {
            await cloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        public async Task<T> InsertAsync(T entity)
        {
            var tableResult = await cloudTable.ExecuteAsync(TableOperation.Insert(entity, true))
                                              .ConfigureAwait(false);

            return (T)tableResult.Result;
        }

        public async Task InsertBatchAsync(IEnumerable<T> entities)
        {
            var batch = new TableBatchOperation();

            foreach (var entity in entities)
            {
                batch.Add(TableOperation.Insert(entity, false));
            }

            await cloudTable.ExecuteBatchAsync(batch)
                            .ConfigureAwait(false);
        }

        public async Task UpdateAsync(T entity)
        {
            await cloudTable.ExecuteAsync(TableOperation.Merge(entity))
                            .ConfigureAwait(false);
        }

        public async Task<TableResult> InsertOrUpdateAsync(T entity)
        {
            var tableResult = await cloudTable.ExecuteAsync(TableOperation.InsertOrMerge(entity))
                                              .ConfigureAwait(false);

            return tableResult;
        }
    }
}