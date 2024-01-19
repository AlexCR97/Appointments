using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Api.Tenant.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IMvcBuilder AddTenantApi(this IMvcBuilder builder)
    {
        return builder
            .AddApplicationPart(typeof(IAssemblyRef).Assembly);
    }

    public static AuthorizationOptions AddTenantApiPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(TenantApiPolicy.Me.Scope, policy => policy
            .RequireClaim("scope", TenantApiPolicy.Me.Scope));

        options.AddPolicy(TenantApiPolicy.Tenants.Scope, policy => policy
            .RequireClaim("scope", TenantApiPolicy.Tenants.Scope));

        return options;
    }
}
