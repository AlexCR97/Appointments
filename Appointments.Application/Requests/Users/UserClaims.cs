using System.Security.Claims;

namespace Appointments.Application.Requests.Users;

internal sealed record IdTokenClaims(
    Guid Id,
    string Username,
    string FirstName,
    string LastName,
    string? ProfileImage)
{
    public IReadOnlyList<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("id", Id.ToString(), ClaimValueTypes.String),
            new Claim("username", Username, ClaimValueTypes.String),
            new Claim("firstName", FirstName, ClaimValueTypes.String),
            new Claim("lastName", LastName, ClaimValueTypes.String),
        };

        if (ProfileImage is not null)
            claims.Add(new Claim("profileImage", ProfileImage, ClaimValueTypes.String));

        return claims;
    }
}

internal sealed record AccessTokenClaims(
    string Username,
    string? Scope,
    Guid? TenantId)
{
    public IReadOnlyList<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("username", Username, ClaimValueTypes.String),
        };

        if (Scope is not null)
            claims.Add(new Claim("scope", Scope, ClaimValueTypes.String));

        if (TenantId is not null)
            claims.Add(new Claim("tenantId", TenantId.Value.ToString(), ClaimValueTypes.String));

        return claims;
    }
}

internal sealed record class RefreshTokenClaims(
    string Username,
    string? Scope,
    Guid? TenantId)
{
    public IReadOnlyList<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim("username", Username, ClaimValueTypes.String),
        };

        if (Scope is not null)
            claims.Add(new Claim("scope", Scope, ClaimValueTypes.String));

        if (TenantId is not null)
            claims.Add(new Claim("tenantId", TenantId.Value.ToString(), ClaimValueTypes.String));

        return claims;
    }
}
