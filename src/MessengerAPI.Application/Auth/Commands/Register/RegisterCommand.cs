using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

namespace MessengerAPI.Application.Auth.Commands.Register;

/// <summary>
/// Register new user command
/// </summary>
/// <param name="Username">Username</param>
/// <param name="GlobalName">Global name</param>
/// <param name="Password">Password</param>
public record RegisterCommand(
    string Username,
    string GlobalName,
    string Password) : IRequest<ErrorOr<UserPrivateSchema>>;
