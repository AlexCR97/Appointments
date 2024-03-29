﻿using Appointments.Common.Utils.Exceptions;
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
    internal static UserProfileResponse From(User user, Guid? tenantId = null)
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
            tenantId is null
                ? user.Tenants
                    .Select(UserTenantResponse.From)
                    .ToArray()
                : user.Tenants
                    .Select(UserTenantResponse.From)
                    .Where(x => x.TenantId == tenantId.Value)
                    .ToArray(),
            user.Preferences.Select(UserPreferenceResponse.From).ToArray());
    }
}

public sealed record UserLoginResponse(
    string IdentityProvider,
    bool Confirmed,
    string? Email,
    string? PhoneNumber)
{
    internal static UserLoginResponse From(IUserLogin login)
    {
        if (login is LocalLogin localLogin)
            return From(localLogin);

        throw new MappingExtension<IUserLogin, UserLoginResponse>();
    }

    private static UserLoginResponse From(LocalLogin login)
    {
        return new UserLoginResponse(
            login.IdentityProvider.ToString(),
            login.Confirmed,
            login.Email.Value,
            login.Password);
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
