using Appointments.Common.Domain;

namespace Appointments.Jobs.Domain.Jobs;

public abstract class Job : Entity
{
    protected Job(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, JobType type, JobGroup group, JobName name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        Type = type;
        Group = group;
        Name = name;
        DisplayName = displayName;
    }

    public JobType Type { get; }
    public JobGroup Group { get; private set; }
    public JobName Name { get; private set; }
    public string? DisplayName { get; private set; }
}

public readonly struct JobGroup
{
    public string Value { get; }

    public JobGroup()
    {
        Value = string.Empty;
        // TODO Validate
    }

    public JobGroup(string value)
    {
        Value = value;
        // TODO Validate
    }

    public override string ToString()
    {
        return Value;
    }
}

public readonly struct JobName
{
    public string Value { get; }

    public JobName()
    {
        Value = string.Empty;
        // TODO Validate
    }
    public JobName(string value)
    {
        Value = value;
        // TODO Validate
    }

    public override string ToString()
    {
        return Value;
    }
}
