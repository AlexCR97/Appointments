namespace Appointments.Common.Secrets.Exceptions;

public class SecretsException : Exception
{
    public string Code { get; }

    public SecretsException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
