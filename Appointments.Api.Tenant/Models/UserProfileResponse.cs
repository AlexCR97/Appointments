using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record UserProfileResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string FirstName,
    string LastName,
    string? ProfileImage,
    UserLogin[] Logins,
    UserTenant[] Tenants,
    UserPreference[] Preferences)
{
    internal static UserProfileResponse From(User user)
    {
        return new UserProfileResponse(
            user.Id,
            user.CreatedAt,
            user.CreatedBy,
            user.UpdatedAt,
            user.UpdatedBy,
            user.FirstName,
            user.LastName,
            user.ProfileImage,
            user.Logins.ToArray(),
            user.Tenants.ToArray(),
            user.Preferences.ToArray());
    }
}
