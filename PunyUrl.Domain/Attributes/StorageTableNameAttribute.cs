using System;

namespace PunyUrl.Domain.Attributes
{
    public class StorageTableNameAttribute : Attribute
    {
        public string Name { get; }

        public StorageTableNameAttribute(string name)
        {
            Name = name;
        }
    }
}