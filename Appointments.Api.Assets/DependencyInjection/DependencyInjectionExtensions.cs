using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Assets.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IMvcBuilder AddAssetsApi(this IMvcBuilder builder)
    {
        return builder
            .AddApplicationPart(typeof(IAssemblyRef).Assembly);
    }
}
