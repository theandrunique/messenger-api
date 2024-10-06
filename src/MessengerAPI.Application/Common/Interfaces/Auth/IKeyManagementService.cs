using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IKeyManagementService
{
    IReadOnlyDictionary<string, RSA> Keys { get; }
    bool TryGetKeyById(string keyId, [NotNullWhen(true)] out RSA? key);
    (RSA rsa, string keyId) GetRandomKey();
}
