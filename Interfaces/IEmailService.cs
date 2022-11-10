namespace SESDemo.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string address, string subject, string body, string cc);
    }
}
