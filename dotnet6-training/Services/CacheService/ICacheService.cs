namespace dotnet6_training.Services.CacheService;

public interface ICacheService
{
    bool TryGet<TModel>(string cacheKey, out TModel value);
    TModel Set<TModel>(string cacheKey, TModel value);
    void Remove(string cacheKey);
}