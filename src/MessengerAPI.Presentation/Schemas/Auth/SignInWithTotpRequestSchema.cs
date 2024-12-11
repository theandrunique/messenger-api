namespace MessengerAPI.Presentation.Schemas.Auth;

public record SignInWithTotpRequestSchema(string login, string totp);
