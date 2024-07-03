using aqay_apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace aqay_apis;
[ApiController]
[Route("api/[controller]")]
public class ReportsController: ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IAdminService _adminService;
    public ReportsController(IReportService reportService,IAdminService adminService)
    {
        _reportService = reportService;
        _adminService = adminService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateReport(string title,string intiatorId, string description)
    {
        try
        {
            var report = await _reportService.CreateReportAsync(title, intiatorId, description);
            return CreatedAtAction(nameof(GetReportById),new {id=report.Id},report);
        }
        catch (Exception ex)
        {
            return BadRequest(new{message=ex.Message});
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetReports(int pageIndex)
    {
        var reports= await _reportService.GetReportsAsync(pageIndex);
        return Ok(reports);
    }
    [HttpGet("id")]
    public async Task<IActionResult> GetReportById(int id)
    {
        var report= await _reportService.GetReportByIdAsync(id);
        if (report==null)
        {
            return NotFound();
        }
        return Ok(report);
    }
    [HttpGet("title")]
    public async Task<IActionResult> GetReportByTitle(string title)
    {
        var report= await _reportService.GetReportByTitleAsync(title);
        if (report==null)
        {
            return NotFound();
        }
        return Ok(report);
    }
    [HttpGet("status")]
    public async Task<IActionResult> GetReportByStatus(REPORTSTATUSES reportStatus,int pageIndex)
    {
        var reports= await _reportService.GetReportsByStatusAsync(reportStatus,pageIndex);
        return Ok(reports);
    }
    [HttpPut("open")]
    public async Task<IActionResult> OpenReport(int id,string reviewerId)
    {
        var report=await _reportService.OpenReportAsync(id,reviewerId);
        return Ok(report);
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateReport(int id,string action,REPORTSTATUSES reportStatus)
    {
        var report=await _reportService.UpdateReportAsync(id,action,reportStatus);
        return Ok(report);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteReport(int id)
    {
        var result=await _reportService.DeleteReportAsync(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
    }
    [HttpGet("GetEmailByUserId")]
    public async Task<IActionResult> GetEmailByUserId (string id)
        {
            var email=await _adminService.GetEmailByUserIDAsync(id);
            if (email == null)
            {
                return NotFound();
            }
            return Ok(email);
        }
}
