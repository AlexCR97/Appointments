using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record ServiceListResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid TenantId,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
{
    internal static ServiceListResponse From(Service service)
    {
        return new ServiceListResponse(
            service.Id,
            service.CreatedAt,
            service.CreatedBy,
            service.UpdatedAt,
            service.UpdatedBy,
            service.TenantId,
            service.Name,
            service.Description,
            service.Price,
            service.CustomerDuration,
            service.CalendarDuration);
    }
}
