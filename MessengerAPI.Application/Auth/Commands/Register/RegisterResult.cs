using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Application.Auth.Commands.Register;

public record RegisterResult(
    User user
);
