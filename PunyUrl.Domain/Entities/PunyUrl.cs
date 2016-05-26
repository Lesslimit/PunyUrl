using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PunyUrl.Domain.Attributes;
using PunyUrl.Utilities.Extensions;

namespace PunyUrl.Domain.Entities
{
    [JsonObject]
    [StorageTableName("PunyUrls")]
    public class PunyUrl : TableEntity
    {
        [JsonProperty]
        public string Original { get; set; }

        public PunyUrl(Uri url)
        {
            PartitionKey = url.PathAndQuery.Split('/').Skip(1).First();
            RowKey = url.AbsoluteUri.ToBase64();
        }

        public PunyUrl()
        {
        }
    }
}