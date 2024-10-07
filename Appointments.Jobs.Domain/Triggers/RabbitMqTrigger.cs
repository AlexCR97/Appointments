namespace Appointments.Jobs.Domain.Triggers;

public class RabbitMqTrigger : Trigger
{
    // TODO Add configuration properties

    public RabbitMqTrigger(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, TriggerName name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, TriggerType.RabbitMq, name, displayName)
    {
    }
}
