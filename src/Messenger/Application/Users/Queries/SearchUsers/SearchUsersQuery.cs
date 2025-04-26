using MediatR;
using Messenger.Contracts.Common;

namespace Messenger.Application.Users.Queries.SearchUsers;

public record SearchUsersQuery(string Query) : IRequest<List<UserPublicSchema>>;
