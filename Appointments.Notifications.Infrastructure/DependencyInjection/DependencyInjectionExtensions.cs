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
            .AddEmailSender(configuration);
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpClient<BrevoApi>((_, httpClient) =>
            {
                var brevoApiKey = configuration.GetRequiredSection("Emails:Brevo:ApiKey").Value!;
                httpClient.BaseAddress = new Uri("https://api.brevo.com");
                httpClient.DefaultRequestHeaders.Add("api-key", brevoApiKey);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler())
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        var emailSenderOptions = new BrevoEmailSenderOptions(
            new Subject(
                configuration.GetRequiredSection("Emails:Sender:Name").Value!,
                configuration.GetRequiredSection("Emails:Sender:Email").Value!),
            configuration.GetRequiredSection("Emails:EmailConfirmationUrl").Value!);
        services.AddSingleton(emailSenderOptions);
        services.AddScoped<IEmailSender, BrevoEmailSender>();

        return services;
    }

    public static IBusRegistrationConfigurator AddNotificationsInfrastructure(this IBusRegistrationConfigurator busConfig)
    {
        busConfig.AddConsumer<UserSignedUpWithEmailConsumer>();
        return busConfig;
    }
}
