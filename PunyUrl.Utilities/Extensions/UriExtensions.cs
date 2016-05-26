using System;
using System.Linq;

namespace PunyUrl.Utilities.Extensions
{
    public static class UriExtensions
    {
        public static string GetTopLevelDomain(this Uri uri)
        {
            return uri.Host.Split('.').Last();
        }
    }
}