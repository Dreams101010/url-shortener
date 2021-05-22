using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace URLShortenerDomainLayer.Interfaces
{
    public interface ICache
    {
        public T Get<T>(string key);
        public void Set<T>(string key, T value);
        public bool Has(string key);
        public string GetKey<T>(T value)
        {
            var key = JsonSerializer.Serialize(value);
            return key;
        }
    }
}
