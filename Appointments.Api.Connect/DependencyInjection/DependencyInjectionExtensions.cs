using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Connect.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IMvcBuilder AddConnectApi(this IMvcBuilder builder)
    {
        return builder
            .AddApplicationPart(typeof(IAssemblyRef).Assembly);
    }
}
