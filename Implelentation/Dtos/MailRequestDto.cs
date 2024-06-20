using System.ComponentModel.DataAnnotations;

namespace aqay_apis;

public class MailRequestDto
{
    [Required]
    public string RecieverEmail { get; set;}
    [Required]
    public string Subject { get; set; }
    [Required]
    public string Body { get; set; }
    public IList<IFormFile>? Attachments  { get; set; }
}
