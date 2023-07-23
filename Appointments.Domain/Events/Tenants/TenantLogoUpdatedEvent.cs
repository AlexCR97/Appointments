using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

internal record TenantLogoUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string? UpdatedBy,
    string Logo) : IEvent;
