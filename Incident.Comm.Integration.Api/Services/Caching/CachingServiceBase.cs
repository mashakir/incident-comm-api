using Microsoft.Extensions.Caching.Distributed; 
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Services.Caching
{
    public abstract class CachingServiceBase
    {
        private readonly IApiCache _intranetCache;
        public abstract int CacheTimeSeconds { get; }
        public abstract int StaleCacheTimeSeconds { get; }
        private static ConcurrentDictionary<string, DateTime> _lastCacheRefreshTimes = new ConcurrentDictionary<string, DateTime>();


        protected CachingServiceBase(IApiCache intranetCache)
        {
            _intranetCache = intranetCache;
        }

        public async Task<TOut> GetFromCache<TOut, TIn1, TIn2>(TIn1 parameter1, TIn2 parameter2, Func<TIn1, TIn2, Task<TOut>> getDataFunction, string cacheKey, bool forceRefresh = false)
        {
            if (!_lastCacheRefreshTimes.ContainsKey(cacheKey))
            {
                _lastCacheRefreshTimes.TryAdd(cacheKey, DateTime.MinValue);
            }

            TOut dataOut;

            var cacheEntry = await _intranetCache.GetCacheItem<TOut>(cacheKey);
            if (cacheEntry != null && !forceRefresh)
            {
                dataOut = cacheEntry.CachedObject;
                if (cacheEntry.CreationTime < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                {
                    if (_lastCacheRefreshTimes[cacheKey] < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                    {
                        var taskWeWantToHappenInTheBackground = GetDataAndPopulateCacheInBackground(parameter1, parameter2, getDataFunction, cacheKey);
                    }
                }
            }
            else
            {
                var getDataResult = await getDataFunction(parameter1, parameter2);

                dataOut = getDataResult;
                await _intranetCache.SetCacheItem(dataOut, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));
            }

            return dataOut;
        }

        public async Task<TOut> GetFromCache<TOut, TIn>(TIn identifier, Func<TIn, Task<TOut>> getDataFunction, string cacheKey, bool forceRefresh = false)
        {
            if (!_lastCacheRefreshTimes.ContainsKey(cacheKey))
            {
                _lastCacheRefreshTimes.TryAdd(cacheKey, DateTime.MinValue);
            }

            TOut dataOut;

            var cacheEntry = await _intranetCache.GetCacheItem<TOut>(cacheKey);
            if (cacheEntry != null && !forceRefresh)
            {
                dataOut = cacheEntry.CachedObject;
                if (cacheEntry.CreationTime < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                {
                    if (_lastCacheRefreshTimes[cacheKey] < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                    {
                        var taskWeWantToHappenInTheBackground = GetDataAndPopulateCacheInBackground(identifier, getDataFunction, cacheKey);
                    }
                }
            }
            else
            {
                var getDataResult = await getDataFunction(identifier);

                dataOut = getDataResult;
                await _intranetCache.SetCacheItem(dataOut, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));
            }

            return dataOut;
        }

        public async Task<TOut> GetFromCache<TOut>(Func<Task<TOut>> getDataFunction, string cacheKey, bool forceRefresh = false)
        {
            if (!_lastCacheRefreshTimes.ContainsKey(cacheKey))
            {
                _lastCacheRefreshTimes.TryAdd(cacheKey, DateTime.MinValue);
            }

            TOut dataOut;

            var cacheEntry = await _intranetCache.GetCacheItem<TOut>(cacheKey);
            if (cacheEntry != null && !forceRefresh)
            {
                dataOut = cacheEntry.CachedObject;
                if (cacheEntry.CreationTime < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                {
                    if (_lastCacheRefreshTimes[cacheKey] < DateTime.Now.AddSeconds(CacheTimeSeconds * -1))
                    {
                        var taskWeWantToHappenInTheBackground = GetDataAndPopulateCacheInBackground(getDataFunction, cacheKey);
                    }
                }
            }
            else
            {
                var getDataResult = await getDataFunction();

                dataOut = getDataResult;
                await _intranetCache.SetCacheItem(dataOut, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));
            }

            return dataOut;
        }

        private async Task GetDataAndPopulateCacheInBackground<TOut, TIn1, TIn2>(TIn1 parameter1, TIn2 parameter2, Func<TIn1, TIn2, Task<TOut>> getDataFunction, string cacheKey)
        {
            _lastCacheRefreshTimes[cacheKey] = DateTime.Now;

            var getDataResult = await getDataFunction(parameter1, parameter2);

            await _intranetCache.SetCacheItem(getDataResult, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));
        }

        private async Task<TOut> GetDataAndPopulateCacheInBackground<TOut, TIn>(TIn identifier, Func<TIn, Task<TOut>> getDataFunction, string cacheKey)
        {
            _lastCacheRefreshTimes[cacheKey] = DateTime.Now;

            var getDataResult = await getDataFunction(identifier);
            await _intranetCache.SetCacheItem(getDataResult, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));

            return getDataResult;
        }

        private async Task<TOut> GetDataAndPopulateCacheInBackground<TOut>(Func<Task<TOut>> getDataFunction, string cacheKey)
        {
            _lastCacheRefreshTimes[cacheKey] = DateTime.Now;

            var getDataResult = await getDataFunction();

            await _intranetCache.SetCacheItem(getDataResult, cacheKey, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(StaleCacheTimeSeconds)));


            return getDataResult;
        }

        protected async Task ClearCache(string cacheKey)
        {
            if (_lastCacheRefreshTimes.ContainsKey(cacheKey))
            {
                _lastCacheRefreshTimes[cacheKey] = DateTime.MinValue;
            }

            await _intranetCache.ClearCache(cacheKey);
        }
    }
}
