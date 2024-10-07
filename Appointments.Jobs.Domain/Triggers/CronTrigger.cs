namespace Appointments.Jobs.Domain.Triggers;

public class CronTrigger : Trigger
{
    public CronExpression CronExpression { get; }

    public CronTrigger(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, TriggerName name, string? displayName, CronExpression cronExpression)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, TriggerType.Cron, name, displayName)
    {
        CronExpression = cronExpression;
    }
}
