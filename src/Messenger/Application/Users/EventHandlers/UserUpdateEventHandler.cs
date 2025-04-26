using MediatR;
using Messenger.Application.Users.Common;
using Messenger.Domain.Events;

namespace Messenger.Application.Users.EventHandlers;

public class UserUpdateEventHandler : INotificationHandler<UserUpdateDomainEvent>
{
    private readonly IUserSearchService _searchService;

    public UserUpdateEventHandler(IUserSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task Handle(UserUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _searchService.UpdateAsync(@event.User, cancellationToken);
    }
}
