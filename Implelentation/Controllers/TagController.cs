using aqay_apis.Models;
using Microsoft.AspNetCore.Mvc;

namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Tag name is null or empty.");
            }

            var createdTag = await _tagService.CreateTagAsync(name);
            return CreatedAtAction(nameof(GetTagById), new { id = createdTag.Id }, createdTag);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags([FromQuery] int pageIndex = 1)
        {
            var tags = await _tagService.GetAllTagsAsync(pageIndex);
            return Ok(tags);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Tag name is null or empty.");
            }

            var updatedTag = await _tagService.UpdateTagAsync(id, name);
            if (updatedTag == null)
            {
                return NotFound();
            }

            return Ok(updatedTag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var isDeleted = await _tagService.DeleteTagAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
