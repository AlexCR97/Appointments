using Appointments.Domain.Events.Abstractions;

namespace Appointments.Application.Services.Events;

public interface IEventProcessor
{
    Task ProcessAsync(IEnumerable<IEvent> events);
}
