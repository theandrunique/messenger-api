using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Auth.Commands.Login;

public record LoginCommand(string Login, string Password, string UserAgent, string IpAddress) : IRequest<ErrorOr<LoginCommandResult>>;
