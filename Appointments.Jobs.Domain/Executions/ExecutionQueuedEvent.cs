using Appointments.Common.Domain;
using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Jobs.Domain.Executions;

public sealed record ExecutionQueuedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid ExecutionId,
    Job JobSnapshot,
    Trigger TriggerSnapshot,
    TimeSpan? Timeout)
    : IDomainEvent;
