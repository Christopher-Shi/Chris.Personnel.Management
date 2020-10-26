using Chris.Personnel.Management.Common.CommonService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class MemoryCacheSetup
    {
        public static void AddMemoryCacheSetup(this IServiceCollection services)
        {
            services.AddScoped<IMemoryCaching, MemoryCaching>();
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var memoryCache = new MemoryCache(new MemoryCacheOptions());
                return memoryCache;
            });
        }
    }
}
