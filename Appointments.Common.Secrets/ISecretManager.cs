namespace Appointments.Common.Secrets;

public interface ISecretManager
{
    Task SetAsync(string key, string value);
    Task<string> GetAsync(string key);
    Task DeleteAsync(string key);
}
