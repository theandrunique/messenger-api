using System.Security.Claims;
using System.Security.Cryptography;

namespace Messenger.Application.Auth.Common.Interfaces;

public interface IJwtHelper
{
    string Generate(Claim[] payload, RSA key, string keyId, string issuer, string audience, DateTimeOffset expiryTime);
}
