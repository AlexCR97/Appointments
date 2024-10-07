using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Jobs;

namespace Appointments.Jobs.Infrastructure.UseCases.Jobs;

public sealed record JobDto(
    string Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string Type,
    string Group,
    string Name,
    string? DisplayName);

public static class JobDtoExtensions
{
    public static JobDto ToDto(this Job job)
    {
        return new JobDto(
            job.Id.ToString(),
            job.CreatedAt,
            job.CreatedBy,
            job.UpdatedAt,
            job.UpdatedBy,
            job.Type.ToString(),
            job.Group.Value,
            job.Name.Value,
            job.DisplayName);
    }

    public static Job ToEntity(this JobDto job)
    {
        var jobType = job.Type.ToEnum<JobType>();

        if (jobType == JobType.LoginMethodConfirmationReminder)
        {
            return new LoginMethodConfirmationReminderJob(
                Guid.Parse(job.Id),
                job.CreatedAt,
                job.CreatedBy,
                job.UpdatedAt,
                job.UpdatedBy,
                new JobGroup(job.Group),
                new JobName(job.Name),
                job.DisplayName);
        }

        throw new UnsupportedJobTypeException(jobType);
    }
}
