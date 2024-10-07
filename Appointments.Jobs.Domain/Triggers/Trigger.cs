using Appointments.Common.Domain;

namespace Appointments.Jobs.Domain.Triggers;

public abstract class Trigger : Entity
{
    public TriggerType Type { get; }
    public TriggerName Name { get; set; }
    public string? DisplayName { get; set; }

    protected Trigger(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        TriggerType type, TriggerName name, string? displayName)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        Type = type;
        Name = name;
        DisplayName = displayName;
    }
}

public readonly struct TriggerName
{
    public string Value { get; }

    public TriggerName()
    {
        Value = string.Empty;
        // TODO Validate
    }

    public TriggerName(string value)
    {
        Value = value;
        // TODO Validate
    }

    public override string ToString()
    {
        return Value;
    }
}
