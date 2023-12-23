using Appointments.Domain.Exceptions;

namespace Appointments.Application.Requests.Users;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException()
        : base("InvalidCredentials", "Invalid user credentials")
    {
    }
}
