using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace MessengerAPI.Infrastructure.Auth.Interfaces;

public interface IJweHelper
{
    /// <summary>
    /// Get key id from JSON Web Encryption header
    /// </summary>
    /// <param name="jwe">JSON Web Encryption</param>
    /// <param name="keyId">Key id if found</param>
    /// <returns>True if key id was found</returns>
    bool TryGetKeyIdFromJwe(string jwe, [NotNullWhen(true)] out string? keyId);

    /// <summary>
    /// Encrypt data in JSON Web Encryption
    /// </summary>
    /// <param name="data">Data to encrypt</param>
    /// <param name="recipient">Key to use to encrypt</param>
    /// <param name="keyId">Key id of provided key</param>
    /// <returns>JSON Web Encryption</returns>
    string Encrypt(string data, RSA recipient, string keyId);

    /// <summary>
    /// Decrypt JSON Web Encryption
    /// </summary>
    /// <param name="jwe">JWE data to decrypt</param>
    /// <param name="key">Key to use to decrypt</param>
    /// <param name="data">Encrypted data if successful</param>
    /// <returns>True if decryption was successful</returns>
    public bool TryDecrypt(string jwe, RSA key, [NotNullWhen(true)] out string? data);
}
