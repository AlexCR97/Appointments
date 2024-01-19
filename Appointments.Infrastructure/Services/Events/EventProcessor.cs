using Appointments.Common.Domain;
using Appointments.Core.Contracts.Users;
using MassTransit;

namespace Appointments.Core.Infrastructure.Services.Events;

internal sealed class EventProcessor : IEventProcessor
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventProcessor(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task ProcessAsync(IDomainEvent @event)
    {
        var integrationEvent = MapToIntegrationEvent(@event);
        await _publishEndpoint.Publish(integrationEvent);
    }

    public async Task ProcessAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            await ProcessAsync(@event);
        }
    }

    private static object MapToIntegrationEvent(object @event)
    {
        if (@event is Application.Requests.Users.UserSignedUpWithEmailEvent userSignedUpWithEmailEvent)
        {
            return new UserSignedUpWithEmailEvent(
                userSignedUpWithEmailEvent.Id,
                userSignedUpWithEmailEvent.OccurredAt,
                userSignedUpWithEmailEvent.UserId);
        }

        throw new InvalidOperationException(@$"No such integration event for ""{@event.GetType().Name}""");
    }
}
