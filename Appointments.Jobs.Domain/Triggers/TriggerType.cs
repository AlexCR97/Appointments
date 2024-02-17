namespace Appointments.Jobs.Domain.Triggers;

public enum TriggerType
{
    /// <summary>
    /// Run the job a single time as soon as possible.
    /// </summary>
    FireAndForget,

    /// <summary>
    /// Run the job at a specific time.
    /// </summary>
    Scheduled,

    /// <summary>
    /// Run the job on a cron schedule.
    /// </summary>
    Cron,

    /// <summary>
    /// Run the job when a webhook is triggered.
    /// </summary>
    Webhook,

    /// <summary>
    /// Run the job when a RabbitMQ message is received.
    /// </summary>
    RabbitMq,
}
