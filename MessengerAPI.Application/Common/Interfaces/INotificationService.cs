namespace MessengerAPI.Application.Common.Interfaces;

public interface INotificationService
{
    Task Notify(NotificationMessage message);
}
