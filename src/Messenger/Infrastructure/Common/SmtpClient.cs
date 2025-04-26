using System.Net;
using System.Net.Mail;
using Messenger.Application.Common;
using Messenger.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Common;

public class SmtpClient : ISmtpClient
{
    private readonly SmtpOptions _options;

    public SmtpClient(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }

    public Task SendEmailAsync(string recipient, string subject, string body)
    {
        MailAddress from = new MailAddress(_options.FromEmail, _options.FromName);

        MailAddress to = new MailAddress(recipient);

        MailMessage m = new MailMessage(from, to);

        m.Subject = subject;

        m.Body = body;

        m.IsBodyHtml = true;

        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(_options.Server, _options.Port);

        smtp.Credentials = new NetworkCredential(_options.Login, _options.Password);
        smtp.EnableSsl = _options.EnableSsl;
        return smtp.SendMailAsync(m);
    }
}
