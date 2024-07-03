using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetOrdersByConsumerId(string consumerId, int pageNumber)
        {
            var result = await _orderService.GetOrdersByConsumerIdAsync(consumerId, pageNumber);
            return Ok(result);
        }
        [HttpGet("brand/orders/")]
        public async Task<IActionResult> GetOrdersByMerchantId(int brandId, int pageNumber)
        {
            var result = await _orderService.GetOrdersByMerchantIdAsync(brandId, pageNumber);
            return Ok(result);
        }
        [HttpGet("id/")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPut("order/status/")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, ORDERSTATUSES newStatus)
        {
            var result = await _orderService.ChangeOrderStatusAsync(orderId, newStatus);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("order/accept/")]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var result = await _orderService.AcceptOrderAsync(orderId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("brand/brand/orders/status/")]
        public async Task<IActionResult> GetOrdersByMerchantAndStatus(int brandId, ORDERSTATUSES status, int pageNumber)
        {
            var result = await _orderService.GetOrdersByMerchantAndStatusAsync(brandId, status, pageNumber);
            return Ok(result);
        }
        [HttpGet("consumer/orderHistory/status/{status}")]
        public async Task<IActionResult> GetOrderHistoryByConsumerId(string consumerId, ORDERSTATUSES status, int pageNumber)
        {
            var result = await _orderService.GetOrderHistoryByConsumerIdAsync(consumerId, status, pageNumber);
            return Ok(result);
        }
        [HttpPost("createPromoCode")]
        public async Task<IActionResult> CreatePromoCode([FromBody] PromoCodeDto promoCodeDto)
        {
            var promoCode = await _orderService.CreatePromoCodeAsync(promoCodeDto);
            return Ok(promoCode);
        }
        [HttpPost("checkout")]
        public async Task<ActionResult<bool>> Checkout(int ShoppingCartId, string? PromoCode, string address)
        {
            try
            {
                var result = await _orderService.CheckoutAsync(ShoppingCartId,PromoCode, address);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
