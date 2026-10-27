using System.Security.Cryptography;
using System.Text;
using ZU.Training.Application.Security;

namespace ZU.Training.Infrastructure.Security;

public sealed class AesCredentialCipher : ICredentialCipher
{
    private readonly byte[] _key;

    public AesCredentialCipher(string base64Key)
    {
        _key = Convert.FromBase64String(base64Key);
        if (_key.Length != 32)
            throw new ArgumentException("The credential encryption key must contain 32 bytes.", nameof(base64Key));
    }

    public string Encrypt(string plainText)
    {
        var nonce = RandomNumberGenerator.GetBytes(12);
        var plain = Encoding.UTF8.GetBytes(plainText);
        var encrypted = new byte[plain.Length];
        var tag = new byte[16];
        using var aes = new AesGcm(_key, tag.Length);
        aes.Encrypt(nonce, plain, encrypted, tag);
        return "v1:" + Convert.ToBase64String([.. nonce, .. tag, .. encrypted]);
    }

    public string Decrypt(string encryptedText)
    {
        if (!encryptedText.StartsWith("v1:", StringComparison.Ordinal))
            throw new CryptographicException("Unsupported credential format.");

        var value = Convert.FromBase64String(encryptedText[3..]);
        if (value.Length < 28)
            throw new CryptographicException("Invalid credential format.");
        var plain = new byte[value.Length - 28];
        using var aes = new AesGcm(_key, 16);
        aes.Decrypt(value.AsSpan(0, 12), value.AsSpan(28), value.AsSpan(12, 16), plain);
        return Encoding.UTF8.GetString(plain);
    }
}
