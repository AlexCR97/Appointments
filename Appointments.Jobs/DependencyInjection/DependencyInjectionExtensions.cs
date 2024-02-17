using Appointments.Jobs.Application.DependencyInjection;
using Appointments.Jobs.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Jobs.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddJobsModule(this IServiceCollection services)
    {
        return services
            .AddApplication()
            .AddInfrastructure();
    }
}
