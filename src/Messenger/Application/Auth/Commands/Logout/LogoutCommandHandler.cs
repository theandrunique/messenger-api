using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Data.Auth;

namespace Messenger.Application.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IClientInfoProvider _clientInfo;

    public LogoutCommandHandler(ISessionRepository sessionRepository, IClientInfoProvider clientInfo)
    {
        _sessionRepository = sessionRepository;
        _clientInfo = clientInfo;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByTokenIdOrNullAsync(_clientInfo.TokenId);
        if (session == null)
        {
            throw new Exception("Session was expected to be found here.");
        }

        await _sessionRepository.RemoveByIdAsync(session.UserId, session.Id);
        return Unit.Value;
    }
}
