namespace Appointments.Jobs.Domain.Triggers;

public class FireAndForgetTrigger : Trigger
{
    public FireAndForgetTrigger(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, TriggerName name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, TriggerType.FireAndForget, name, displayName)
    {
    }
}
