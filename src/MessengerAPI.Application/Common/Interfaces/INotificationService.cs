using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Send notification
    /// </summary>
    /// <param name="jsonMessage">message in json</param>
    /// <param name="recipientIds">list of recipient ids</param>
    /// <returns></returns>
    Task Notify(List<UserId> recipientIds, string jsonMessage);
}
