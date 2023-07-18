namespace Appointments.Application.Exceptions;

public class ApplicationException : Exception
{
    public string Code { get; }

    public ApplicationException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
