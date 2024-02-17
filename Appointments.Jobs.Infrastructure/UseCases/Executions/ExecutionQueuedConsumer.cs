using Appointments.Jobs.Domain;
using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Domain.Triggers;
using Appointments.Jobs.Infrastructure.Jobs;
using MassTransit;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Appointments.Jobs.Infrastructure.UseCases.Executions;

internal sealed class ExecutionQueuedConsumer : IConsumer<ExecutionQueuedEvent>
{
    private readonly ILogger<ExecutionQueuedConsumer> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public ExecutionQueuedConsumer(ILogger<ExecutionQueuedConsumer> logger, ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task Consume(ConsumeContext<ExecutionQueuedEvent> context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var jobDetail = CreateJobDetail(context.Message.JobSnapshot);
        var quartzTrigger = CreateTrigger(context.Message.TriggerSnapshot);

        var _scheduler = await _schedulerFactory.GetScheduler(context.CancellationToken);
        await _scheduler.ScheduleJob(jobDetail, quartzTrigger);
    }

    private static IJobDetail CreateJobDetail(Job job)
    {
        if (job.Type == JobType.Unknown)
            throw new UnsupportedJobTypeException(JobType.Unknown);

        if (job.Type == JobType.Unknown)
        {
            return JobBuilder
                .Create<LoginMethodConfirmationReminderQuartzJob>()
                .WithIdentity(job.Name.Value, job.Group.Value)
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
