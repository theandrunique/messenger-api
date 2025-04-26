namespace Messenger.Application.Auth.Common;

public record TokenPairResponse(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int ExpiresIn,
    DateTimeOffset IssuedAt);
