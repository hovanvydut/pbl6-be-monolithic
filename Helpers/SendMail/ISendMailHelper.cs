namespace Monolithic.Helpers;

public interface ISendMailHelper
{
    Task SendEmailAsync(MailContent mailContent);
}