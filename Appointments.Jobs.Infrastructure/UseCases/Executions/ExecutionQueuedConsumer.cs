using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Domain.Triggers;
using Appointments.Jobs.Infrastructure.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Triggers;
using MassTransit;
using Quartz;

namespace Appointments.Jobs.Infrastructure.UseCases.Executions;

internal sealed class ExecutionQueuedConsumer : IConsumer<ExecutionQueuedEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public ExecutionQueuedConsumer(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Consume(ConsumeContext<ExecutionQueuedEvent> context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var jobSnapshot = context.Message.JobSnapshot.ToEntity();
        var jobDetail = CreateJobDetail(jobSnapshot, context.Message.ExecutionId);

        var triggerSnapshot = context.Message.TriggerSnapshot.ToEntity();
        var quartzTrigger = CreateTrigger(triggerSnapshot);

        var _scheduler = await _schedulerFactory.GetScheduler(context.CancellationToken);
        await _scheduler.ScheduleJob(jobDetail, quartzTrigger);
    }

    private static IJobDetail CreateJobDetail(Job job, Guid executionId)
    {
        if (job.Type == JobType.Unknown)
            throw new UnsupportedJobTypeException(JobType.Unknown);

        if (job.Type == JobType.LoginMethodConfirmationReminder)
        {
            return JobBuilder
                .Create<LoginMethodConfirmationReminderQuartzJob>()
                .WithIdentity(job.Name.Value, job.Group.Value)
                .UsingJobData(JobDataMapKeys.ExecutionId, executionId)
                .Build();
        }

        throw new UnsupportedJobTypeException(JobType.Unknown);
    }

    private static ITrigger CreateTrigger(Trigger trigger)
    {
        if (trigger.Type == TriggerType.FireAndForget)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity(trigger.Name.Value)
                .StartNow()
                .Build();
        }

        throw new UnsupportedTriggerTypeException(trigger.Type);
    }
}
