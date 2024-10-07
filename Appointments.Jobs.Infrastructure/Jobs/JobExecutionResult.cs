using Appointments.Jobs.Domain.Executions;

namespace Appointments.Jobs.Infrastructure.Jobs;

public enum JobExecutionResult
{
    Failed,
    Completed,
    Succeeded,
}

internal static class JobExecutionResultExtensions
{
    public static ExecutionStatus ToExecutionStatus(this JobExecutionResult result)
    {
        return result switch
        {
            JobExecutionResult.Failed => ExecutionStatus.Failed,
            JobExecutionResult.Completed => ExecutionStatus.Completed,
            JobExecutionResult.Succeeded => ExecutionStatus.Succeeded,
            _ => throw new UnsupportedJobExecutionResultException(result),
        };
    }
}

internal class UnsupportedJobExecutionResultException : Exception
{
    public UnsupportedJobExecutionResultException(JobExecutionResult result)
        : base($"Unsupported {nameof(JobExecutionResult)} {result}")
    {
    }
}
