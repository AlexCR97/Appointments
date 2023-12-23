using Appointments.Domain.Entities;

namespace Appointments.Application.Services.Events;

public interface IEventProcessor
{
    Task ProcessAsync(IEnumerable<IDomainEvent> events);
}
