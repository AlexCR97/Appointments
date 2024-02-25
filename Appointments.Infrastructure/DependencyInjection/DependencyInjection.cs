using Appointments.Common.Domain;
using Appointments.Infrastructure.Events;
using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddElasticsearchClient(configuration)
            .AddModules(configuration)
            .AddRabbitMq(configuration)
            .AddScoped<IEventProcessor, EventProcessor>();
    }

    private static IServiceCollection AddElasticsearchClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton((_) =>
        {
            var uri = configuration.GetRequiredSection("Elasticsearch:Uri").Value!;
            var settings = new ElasticsearchClientSettings(new Uri(uri));
            return new ElasticsearchClient(settings);
        });
    }

    private static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        Assets.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddInfrastructure(services, configuration);
        Core.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddInfrastructure(services, configuration);
        Jobs.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddInfrastructure(services);
        Notifications.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddInfrastructure(services, configuration);
        return services;
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.AddModules();

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

    private static void AddModules(this IBusRegistrationConfigurator busConfig)
    {
        Jobs.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddJobsInfrastructure(busConfig);
        Notifications.Infrastructure.DependencyInjection.DependencyInjectionExtensions.AddNotificationsInfrastructure(busConfig);
    }
}
