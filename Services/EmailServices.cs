using Microsoft.Extensions.Options;
using MimeKit;
using SESDemo.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using SESDemo.Interfaces;

namespace SESDemo.Services
{
    public class EmailServices : IEmailService
    {

        private readonly MailSetting _mailSetting;

        public EmailServices(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;

        }

        public async Task<bool> SendEmailAsync(string address, string subject, string body, string cc)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_mailSetting.SMTPEmailFrom));

                email.To.Add(MailboxAddress.Parse(address));

                email.Cc.Add(MailboxAddress.Parse(cc));

                email.Subject = subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSetting.SMTPHost, _mailSetting.SMTPPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSetting.SMTPUserName, _mailSetting.SMTPPassword);
                string result = await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
