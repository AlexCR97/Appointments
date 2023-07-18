namespace Appointments.Domain.Events.Entities;

public class TagRemovedEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public string Tag { get; }

    public TagRemovedEvent(Guid id, DateTime updatedAt, string? updatedBy, string tag)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Tag = tag;
    }
}
