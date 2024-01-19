using System.Security.Claims;

namespace Appointments.Api.Extensions.AspNetCore;

internal static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user
            .FindFirst(x => x.Type == ClaimTypes.Email)
            ?.Value;
    }
}
