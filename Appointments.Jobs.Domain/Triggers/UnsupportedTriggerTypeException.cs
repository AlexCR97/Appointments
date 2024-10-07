using Appointments.Common.Domain.Exceptions;

namespace Appointments.Jobs.Domain.Triggers;

public class UnsupportedTriggerTypeException : DomainException
{
    public UnsupportedTriggerTypeException(TriggerType type)
        : base("UnsupportedTriggerType", $@"The {nameof(TriggerType)} ""{type}"" is not supported")
    {
    }
}
