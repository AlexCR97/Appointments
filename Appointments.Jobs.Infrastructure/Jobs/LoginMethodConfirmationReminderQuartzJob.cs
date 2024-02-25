using Appointments.Common.Domain;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Infrastructure.Elasticsearch.Executions;
using Microsoft.Extensions.Logging;

namespace Appointments.Jobs.Infrastructure.Jobs;

internal sealed class LoginMethodConfirmationReminderQuartzJob : JobExecution
{
    public LoginMethodConfirmationReminderQuartzJob(IEventProcessor eventProcessor, IExecutionRepository executionRepository, ILogger<LoginMethodConfirmationReminderQuartzJob> logger, ExecutionLoggerFactory executionLoggerFactory) : base(eventProcessor, executionRepository, logger, executionLoggerFactory)
    {
    }

    public override Task<JobExecutionResult> ExecuteAsync(ExecutionContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        context.Logger.Debug("Execution started!");

        context.Logger.Debug("Execution succeeded!");

        return Task.FromResult(JobExecutionResult.Succeeded);
    }
}
