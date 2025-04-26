using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Users.Queries.SearchUsers;

public record SearchUsersQuery(string Query) : IRequest<List<UserPublicSchema>>;
