using Appointments.Notifications.Application.Emails;
using Appointments.Notifications.Infrastructure.Emails;
using Appointments.Notifications.Infrastructure.UseCases.Users;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Notifications.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRabbitMq(configuration)
            .AddEmailSender(configuration);
    }

    private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<UserSignedUpWithEmailConsumer>();

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

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var brevoApiOptions = new BrevoApiOptions(configuration.GetRequiredSection("Emails:Brevo:ApiKey").Value!);
        services.AddSingleton(brevoApiOptions);
        services.AddScoped<IBrevoApi, BrevoApi>();

        var emailSenderOptions = new BrevoEmailSenderOptions(
            new Subject(
                configuration.GetRequiredSection("Emails:Sender:Name").Value!,
                configuration.GetRequiredSection("Emails:Sender:Email").Value!),
            configuration.GetRequiredSection("Emails:EmailConfirmationUrl").Value!);
        services.AddSingleton(emailSenderOptions);
        services.AddScoped<IEmailSender, BrevoEmailSender>();

        return services;
    }
}
