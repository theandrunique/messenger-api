namespace MessengerAPI.Application.Auth.Common;

public record TokenPairResponse(
    string AccessToken,
    string RefreshToken);
