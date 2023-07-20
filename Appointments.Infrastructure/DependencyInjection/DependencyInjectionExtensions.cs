using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Users;
using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Appointments.Infrastructure.Mapper.Mapsters.DependencyInjection;
using Appointments.Infrastructure.Mongo.Documents;
using Appointments.Infrastructure.Repositories.Users;
using Appointments.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMapper()
            .AddMongo(configuration)
            .AddRepositories()
            .AddSecretManager(configuration)
            .AddOtherServices()
            ;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services
            .AddMapsterMapper();
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("Mongo:ConnectionString");
        var databaseName = configuration.GetRequiredString("Mongo:DatabaseName");

        return services
            .AddMongoDatabase(connectionString, databaseName)
            .AddMongoRepository<TenantDocument>(TenantDocument.CollectionName)
            .AddMongoRepository<UserDocument>(UserDocument.CollectionName)
            .AddMongoRepository<BranchOfficeDocument>(BranchOfficeDocument.CollectionName)
            .AddMongoRepository<ServiceDocument>(ServiceDocument.CollectionName);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>();
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
