using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record TenantProfileResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string Name,
    string? Slogan,
    string UrlId,
    string? Logo,
    SocialMediaContact[] Contacts,
    WeeklySchedule? Schedule)
{
    internal static TenantProfileResponse From(Core.Domain.Entities.Tenant tenant)
    {
        return new TenantProfileResponse(
            tenant.Id,
            tenant.CreatedAt,
            tenant.CreatedBy,
            tenant.UpdatedAt,
            tenant.UpdatedBy,
            tenant.Name,
            tenant.Slogan,
            tenant.UrlId.Value,
            tenant.Logo,
            tenant.Contacts.ToArray(),
            tenant.Schedule);
    }
}
