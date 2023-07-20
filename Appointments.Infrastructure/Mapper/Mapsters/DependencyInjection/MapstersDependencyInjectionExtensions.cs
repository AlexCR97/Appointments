using Appointments.Infrastructure.Mapper.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.Mapper.Mapsters.DependencyInjection;

internal static class MapstersDependencyInjectionExtensions
{
    public static IServiceCollection AddMapsterMapper(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMapper, MapsterMapper>();
    }
}
