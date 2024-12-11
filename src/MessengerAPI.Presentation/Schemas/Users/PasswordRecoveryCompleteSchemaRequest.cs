namespace MessengerAPI.Presentation.Schemas.Users;

public record PasswordRecoveryCompleteSchemaRequest(string login, string code, string newPassword);
