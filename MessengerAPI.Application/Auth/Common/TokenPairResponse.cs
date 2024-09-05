namespace MessengerAPI.Application.Auth.Common;

public record TokenPairResponse(
    string AccessToken,
    string RefreshToken,
    string TokenType = "Bearer",
    int ExpiresIn = 3600);
