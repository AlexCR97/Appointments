using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Executions;

namespace Appointments.Jobs.Infrastructure.Elasticsearch.Executions;

internal sealed record ExecutionLogDocument(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    Guid ExecutionId,
    Guid JobId,
    Guid TriggerId,
    string Level,
    string Message)
{
    public const string IndexName = "jobs-executions";
}

internal static class ExecutionLogDocumentExtensions
{
    public static ExecutionLogDocument ToDocument(this ExecutionLog log)
    {
        return new ExecutionLogDocument(
            log.Id,
            log.CreatedAt,
            log.CreatedBy,
            log.ExecutionId,
            log.JobId,
            log.TriggerId,
            log.Level.ToString(),
            log.Message);
    }

    public static ExecutionLog ToEntity(this ExecutionLogDocument log)
    {
        return new ExecutionLog(
            log.Id,
            log.CreatedAt,
            log.CreatedBy,
            null,
            null,
            log.ExecutionId,
            log.JobId,
            log.TriggerId,
            log.Level.ToEnum<ExecutionLogLevel>(),
            log.Message);
    }
}
