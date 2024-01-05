namespace Appointments.Core.Application.Jwt;

public interface IJwtOptions
{
    public string SecretKey { get; }
    public string? Issuer { get; }
    public string? Audience { get; }
    public int? ExpiresInMinutes { get; }
}

internal class JwtOptions : IJwtOptions
{
    public string SecretKey { get; set; } = null!;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int? ExpiresInMinutes { get; set; }
}
