using Appointments.Application.Repositories.Services;
using Appointments.Application.Requests.BranchOffices;
using Appointments.Application.Requests.Tenants;
using Appointments.Application.Requests.Users;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Files;
using Appointments.Common.MessageBroker.KafkaMessageBroker;
using Appointments.Common.MessageBroker.KafkaMessageBroker.DependencyInjection;
using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Appointments.Infrastructure.MessageBroker.Kafka;
using Appointments.Infrastructure.Mongo.Documents;
using Appointments.Infrastructure.Repositories;
using Appointments.Infrastructure.Services.Events;
using Appointments.Infrastructure.Services.FileStorages.LocalStorage;
using Appointments.Infrastructure.Services.Geo;
using Appointments.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddFileStorage(configuration)
            .AddMessageBroker(configuration)
            .AddMongo(configuration)
            .AddRepositories()
            .AddSecretManager(configuration)
            .AddOtherServices(configuration);
    }

    private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storagePath = configuration.GetRequiredString("FileStorage:Local:StoragePath");
        var storageOptions = new LocalFileStorageOptions(storagePath);
        services.AddSingleton<ILocalFileStorageOptions>(storageOptions);
        
        services.AddScoped<IFileStorage, LocalFileStorage>();

        return services;
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

    private static IServiceCollection AddOtherServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Events
        services.AddScoped<IEventProcessor, EventProcessor>();

        // Geo
        services.AddGeoService(configuration);

        // Users
        services.AddScoped<IUserPasswordManager, UserPasswordManager>();

        return services;
    }

    private static IServiceCollection AddGeoService(this IServiceCollection services, IConfiguration configuration)
    {
        var apiUrl = configuration.GetRequiredString("Geo:OpenStreetMaps:ApiUrl");
        var format = configuration.GetRequiredString("Geo:OpenStreetMaps:Format");
        var geoServiceOptions = new GeoServiceOptions(
            apiUrl,
            format);

        services.AddSingleton<IGeoServiceOptions>(geoServiceOptions);

        services.AddScoped<IGeoService, GeoService>();

        return services;
    }
}
