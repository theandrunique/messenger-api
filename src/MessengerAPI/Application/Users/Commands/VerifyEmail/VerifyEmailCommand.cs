using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(string Code) : IRequest<ErrorOr<Unit>>;
