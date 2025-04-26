using MediatR;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Users.EventHandlers;

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
