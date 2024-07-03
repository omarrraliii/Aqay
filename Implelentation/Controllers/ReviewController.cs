using aqay_apis.Dtos;
using aqay_apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost]
        public async Task<ActionResult<Review>> ReviewOrder(ReviewDto review)
        {
            var result = await _reviewService.ReviewProductAsync(review);
            return Ok(result);
        }
    }
}
