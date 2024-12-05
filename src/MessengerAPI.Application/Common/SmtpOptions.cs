namespace MessengerAPI.Application.Common;

public class SmtpOptions
{
    public int Port { get; init; }
    public string Server { get; init; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public bool EnableSsl { get; set; }
}
