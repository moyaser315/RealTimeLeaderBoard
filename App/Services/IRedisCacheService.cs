using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IRedisCacheService
    {
        Task<T?> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveCacheAsync(string key);
    }
}