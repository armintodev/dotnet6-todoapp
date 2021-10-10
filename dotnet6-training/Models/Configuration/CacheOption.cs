namespace dotnet6_training.Models.Configuration;

public class CacheOption
{
    public int AbsoluteExpirationInHours { get; set; }
    public int SlidingExpirationInMinutes { get; set; }
}