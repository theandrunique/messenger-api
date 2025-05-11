using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Infrastructure.Auth;

public class KeyManagementService : IKeyManagementService
{
    private readonly Dictionary<string, RSA> _keys = new();
    public IReadOnlyDictionary<string, RSA> Keys => _keys.AsReadOnly();

    public KeyManagementService(IOptions<ApplicationOptions> options)
    {
        LoadKeys(options.Value.AuthOptions.KeysDirectory);
    }

    private void LoadKeys(string keysDirectory)
    {
        try
        {
            foreach (var pemPath in Directory.GetFiles(keysDirectory))
            {
                var rsa = RSA.Create();
                var pem = File.ReadAllText(pemPath);
                rsa.ImportFromPem(pem);

                var keyId = GetKeyThumbprint(rsa);

                _keys[keyId] = rsa;
            }
            if (_keys.Count == 0)
            {
                throw new Exception("No keys were found");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load keys.", ex);
        }
    }

    private string GetKeyThumbprint(RSA rsa)
    {
        using var sha256 = SHA256.Create();
        var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
        var hash = sha256.ComputeHash(publicKeyBytes);
        return Base64UrlEncoder.Encode(hash);
    }

    public bool TryGetKeyById(string keyId, [NotNullWhen(true)] out RSA? key)
    {
        return _keys.TryGetValue(keyId, out key);
    }

    public (RSA rsa, string keyId) GetRandomKey()
    {
        var random = new Random();
        var key = _keys.ElementAt(random.Next(0, _keys.Count));
        return (key.Value, key.Key);
    }
}
