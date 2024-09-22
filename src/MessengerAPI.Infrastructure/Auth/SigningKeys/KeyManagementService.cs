using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth.SigningKeys;

public class KeyManagementService
{
    private readonly Dictionary<string, RSA> _keys = new();

    public KeyManagementService()
    {
        LoadKeys();
    }

    private void LoadKeys()
    {
        foreach (var file in Directory.GetFiles("./keys", "*.pem"))
        {
            var rsa = RSA.Create();
            var pem = File.ReadAllText(file);
            rsa.ImportFromPem(pem.ToCharArray());

            var keyId = GetKeyThumbprint(rsa);

            _keys[keyId] = rsa;
        }
    }

    private string GetKeyThumbprint(RSA rsa)
    {
        using var sha256 = SHA256.Create();
        var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
        var hash = sha256.ComputeHash(publicKeyBytes);
        return Base64UrlEncoder.Encode(hash);
    }

    public RSA? GetKeyById(string keyId)
    {
        return _keys.ContainsKey(keyId) ? _keys[keyId] : null;
    }

    public RSA GetKey(out string keyId)
    {
        var random = new Random();
        var key = _keys.ElementAt(random.Next(0, _keys.Count));
        keyId = key.Key;
        return key.Value;
    }

    public List<RSA> GetKeys()
    {
        return _keys.Values.ToList();
    }
}
