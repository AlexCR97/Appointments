namespace Appointments.Common.Secrets.Redis;

public interface IRedisSecretManagerOptions
{
    public string ConnectionString { get; }
    string Key { get; }
}

public class RedisSecretManagerOptions : IRedisSecretManagerOptions
{
    public string ConnectionString { get; }
    public string Key { get; }

    public RedisSecretManagerOptions(string connectionString, string key)
    {
        ConnectionString = connectionString;
        Key = key;
    }
}
