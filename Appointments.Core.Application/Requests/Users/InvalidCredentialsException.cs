using Appointments.Common.Domain.Exceptions;

namespace Appointments.Core.Application.Requests.Users;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException()
        : base("InvalidCredentials", "Invalid user credentials")
    {
    }
}
