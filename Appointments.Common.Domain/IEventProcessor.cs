namespace Appointments.Common.Domain;

public interface IEventProcessor
{
    Task ProcessAsync(IDomainEvent @event);
    Task ProcessAsync(IEnumerable<IDomainEvent> events);
}
