using System.Text;
using System.Text.Json;
using Jose;
using MessengerAPI.Application.Common.Interfaces.Auth;
using Microsoft.Extensions.Options;

namespace MessengerAPI.Infrastructure.Auth;

public class JweHelper : IJweHelper
{
    private readonly JwtSettings _settings;

    public JweHelper(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string Encrypt(Dictionary<string, object> payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);
        // var key = Encoding.UTF8.GetBytes(_settings.Secret);

        var recipients = new[]
        {
            new JweRecipient(JweAlgorithm.PBES2_HS512_A256KW, _settings.Secret),
        };

        var jwe = JWE.Encrypt(jsonPayload, recipients, JweEncryption.A256CBC_HS512, mode: SerializationMode.Compact);

        return jwe;
    }

    public Dictionary<string, object>? Decrypt(string token)
    {
        try
        {
            // var key = Encoding.UTF8.GetBytes(_settings.Secret);

            var jwe = JWE.Decrypt(token, _settings.Secret);

            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(jwe.Plaintext);
            return data;
        }
        catch
        {
            return null;
        }
    }
}