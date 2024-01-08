using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Assets.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IMvcBuilder AddAssetsApi(this IMvcBuilder builder)
    {
        return builder
            .AddApplicationPart(typeof(IAssemblyRef).Assembly);
    }

    public static AuthorizationOptions AddAssetsApiPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(AssetsApiPolicy.Assets.Scope, policy => policy
            .RequireClaim("scope", AssetsApiPolicy.Assets.Scope));

        return options;
    }
}
