using Appointments.Assets.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Assets.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddLocalAssetStore(this IServiceCollection services, LocalAssetStoreOptions options)
    {
        return services
            .AddSingleton(options)
            .AddTransient<IAssetStore, LocalAssetStore>();
    }
}
