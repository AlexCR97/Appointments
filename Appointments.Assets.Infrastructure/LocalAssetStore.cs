using Appointments.Assets.Application;

namespace Appointments.Assets.Infrastructure;

public sealed record LocalAssetStoreOptions(
    string StoragePath);

internal sealed class LocalAssetStore : IAssetStore
{
    private readonly LocalAssetStoreOptions _options;

    public LocalAssetStore(LocalAssetStoreOptions options)
    {
        _options = options;
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var absolutePath = AbsolutePath(path);
        File.Delete(absolutePath);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var absolutePath = AbsolutePath(path);
        var exists = File.Exists(absolutePath);
        return Task.FromResult(exists);
    }

    public Task<string> GetAbsolutePathAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var absolutePath = AbsolutePath(path);
        return Task.FromResult(absolutePath);
    }

    public async Task MoveAsync(string oldPath, string newPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var data = await ReadAsync(oldPath, cancellationToken);
        await WriteAsync(newPath, data, cancellationToken);
        await DeleteAsync(oldPath, cancellationToken);
    }

    public async Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var absolutePath = AbsolutePath(path);
        return await File.ReadAllBytesAsync(absolutePath, cancellationToken);
    }

    public async Task WriteAsync(string path, byte[] data, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var absolutePath = AbsolutePath(path);
        await EnsureDirectoryAsync(absolutePath, cancellationToken);
        await File.WriteAllBytesAsync(absolutePath, data, cancellationToken);
    }

    private string AbsolutePath(string relativePath)
        => Path.Join(_options.StoragePath, relativePath);

    private static Task EnsureDirectoryAsync(string absolutePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(absolutePath))
            Directory.CreateDirectory(absolutePath);

        return Task.CompletedTask;
    }
}
