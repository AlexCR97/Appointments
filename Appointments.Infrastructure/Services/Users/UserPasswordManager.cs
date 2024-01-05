using Appointments.Common.Secrets;
using Appointments.Core.Application.Requests.Users;

namespace Appointments.Core.Infrastructure.Services.Users;

internal sealed class UserPasswordManager : IUserPasswordManager
{
    private const string _userPasswordPrefix = "UserPassword";

    private readonly ISecretManager _secretManager;

    public UserPasswordManager(ISecretManager secretManager)
    {
        _secretManager = secretManager;
    }

    public async Task<string> GetAsync(string email)
    {
        var key = BuildKey(_userPasswordPrefix, email);
        return await _secretManager.GetAsync(key);
    }

    public async Task<string> SetAsync(string email, string password)
    {
        var key = BuildKey(_userPasswordPrefix, email);
        await _secretManager.SetAsync(key, password);
        return key;
    }

    private static string BuildKey(params string[] keyParts)
        => string.Join("__", keyParts);
}
