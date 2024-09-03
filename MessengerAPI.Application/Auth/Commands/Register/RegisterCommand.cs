using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Username,
    string GlobalName,
    string Password) : IRequest<ErrorOr<RegisterResult>>;
