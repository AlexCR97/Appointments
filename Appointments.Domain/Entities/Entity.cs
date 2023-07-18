using Appointments.Domain.Entities.Abstractions;
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
    public List<string> Tags { get; protected set; } = new();
    public Dictionary<string, string?> Extensions { get; protected set; } = new();

    #region Tags

    public void AddTag(string tag)
        => Tags.Add(tag);

    public void RemoveTag(string tag)
        => Tags.Remove(tag);

    #endregion

    #region Extensions

    public void AddExtension(string key, string? value)
        => Extensions.Add(key, value);

    public void RemoveExtension(string key)
        => Extensions.Remove(key);

    #endregion

    public override string ToString()
        => JsonSerializer.Serialize(this);
}
