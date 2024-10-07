using Appointments.Common.Domain;
using Appointments.Infrastructure.Events.Mappers;
using MassTransit;
using System.Reflection;

namespace Appointments.Infrastructure.Events;

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

    private static object MapToIntegrationEvent(object domainEvent)
    {
        var domainEventAssembly = domainEvent.GetType().Assembly;

        if (!_eventMappers.ContainsKey(domainEventAssembly))
            throw new IntegrationEventException(domainEvent);

        var eventMapper = _eventMappers[domainEventAssembly];

        return eventMapper.ToIntegrationEvent(domainEvent);
    }

    private static readonly IEventMapper _coreEventMapper = new CoreEventMapper();
    private static readonly IEventMapper _jobsEventMapper = new JobsEventMapper();
    private static readonly IReadOnlyDictionary<Assembly, IEventMapper> _eventMappers = new Dictionary<Assembly, IEventMapper>
    {
        [typeof(Core.Domain.Entities.Appointment).Assembly] = _coreEventMapper,
        [typeof(Core.Application.DependencyInjection.DependencyInjectionExtensions).Assembly] = _coreEventMapper,

        [typeof(Jobs.Domain.Batch).Assembly] = _jobsEventMapper,
    };
}
