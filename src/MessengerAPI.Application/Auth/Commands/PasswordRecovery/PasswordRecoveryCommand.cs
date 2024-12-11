using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Auth.Commands.PasswordRecovery;

public record PasswordRecoveryCommand(string Login) : IRequest<ErrorOr<bool>>;

