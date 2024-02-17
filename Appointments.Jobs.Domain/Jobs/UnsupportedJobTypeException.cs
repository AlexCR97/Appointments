using Appointments.Common.Domain.Exceptions;

namespace Appointments.Jobs.Domain.Jobs;

public class UnsupportedJobTypeException : DomainException
{
    public UnsupportedJobTypeException(JobType type)
        : base("UnsupportedJobType", $@"The {nameof(JobType)} ""{type}"" is not supported")
    {
    }
}
