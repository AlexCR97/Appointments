using Appointments.Common.Domain;
using Appointments.Jobs.Application.UseCases.Executions;
using Microsoft.Extensions.Logging;

namespace Appointments.Jobs.Infrastructure.Jobs;

internal sealed class LoginMethodConfirmationReminderQuartzJob : JobExecution
{
    public LoginMethodConfirmationReminderQuartzJob(IEventProcessor eventProcessor, IExecutionRepository executionRepository, ILogger<JobExecution> logger) : base(eventProcessor, executionRepository, logger)
    {
    }

    public override async Task<JobExecutionResult> ExecuteAsync(ExecutionContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);
        return JobExecutionResult.Succeeded;
    }
}
