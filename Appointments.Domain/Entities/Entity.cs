using Newtonsoft.Json;

namespace Appointments.Domain.Entities;

public interface IEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? UpdatedAt { get; }
    public string? UpdatedBy { get; }
}

public class Entity : IEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    public Entity(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
    }

    #region Events

    private readonly List<IDomainEvent> _events = new();
    public IReadOnlyList<IDomainEvent> Events => _events;

    public bool HasChanged => _events.Count > 0;

    protected void AddEvent(IDomainEvent @event)
        => _events.Add(@event);

    #endregion

    public override string ToString()
        => JsonConvert.SerializeObject(this);
}
