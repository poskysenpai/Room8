namespace Room8.Core.Abstractions
{
    public interface IEmailService
    {
        Task<string> SendEmail(string recipientEmail, string subject, string body);

    }
}
