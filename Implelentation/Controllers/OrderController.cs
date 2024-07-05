using Microsoft.AspNetCore.Mvc;
using aqay_apis.Services;

namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("consumer/orders/")]
        public async Task<IActionResult> GetOrdersByConsumerId(string consumerId)
        {
            try
            {
                var result = await _orderService.GetOrdersByConsumerIdAsync(consumerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("brand/orders/")]
        public async Task<IActionResult> GetOrdersByMerchantId(int brandId)
        {
            try
            {
                var result = await _orderService.GetOrdersByMerchantIdAsync(brandId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("id/")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("order/status/")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, ORDERSTATUSES newStatus)
        {
            try
            {
                var result = await _orderService.ChangeOrderStatusAsync(orderId, newStatus);
                if (!result)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("order/accept/")]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            try
            {
                var result = await _orderService.AcceptOrderAsync(orderId);
                if (!result)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("brand/brand/orders/status/")]
        public async Task<IActionResult> GetOrdersByMerchantAndStatus(int brandId, ORDERSTATUSES status)
        {
            try
            {
                var result = await _orderService.GetOrdersByMerchantAndStatusAsync(brandId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("consumer/orderHistory/status/{status}")]
        public async Task<IActionResult> GetOrderHistoryByConsumerId(string consumerId, ORDERSTATUSES status)
        {
            try
            {
                var result = await _orderService.GetOrderHistoryByConsumerIdAsync(consumerId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("createPromoCode")]
        public async Task<IActionResult> CreatePromoCode([FromBody] PromoCodeDto promoCodeDto)
        {
            try
            {
                var promoCode = await _orderService.CreatePromoCodeAsync(promoCodeDto);
                return Ok(promoCode);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<bool>> Checkout(int ShoppingCartId, string? PromoCode, string address)
        {
            try
            {
                var result = await _orderService.CheckoutAsync(ShoppingCartId, PromoCode, address);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
