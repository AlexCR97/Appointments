using Appointments.Domain.Entities.Abstractions;
using Appointments.Domain.Events.Abstractions;
using Appointments.Domain.Events.Entities;
using Newtonsoft.Json;

namespace Appointments.Domain.Entities;

public class Entity : IEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    public Entity()
    {
        // Needed for auto-mapping
    }

    public Entity(
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, object?> extensions)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
        Tags = tags;
        Extensions = extensions;
    }

    #region Tags

    public List<string> Tags { get; protected set; }

    public void AddTag(string tag, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        Tags.Add(tag);

        AddEvent(new TagAddedEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            tag));
    }

    public void RemoveTag(string tag, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        
        Tags.Remove(tag);

        AddEvent(new TagRemovedEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            tag));
    }

    #endregion

    #region Extensions

    public Dictionary<string, object?> Extensions { get; protected set; }

    public void SetExtension(string key, object? value, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Extensions[key] = value;

        AddEvent(new ExtensionSetEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            key,
            value));
    }

    public void RemoveExtension(string key, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        Extensions.Remove(key);

        AddEvent(new ExtensionRemovedEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            key));
    }

    #endregion

    #region Events

    private readonly List<IEvent> _events = new();
    public IReadOnlyList<IEvent> Events => _events;

    public bool HasChanged => _events.Count > 0;

    protected void AddEvent(IEvent @event)
        => _events.Add(@event);

    #endregion

    public override string ToString()
        => JsonConvert.SerializeObject(this);
}
