namespace Appointments.Jobs.Domain;

public readonly struct CronExpression
{
    public string Value { get; }

    public CronExpression()
    {
        Value = string.Empty;
        // TODO Validate
    }

    public CronExpression(string value)
    {
        Value = value;
        // TODO Validate
    }

    public override string ToString()
    {
        return Value;
    }
}
