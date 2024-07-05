using Microsoft.AspNetCore.Mvc;
using aqay_apis.Dashboards;
using Microsoft.AspNetCore.Authorization;

namespace aqay_apis.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AdminDashboardService _adminDashboardService;

        public AdminDashboardController(AdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<AdminDashboardStatistics>> GetAdminDashboardStatistics()
        {
            try
            {
                var adminDashboardStats = await _adminDashboardService.GetAdminDashboardStatistics();
                return Ok(adminDashboardStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
