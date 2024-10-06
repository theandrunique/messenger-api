using System.Diagnostics.CodeAnalysis;
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

    private bool TryGetKeyIdFromJwe(string jwe, [NotNullWhen(true)] out string? kid)
    {
        kid = null;
        try
        {
            var headers = JWE.Headers(jwe);

            // Decoding json from protected header
            var protectedHeaderBytes = headers.ProtectedHeaderBytes;
            string protectedHeaderJson = Encoding.UTF8.GetString(protectedHeaderBytes);

            var protectedHeader = JsonSerializer.Deserialize<Dictionary<string, object>>(protectedHeaderJson);
            if (protectedHeader == null)
            {
                return false;
            }

            if (!protectedHeader.TryGetValue("kid", out var kidObject))
            {
                return false;
            }

            kid = kidObject.ToString();
            if (kid == null)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string Encrypt(RefreshTokenPayload payload)
    {
        var jsonPayload = JsonSerializer.Serialize(payload);
        // Get random key with its id
        var (rsa, keyId) = _keyService.GetRandomKey();

        var recipients = new[]
        {
            new JweRecipient(JweAlgorithm.RSA_OAEP_256, rsa),
        };
        var headers = new Dictionary<string, object>
        {
            { "kid", keyId },
        };
        // Encrypt the payload with given key
        var jwe = JWE.Encrypt(
            jsonPayload,
            recipients,
            JweEncryption.A256GCM,
            mode: SerializationMode.Compact,
            extraProtectedHeaders: headers);

        return jwe;
    }

    public bool TryDecrypt(string token, [NotNullWhen(true)] out RefreshTokenPayload? payload)
    {
        payload = null;
        try
        {
            // Extracting headers
            if (!TryGetKeyIdFromJwe(token, out var kid))
            {
                return false;
            }
            if (!_keyService.TryGetKeyById(kid, out var rsa))
            {
                return false;
            }

            var jwe = JWE.Decrypt(token, rsa);

            payload = JsonSerializer.Deserialize<RefreshTokenPayload>(jwe.Plaintext);
            if (payload == null)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}