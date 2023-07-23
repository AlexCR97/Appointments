using Appointments.Application.Services.Files;

namespace Appointments.Infrastructure.Services.FileStorages.LocalStorage;

internal class LocalFileStorage : IFileStorage
{
    private readonly ILocalFileStorageOptions _options;

    public LocalFileStorage(ILocalFileStorageOptions options)
    {
        _options = options;
    }

    public Task DeleteAsync(string path)
    {
        var absolutePath = AbsolutePath(path);
        File.Delete(absolutePath);
        return Task.CompletedTask;
    }

    public Task EnsureDirectoryAsync(string path)
    {
        var absolutePath = AbsolutePath(path);

        if (!Directory.Exists(absolutePath))
            Directory.CreateDirectory(absolutePath);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path)
    {
        var absolutePath = AbsolutePath(path);
        var exists = File.Exists(absolutePath);
        return Task.FromResult(exists);
    }

    public Task<string> GetAsync(string path)
    {
        var absolutePath = AbsolutePath(path);
        return Task.FromResult(absolutePath);
    }

    public async Task WriteAsync(string path, byte[] file)
    {
        var absolutePath = AbsolutePath(path);
        await File.WriteAllBytesAsync(absolutePath, file);
    }

    private string AbsolutePath(string relativePath)
        => Path.Join(_options.StoragePath, relativePath);
}
