using System.Threading.Tasks;
using PunyUrl.DataAccess.Azure;
using PunyUrl.Utilities.Extensions;

namespace PunyUrl.Services
{
    public interface IUrlMetadataProvider
    {
        Task<int> CountForDomainAsync(string domain);
    }

    public class AzureTableUrlMetadataProvider : IUrlMetadataProvider
    {
        private readonly ITableStorage tableStorage;

        public AzureTableUrlMetadataProvider(ITableStorage tableStorage)
        {
            this.tableStorage = tableStorage;
        }

        public async Task<int> CountForDomainAsync(string domain)
        {
            domain = domain.ToBase64();

            return await tableStorage.Table<Domain.Entities.PunyUrl>()
                                     .Query()
                                     .Where(ue => ue.PartitionKey == domain)
                                     .CountAsync()
                                     .ConfigureAwait(false);
        }
    }
}