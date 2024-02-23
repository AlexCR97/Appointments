namespace Appointments.Infrastructure.Events.Mappers;

internal class IntegrationEventException : Exception
{
    public IntegrationEventException(object domainEvent)
        : base(@$"No such integration event for ""{domainEvent.GetType().Name}""")
    {
    }
}
