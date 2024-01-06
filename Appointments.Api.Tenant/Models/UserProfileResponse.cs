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
    UserLoginResponse[] Logins,
    UserTenantResponse[] Tenants,
    UserPreferenceResponse[] Preferences)
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
            user.Logins.Select(UserLoginResponse.From).ToArray(),
            user.Tenants.Select(UserTenantResponse.From).ToArray(),
            user.Preferences.Select(UserPreferenceResponse.From).ToArray());
    }
}

public sealed record UserLoginResponse(
    string IdentityProvider,
    string? Email,
    string? PhoneNumber)
{
    internal static UserLoginResponse From(UserLogin userLogin)
    {
        return new UserLoginResponse(
            userLogin.IdentityProvider.ToString(),
            userLogin.Email?.Value,
            userLogin.PhoneNumber);
    }
}

public sealed record UserTenantResponse(
    Guid TenantId,
    string TenantName)
{
    internal static UserTenantResponse From(UserTenant userTenant)
    {
        return new UserTenantResponse(
            userTenant.TenantId,
            userTenant.TenantName);
    }
}

public sealed record UserPreferenceResponse(
    string Key,
    string Value)
{
    internal static UserPreferenceResponse From(UserPreference userPreference)
    {
        return new UserPreferenceResponse(
            userPreference.Key,
            userPreference.Value);
    }
}
