using System.Security.Cryptography;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IKeyManagementService
{
    IReadOnlyDictionary<string, RSA> Keys { get; }
    RSA? GetKeyById(string keyId);
    RSA GetKey(out string keyId);
    List<RSA> GetKeys();
}
