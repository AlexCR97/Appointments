using Appointments.Common.Secrets.Crypto;
using Appointments.Common.Secrets.Null;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Appointments.Common.Secrets.Redis.DependencyInjection;

public static class RedisSecretManagerExtensions
{
    public static IServiceCollection AddRedisSecretManager(this IServiceCollection services, IRedisSecretManagerOptions options)
    {
        var logger = LoggerFactory
            .Create(builder => builder
                .AddConsole())
            .CreateLogger(nameof(RedisSecretManagerExtensions));

        var database = GetDatabaseOrDefault(
            logger,
            options.ConnectionString);

        if (database is null)
        {
            logger.LogWarning("The {ServiceName} will be used as a fallback.", nameof(NullSecretManager));
            return services.AddNullSecretManager();
        }

        var cryptoService = new CryptoService(new CryptoOptions(options.Key));
        var secretManager = new RedisSecretManager(database, cryptoService);
        return services.AddSingleton<ISecretManager>(secretManager);
    }

    private static IDatabase? GetDatabaseOrDefault(ILogger logger, string connectionString)
    {
        logger.LogInformation("Connecting to Redis...");

        try
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString, config =>
            {
                //config.AbortOnConnectFail = true;
                config.ConnectRetry = 2;
                config.IncludeDetailInExceptions = true;
            });

            logger.LogInformation("A successful connection to Redis was established!");

            return connectionMultiplexer.GetDatabase();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not connect to Redis.");
            return null;
        }
    }
}
