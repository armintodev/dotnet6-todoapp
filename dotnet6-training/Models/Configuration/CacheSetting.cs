namespace dotnet6_training.Models.Configuration;

public class CacheSetting
{
    public CacheOption CacheOption { get; set; }
    public RedisServer RedisServer { get; set; }
    public CacheSide CacheSide { get; set; }
}