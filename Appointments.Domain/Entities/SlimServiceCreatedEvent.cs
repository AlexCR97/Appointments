using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Entities;

public class SlimServiceCreatedEvent : IEvent
{
    public Guid Id { get; }
    public string? CreatedBy { get; }
    public Guid TenantId { get; }
    public SlimServiceCreatedEvent(Guid id, string? createdBy, Guid tenantId)
    {
        Id = id;
        CreatedBy = createdBy;
        TenantId = tenantId;
    }
}
