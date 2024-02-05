namespace Aqay_v2.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
    }
}
