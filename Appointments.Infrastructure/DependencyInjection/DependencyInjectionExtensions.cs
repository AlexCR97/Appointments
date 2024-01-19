using Appointments.Common.Domain;
using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Appointments.Core.Application.Requests.Appointments;
using Appointments.Core.Application.Requests.BranchOffices;
using Appointments.Core.Application.Requests.Customers;
using Appointments.Core.Application.Requests.Services;
using Appointments.Core.Application.Requests.Tenants;
using Appointments.Core.Application.Requests.Users;
using Appointments.Core.Infrastructure.Mongo.Appointments;
using Appointments.Core.Infrastructure.Mongo.BranchOffices;
using Appointments.Core.Infrastructure.Mongo.Customers;
using Appointments.Core.Infrastructure.Mongo.Services;
using Appointments.Core.Infrastructure.Mongo.Tenants;
using Appointments.Core.Infrastructure.Mongo.Users;
using Appointments.Core.Infrastructure.Services.Events;
using Appointments.Core.Infrastructure.Services.Geo;
using Appointments.Core.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Core.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongo(configuration)
            .AddRepositories()
            .AddSecretManager(configuration)
            .AddOtherServices(configuration);
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("Mongo:ConnectionString");
        var databaseName = configuration.GetRequiredString("Mongo:DatabaseName");

        return services
            .AddMongoDatabase(connectionString, databaseName)
            .AddMongoRepository<AppointmentDocument>(AppointmentDocument.CollectionName)
            .AddMongoRepository<BranchOfficeDocument>(BranchOfficeDocument.CollectionName)
            .AddMongoRepository<CustomerDocument>(CustomerDocument.CollectionName)
            .AddMongoRepository<ServiceDocument>(ServiceDocument.CollectionName)
            .AddMongoRepository<TenantDocument>(TenantDocument.CollectionName)
            .AddMongoRepository<UserDocument>(UserDocument.CollectionName);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAppointmentRepository, AppointmentRepository>()
            .AddScoped<IBranchOfficeRepository, BranchOfficeRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
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
