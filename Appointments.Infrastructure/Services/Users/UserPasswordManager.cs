using Appointments.Application.Requests.Users;
using Appointments.Common.Secrets;

namespace Appointments.Infrastructure.Services.Users;

internal class UserPasswordManager : IUserPasswordManager
{
    private const string _userPasswordPrefix = "UserPassword";

    private readonly ISecretManager _secretManager;

    public UserPasswordManager(ISecretManager secretManager)
    {
        _secretManager = secretManager;
    }

    public async Task<string> GetAsync(Guid userId)
    {
        var key = BuildKey(_userPasswordPrefix, userId.ToString());
        return await _secretManager.GetAsync(key);
    }

    public async Task<string> SetAsync(Guid userId, string password)
    {
        var key = BuildKey(_userPasswordPrefix, userId.ToString());
        await _secretManager.SetAsync(key, password);
        return key;
    }

    private static string BuildKey(params string[] keyParts)
        => string.Join("__", keyParts);
}
