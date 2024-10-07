namespace Appointments.Jobs.Domain.Triggers;

public class WebhookTrigger : Trigger
{
    // TODO Add configuration properties

    public WebhookTrigger(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, TriggerName name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, TriggerType.Webhook, name, displayName)
    {
    }
}
