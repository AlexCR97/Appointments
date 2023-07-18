using Appointments.Domain.Entities.Abstractions;

namespace Appointments.Domain.Entities;

public class Entity : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public List<string>? Tags { get; set; }
    public Dictionary<string, string?>? Extensions { get; set; }
}
