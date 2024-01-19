namespace Appointments.Common.Secrets.Crypto;

internal interface ICryptoOptions
{
    /// <summary>
    /// For 128 bit - Length of the secret key should be 16 for 128 bits key size:<br></br>
    /// mysmallkey123456
    /// <br></br><br></br>
    /// 
    /// For 192 bit - Length of the secret key should be 24 for 192 bits key size:<br></br>
    /// mysmallkey12345512987651
    /// <br></br><br></br>
    /// 
    /// For 256 bit - Length of the secret key should be 32 for 256 bits key size:<br></br>
    /// mysmallkey1234551298765134567890
    /// <br></br><br></br>
    /// 
    /// More info at https://stackoverflow.com/questions/56294646/aes-encryption-in-net-core-3-0-specified-key-is-not-a-valid-size-for-this-algo.
    /// </summary>
    string Key { get; }
}

internal class CryptoOptions : ICryptoOptions
{
    public string Key { get; }

    public CryptoOptions(string key)
    {
        Key = key;
    }
}
