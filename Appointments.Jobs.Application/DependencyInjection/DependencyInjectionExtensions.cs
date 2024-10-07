using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Jobs.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
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
