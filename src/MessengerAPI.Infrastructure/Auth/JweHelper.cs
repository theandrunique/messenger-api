using System.Text;
using System.Text.Json;
using Jose;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces.Auth;

namespace MessengerAPI.Infrastructure.Auth;

public class JweHelper : IJweHelper
{
    private readonly IKeyManagementService _keyService;

    public JweHelper(IKeyManagementService keyService)
    {
        _keyService = keyService;
    }

    public string Encrypt(RefreshTokenPayload payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);
        var rsa = _keyService.GetKey(out string keyId);

        var recipients = new[]
        {
            new JweRecipient(JweAlgorithm.RSA_OAEP_256, rsa),
        };
        var headers = new Dictionary<string, object>
        {
            { "kid", keyId },
        };

        var jwe = JWE.Encrypt(jsonPayload, recipients, JweEncryption.A256GCM, mode: SerializationMode.Compact, extraProtectedHeaders: headers);

        return jwe;
    }

    public RefreshTokenPayload? Decrypt(string token)
    {
        try
        {
            var headers = JWE.Headers(token);

            var protectedHeaderBytes = headers.ProtectedHeaderBytes;
            string protectedHeaderJson = Encoding.UTF8.GetString(protectedHeaderBytes);
            var protectedHeader = JsonSerializer.Deserialize<Dictionary<string, object>>(protectedHeaderJson);
            if (protectedHeader == null)
            {
                return null;
            }

            if (!protectedHeader.TryGetValue("kid", out var kid))
            {
                return null;
            }

            string? kidString = kid.ToString();
            if (kidString == null)
            {
                return null;
            }

            var key = _keyService.GetKeyById(kidString);

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