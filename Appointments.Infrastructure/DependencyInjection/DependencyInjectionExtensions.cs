using Appointments.Application.Services.Users;
using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Appointments.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongo(configuration)
            .AddSecretManager(configuration)
            .AddOtherServices()
            ;
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("Mongo:ConnectionString");
        var databaseName = configuration.GetRequiredString("Mongo:DatabaseName");

        return services
            .AddMongoDatabase(connectionString, databaseName);
    }

    private static IServiceCollection AddSecretManager(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("Secrets:Redis:ConnectionString");
        var hashKey = configuration.GetRequiredString("Secrets:Redis:HashKey");

        return services.AddRedisSecretManager(new RedisSecretManagerOptions(
            connectionString,
            hashKey));
    }

    private static IServiceCollection AddOtherServices(this IServiceCollection services)
    {
        // Users
        services.AddScoped<IUserPasswordManager, UserPasswordManager>();

        return services;
    }
}
