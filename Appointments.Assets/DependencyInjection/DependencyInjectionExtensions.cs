using Appointments.Assets.Application.DependencyInjection;
using Appointments.Assets.Infrastructure;
using Appointments.Assets.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Assets.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetRequiredSection("Assets")
            .Get<LocalAssetStoreOptions>()
            ?? throw new InvalidOperationException($"Could not bind Assets options");

        return services
            .AddApplication()
            .AddLocalAssetStore(options);
    }
}
