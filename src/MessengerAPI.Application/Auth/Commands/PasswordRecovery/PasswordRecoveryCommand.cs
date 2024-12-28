using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.PasswordRecovery;

public record PasswordRecoveryCommand(string Login) : IRequest<ErrorOr<bool>>;

