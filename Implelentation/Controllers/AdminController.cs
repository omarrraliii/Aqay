using aqay_apis.Helpers;
using aqay_apis.Models;
using aqay_apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aqay_apis.Controllers
{
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
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResult<PendingMerchant>>> ListAllPendingMerchants([FromQuery] int pageIndex = 1)
        {
            var result = await _adminService.ListAllPendingMerchantsAsync(pageIndex);
            return Ok(result);
        }

        [HttpPost("accept merchant")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> AcceptMerchant( int pendingMerchantId)
        {
            var result = await _adminService.AcceptMerchantAsync(pendingMerchantId);
            if (result == "Pending merchant not found")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("reject merchant")]
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResult<Consumer>>> ListAllConsumers([FromQuery] int pageIndex = 1)
        {
            var result = await _adminService.ListAllConsumersAsync(pageIndex);
            return Ok(result);
        }

        [HttpGet("merchants")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResult<Merchant>>> ListAllMerchants([FromQuery] int pageIndex = 1)
        {
            var result = await _adminService.ListAllMerchantsAsync(pageIndex);
            return Ok(result);
        }

        [HttpGet("brands")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResult<Brand>>> ListAllBrands([FromQuery] int pageIndex = 1)
        {
            var result = await _adminService.ListAllBrandsAsync(pageIndex);
            return Ok(result);
        }
    }
}
