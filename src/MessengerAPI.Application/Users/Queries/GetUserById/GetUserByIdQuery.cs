using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(long UserId) : IRequest<ErrorOr<UserPublicSchema>>;
