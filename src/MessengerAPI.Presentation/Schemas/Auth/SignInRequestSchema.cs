namespace MessengerAPI.Presentation.Schemas.Auth;

/// <summary>
/// Request schema for sign in
/// </summary>
/// <param name="login">Login</param>
/// <param name="password">Password</param>
public record SignInRequestSchema(string login, string password);
