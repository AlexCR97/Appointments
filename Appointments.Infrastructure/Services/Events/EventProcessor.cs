using Appointments.Application.Services.Events;
using Appointments.Common.MessageBroker.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.MessageBroker.Kafka;

namespace Appointments.Infrastructure.Services.Events;

internal class EventProcessor : IEventProcessor
{
    private readonly IPublisher<IEventsQueue> _publisher;

    public EventProcessor(IPublisher<IEventsQueue> publisher)
    {
        _publisher = publisher;
    }

    public async Task ProcessAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            await _publisher.PublishAsync(@event);
        }
    }
}
