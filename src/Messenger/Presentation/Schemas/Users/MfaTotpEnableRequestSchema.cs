namespace Messenger.Presentation.Schemas.Users;

public record MfaTotpEnableRequestSchema(string password, string? emailCode);
