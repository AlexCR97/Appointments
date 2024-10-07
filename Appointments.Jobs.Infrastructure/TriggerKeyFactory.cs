using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Domain.Triggers;
using Quartz;

namespace Appointments.Jobs.Infrastructure;

internal static class TriggerKeyFactory
{
    public static TriggerKey CreateTriggerKey(Trigger trigger, Job job)
    {
        return new TriggerKey(trigger.Name.Value, job.Id.ToString());
    }
}
