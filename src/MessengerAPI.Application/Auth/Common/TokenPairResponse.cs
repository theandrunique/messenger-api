namespace MessengerAPI.Application.Auth.Common;

/// <summary>
/// Pair of access and refresh tokens
/// </summary>
/// <param name="AccessToken">Access token</param>
/// <param name="RefreshToken">Refresh token</param>
/// <param name="TokenType">Token type</param>
/// <param name="ExpiresIn">Expiration time</param>
public record TokenPairResponse(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int ExpiresIn);
