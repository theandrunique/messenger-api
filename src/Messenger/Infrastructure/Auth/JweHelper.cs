using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Jose;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Core;

namespace Messenger.Infrastructure.Auth;

public class JweHelper : IJweHelper
{
    public bool TryGetKeyIdFromJwe(string jwe, [NotNullWhen(true)] out string? kid)
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

            if (!protectedHeader.TryGetValue(MessengerConstants.Auth.KeyIdHeaderName, out var kidObject))
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

    public string Encrypt(string jsonPayload, RSA recipient, string keyId)
    {
        var recipients = new[]
        {
            new JweRecipient(JweAlgorithm.RSA_OAEP_256, recipient),
        };
        var headers = new Dictionary<string, object>
        {
            { MessengerConstants.Auth.KeyIdHeaderName, keyId },
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

    public bool TryDecrypt(string encryptedData, RSA key, [NotNullWhen(true)] out string? jsonPayload)
    {
        jsonPayload = null;
        try
        {
            var jwe = JWE.Decrypt(encryptedData, key);

            jsonPayload = jwe.Plaintext;
            if (jsonPayload == null)
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