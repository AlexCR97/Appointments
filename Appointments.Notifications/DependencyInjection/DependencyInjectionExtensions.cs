using Appointments.Notifications.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Notifications.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddInfrastructure(configuration);
    }
}
