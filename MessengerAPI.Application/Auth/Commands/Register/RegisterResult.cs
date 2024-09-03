using MessengerAPI.Domain.User;

namespace MessengerAPI.Application.Auth.Commands.Register;

public record RegisterResult(
    User user
);
