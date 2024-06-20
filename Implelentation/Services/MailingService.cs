
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace aqay_apis;

public class MailingService : IMailingService
{
    private readonly MailSettings _mailSettings;
    public MailingService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings=mailSettings.Value;
    }
    public async Task SendEmailAsync(string reciever, string subject, string body, IList<IFormFile> attachments = null)
    {
        //initialize sender and subject
        var email= new MimeMessage 
        {
            Sender=MailboxAddress.Parse(_mailSettings.Email),
            Subject=subject
        };
        //initialize reciever
        email.To.Add(MailboxAddress.Parse(reciever)); 
        var builder = new BodyBuilder();
        //check for attachments and loop on them
        if (attachments != null)                                        
        {
            byte[] fileBytes;
            foreach (var file in attachments)
            {
                if (file.Length > 0)
                {
                    using var ms= new MemoryStream();
                    file.CopyTo(ms);
                    fileBytes= ms.ToArray();
                    builder.Attachments.Add(file.FileName,fileBytes,ContentType.Parse(file.ContentType));
                }
            }
        }
        //prepare the rest of the body
        builder.HtmlBody=body;
        email.Body=builder.ToMessageBody();
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName,_mailSettings.Email));
        //connect to the SMTP
        using var smtp=new SmtpClient();
        smtp.Connect(_mailSettings.Host,_mailSettings.Port,SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Email,_mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}
