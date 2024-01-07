using Appointments.Assets.Application.DependencyInjection;
using Appointments.Assets.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Assets.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddApplication()
            .AddInfrastructure(configuration);
    }
}
