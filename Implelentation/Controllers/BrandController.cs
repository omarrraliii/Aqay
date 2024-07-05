using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
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
        [HttpGet]
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
    }
}
