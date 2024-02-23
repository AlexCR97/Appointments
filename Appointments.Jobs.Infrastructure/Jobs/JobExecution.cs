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
    protected readonly ILogger _logger;

    protected JobExecution(IEventProcessor eventProcessor, IExecutionRepository executionRepository, ILogger logger)
    {
        _eventProcessor = eventProcessor;
        _executionRepository = executionRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var executionId = context.MergedJobDataMap.GetGuidValue(JobDataMapKeys.ExecutionId);
        var execution = await _executionRepository.GetAsync(executionId);

        var manualCancellationToken = context.CancellationToken;
        var timeoutCancellationToken = new CancellationTokenSource(execution.Timeout ?? Execution.DefaultTimeout).Token;
        var linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(manualCancellationToken, timeoutCancellationToken).Token;

        try
        {
            _logger.LogDebug("Execution {ExecutionId} started at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);
            await UpdateExecutionStatusAsync(execution, ExecutionStatus.Running);

            var jobExecutionResult = await ExecuteAsync(new ExecutionContext(execution), linkedCancellationToken);
            
            var executionStatus = jobExecutionResult.ToExecutionStatus();

            _logger.LogDebug("Execution {ExecutionId} finished at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);
            await UpdateExecutionStatusAsync(execution, executionStatus);
        }
        catch (OperationCanceledException)
        {
            if (manualCancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Execution {ExecutionId} cancelled at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);
                await UpdateExecutionStatusAsync(execution, ExecutionStatus.Cancelled);
            }
            else if (timeoutCancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Execution {ExecutionId} timed out at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);
                await UpdateExecutionStatusAsync(execution, ExecutionStatus.TimedOut);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution {ExecutionId} aborted at {ExecutionStartDateTime}", execution.Id, DateTime.UtcNow);
            await UpdateExecutionStatusAsync(execution, ExecutionStatus.Aborted);
        }
    }

    public abstract Task<JobExecutionResult> ExecuteAsync(
        ExecutionContext context,
        CancellationToken cancellationToken);

    private async Task UpdateExecutionStatusAsync(
        Execution execution,
        ExecutionStatus status)
    {
        execution.SetStatus(typeof(JobExecution).FullName ?? typeof(JobExecution).Name, status);
        await _executionRepository.UpdateAsync(execution);
        await _eventProcessor.ProcessAsync(execution.Events);
    }
}

public record ExecutionContext(Execution Execution);
