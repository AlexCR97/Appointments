namespace Appointments.Domain.Events.Entities;

public class ExtensionRemovedEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string Key { get; set; }

    public ExtensionRemovedEvent(Guid id, DateTime updatedAt, string? updatedBy, string key)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Key = key;
    }
}
