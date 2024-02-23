using Appointments.Jobs.Domain.Jobs;
using Quartz;

namespace Appointments.Jobs.Infrastructure.UseCases.Executions;

internal static class JobKeyFactory
{
    public static JobKey CreateJobKey(Job job)
    {
        return new JobKey(job.Id.ToString(), job.Group.Value);
    }
}
