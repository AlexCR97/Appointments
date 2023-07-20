using StackExchange.Redis;

namespace Appointments.Common.Secrets.Redis;

internal class RedisDatabaseFactory
{
    private readonly string _connectionString;

    public RedisDatabaseFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDatabase CreateDatabase()
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(_connectionString);
        return connectionMultiplexer.GetDatabase();
    }
}
