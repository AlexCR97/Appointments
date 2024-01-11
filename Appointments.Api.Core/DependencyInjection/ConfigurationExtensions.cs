using Microsoft.Extensions.Configuration;

namespace Appointments.Api.Core.DependencyInjection;

internal static class ConfigurationExtensions
{
    public static string GetRequiredString(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (!string.IsNullOrWhiteSpace(value))
            return value;

        throw new KeyNotFoundException(@$"Could not find key ""{key}"" in configuration.");
    }
}
