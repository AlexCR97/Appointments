using System.Security.Cryptography;

namespace Appointments.Core.Domain.Common.Keys;

public static class KeyGenerator
{
    private const string _charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string Random(int length)
    {

        var bytes = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new char[length];
        var cursor = 0;

        for (var i = 0; i < length; i++)
        {
            cursor += bytes[i];
            result[i] = _charset[cursor % _charset.Length];
        }

        return new string(result);
    }
}
