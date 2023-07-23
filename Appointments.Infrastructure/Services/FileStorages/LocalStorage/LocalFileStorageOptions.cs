namespace Appointments.Infrastructure.Services.FileStorages.LocalStorage;

public interface ILocalFileStorageOptions
{
    string StoragePath { get; }
}

internal record LocalFileStorageOptions(
    string StoragePath) : ILocalFileStorageOptions;
