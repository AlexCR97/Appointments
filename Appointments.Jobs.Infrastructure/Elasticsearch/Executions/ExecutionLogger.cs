using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Domain.Executions;

namespace Appointments.Jobs.Infrastructure.Elasticsearch.Executions;

internal interface IExecutionLogger
{
    void Log(ExecutionLogLevel level, string message);
}

internal static class IExecutionLoggerExtensions
{
    public static void Debug(this IExecutionLogger logger, string message)
        => logger.Log(ExecutionLogLevel.Debug, message);

    public static void Information(this IExecutionLogger logger, string message)
        => logger.Log(ExecutionLogLevel.Information, message);

    public static void Warn(this IExecutionLogger logger, string message)
        => logger.Log(ExecutionLogLevel.Warning, message);

    public static void Error(this IExecutionLogger logger, string message)
        => logger.Log(ExecutionLogLevel.Error, message);
}

internal sealed class ExecutionLogger : IExecutionLogger
{
    private readonly Execution _execution;
    private readonly IExecutionLogRepository _executionLogRepository;

    public ExecutionLogger(Execution execution, IExecutionLogRepository executionLogRepository)
    {
        _execution = execution;
        _executionLogRepository = executionLogRepository;
    }

    public async void Log(ExecutionLogLevel level, string message)
    {
        var log = ExecutionLog.Create(
            typeof(ExecutionLogger).FullName ?? typeof(ExecutionLogger).Name,
            _execution.Id,
            _execution.JobSnapshot.Id,
            _execution.TriggerSnapshot.Id,
            level,
            message);

        await _executionLogRepository.CreateAsync(log);
    }
}

internal sealed class ExecutionLoggerFactory
{
    private readonly IExecutionLogRepository _executionLogRepository;

    public ExecutionLoggerFactory(IExecutionLogRepository executionLogRepository)
    {
        _executionLogRepository = executionLogRepository;
    }

    public IExecutionLogger CreateExecutionLogger(Execution execution)
    {
        return new ExecutionLogger(
            execution,
            _executionLogRepository);
    }
}
