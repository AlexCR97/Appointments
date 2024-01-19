using Appointments.Common.Secrets.Null;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Common.Secrets.Redis.DependencyInjection;

internal static class NullSecretManagerExtensions
{
    public static IServiceCollection AddNullSecretManager(this IServiceCollection services)
    {
        return services.AddSingleton<ISecretManager, NullSecretManager>();
    }
}
