using Microsoft.AspNetCore.Mvc;

namespace aqay_apis;
[ApiController]
[Route("api/[controller]")]
public class FAQController:ControllerBase
{
    private readonly IFAQService _faqService;
    public FAQController(IFAQService fAQService)
    {
        _faqService = fAQService;
    }
    [HttpGet("GetAllFAQ")]
    public async Task<IActionResult> GetAllFAQs(int pageIndex)
    {
        var FAQs=await _faqService.GetAllFAQs(pageIndex);
        if (FAQs==null)
        {
            return NotFound();
        }
        return Ok(FAQs);
    }
    [HttpGet("GetFAQById")]
    public async Task<IActionResult> GetFAQById(int id)
    {
        var FAQ=await _faqService.GetFAQById(id);
        if (FAQ==null)
        {
            return NotFound();
        }
        return Ok(FAQ);
    }
    [HttpPost("Create")]
    public async Task<IActionResult> CreateFAQ(string question,string answer)
    {
        try
        {
            var FAQ= await _faqService.CreateFAQ(question,answer);
            return CreatedAtAction(nameof(GetFAQById), new {id=FAQ.Id}, FAQ);
        }
        catch (Exception ex)
        {
            return BadRequest(new {message=ex.Message});
        }
        
    }
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateFAQ(int id,string question,string answer)
    {
        var FAQ=await _faqService.UpdateFAQ(id,question,answer);
        if (FAQ==null)
        {
            return NotFound();
        }
        return Ok(FAQ);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteFAQ(int id)
    {
        var FAQ=await _faqService.DeleteFAQ(id);
        if (FAQ)
        {
            return NoContent();
        }
        return NotFound();
    }
    [HttpGet("Search")]
    public async Task<IActionResult> SearchFAQs(string query)
    {
        var FAQs = await _faqService.SearchFAQs(query);
        if (FAQs==null)
        {
            return NotFound();
        }
        return Ok(FAQs);
    }
}
