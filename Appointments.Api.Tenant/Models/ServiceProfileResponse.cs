using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record ServiceProfileResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid TenantId,
    string Name,
    string? Description,
    decimal Price,
    IndexedResourceModel[] Images,
    string[] TermsAndConditions,
    string? Notes,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
{
    internal static ServiceProfileResponse From(Service service)
    {
        return new ServiceProfileResponse(
            service.Id,
            service.CreatedAt,
            service.CreatedBy,
            service.UpdatedAt,
            service.UpdatedBy,
            service.TenantId,
            service.Name,
            service.Description,
            service.Price,
            service.Images
                .Select(IndexedResourceModel.From)
                .ToArray(),
            service.TermsAndConditions.ToArray(),
            service.Notes,
            service.CustomerDuration,
            service.CalendarDuration);
    }
}
