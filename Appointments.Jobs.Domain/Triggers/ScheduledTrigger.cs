namespace Appointments.Jobs.Domain.Triggers;

public class ScheduledTrigger : Trigger
{
    public DateTime TriggerAt { get; }

    public ScheduledTrigger(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, TriggerName name, string? displayName, DateTime triggerAt)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, TriggerType.Scheduled, name, displayName)
    {
        TriggerAt = triggerAt;
    }
}
