using Quartz;

namespace Appointments.Jobs.Infrastructure.Jobs;

internal sealed class LoginMethodConfirmationReminderQuartzJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        return Task.CompletedTask;
    }
}
