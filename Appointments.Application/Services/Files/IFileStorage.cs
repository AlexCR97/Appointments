namespace Appointments.Application.Services.Files;

public interface IFileStorage
{
    /// <summary>
    /// Deletes the specified file.
    /// </summary>
    Task DeleteAsync(string path);
    
    /// <summary>
    /// Ensures that the specified directory exists.
    /// </summary>
    Task EnsureDirectoryAsync(string path);

    /// <summary>
    /// Checks if the specified file exists.
    /// </summary>
    Task<bool> ExistsAsync(string path);

    /// <summary>
    /// Gets the absolute path to the specified file.
    /// </summary>
    Task<string> GetAsync(string path);

    /// <summary>
    /// Writes the file to the specified path.
    /// </summary>
    Task WriteAsync(string path, byte[] file);
}
