using Appointments.Assets.Application;
using Appointments.Assets.Infrastructure.Mongo.Assets;
using Appointments.Common.MongoClient.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Assets.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMongo()
            .AddLocalAssetStore(configuration);
    }

    private static IServiceCollection AddMongo(this IServiceCollection services)
    {
        return services
            .AddMongoRepository<AssetDocument>(AssetDocument.CollectionName)
            .AddScoped<IAssetRepository, AssetRepository>();
    }

    private static IServiceCollection AddLocalAssetStore(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetRequiredSection("Assets")
            .Get<LocalAssetStoreOptions>()
            ?? throw new InvalidOperationException($"Could not bind Assets options");

        return services
            .AddSingleton(options)
            .AddTransient<IAssetStore, LocalAssetStore>();
    }
}
