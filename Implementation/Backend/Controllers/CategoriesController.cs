using Aqay_v2.Dtos;
using Aqay_v2.Models;
using Aqay_v2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aqay_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            var Categories = await _categoryService.ReadAll();  
            return Ok(Categories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(string Name)
        {
            var category = new Category { Name = Name };
            await _categoryService.Create(category);
            return Ok(category);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync ([FromBody] CategoryUpdateDto model)
        {
            
            var existingCategory = await _categoryService.ReadByName(model.Name);

            if (existingCategory == null)
            {
                return NotFound($"Category with name '{model.Name}' not found");
            }
            existingCategory.Name = model.NewName;
            _categoryService.Update(existingCategory);
            return Ok(existingCategory);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] string Name)
        {
            var category = await _categoryService.ReadByName(Name);
            if (category == null)
            {
                return NotFound($"Category with name '{Name}' not found");
            }
            _categoryService.Delete(category);
            return Ok("Deletion done");
        }

    }
}
