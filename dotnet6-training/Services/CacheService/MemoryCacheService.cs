using dotnet6_training.Models.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace dotnet6_training.Services.CacheService;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheSetting _cacheSetting;
    private readonly MemoryCacheEntryOptions _memoryCacheOptions;

    public MemoryCacheService(IMemoryCache memoryCache,
        IOptionsMonitor<CacheSetting> cacheSetting)
    {
        _memoryCache = memoryCache;
        _cacheSetting = cacheSetting.CurrentValue;
        if (_cacheSetting is not null)
        {
            _memoryCacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(_cacheSetting.CacheOption.AbsoluteExpirationInHours),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(_cacheSetting.CacheOption.SlidingExpirationInMinutes)
            };
        }
    }

    public bool TryGet<TModel>(string cacheKey, out TModel value)
    {
        _memoryCache.TryGetValue(cacheKey, out value);
        if (value is null) return false;
        else return true;
    }

    public TModel Set<TModel>(string cacheKey, TModel value)
    {
        return _memoryCache.Set(cacheKey, value);
    }

    public void Remove(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }
}