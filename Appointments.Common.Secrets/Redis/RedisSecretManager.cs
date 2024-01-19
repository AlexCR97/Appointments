using Appointments.Common.Secrets.Crypto;
using Appointments.Common.Secrets.Exceptions;
using StackExchange.Redis;

namespace Appointments.Common.Secrets.Redis;

internal class RedisSecretManager : ISecretManager
{
    private readonly IDatabase _database;
    private readonly ICryptoService _cryptoService;

    public RedisSecretManager(IDatabase database, ICryptoService cryptoService)
    {
        _database = database;
        _cryptoService = cryptoService;
    }

    public async Task SetAsync(string key, string value)
    {
        var encryptedValue = _cryptoService.Encrypt(value);
        await _database.StringSetAsync(key, encryptedValue);
    }

    public async Task<string> GetAsync(string key)
    {
        var redisValue = await _database.StringGetAsync(key);

        if (redisValue.IsNull)
            throw new SecretNotFoundException(key);

        var encryptedValue = redisValue.ToString();

        return _cryptoService.Decrypt(encryptedValue);
    }

    public async Task DeleteAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
