using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Table;
using PunyUrl.Domain.Attributes;

namespace PunyUrl.DataAccess.Azure
{
     public class TableStorage : ITableStorage
    {
        private readonly Regex validTableNameRegex = new Regex(@"^[A-Za-z][A-Za-z0-9]{2,62}$", RegexOptions.Compiled);

        private readonly List<string> reservedWords = new List<string> { "tables" };

        private readonly CloudTableClient tableClient;

        public TableStorage(CloudTableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public IStorageTable<T> Table<T>(string name = null) where T : TableEntity, new()
        {
            return new StorageTable<T>(TableReference<T>(name));
        }

        private CloudTable TableReference<T>(string name) where T : TableEntity, new()
        {
            var tableName = name ?? typeof(T).GetCustomAttribute<StorageTableNameAttribute>()?.Name ?? typeof(T).Name;

            if (!IsTableNameValid(tableName))
            {
                throw new InvalidOperationException($"Invalid Azure Storage table name: {tableName}");
            }

            return tableClient.GetTableReference(tableName);
        }

        public bool IsTableNameValid(string tableName)
        {
            return !reservedWords.Contains(tableName) && validTableNameRegex.IsMatch(tableName);
        }
    }
}
