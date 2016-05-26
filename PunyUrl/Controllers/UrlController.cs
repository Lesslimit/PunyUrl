using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PunyUrl.Domain.Entities;
using PunyUrl.Services;
using PunyUrl.Utilities.Extensions;

namespace PunyUrl.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("v1.0/api/url")]
    public class UrlController : ApiController
    {
        private readonly IUrlStorage<Domain.Entities.PunyUrl> storage;
        private readonly IUrlShortener shortener;

        public UrlController(IUrlStorage<Domain.Entities.PunyUrl> storage,
                             IUrlShortener shortener)
        {
            this.storage = storage;
            this.shortener = shortener;
        }

        [Route("all")]
        public async Task<IHttpActionResult> GetLink()
        {
            var urls = await storage.AllAsync();

            return Json(urls.Select(pu => new
            {
                topDomain = pu.PartitionKey.FromBase64(),
                puny = pu.RowKey.FromBase64(),
                original = pu.Original
            }));
        }

        [Route("create")]
        public async Task<IHttpActionResult> PostLink([FromUri]SmartUrl url)
        {
            if (url.IsEmpty)
            {
                return BadRequest("Emtpy/Bad Url");
            }

            Uri PunyUrl = await shortener.ProcessAsync(url);
            Domain.Entities.PunyUrl punyUrl = await storage.AddOrUpdateAsync(url, PunyUrl);

            return Ok(new
            {
                topDomain = punyUrl.PartitionKey.FromBase64(),
                puny = punyUrl.RowKey.FromBase64(),
                original = punyUrl.Original
            });
        }
    }
}