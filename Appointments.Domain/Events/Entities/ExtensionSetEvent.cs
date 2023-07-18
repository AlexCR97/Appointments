namespace Appointments.Domain.Events.Entities;

public class ExtensionSetEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string Key { get; set; }
    public string? Value { get; set; }

    public ExtensionSetEvent(Guid id, DateTime updatedAt, string? updatedBy, string key, string? value)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Key = key;
        Value = value;
    }
}
