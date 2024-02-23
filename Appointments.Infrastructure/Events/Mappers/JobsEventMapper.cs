using Appointments.Jobs.Infrastructure.UseCases.Executions;
using Appointments.Jobs.Infrastructure.UseCases.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Triggers;

namespace Appointments.Infrastructure.Events.Mappers;

internal sealed class JobsEventMapper : IEventMapper
{
    public object ToIntegrationEvent(object domainEvent)
    {
        if (domainEvent is Jobs.Domain.Executions.ExecutionQueuedEvent executionQueuedEvent)
        {
            return new ExecutionQueuedEvent(
                executionQueuedEvent.Id,
                executionQueuedEvent.OccurredAt,
                executionQueuedEvent.ExecutionId,
                executionQueuedEvent.JobSnapshot.ToDto(),
                executionQueuedEvent.TriggerSnapshot.ToDto(),
                executionQueuedEvent.Timeout?.TotalMilliseconds);
        }

        if (domainEvent is Jobs.Domain.Jobs.JobCreatedEvent jobCreatedEvent)
        {
            return new JobCreatedEvent(
                jobCreatedEvent.Id,
                jobCreatedEvent.OccurredAt,
                jobCreatedEvent.CreatedBy,
                jobCreatedEvent.Group.Value,
                jobCreatedEvent.Name.Value,
                jobCreatedEvent.DisplayName);
        }

        throw new IntegrationEventException(domainEvent);
    }
}
