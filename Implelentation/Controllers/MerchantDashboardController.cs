using Microsoft.AspNetCore.Mvc;
using aqay_apis.Dashboards;
using Microsoft.AspNetCore.Authorization;
namespace aqay_apis.Controllers
{
    //[Authorize(Roles = "Merchant")]
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
            try
            {
                var dashboardStatistics = await _merchantDashboardService.GetDashboardStatistics(brandId);
                return Ok(dashboardStatistics);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

    }
}
