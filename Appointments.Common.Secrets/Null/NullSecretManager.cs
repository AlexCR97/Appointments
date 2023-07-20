using Appointments.Common.Secrets.Exceptions;

namespace Appointments.Common.Secrets.Null;

internal class NullSecretManager : ISecretManager
{
    public Task DeleteAsync(string key)
    {
        throw new NullSecretManagerException();
    }

    public Task<string> GetAsync(string key)
    {
        throw new NullSecretManagerException();
    }

    public Task SetAsync(string key, string value)
    {
        throw new NullSecretManagerException();
    }
}
