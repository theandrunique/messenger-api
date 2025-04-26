using MediatR;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Users.EventHandlers;

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
