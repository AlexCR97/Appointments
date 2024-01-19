namespace Appointments.Assets.Application;

public interface IAssetStore
{
    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);
    Task<string> GetAbsolutePathAsync(string path, CancellationToken cancellationToken = default);
    Task MoveAsync(string oldPath, string newPath, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsync(string path, CancellationToken cancellationToken = default);
    Task WriteAsync(string path, byte[] data, CancellationToken cancellationToken = default);
}
