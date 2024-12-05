using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Users.Queries.RequestVerifyEmail;

public record RequestVerifyEmailCommand(Guid Sub) : IRequest<ErrorOr<bool>>;
