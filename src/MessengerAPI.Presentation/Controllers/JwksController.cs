using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Presentation.Controllers;

public class JsonWebKeySet
{
    [JsonPropertyName("keys")]
    public List<JsonWebKey> Keys { get; set; }
}

public class JsonWebKey
{
    [JsonPropertyName("kty")]
    public string KeyType { get; set; }

    [JsonPropertyName("use")]
    public string Use { get; set; }

    [JsonPropertyName("kid")]
    public string KeyId { get; set; }

    [JsonPropertyName("alg")]
    public string Algorithm { get; set; }

    [JsonPropertyName("n")]
    public string N { get; set; }  // RSA Modulus

    [JsonPropertyName("e")]
    public string E { get; set; }  // RSA Exponent
}


public class RsaKeyService
{
    private readonly RSA _privateKey;
    private readonly List<RSA> _privateKeys = new List<RSA>();

    public RsaKeyService()
    {
        for (int i = 0; i < 5; i++)
        {
            _privateKeys.Add(RSA.Create(4096));
        }
        _privateKey = RSA.Create(2048);
    }

    public RSAParameters GetPublicKeyParameters()
    {
        return _privateKey.ExportParameters(false);
    }

    public string GetKeyId()
    {
        // Implement a consistent way to generate a Key ID (kid), e.g., using the SHA-256 hash of the public key
        var keyParams = GetPublicKeyParameters();
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(keyParams.Modulus);
        return Convert.ToBase64String(hash);
    }

    public SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(
            new RsaSecurityKey(_privateKeys[new Random().Next(0, _privateKeys.Count)]),
            SecurityAlgorithms.RsaSha256);
    }
}



[Route(".well-known")]
[AllowAnonymous]
public class JwksController : ApiController
{
    private readonly RsaKeyService _rsaKeyService;

    public JwksController()
    {
        _rsaKeyService = new RsaKeyService();
    }

    [HttpGet("jwks.json")]
    public IActionResult GetJwks()
    {
        var publicKeyParams = _rsaKeyService.GetPublicKeyParameters();
        var jwk = new JsonWebKey
        {
            KeyType = "RSA",
            Use = "sig",
            KeyId = _rsaKeyService.GetKeyId(),
            Algorithm = SecurityAlgorithms.RsaSha256,
            N = Base64UrlEncoder.Encode(publicKeyParams.Modulus),
            E = Base64UrlEncoder.Encode(publicKeyParams.Exponent)
        };

        var jwks = new JsonWebKeySet
        {
            Keys = new List<JsonWebKey> { jwk }
        };

        return Ok(jwks);
    }
}
