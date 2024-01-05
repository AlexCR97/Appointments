using Appointments.Common.Domain;
using Appointments.Common.MessageBroker.Abstractions;
using Appointments.Core.Infrastructure.MessageBroker.Kafka;

namespace Appointments.Core.Infrastructure.Services.Events;

internal sealed class EventProcessor : IEventProcessor
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
