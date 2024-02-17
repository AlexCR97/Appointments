using Appointments.Common.Domain;
using Appointments.Common.Domain.Exceptions;
using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Domain.Triggers;
using System.Collections.Immutable;

namespace Appointments.Jobs.Domain;

public class Execution : Entity
{
    public Execution(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        Job jobSnapshot, Trigger triggerSnapshot, IImmutableStack<ExecutionStatusAudit> statusAudit)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        JobSnapshot = jobSnapshot;
        TriggerSnapshot = triggerSnapshot;
        _statusAudit = new Stack<ExecutionStatusAudit>(statusAudit);
    }

    public Job JobSnapshot { get; }
    
    public Trigger TriggerSnapshot { get; }

    public IImmutableStack<ExecutionStatusAudit> StatusAudit => ImmutableStack.CreateRange(_statusAudit);
    private readonly Stack<ExecutionStatusAudit> _statusAudit;

    public ExecutionStatus Status => LastAuditOrDefault?.Status ?? ExecutionStatus.Unknown;
    public DateTime? OccurredAt => LastAuditOrDefault?.OccurredAt;
    private ExecutionStatusAudit? LastAuditOrDefault => _statusAudit.TryPeek(out var audit) ? audit : null;

    public void SetStatus(ExecutionStatus status)
    {
        if (!Status.CanTransitionTo(status))
            throw new DomainException("InvalidExecutionStatusTransition", $"Cannot transition from {Status} to {status}");

        _statusAudit.Push(new ExecutionStatusAudit(
            status,
            DateTime.UtcNow));

        // TODO Add event
    }

    public static Execution Enqueue(
        string createdBy,
        Job jobSnapshot,
        Trigger triggerSnapshot)
    {
        var execution = new Execution(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            jobSnapshot,
            triggerSnapshot,
            ImmutableStack.Create(new ExecutionStatusAudit(
                ExecutionStatus.Queued,
                DateTime.UtcNow)));

        // TODO Add Event
        execution.AddEvent(new ExecutionQueuedEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            execution.Id,
            execution.JobSnapshot,
            execution.TriggerSnapshot));

        return execution;
    }
}


public readonly record struct ExecutionStatusAudit(
    ExecutionStatus Status,
    DateTime OccurredAt);

public sealed record ExecutionQueuedEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid ExecutionId,
    Job JobSnapshot,
    Trigger TriggerSnapshot)
    : IDomainEvent;
