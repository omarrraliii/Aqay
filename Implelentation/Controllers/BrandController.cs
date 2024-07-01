using aqay_apis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDto brandDto)
        {
            if (brandDto == null)
            {
                return BadRequest("Brand data is null.");
            }

            var createdBrand = await _brandService.CreateBrandAsync(brandDto);
            return CreatedAtAction(nameof(GetBrandById), new { id = createdBrand.Id }, createdBrand);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands([FromQuery] int pageIndex = 1)
        {
            var paginatedBrands = await _brandService.GetAllBrandsAsync(pageIndex);
            return Ok(paginatedBrands);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDto brandDto)
        {
            if (brandDto == null)
            {
                return BadRequest("Brand data is null.");
            }

            var updatedBrand = await _brandService.UpdateBrandAsync(id, brandDto);
            if (updatedBrand == null)
            {
                return NotFound();
            }

            return Ok(updatedBrand);
        }

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
