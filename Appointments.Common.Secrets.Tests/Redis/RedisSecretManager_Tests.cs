using Appointments.Common.Secrets.Redis;
using Appointments.Common.Secrets.Redis.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Appointments.Common.Secrets.Tests.Redis;

public class RedisSecretManager_Tests
{
    private const string _connectionString = "localhost";
    private const string _key = "B9vzAbq5P4oURB5oAzxAQVhsnGegSDym";

    [Fact]
    public async void CanStoreASecret()
    {
        var services = new ServiceCollection()
            .AddRedisSecretManager(new RedisSecretManagerOptions(
                _connectionString,
                _key))
            .BuildServiceProvider();

        var secretManager = services.GetRequiredService<ISecretManager>();

        await secretManager.SetAsync("key", "value");
    }

    [Fact]
    public async void CanStoreMultipleSecrets()
    {
        var services = new ServiceCollection()
            .AddRedisSecretManager(new RedisSecretManagerOptions(
                _connectionString,
                _key))
            .BuildServiceProvider();

        var secretManager = services.GetRequiredService<ISecretManager>();

        await secretManager.SetAsync("key1", "value1");
        await secretManager.SetAsync("key2", "value2");
        await secretManager.SetAsync("key3", "value3");
    }

    [Fact]
    public async void CanGetASecret()
    {
        var services = new ServiceCollection()
            .AddRedisSecretManager(new RedisSecretManagerOptions(
                _connectionString,
                _key))
            .BuildServiceProvider();

        var secretManager = services.GetRequiredService<ISecretManager>();

        await secretManager.SetAsync("key", "value");

        var secret = await secretManager.GetAsync("key");

        Assert.Equal("value", secret);
    }

    [Fact]
    public async void CanDeleteASecret()
    {
        var services = new ServiceCollection()
            .AddRedisSecretManager(new RedisSecretManagerOptions(
                _connectionString,
                _key))
            .BuildServiceProvider();

        var secretManager = services.GetRequiredService<ISecretManager>();

        await secretManager.SetAsync("key", "value");

        await secretManager.DeleteAsync("key");
    }
}
