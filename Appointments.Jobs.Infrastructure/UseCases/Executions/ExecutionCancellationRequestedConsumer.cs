using Appointments.Jobs.Infrastructure.UseCases.Jobs;
using Appointments.Jobs.Infrastructure.UseCases.Triggers;
using MassTransit;
using Quartz;

namespace Appointments.Jobs.Infrastructure.UseCases.Executions;

internal sealed class ExecutionCancellationRequestedConsumer : IConsumer<ExecutionCancellationRequestedEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;
    
    public ExecutionCancellationRequestedConsumer(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Consume(ConsumeContext<ExecutionCancellationRequestedEvent> context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var job = context.Message.JobSnapshot.ToEntity();

        var scheduler = await _schedulerFactory.GetScheduler(context.CancellationToken);

        await scheduler.Interrupt(JobKeyFactory.CreateJobKey(job));
    }
}
