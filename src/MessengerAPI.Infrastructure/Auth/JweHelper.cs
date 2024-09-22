using System.Text;
using System.Text.Json;
using Jose;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerAPI.Infrastructure.Auth;

public class JweHelper : IJweHelper
{
    private readonly JwtSettings _settings;
    private readonly Jwk key;

    public JweHelper(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
        key = new Jwk
        {
            Alg = "A256GCMKW",
            K = Base64UrlEncoder.Encode(_settings.Secret),
            Kty = "oct",
            Use = "enc",
        };
    }

    public string Encrypt(RefreshTokenPayload payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);

        var recipients = new[]
        {
            new JweRecipient(JweAlgorithm.A256GCMKW, key),
        };

        var jwe = JWE.Encrypt(jsonPayload, recipients, JweEncryption.A256CBC_HS512, mode: SerializationMode.Compact);

        return jwe;
    }

    public RefreshTokenPayload? Decrypt(string token)
    {
        try
        {
            var jwe = JWE.Decrypt(token, key);

            var data = JsonSerializer.Deserialize<RefreshTokenPayload>(jwe.Plaintext);
            return data;
        }
        catch
        {
            return null;
        }
    }
}