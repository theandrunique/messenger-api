using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(string Code) : IRequest<ErrorOr<Unit>>;
