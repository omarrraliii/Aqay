namespace aqay_apis;
public interface IMailingService
{
    Task SendEmailAsync(string email, string subject, string body,IList<IFormFile> attachments=null);
}
