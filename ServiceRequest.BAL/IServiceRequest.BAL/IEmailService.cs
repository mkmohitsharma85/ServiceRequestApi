namespace ServiceRequest.BAL.IServiceRequest.BAL
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
