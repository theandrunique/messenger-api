using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(long UserId) : IRequest<ErrorOr<UserPublicSchema>>;
