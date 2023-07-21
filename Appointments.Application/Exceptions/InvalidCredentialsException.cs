namespace Appointments.Application.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException()
        : base("InvalidCredentials", "The user's credentials are invalid.")
    {
    }
}
