using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.Mapper.Mapsters.DependencyInjection;

internal static class MapstersDependencyInjectionExtensions
{
    public static IServiceCollection AddMapsterMapper(this IServiceCollection services)
    {
        var config = new Mapster.TypeAdapterConfig();

        return services
            .AddSingleton(config)
            .AddScoped<MapsterMapper.IMapper, MapsterMapper.ServiceMapper>()
            .AddScoped<Abstractions.IMapper, Mapper>();
    }
}
