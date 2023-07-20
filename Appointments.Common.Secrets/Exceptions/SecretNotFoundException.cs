namespace Appointments.Common.Secrets.Exceptions;

public class SecretNotFoundException : SecretsException
{
    public SecretNotFoundException(string key)
        : base("NotFound", $@"Could not find secret with key ""{key}"".")
    {
    }
}
