using LinFx.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace LinFx.Security.Encryption;

/// <summary>
/// Can be used to simply encrypt/decrypt texts.
/// </summary>
[Service]
public class StringEncryptionService(IOptions<StringEncryptionOptions> options) : IStringEncryptionService
{
    protected StringEncryptionOptions Options { get; } = options.Value;

    public virtual string Encrypt(string plainText, string? passPhrase = null, byte[]? salt = null)
    {
        if (plainText is null)
            throw new ArgumentNullException(nameof(plainText));

        passPhrase ??= Options.DefaultPassPhrase;
        salt ??= Options.DefaultSalt;

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using var password = new Rfc2898DeriveBytes(passPhrase, salt);
        var keyBytes = password.GetBytes(Options.Keysize / 8);
        using var symmetricKey = Aes.Create();
        symmetricKey.Mode = CipherMode.CBC;
        using var encryptor = symmetricKey.CreateEncryptor(keyBytes, Options.InitVectorBytes);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        var cipherTextBytes = memoryStream.ToArray();
        return Convert.ToBase64String(cipherTextBytes);
    }

    public virtual string Decrypt(string cipherText, string? passPhrase = null, byte[]? salt = null)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentException($"“{nameof(cipherText)}”不能为 null 或空。", nameof(cipherText));

        passPhrase ??= Options.DefaultPassPhrase;
        salt ??= Options.DefaultSalt;

        var cipherTextBytes = Convert.FromBase64String(cipherText);
        using var password = new Rfc2898DeriveBytes(passPhrase, salt);
        var keyBytes = password.GetBytes(Options.Keysize / 8);
        using var symmetricKey = Aes.Create();
        symmetricKey.Mode = CipherMode.CBC;
        using var decryptor = symmetricKey.CreateDecryptor(keyBytes, Options.InitVectorBytes);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var plainTextBytes = new byte[cipherTextBytes.Length];
        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    }
}