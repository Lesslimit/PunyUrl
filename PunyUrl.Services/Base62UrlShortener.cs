using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using PunyUrl.Utilities.Extensions;

namespace PunyUrl.Services
{
    public class Base62UrlShortener : IUrlShortener
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int Base = 62;

        private readonly IUrlMetadataProvider metadataProvider;
        private readonly string baseUrl;

        public Base62UrlShortener(IUrlMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;
            baseUrl = ConfigurationManager.AppSettings[nameof(baseUrl)];
        }

        public async Task<Uri> ProcessAsync(Uri url)
        {
            var topDomain = url.GetTopLevelDomain();

            int count = await metadataProvider.CountForDomainAsync(topDomain)
                                              .ConfigureAwait(false);

            var encode = Encode(count);

            return new Uri(new Uri(baseUrl), $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(topDomain))}/{encode}");
        }

        private static string Encode(int num)
        {
            var sb = new StringBuilder();

            if (num < 0)
            {
                throw new ArgumentException(nameof(num));
            }

            if (num < 61)
            {
                return Alphabet[num].ToString();
            }

            while (num > 0)
            {
                sb.Append(Alphabet[(num % Base)]);
                num /= Base;
            }

            var builder = new StringBuilder();
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                builder.Append(sb[i]);
            }

            return builder.ToString();
        }
    }
}