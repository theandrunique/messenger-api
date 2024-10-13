using System.Security.Claims;
using System.Security.Cryptography;

namespace MessengerAPI.Infrastructure.Auth.Interfaces;

public interface IJwtHelper
{
    /// <summary>
    /// Generates JWT token
    /// </summary>
    /// <param name="payload">JWT public payload</param>
    /// <param name="key">Key to use to sign</param>
    /// <param name="keyId">Key id of provided key</param>
    /// <param name="issuer">Issuer</param>
    /// <param name="audience">Audience</param>
    /// <param name="expiryTime">Expiration time</param>
    /// <returns>JSON Web Token</returns>
    string Generate(Claim[] payload, RSA key, string keyId, string issuer, string audience, DateTime expiryTime);
}
