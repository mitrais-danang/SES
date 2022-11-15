namespace SESDemo.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string address, string subject, string body, string cc);
    }
}
