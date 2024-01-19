namespace Appointments.Api.Client.PowerShell.DependencyInjection;

internal static class PowerShellDependencyInjection
{
    public static IServiceCollection AddPowerShell(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPowerShellOptions(configuration)
            .AddSingleton<IPowerShellRunSpace, PowerShellRunSpace>()
            .AddSingleton<IPowerShellScript, PowerShellScript>();
    }

    private static IServiceCollection AddPowerShellOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var powerShellOptions = new PowerShellOptions();
        configuration.GetSection(PowerShellOptions.SectionName).Bind(powerShellOptions);
        services.AddSingleton(powerShellOptions);
        return services;
    }
}
