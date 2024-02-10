using System.ComponentModel.DataAnnotations;

namespace Aqay_v2.Dtos
{
    public class MailDto
    {
        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}
