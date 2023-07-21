namespace Appointments.Domain.Models.Auth;

public record OAuthToken(
    string access_token,
    string token_type,
    int? expires_in,
    string? scope,
    string? id_token,
    string? refresh_token);
