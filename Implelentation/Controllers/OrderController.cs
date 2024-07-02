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
        [HttpGet("consumer/{consumerId}/orders")]
        public async Task<IActionResult> GetOrdersByConsumerId(string consumerId, int pageNumber)
        {
            var result = await _orderService.GetOrdersByConsumerIdAsync(consumerId, pageNumber);
            return Ok(result);
        }
        [HttpGet("merchant/{merchantId}/orders")]
        public async Task<IActionResult> GetOrdersByMerchantId(int merchantId, int pageNumber)
        {
            var result = await _orderService.GetOrdersByMerchantIdAsync(merchantId, pageNumber);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, ORDERSTATUSES newStatus)
        {
            var result = await _orderService.ChangeOrderStatusAsync(orderId, newStatus);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("consumer/{consumerId}/product/{productId}/order")]
        public async Task<IActionResult> CreateOrder(string consumerId, int productId)
        {
            var order = await _orderService.CreateOrderAsync(consumerId, productId);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        [HttpPost("{orderId}/accept")]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var result = await _orderService.AcceptOrderAsync(orderId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("merchant/{brandId}/orders/status/{status}")]
        public async Task<IActionResult> GetOrdersByMerchantAndStatus(int brandId, ORDERSTATUSES status, int pageNumber)
        {
            var result = await _orderService.GetOrdersByMerchantAndStatusAsync(brandId, status, pageNumber);
            return Ok(result);
        }
        [HttpGet("consumer/{consumerId}/orderHistory/status/{status}")]
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
        public async Task<ActionResult<bool>> Checkout(int ShoppingCartId, string PromoCode)
        {
            try
            {
                var result = await _orderService.CheckoutAsync(ShoppingCartId,PromoCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
