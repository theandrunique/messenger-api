namespace MessengerAPI.Presentation.Schemas.Auth;

/// <summary>
/// Request schema for sign up
/// </summary>
/// <param name="username">Username</param>
/// <param name="globalName">Global name</param>
/// <param name="password">Password</param>
public record SignUpRequestSchema(string username, string globalName, string password);
