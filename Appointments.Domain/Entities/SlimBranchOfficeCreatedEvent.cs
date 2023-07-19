using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Entities;
internal class SlimBranchOfficeCreatedEvent : IEvent
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public Guid TenantId { get; }
    public Location Empty { get; }
    public WeeklySchedule NineToFive { get; }

    public SlimBranchOfficeCreatedEvent(Guid id, DateTime createdAt, string? createdBy, Guid tenantId, Location empty, WeeklySchedule nineToFive)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        TenantId = tenantId;
        Empty = empty;
        NineToFive = nineToFive;
    }
}
