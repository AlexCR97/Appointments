using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Executions;
using Appointments.Jobs.Infrastructure.Mongo.Documents;
using Appointments.Jobs.Infrastructure.UseCases.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Triggers;
using System.Collections.Immutable;

namespace Appointments.Jobs.Infrastructure.Mongo.Executions;

internal sealed class ExecutionDocument : MongoDocument
{
    public const string CollectionName = "jobs-executions";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ExecutionDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public ExecutionDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        JobDto jobSnapshot, TriggerDto triggerSnapshot, ExecutionStatusAuditDocument[] statusAudit)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        JobSnapshot = jobSnapshot;
        TriggerSnapshot = triggerSnapshot;
        StatusAudit = statusAudit;
    }

    public JobDto JobSnapshot { get; set; }

    public TriggerDto TriggerSnapshot { get; set; }

    public ExecutionStatusAuditDocument[] StatusAudit { get; set; }

    internal static ExecutionDocument From(Execution execution)
    {
        return new ExecutionDocument(
            execution.Id,
            execution.CreatedAt,
            execution.CreatedBy,
            execution.UpdatedAt,
            execution.UpdatedBy,
            execution.JobSnapshot.ToDto(),
            execution.TriggerSnapshot.ToDto(),
            execution.StatusAudit
                .Select(x => new ExecutionStatusAuditDocument(
                    x.Status.ToString(),
                    x.OccurredAt))
                .ToArray());
    }

    internal Execution ToEntity()
    {
        return new Execution(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            JobSnapshot.ToEntity(),
            TriggerSnapshot.ToEntity(),
            ImmutableStack.CreateRange(StatusAudit.Select(x => new ExecutionStatusAudit(
                x.Status.ToEnum<ExecutionStatus>(),
                x.OccurredAt))));
    }
}

internal sealed record ExecutionStatusAuditDocument(
    string Status,
    DateTime OccurredAt);
