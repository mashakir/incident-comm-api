using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Incident.Comm.Integration.Api.Config;
using System.Text;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Services.Caching
{
    public class ApiCache : IApiCache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly string _cacheKeyEnvironmentPrefix;

        public ApiCache(IDistributedCache distributedCache, CachingSection cachingSection)
        {
            _distributedCache = distributedCache;
            _cacheKeyEnvironmentPrefix = cachingSection.CacheKeyEnvironmentPrefix;
        }
        public async Task<CacheItem<T>> GetCacheItem<T>(string cacheKey)
        {
            var cacheEntryBytes = await _distributedCache.GetAsync($"{_cacheKeyEnvironmentPrefix}{cacheKey}");

            if (cacheEntryBytes != null)
            {
                var cacheEntryAsJson = Encoding.UTF8.GetString(cacheEntryBytes);

                return JsonConvert.DeserializeObject<CacheItem<T>>(cacheEntryAsJson);
            }

            return null;
        }

        public async Task SetCacheItem<T>(T objectToCache, string cacheKey, DistributedCacheEntryOptions distributedCacheEntryOptions)
        {
            var newCacheEntry = new CacheItem<T>(objectToCache);

            var newCacheEntryJson = JsonConvert.SerializeObject(newCacheEntry);

            byte[] encodedJson = Encoding.UTF8.GetBytes(newCacheEntryJson);

            await _distributedCache.SetAsync($"{_cacheKeyEnvironmentPrefix}{cacheKey}", encodedJson, distributedCacheEntryOptions);
        }

        public async Task ClearCache(string cacheKey)
        {
            await _distributedCache.RemoveAsync($"{_cacheKeyEnvironmentPrefix}{cacheKey}");
        }
    }

    public interface IApiCache
    {
        Task<CacheItem<T>> GetCacheItem<T>(string cacheKey);
        Task SetCacheItem<T>(T objectToCache, string cacheKey, DistributedCacheEntryOptions distributedCacheEntryOptions);
        Task ClearCache(string cacheKey);
    }
}
