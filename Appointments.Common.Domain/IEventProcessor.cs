namespace Appointments.Common.Domain;

public interface IEventProcessor
{
    Task ProcessAsync(IEnumerable<IDomainEvent> events);
}
