using Microsoft.Extensions.Options;
using MimeKit;
using SESDemo.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using SESDemo.Interfaces;
using Amazon.SimpleEmail;
using System.Text.Json;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace SESDemo.Services
{
    public class EmailServices : IEmailService
    {

        private readonly MailSetting _mailSetting;
        private readonly AWSAccount _awscredential;
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
        public EmailServices(IOptions<MailSetting> mailSetting, IOptions<AWSAccount> awscredential, IAmazonSimpleEmailService amazonSimpleEmailService)
        {
            _mailSetting = mailSetting.Value;
            _amazonSimpleEmailService = amazonSimpleEmailService;
            _awscredential = awscredential.Value;

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
            catch (Exception ex)
            {
                Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
            }
            return false;

        }

        public async Task<bool> SendTemplateEmailAsync()
        {
            try
            {

                var client = new AmazonSimpleEmailServiceClient(_awscredential.AccessKeyID, _awscredential.Secret, Amazon.RegionEndpoint.APSoutheast1);

                List<BulkEmailDestination> destinations = new  List<BulkEmailDestination>();

                var destination1 = new BulkEmailDestination
                {
                    Destination = new Destination(new List<string> { "danang.kusumayudha@mitrais.com" }),
                    ReplacementTemplateData = "{\"name\":\"Danang\",\"favoriteanimal\":\"Lion\"}"
                };

                var destination2 = new BulkEmailDestination
                {
                    Destination = new Destination(new List<string> { "kusumayudha32@gmail.com" }),
                    ReplacementTemplateData = "{\"name\":\"Eko\",\"favoriteanimal\":\"Cat\"}"
                };

                destinations.Add(destination1);
                destinations.Add(destination2);

                var bulkTemplate = new SendBulkTemplatedEmailRequest
                {
                    Source = _mailSetting.SMTPEmailFrom,
                    Template = "MyTemplate",        
                    DefaultTemplateData = "{}",
                    Destinations = destinations
                };

                await client.SendBulkTemplatedEmailAsync(bulkTemplate);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendTemplateEmailAsync failed with exception: " + ex.Message);
            }

            return false;
        }

    }
}
