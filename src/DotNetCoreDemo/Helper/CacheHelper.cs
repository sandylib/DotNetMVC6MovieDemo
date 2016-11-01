using System;
using Microsoft.Extensions.Caching.Memory;


namespace DotNetCoreDemo.Helper
{
    public class CacheHelper
    {
        private readonly IMemoryCache _memoryCache;

        public CacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SaveTocache(string cacheKey, object savedItem, DateTime absoluteExpiration)
        {
           
            if (IsIncache(cacheKey))
                _memoryCache.Remove(cacheKey);

            _memoryCache.Set(cacheKey, savedItem, 
                new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = absoluteExpiration,
                    AbsoluteExpirationRelativeToNow = new TimeSpan(24,0,0),
                    Priority = CacheItemPriority.Normal
                });
        }

        public T GetFromCache<T>(string cacheKey) where T : class
        {
             return _memoryCache.Get(cacheKey) as T;
        }

        public  void RemoveFromCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public bool IsIncache(string cacheKey)
        {
            return _memoryCache.Get(cacheKey) != null;
        }
    }
}
