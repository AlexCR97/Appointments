using Appointments.Api.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Appointments.Api.Tenant.Controllers;

internal sealed class AuthorizeTenantAttribute : TypeFilterAttribute
{
    public AuthorizeTenantAttribute() : base(typeof(TenantAuthorizationFilter))
    {
    }
}

internal sealed class TenantAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var routeTenantToken = context.RouteData.Values
            .FirstOrDefault(x => x.Key == "tenantId")
            .Value
            ?? throw new InvalidOperationException(@"Route does not have a token with Key=""tenantId""");

        var routeTenantTokenStr = (string)routeTenantToken;

        var routeTenantId = Guid.Parse(routeTenantTokenStr);

        var tokenTenantId = context.HttpContext.User.GetAccessToken().TenantId;

        if (tokenTenantId is null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }
        
        if (tokenTenantId != routeTenantId)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        // Grant access
    }
}
