using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Messenger.Application.Auth.Common.Interfaces;

public interface IJweHelper
{
    bool TryGetKeyIdFromJwe(string jwe, [NotNullWhen(true)] out string? keyId);

    string Encrypt(string data, RSA recipient, string keyId);

    public bool TryDecrypt(string jwe, RSA key, [NotNullWhen(true)] out string? data);
}
