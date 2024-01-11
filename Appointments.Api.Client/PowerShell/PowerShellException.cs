namespace Appointments.Api.Client.PowerShell;

public class PowerShellException : Exception
{
    public PowerShellException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

    public PowerShellException WithData(string key, object? value)
    {
        Data.Add(key, value);
        return this;
    }
}
