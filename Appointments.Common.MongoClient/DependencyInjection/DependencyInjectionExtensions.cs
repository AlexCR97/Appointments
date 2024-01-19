using Appointments.Common.MongoClient.Abstractions;
using Appointments.Common.MongoClient.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Appointments.Common.MongoClient.DependencyInjection;

public static class DependencyInjectionExtensions 
{
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string connectionString, string databaseName)
    {
        var client = new MongoDB.Driver.MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        return services
            .AddSingleton<IMongoClient>(client)
            .AddSingleton<IMongoDatabase>(database)
            ;
    }

    public static IServiceCollection AddMongoRepository<TDocument>(this IServiceCollection services, string collectionName)
        where TDocument : IMongoDocument
    {
        return services
            .AddMongoCollection<TDocument>(collectionName)
            .AddScoped<IMongoRepository<TDocument>, MongoRepository<TDocument>>();
    }

    private static IServiceCollection AddMongoCollection<TDocument>(this IServiceCollection services, string collectionName)
    {
        return services.AddTransient<IMongoCollection<TDocument>>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<TDocument>(collectionName);
        });
    }
}
