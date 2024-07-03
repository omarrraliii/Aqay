using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using aqay_apis.Dashboards;
using aqay_apis.Models; // Assuming MerchantDashboardStatistics and other models are defined here

namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantDashboardController : ControllerBase
    {
        private readonly MerchantDashboardService _merchantDashboardService;

        public MerchantDashboardController(MerchantDashboardService merchantDashboardService)
        {
            _merchantDashboardService = merchantDashboardService;
        }

        [HttpGet("statistics/")]
        public async Task<ActionResult<MerchantDashboardStatistics>> GetDashboardStatistics([FromQuery] int brandId)
        {
            var dashboardStatistics = await _merchantDashboardService.GetDashboardStatistics(brandId);
            return Ok(dashboardStatistics);
        }

    }
}
