using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.Mapper.Masters.DependencyInjection;

internal static class MapstersDependencyInjectionExtensions
{
    public static IServiceCollection AddMapsterMapper(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();

        return services
            .AddSingleton(config)
            .AddScoped<IMapper, ServiceMapper>()
            .AddScoped<Abstractions.IMapper, Mapper>();
    }
}
