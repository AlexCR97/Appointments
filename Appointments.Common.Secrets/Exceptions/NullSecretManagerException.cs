namespace Appointments.Common.Secrets.Exceptions;

public class NullSecretManagerException : SecretsException
{
    public NullSecretManagerException()
        : base("NullSecretManager", $"You are using a null secret manager. Was there an error with your initial configuration?")
    {
    }
}
