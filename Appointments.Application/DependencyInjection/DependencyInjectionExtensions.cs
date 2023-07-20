using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCqrs();
    }

    private static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        return services
            .AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining<IApplicationMarker>());
    }
}
