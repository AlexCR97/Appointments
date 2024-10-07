using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Jobs;
using Appointments.Jobs.Infrastructure.Mongo.Documents;

namespace Appointments.Jobs.Infrastructure.Mongo.Jobs;

internal sealed class JobDocument : MongoDocument
{
    public const string CollectionName = "jobs-jobs";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public JobDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public JobDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        string type, string group, string name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        Type = type;
        Group = group;
        Name = name;
        DisplayName = displayName;
    }

    public string Type { get; set; }
    public string Group { get; set; }
    public string Name { get; set; }
    public string? DisplayName { get; set; }

    internal static JobDocument From(Job job)
    {
        return new JobDocument(
            job.Id,
            job.CreatedAt,
            job.CreatedBy,
            job.UpdatedAt,
            job.UpdatedBy,
            job.Type.ToString(),
            job.Group.Value,
            job.Name.Value,
            job.DisplayName);
    }

    internal Job ToEntity()
    {
        var jobType = Type.ToEnum<JobType>();

        if (jobType == JobType.Unknown)
            throw new UnsupportedJobTypeException(jobType);

        if (jobType == JobType.LoginMethodConfirmationReminder)
        {
            return new LoginMethodConfirmationReminderJob(
                Id,
                CreatedAt,
                CreatedBy,
                UpdatedAt,
                UpdatedBy,
                new JobGroup(Group),
                new JobName(Name),
                DisplayName);
        }

        throw new UnsupportedJobTypeException(jobType);
    }
}
