using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Monolithic.Helpers;

public class SendMailHelper : ISendMailHelper
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<SendMailHelper> _logger;

    public SendMailHelper(IOptions<MailSettings> mailSettings,ILogger<SendMailHelper> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(MailContent mailContent)
    {
        MimeMessage email = new MimeMessage();
        email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(mailContent.ToEmail));
        email.Subject = mailContent.Subject;

        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = mailContent.Body;
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            _logger.LogInformation("Send mail to " + mailContent.ToEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error when sending email to " + mailContent.ToEmail);
            _logger.LogError(ex.Message);
        }

        smtp.Disconnect(true);
    }
}