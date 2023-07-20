using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.BranchOffices;
internal class MinimalBranchOfficeCreatedEvent : IEvent
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public Guid TenantId { get; }
    public Location Empty { get; }
    public WeeklySchedule NineToFive { get; }

    public MinimalBranchOfficeCreatedEvent(Guid id, DateTime createdAt, string? createdBy, Guid tenantId, Location empty, WeeklySchedule nineToFive)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        TenantId = tenantId;
        Empty = empty;
        NineToFive = nineToFive;
    }
}
