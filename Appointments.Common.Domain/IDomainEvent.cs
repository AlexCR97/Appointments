namespace Appointments.Common.Domain;

public interface IDomainEvent
{
    public Guid Id { get; }
    public DateTime OccurredAt { get; }
}
