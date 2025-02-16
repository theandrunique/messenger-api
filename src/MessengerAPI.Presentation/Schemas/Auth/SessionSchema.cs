namespace MessengerAPI.Presentation.Schemas.Auth;

public record SessionSchema(
    string refreshToken,
    string accessToken,
    string tokenType,
    int expiresIn,
    DateTimeOffset issuedAt);
