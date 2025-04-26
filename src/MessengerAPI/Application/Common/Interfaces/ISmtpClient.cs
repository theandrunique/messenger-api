namespace MessengerAPI.Application.Common.Interfaces;

public interface ISmtpClient
{
    Task SendEmailAsync(string to, string subject, string message);
}
