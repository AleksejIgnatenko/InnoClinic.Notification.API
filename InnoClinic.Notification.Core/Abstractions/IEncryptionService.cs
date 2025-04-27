namespace InnoClinic.Notification.Core.Abstractions;

/// <summary>
/// Defines methods for encrypting and decrypting data.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Decrypts the specified encrypted data.
    /// </summary>
    /// <param name="encryptedData">The encrypted data to be decrypted.</param>
    /// <returns>The decrypted data as a string.</returns>
    string DecryptData(string encryptedData);
}