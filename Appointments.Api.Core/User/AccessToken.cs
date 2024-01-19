using System.Security.Claims;

namespace Appointments.Api.Core.User;

public sealed record AccessToken(
    string Username,
    string? Scope,
    Guid? TenantId);

public static class AccessTokenExtensions
{
    public static AccessToken GetAccessToken(this ClaimsPrincipal principal)
    {
        return new AccessToken(
            principal.GetClaimValue("username"),
            principal.GetClaimValueOrDefault("scope"),
            principal.TryGetClaimValue("tenantId", out var tenantId)
                ? Guid.Parse(tenantId)
                : null);
    }

    private static string GetClaimValue(this ClaimsPrincipal principal, string type)
    {
        return principal.GetClaimValueOrDefault(type)
            ?? throw new InvalidOperationException(@$"No such claim of type ""{type}""");
    }

    private static string? GetClaimValueOrDefault(this ClaimsPrincipal principal, string type)
    {
        return principal.FindFirst(type)?.Value;
    }

    private static bool TryGetClaimValue(this ClaimsPrincipal principal, string type, out string value)
    {
        var claimValue = principal.GetClaimValueOrDefault(type);

        if (claimValue is null)
        {
            value = string.Empty;
            return false;
        }

        value = claimValue;
        return true;
    }
}
