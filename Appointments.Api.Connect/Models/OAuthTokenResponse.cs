using Appointments.Common.Domain.Models;

namespace Appointments.Api.Connect.Models;

public sealed record OAuthTokenResponse(
    string access_token,
    string? token_type,
    int? expires_in,
    string? scope,
    string? id_token,
    string? refresh_token)
{
    internal static OAuthTokenResponse From(OAuthToken token)
    {
        return new OAuthTokenResponse(
            token.access_token,
            token.token_type,
            token.expires_in,
            token.scope,
            token.id_token,
            token.refresh_token);
    }
}
