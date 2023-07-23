namespace Appointments.Api.Extensions.Files;

internal static class FileExtensions
{
    public static byte[] GetBytes(this IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms.ToArray();
    }
}
