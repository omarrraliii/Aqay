using Microsoft.AspNetCore.Mvc;
using aqay_apis.Context;
using aqay_apis.Models;
using Microsoft.AspNetCore.Identity;
namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public BrandController(IBrandService brandService, UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _brandService = brandService;
            _userManager = userManager;
            _context = applicationDbContext;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                return Ok(brand);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
         }
        [HttpGet("all brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var paginatedBrands = await _brandService.GetAllBrandsAsync();
                return Ok(paginatedBrands);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Owner,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandDto brandDto)
        {
            try
            {
                var updatedBrand = await _brandService.EditProfileAsync(id, brandDto);
                return Ok(updatedBrand);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize(Roles = "Owner, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var isDeleted = await _brandService.DeleteBrandAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("merchant-info")]
        public async Task<IActionResult> GetBrandOwnerInfo(int id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    return NotFound($"Brand with ID {id} not found.");
                }
                var merchantId = brand.BrandOwnerId;
                var merchant = await _context.Merchants.FindAsync(merchantId);
                if (merchant == null)
                {
                    return NotFound("Merchant information not found.");
                }

                return Ok(merchant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
