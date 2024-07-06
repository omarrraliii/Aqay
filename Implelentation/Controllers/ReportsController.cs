using aqay_apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace aqay_apis;
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IAdminService _adminService;
    public ReportsController(IReportService reportService, IAdminService adminService)
    {
        _reportService = reportService;
        _adminService = adminService;
    }
    [HttpPost("CreateReport")]
    public async Task<IActionResult> CreateReport([FromBody] ReportDto request)
    {
        try
        {
            var report = await _reportService.CreateReportAsync(request.Title, request.InitiatorId, request.Description);
            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetReports()
    {
        try
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(reports);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("id")]
    public async Task<IActionResult> GetReportById(int id)
    {
        try
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("title")]
    public async Task<IActionResult> GetReportByTitle(string title)
    {
        try
        {
            var report = await _reportService.GetReportByTitleAsync(title);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("status")]
    public async Task<IActionResult> GetReportByStatus(REPORTSTATUSES reportStatus)
    {
        try
        {
            var reports = await _reportService.GetReportsByStatusAsync(reportStatus);
            return Ok(reports);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //[Authorize(Roles = "Admin")]
    [HttpPut("open")]
    public async Task<IActionResult> OpenReport(int id, string reviewerId)
    {
        try
        {
            var report = await _reportService.OpenReportAsync(id, reviewerId);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //[Authorize(Roles = "Admin")]
    [HttpPut("updateAction")]
    public async Task<IActionResult> UpdateReportAction(int id, string? action)
    {
        try
        {
            if (action.IsNullOrEmpty())
            {
                return Ok();
            }
            var report = await _reportService.UpdateReportActionAsync(id, action);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //[Authorize(Roles = "Admin")]
    [HttpPut("updateStatus")]
    public async Task<IActionResult> UpdateReportStatus(int id)
    {
        try
        {
            var report = await _reportService.UpdateReportStatusAsync(id);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //[Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteReport(int id)
    {
        try
        {
            var result = await _reportService.DeleteReportAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetEmailByUserId")]
    public async Task<IActionResult> GetEmailByUserId(string id)
    {
        try
        {
            var email = await _adminService.GetEmailByUserIDAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return Ok(email);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
