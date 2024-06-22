using aqay_apis.Services;
using Microsoft.AspNetCore.Mvc;
namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("plans")]
        public async Task<ActionResult<ICollection<Plan>>> GetAllPlans()
        {
            var plans = await _subscriptionService.GetAllPlansAsync();
            return Ok(plans);
        }

        [HttpGet("plans/{id}")]
        public async Task<ActionResult<Plan>> GetPlanById(int id)
        {
            var plan = await _subscriptionService.GetPlanByIdAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return Ok(plan);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(string userId, int planId)
        {
            try
            {
                await _subscriptionService.SubscribeAsync(userId, planId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("payment-options")]
        public ActionResult<IEnumerable<PAYMENTOPTIONS>> GetAllPaymentOptions()
        {
            var options = _subscriptionService.GetAllPaymentOptions();
            return Ok(options);
        }
    }
}
