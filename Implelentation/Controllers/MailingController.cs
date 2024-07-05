using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace aqay_apis;
//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class MailingController:ControllerBase
{
    private readonly IMailingService _mailingService;
    public MailingController(IMailingService mailingService)
    {
        _mailingService= mailingService;
    }
    [HttpPost]
    public async Task<ActionResult> SendMail([FromForm]MailRequestDto dto)
    {
        await _mailingService.SendEmailAsync(dto.RecieverEmail,dto.Subject,dto.Body,dto.Attachments);
        return Ok();
    }
}
