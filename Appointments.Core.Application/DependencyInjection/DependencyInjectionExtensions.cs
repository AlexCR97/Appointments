using Appointments.Core.Application.Jwt;
using Appointments.Core.Application.Requests.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Core.Application.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCqrs()
            .AddJwtService(configuration)
            .AddOtherServices();
    }

    private static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        return services
            .AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining<IApplicationMarker>());
    }

    private static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();

        configuration
            .GetRequiredSection("Jwt")
            .Bind(jwtOptions);

        return services
            .AddSingleton<IJwtOptions>(jwtOptions)
            .AddScoped<IJwtService, JwtService>();
    }

    private static IServiceCollection AddOtherServices(this IServiceCollection services)
    {
        return services
            // Users
            .AddScoped<IUserService, UserService>();
    }
}
