namespace Appointments.Domain.Events.Entities;

public class ExtensionSetEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string Key { get; set; }
    public object? Value { get; set; }

    public ExtensionSetEvent(Guid id, DateTime updatedAt, string? updatedBy, string key, object? value)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Key = key;
        Value = value;
    }
}
