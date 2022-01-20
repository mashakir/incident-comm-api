using System;

namespace Incident.Comm.Integration.Api.Services.Caching
{
    public class CacheItem<T>
    {
        public CacheItem()
        {
        }

        public CacheItem(T objectToCache)
        {
            CachedObject = objectToCache;
            CreationTime = DateTime.Now;
        }
        public T CachedObject { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
