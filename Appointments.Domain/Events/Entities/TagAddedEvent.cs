namespace Appointments.Domain.Events.Entities;

public class TagAddedEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public string Tag { get; }

    public TagAddedEvent(Guid id, DateTime updatedAt, string? updatedBy, string tag)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Tag = tag;
    }
}
