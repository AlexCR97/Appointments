using Appointments.Domain.Entities.Abstractions;
using Appointments.Domain.Events.Abstractions;
using Appointments.Domain.Events.Entities;
using System.Text.Json;

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

    public Entity(
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, string?> extensions)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
        _tags = tags;
        _extensions = extensions;
    }

    #region Tags

    private readonly List<string> _tags;
    public IReadOnlyList<string> Tags => _tags;

    public void AddTag(string tag, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        _tags.Add(tag);

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
        
        _tags.Remove(tag);

        AddEvent(new TagRemovedEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            tag));
    }

    #endregion

    #region Extensions

    public Dictionary<string, string?> _extensions;
    public IReadOnlyDictionary<string, string?> Extensions => _extensions;

    public void SetExtension(string key, string? value, string? updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        _extensions[key] = value;

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

        _extensions.Remove(key);

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
        => JsonSerializer.Serialize(this);
}
