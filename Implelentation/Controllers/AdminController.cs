using aqay_apis.Models;
using aqay_apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aqay_apis.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("pending merchants")]
        public async Task<ActionResult<IEnumerable<PendingMerchant>>> ListAllPendingMerchants()
        {
            var result = await _adminService.ListAllPendingMerchantsAsync();
            return Ok(result);
        }
        [HttpPost("accept merchant")]
        public async Task<ActionResult<string>> AcceptMerchant(int pendingMerchantId)
        {
            var result = await _adminService.AcceptMerchantAsync(pendingMerchantId);
            if (result == "Pending merchant not found")
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost("reject merchant")]
        public async Task<ActionResult<string>> RejectMerchant( int pendingMerchantId)
        {
            var result = await _adminService.RejectMerchantAsync(pendingMerchantId);
            if (result == "Pending merchant not found")
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost("toggle activity")]
        public async Task<ActionResult<string>> ToggleActivity([FromQuery] string id)
        {
            var result = await _adminService.ToggleActivityAsync(id);
            if (result == "User not found")
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpGet("consumers")]
        public async Task<ActionResult<IEnumerable<Consumer>>> ListAllConsumers()
        {
            var result = await _adminService.ListAllConsumersAsync();
            return Ok(result);
        }
        [HttpGet("merchants")]
        public async Task<ActionResult<IEnumerable<Merchant>>> ListAllMerchants()
        {
            var result = await _adminService.ListAllMerchantsAsync();
            return Ok(result);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<Brand>>> ListAllBrands()
        {
            var result = await _adminService.ListAllBrandsAsync();
            return Ok(result);
        }
    }
}
