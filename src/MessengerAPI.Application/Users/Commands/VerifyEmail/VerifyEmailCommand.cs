using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(long Sub, string Code) : IRequest<ErrorOr<bool>>;
