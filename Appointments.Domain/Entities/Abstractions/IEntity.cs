namespace Appointments.Domain.Entities.Abstractions;

public interface IEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public DateTime? UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public DateTime? DeletedAt { get; }
    public string? DeletedBy { get; }
}
