using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunyUrl.Services
{
    public interface IUrlStorage<TUrl>
    {
        Task<IList<TUrl>> AllAsync();
        Task<TUrl> AddOrUpdateAsync(Uri original, Uri puny);
        Task<TUrl> GetAsync(Uri url);
    }
}