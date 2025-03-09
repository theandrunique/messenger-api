namespace MessengerAPI.Presentation.Schemas.Auth;

/// <summary>
/// Request schema for sign in
/// </summary>
public record SignInRequestSchema(
    string login,
    string password,
    string? totp);
