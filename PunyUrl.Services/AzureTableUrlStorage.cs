using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PunyUrl.DataAccess.Azure;
using PunyUrl.Utilities.Extensions;

namespace PunyUrl.Services
{
    public class AzureTableUrlStorage : IUrlStorage<Domain.Entities.PunyUrl> 
    {
        private readonly ITableStorage tableStorage;

        public AzureTableUrlStorage(ITableStorage tableStorage)
        {
            this.tableStorage = tableStorage;
        }

        public async Task<IList<Domain.Entities.PunyUrl>> AllAsync()
        {
            return (await tableStorage.Table<Domain.Entities.PunyUrl>()
                                      .Query()
                                      .ToListAsync()
                                      .ConfigureAwait(false));
        }

        public async Task<Domain.Entities.PunyUrl> AddOrUpdateAsync(Uri original, Uri puny)
        {
            var urlEntity = new Domain.Entities.PunyUrl(puny)
            {
                Original = original.AbsoluteUri
            };

            var result = await tableStorage.Table<Domain.Entities.PunyUrl>()
                                            .InsertOrUpdateAsync(urlEntity)
                                            .ConfigureAwait(false);

            return (Domain.Entities.PunyUrl) result.Result;
        }

        public async Task<Domain.Entities.PunyUrl> GetAsync(Uri url)
        {
            var topDomain = url.PathAndQuery.Split('/').Skip(1).First();
            var rowKey = url.AbsoluteUri.ToBase64();

            return await tableStorage.Table<Domain.Entities.PunyUrl>()
                                     .Query()
                                     .Where(pu => pu.PartitionKey == topDomain &&
                                                  pu.RowKey == rowKey)
                                     .FirstOrDefaultAsync()
                                     .ConfigureAwait(false);
        }
    }
}