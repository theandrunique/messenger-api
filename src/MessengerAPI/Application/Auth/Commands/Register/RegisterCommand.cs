using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.Register;

/// <summary>
/// Register new user command
/// </summary>
/// <param name="Username">Username</param>
/// <param name="Email">Email</param>
/// <param name="GlobalName">Global name</param>
/// <param name="Password">Password</param>
public record RegisterCommand(
    string Username,
    string Email,
    string GlobalName,
    string Password) : IRequest<ErrorOr<UserPrivateSchema>>;
