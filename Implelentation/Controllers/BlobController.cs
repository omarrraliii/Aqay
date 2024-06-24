using Microsoft.AspNetCore.Mvc;
using aqay_apis.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly IAzureBlobService _blobService;

        public BlobController(IAzureBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var result = await _blobService.UploadAsync(file);
            return Ok(new { url = result });
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string blobName)
        {
            var stream = await _blobService.DownloadAsync(blobName);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", blobName);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string blobName)
        {
            await _blobService.DeleteAsync(blobName);
            return NoContent();
        }
    }
}
