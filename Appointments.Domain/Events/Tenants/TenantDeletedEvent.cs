using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

public class TenantDeletedEvent : IEvent
{
    public DateTime Value { get; }
    public string? DeletedBy { get; }

    public TenantDeletedEvent(DateTime value, string? deletedBy)
    {
        Value = value;
        DeletedBy = deletedBy;
    }
}
