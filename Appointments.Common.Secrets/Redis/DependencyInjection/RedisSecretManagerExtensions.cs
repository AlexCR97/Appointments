using Appointments.Common.Secrets.Crypto;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Common.Secrets.Redis.DependencyInjection;

public static class RedisSecretManagerExtensions
{
    public static IServiceCollection AddRedisSecretManager(this IServiceCollection services, IRedisSecretManagerOptions options)
    {
        var redisDatabase = new RedisDatabaseFactory(options.ConnectionString).CreateDatabase();
        var cryptoService = new CryptoService(new CryptoOptions(options.Key));
        var secretManager = new RedisSecretManager(redisDatabase, cryptoService);
        return services.AddSingleton<ISecretManager>(secretManager);
    }
}
