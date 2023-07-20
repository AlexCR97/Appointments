using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Services;

public class MinimalServiceCreatedEvent : IEvent
{
    public Guid Id { get; }
    public string? CreatedBy { get; }
    public Guid TenantId { get; }

    public MinimalServiceCreatedEvent(Guid id, string? createdBy, Guid tenantId)
    {
        Id = id;
        CreatedBy = createdBy;
        TenantId = tenantId;
    }
}
