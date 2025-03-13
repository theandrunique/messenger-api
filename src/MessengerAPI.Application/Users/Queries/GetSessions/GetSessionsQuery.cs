using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Users.Queries.GetSessions;

public record GetSessionsQuery : IRequest<List<SessionSchema>>;
