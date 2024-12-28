using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(Guid Sub, string Code) : IRequest<ErrorOr<bool>>;
