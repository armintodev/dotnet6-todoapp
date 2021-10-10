namespace dotnet6_training.Services.CacheService;

public class RedisCacheService : ICacheService
{
    public RedisCacheService()
    {

    }

    public bool TryGet<TModel>(string cacheKey, out TModel value)
    {
        throw new NotImplementedException();
    }

    public TModel Set<TModel>(string cacheKey, TModel value)
    {
        throw new NotImplementedException();
    }

    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }
}
