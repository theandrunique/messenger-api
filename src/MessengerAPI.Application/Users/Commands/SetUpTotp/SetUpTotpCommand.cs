using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.SetUpTotp;

public record SetUpTotpCommand() : IRequest<ErrorOr<SetUpTotpCommandResponse>>;
