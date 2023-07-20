using System.Security.Cryptography;
using System.Text;

namespace Appointments.Common.Secrets.Crypto;

internal interface ICryptoService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

internal class CryptoService : ICryptoService
{
    private readonly ICryptoOptions _options;

    public CryptoService(ICryptoOptions options)
    {
        _options = options;
    }

    private string Key => _options.Key;

    public string Encrypt(string plainText)
    {
        byte[] iv = new byte[16]; // Initialization vector
        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

        using var aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.IV = iv;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using var aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.IV = iv;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
