using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Management.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IMvcBuilder AddManagementApi(this IMvcBuilder builder)
    {
        return builder
            .AddApplicationPart(typeof(IAssemblyRef).Assembly);
    }

    public static AuthorizationOptions AddManagementApiPolicies(this AuthorizationOptions options)
    {
        // TODO Add auth policies

        //options.AddPolicy(TenantApiPolicy.Me.Scope, policy => policy
        //    .RequireClaim("scope", TenantApiPolicy.Me.Scope));

        //options.AddPolicy(TenantApiPolicy.Tenants.Scope, policy => policy
        //    .RequireClaim("scope", TenantApiPolicy.Tenants.Scope));

        return options;
    }
}
