using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Appointments.Core.Application.Jwt;

internal static class JwtGenerator
{
    public static string Generate(
        string secretKey,
        string? issuer = null,
        string? audience = null,
        int? expiresInMinutes = null,
        IReadOnlyList<Claim>? claims = null)
    {
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: GetExpirationInMinutes(expiresInMinutes),
            signingCredentials: GenerateSigningCredentials(secretKey));

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(jwtSecurityToken);
    }

    private static DateTime? GetExpirationInMinutes(int? expiresInMinutes)
    {
        return expiresInMinutes is null
            ? null
            : DateTime.UtcNow.AddMinutes(expiresInMinutes.Value);
    }

    private static SigningCredentials GenerateSigningCredentials(string secretKey)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        return signingCredentials;
    }
}
