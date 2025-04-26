using MediatR;
using Messenger.Application.Users.Common;
using Messenger.Domain.Events;

namespace Messenger.Application.Users.EventHandlers;

public class UserRegisterEventHandler : INotificationHandler<UserCreateDomainEvent>
{
    private readonly IUserSearchService _searchService;

    public UserRegisterEventHandler(IUserSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task Handle(UserCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _searchService.IndexAsync(@event.User, cancellationToken);
    }
}
