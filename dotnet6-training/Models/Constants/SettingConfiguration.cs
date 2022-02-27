namespace dotnet6_training.Models.Constants;

public class SettingConfiguration
{
    //Connection Strings Name
    public const string SQL_SERVER_CONNECTION_STRING = "TodoConnection";
    public const string SQL_SERVER_DOCKER_CONNECTION_STRING = "DockerTodoConnection";

    //Cache Section Name in appsetting
    public const string CACHE_SETTING = "CacheSetting";

    //Cache Tech
    public const string REDIS = "Redis";
    public const string MEMORY = "Memory";
}