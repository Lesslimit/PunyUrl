using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;
using Nancy.Helpers;
using PunyUrl.Services;

namespace PunyUrl.Middleware
{
    public class RedirectMiddleware : OwinMiddleware
    {
        private readonly IUrlStorage<Domain.Entities.PunyUrl> urlStorage;

        public RedirectMiddleware(OwinMiddleware next,
                                  IUrlStorage<Domain.Entities.PunyUrl> urlStorage) : base(next)
        {
            this.urlStorage = urlStorage;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var url = new Uri(HttpUtility.UrlDecode(context.Request.Uri.AbsoluteUri));

            var punyUrl = await urlStorage.GetAsync(url).ConfigureAwait(false);

            if (punyUrl != default(Domain.Entities.PunyUrl))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Redirect;
                context.Response.Headers.Set(nameof(HttpResponseHeader.Location), punyUrl.Original);
            }
            else
            {
                await Next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}