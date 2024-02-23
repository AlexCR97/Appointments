using Appointments.Common.Domain;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Domain.Executions;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Appointments.Jobs.Infrastructure.Jobs;

internal abstract class JobExecution : IJob
{
    protected readonly IEventProcessor _eventProcessor;
    protected readonly IExecutionRepository _executionRepository;
    protected readonly ILogger<JobExecution> _logger;

    protected JobExecution(IEventProcessor eventProcessor, IExecutionRepository executionRepository, ILogger<JobExecution> logger)
    {
        _eventProcessor = eventProcessor;
        _executionRepository = executionRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;

        cancellationToken.ThrowIfCancellationRequested();

        var executionId = context.MergedJobDataMap.GetGuidValue(JobDataMapKeys.ExecutionId);

        var execution = await _executionRepository.GetAsync(executionId);

        try
        {
            _logger.LogDebug("Execution {ExecutionId} started at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);

            await UpdateExecutionStatusAsync(execution, ExecutionStatus.Running, cancellationToken);

            var jobExecutionResult = await ExecuteAsync(new ExecutionContext(execution), cancellationToken);

            var executionStatus = jobExecutionResult.ToExecutionStatus();

            _logger.LogDebug("Execution {ExecutionId} finished at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);

            await UpdateExecutionStatusAsync(execution, executionStatus, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution {ExecutionId} aborted at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);

            await UpdateExecutionStatusAsync(execution, ExecutionStatus.Aborted, cancellationToken);
        }
    }

    public abstract Task<JobExecutionResult> ExecuteAsync(
        ExecutionContext context,
        CancellationToken cancellationToken);

    private async Task UpdateExecutionStatusAsync(
        Execution execution,
        ExecutionStatus status,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        execution.SetStatus(status);
        await _executionRepository.UpdateAsync(execution);
        await _eventProcessor.ProcessAsync(execution.Events);
    }
}

public record ExecutionContext(Execution Execution);
