using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace MessengerAPI.Application.Auth.Common.Interfaces;

public interface IKeyManagementService
{
    /// <summary>
    /// Dictionary of RSA keys (with private data) existed in the system, where key is a key id
    /// </summary>
    IReadOnlyDictionary<string, RSA> Keys { get; }
    /// <summary>
    /// Try get RSA key by key id
    /// </summary>
    /// <param name="keyId">Key id</param>
    /// <param name="key">RSA key</param>
    /// <returns>true if key with given id exists</returns>
    bool TryGetKeyById(string keyId, [NotNullWhen(true)] out RSA? key);
    /// <summary>
    /// Get random RSA key from existing keys
    /// </summary>
    /// <returns>RSA key and its id</returns>
    (RSA rsa, string keyId) GetRandomKey();
}
