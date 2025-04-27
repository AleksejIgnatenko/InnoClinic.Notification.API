using InnoClinic.Notification.Core.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace InnoClinic.Notification.Application.Services;

/// <summary>
/// Provides methods for encrypting and decrypting data using RSA encryption.
/// </summary>
public class EncryptionService : IEncryptionService
{
    /// <summary>
    /// Decrypts the specified encrypted data using RSA with OAEP SHA-256 padding.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a base64-encoded string.</param>
    /// <returns>The decrypted data as a UTF-8 encoded string.</returns>
    /// <exception cref="CryptographicException">Thrown when decryption fails due to invalid data or key.</exception>
    public string DecryptData(string encryptedData)
    {
        using RSA rsa = RSA.Create();
        rsa.FromXmlString(File.ReadAllText("privateKey.xml"));

        byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
        byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}