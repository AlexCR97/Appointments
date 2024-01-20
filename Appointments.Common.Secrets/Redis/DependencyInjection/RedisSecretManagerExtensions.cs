using Appointments.Common.Secrets.Crypto;
using Appointments.Common.Secrets.Null;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Appointments.Common.Secrets.Redis.DependencyInjection;

public static class RedisSecretManagerExtensions
{
    /// <summary>
    /// This class is used for logging only.
    /// </summary>
    private sealed class RedisSecretManagerDependencyInjection { }

    public static IServiceCollection AddRedisSecretManager(this IServiceCollection services, IRedisSecretManagerOptions options)
    {
        return services.AddSingleton<ISecretManager>((serviceProvider) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<RedisSecretManagerDependencyInjection>>();

            var database = GetDatabaseOrDefault(
                logger,
                options.ConnectionString);

            if (database is null)
            {
                logger.LogWarning("The {ServiceName} will be used as a fallback.", nameof(NullSecretManager));
                return new NullSecretManager();
            }

            var cryptoService = new CryptoService(new CryptoOptions(options.Key));

            return new RedisSecretManager(database, cryptoService);
        });
    }

    private static IDatabase? GetDatabaseOrDefault(ILogger logger, string connectionString)
    {
        logger.LogDebug("Connecting to Redis...");

        try
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString, config =>
            {
                //config.AbortOnConnectFail = true;
                config.ConnectRetry = 2;
                config.IncludeDetailInExceptions = true;
            });

            logger.LogDebug("A successful connection to Redis was established!");

            return connectionMultiplexer.GetDatabase();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not connect to Redis.");
            return null;
        }
    }
}
