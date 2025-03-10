using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace App.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var data = await _db.StringGetAsync(key);
            return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data!);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonData, expiration);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    
    }
}