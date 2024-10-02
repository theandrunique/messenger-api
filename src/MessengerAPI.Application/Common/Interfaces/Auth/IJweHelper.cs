using System.Diagnostics.CodeAnalysis;

namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJweHelper
{
    /// <summary>
    /// Encrypt refresh token
    /// </summary>
    /// <param name="payload"><see cref="RefreshTokenPayload"/></param>
    /// <returns>Refresh token</returns>
    string Encrypt(RefreshTokenPayload payload);
    /// <summary>
    /// Decrypt refresh token
    /// </summary>
    /// <param name="token">Refresh token to decrypt</param>
    /// <returns><see cref="RefreshTokenPayload"/></returns>
    bool TryDecrypt(string token, [NotNullWhen(true)] out RefreshTokenPayload? payload);
}
