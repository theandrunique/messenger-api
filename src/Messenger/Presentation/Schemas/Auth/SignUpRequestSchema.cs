namespace Messenger.Presentation.Schemas.Auth;

/// <summary>
/// Request schema for sign up
/// </summary>
public record SignUpRequestSchema(
    string username,
    string email,
    string globalName,
    string password);
