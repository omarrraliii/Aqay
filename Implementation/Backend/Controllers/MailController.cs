using Aqay_v2.Dtos;
using Aqay_v2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aqay_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailDto dto)
        {
            await _mailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok();
        }
    }
}
