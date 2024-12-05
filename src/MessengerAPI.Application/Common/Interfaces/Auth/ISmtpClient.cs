namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface ISmtpClient
{
    Task SendEmailAsync(string to, string subject, string message);
}
