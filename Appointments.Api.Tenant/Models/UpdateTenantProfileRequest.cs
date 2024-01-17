using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record UpdateTenantProfileRequest(
    string Name,
    string? Slogan,
    string UrlId,
    SocialMediaContactModel[]? Contacts)
{
    internal Appointments.Core.Application.Requests.Tenants.UpdateTenantProfileRequest ToApplicationRequest(
        string updatedBy,
        Guid tenantId)
    {
        return new Appointments.Core.Application.Requests.Tenants.UpdateTenantProfileRequest(
            updatedBy,
            tenantId,
            Name,
            Slogan,
            new TenantUrlId(UrlId),
            Contacts
                ?.Select(x => x.ToModel())
                .ToArray()
                ?? Array.Empty<SocialMediaContact>());
    }
}
