using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record MyProfileResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string FirstName,
    string LastName,
    string? ProfileImage,
    UserLogin[] Logins,
    UserTenant Tenant,
    UserPreference[] Preferences)
{
    internal static MyProfileResponse From(User user, Guid tenantId)
    {
        return new MyProfileResponse(
            user.Id,
            user.CreatedAt,
            user.CreatedBy,
            user.UpdatedAt,
            user.UpdatedBy,
            user.FirstName,
            user.LastName,
            user.ProfileImage,
            user.Logins.ToArray(),
            user.GetTenant(tenantId),
            user.Preferences.ToArray());
    }
}
