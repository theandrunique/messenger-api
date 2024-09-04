namespace MessengerAPI.Application.Auth.Commands.Login;

public record LoginCommandResult(string RefreshToken, string AccessToken);
