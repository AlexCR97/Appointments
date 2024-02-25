using Appointments.Jobs.Domain.Executions;

namespace Appointments.Api.Management.Models;

public sealed record ExecutionLogResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid ExecutionId,
    Guid JobId,
    Guid TriggerId,
    ExecutionLogLevel Level,
    string Message)
{
    internal static ExecutionLogResponse From(ExecutionLog log)
    {
        return new ExecutionLogResponse(
            log.Id,
            log.CreatedAt,
            log.CreatedBy,
            log.UpdatedAt,
            log.UpdatedBy,
            log.ExecutionId,
            log.JobId,
            log.TriggerId,
            log.Level,
            log.Message);
    }
}
