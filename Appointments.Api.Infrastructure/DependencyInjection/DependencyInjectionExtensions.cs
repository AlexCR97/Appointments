using Appointments.Jobs.Infrastructure.DependencyInjection;
using Appointments.Notifications.Infrastructure.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddRabbitMq(configuration);
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.AddJobsInfrastructure();
            busConfig.AddNotificationsInfrastructure();

            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.UsingRabbitMq((busContext, rabbitMqConfig) =>
            {
                var rabbitMqSection = configuration.GetRequiredSection("RabbitMq");

                rabbitMqConfig.Host(rabbitMqSection.GetRequiredSection("Host").Value, hostConfig =>
                {
                    hostConfig.Username(rabbitMqSection.GetRequiredSection("Username").Value);
                    hostConfig.Password(rabbitMqSection.GetRequiredSection("Password").Value);
                });

                rabbitMqConfig.ConfigureEndpoints(busContext);
            });
        });
    }
}
