using System.Security.Claims;

namespace Appointments.Application.Services.Jwt;

internal interface IJwtService
{
    string GenerateJwt(IReadOnlyList<Claim> claims);
}

internal class JwtService : IJwtService
{
    private readonly IJwtOptions _jwtOptions;

    public JwtService(IJwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateJwt(IReadOnlyList<Claim> claims)
    {
        return JwtGenerator.Generate(
            _jwtOptions.SecretKey,
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expiresInMinutes: _jwtOptions.ExpiresInMinutes,
            claims: claims);
    }
}
