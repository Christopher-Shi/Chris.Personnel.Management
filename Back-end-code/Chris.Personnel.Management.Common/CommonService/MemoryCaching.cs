using Microsoft.Extensions.Caching.Memory;
using System;

namespace Chris.Personnel.Management.Common.CommonService
{
    public class MemoryCaching : IMemoryCaching
    {
        //引用Microsoft.Extensions.Caching.Memory;这个和.net 还是不一样，没有了Httpruntime了
        private readonly IMemoryCache _memoryCache;

        public MemoryCaching(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object Get(string cacheKey)
        {
            return _memoryCache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue)
        {
            _memoryCache.Set(cacheKey, cacheValue);
        }

        public void Set(string cacheKey, object cacheValue, double expirationMinutes)
        {
            _memoryCache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(expirationMinutes * 60));
        }
    }
}
