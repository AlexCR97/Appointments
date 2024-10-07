using Appointments.Common.Domain;

namespace Appointments.Jobs.Domain.Executions;

public class ExecutionLog : Entity
{
    public ExecutionLog(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, Guid executionId, Guid jobId, Guid triggerId, ExecutionLogLevel level, string message) : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        ExecutionId = executionId;
        JobId = jobId;
        TriggerId = triggerId;
        Level = level;
        Message = message;
    }

    public Guid ExecutionId { get; }
    public Guid JobId { get; }
    public Guid TriggerId { get; }
    public ExecutionLogLevel Level { get; }
    public string Message { get; }

    public static ExecutionLog Create(string createdBy, Guid executionId, Guid jobId, Guid triggerId, ExecutionLogLevel level, string message)
    {
        return new ExecutionLog(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            executionId,
            jobId,
            triggerId,
            level,
            message);
    }
}

public enum ExecutionLogLevel
{
    Debug,
    Information,
    Warning,
    Error,
}
