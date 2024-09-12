using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Username,
    string GlobalName,
    string Password) : IRequest<ErrorOr<UserPrivateSchema>>;
