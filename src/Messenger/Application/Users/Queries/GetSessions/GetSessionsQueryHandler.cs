using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Domain.Data.Auth;

namespace Messenger.Application.Users.Queries.GetSessions;

public class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, List<SessionSchema>>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetSessionsQueryHandler(ISessionRepository sessionRepository, IClientInfoProvider clientInfo)
    {
        _sessionRepository = sessionRepository;
        _clientInfo = clientInfo;
    }

    public async Task<List<SessionSchema>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _sessionRepository.GetSessionsByUserId(_clientInfo.UserId);

        return SessionSchema.From(sessions);
    }
}
