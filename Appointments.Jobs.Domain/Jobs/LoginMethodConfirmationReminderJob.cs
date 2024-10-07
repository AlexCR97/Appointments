namespace Appointments.Jobs.Domain.Jobs;

public class LoginMethodConfirmationReminderJob : Job
{
    public LoginMethodConfirmationReminderJob(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        JobGroup group, JobName name, string? displayName)
    : base(id, createdAt, createdBy, updatedAt, updatedBy,
        JobType.LoginMethodConfirmationReminder, group, name, displayName)
    {
    }

    public static LoginMethodConfirmationReminderJob Create(string createdBy, JobGroup group, JobName name, string? displayName)
    {
        var job = new LoginMethodConfirmationReminderJob(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            group,
            name,
            displayName);

        job.AddEvent(new JobCreatedEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            job.CreatedBy,
            job.Group,
            job.Name,
            job.DisplayName));

        return job;
    }
}
