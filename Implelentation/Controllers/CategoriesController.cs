using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using aqay_apis.Services;

namespace aqay_apis.controllers
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
        public async Task<ActionResult<IEnumerable<Category>>> getCategories()
        {
            var categories = await _categoryService.getCategories();
            return Ok(categories);
        }
        [HttpGet("id")]
        public async Task<ActionResult<Category>> getCategoryById(int id)
        {
            var category = await _categoryService.getCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet("name")]
        public async Task<ActionResult<Category>> getCategoryByName(string name)
        {
            var category = await _categoryService.getCategoryByName(name);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        //[Authorize(Roles = "Owner, Admin")]
        public async Task<ActionResult<Category>> createCategory(string name)
        {
            try
            {
                var createdCategory = await _categoryService.createCategory(name);
                return Ok(createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut]
        //[Authorize(Roles = "Owner, Admin")]
        public async Task<IActionResult> updateCategory(string oldName,string newName)
        {
            try
            {
                var updatedCategory = await _categoryService.updateCategory(oldName, newName);
                if (updatedCategory == null)
                {
                    return NotFound();
                }
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteCategory(string name)
        {
            try
            {
                var result = await _categoryService.deleteCategory(name);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
