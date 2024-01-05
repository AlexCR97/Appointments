using Appointments.Core.Application.DependencyInjection;
using Appointments.Core.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Core.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCoreModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddApplication(configuration)
            .AddInfrastructure(configuration);
    }
}
