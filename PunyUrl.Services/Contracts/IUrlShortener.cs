using System;
using System.Threading.Tasks;

namespace PunyUrl.Services
{
    public interface IUrlShortener
    {
        Task<Uri> ProcessAsync(Uri url);
    }
}