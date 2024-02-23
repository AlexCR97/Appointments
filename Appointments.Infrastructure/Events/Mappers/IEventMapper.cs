namespace Appointments.Infrastructure.Events.Mappers;

internal interface IEventMapper
{
    object ToIntegrationEvent(object domainEvent);
}
