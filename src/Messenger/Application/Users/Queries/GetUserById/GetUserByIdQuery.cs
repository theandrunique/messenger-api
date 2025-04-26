using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(long UserId) : IRequest<ErrorOr<UserPublicSchema>>;
