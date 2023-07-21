using Appointments.Application.Repositories.BranchOffices;
using Appointments.Application.Repositories.Services;
using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Users;
using Appointments.Common.MessageBroker.KafkaMessageBroker;
using Appointments.Common.MessageBroker.KafkaMessageBroker.DependencyInjection;
using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Appointments.Infrastructure.Mapper.Mapsters.DependencyInjection;
using Appointments.Infrastructure.MessageBroker.Kafka;
using Appointments.Infrastructure.Mongo.Documents;
using Appointments.Infrastructure.Repositories;
using Appointments.Infrastructure.Services.Events;
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
            .AddMessageBroker(configuration)
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

    private static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var bootstrapServers = configuration.GetRequiredString("MessageBroker:Kafka:BootstrapServers");

        return services
            .AddKafka(new KafkaOptions(bootstrapServers))
            .AddKafkaProducer<IEventsQueue>(new EventsQueue());
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
            .AddScoped<IBranchOfficeRepository, BranchOfficeRepository>()
            .AddScoped<IServiceRepository, ServiceRepository>()
            .AddScoped<ITenantRepository, TenantRepository>()
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
        // Events
        services.AddScoped<IEventProcessor, EventProcessor>();

        // Users
        services.AddScoped<IUserPasswordManager, UserPasswordManager>();

        return services;
    }
}
