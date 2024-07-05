using aqay_apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace aqay_apis.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<int>> Create(string consumerId)
        {
            try
            {
                var cartId = await _shoppingCartService.CreateAsync(consumerId);
                return Ok(cartId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<ShoppingCart>> ReadById(int id)
        {
            try
            {
                var cart = await _shoppingCartService.ReadByIdAsync(id);
                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<ICollection<ShoppingCart>>> ReadAll()
        {
            try
            {
                var carts = await _shoppingCartService.ReadAllAsync();
                return Ok(carts);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("shoppingCartId/addProduct")]
        public async Task<ActionResult<bool>> AddProductVariant(int shoppingCartId, int productVariantId)
        {
            try
            {
                var success = await _shoppingCartService.AddProductVariantAsync(shoppingCartId, productVariantId);
                if (success)
                {
                    return Ok(true);
                }
                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("shoppingCartId/removeProduct")]
        public async Task<ActionResult<bool>> RemoveProductVariant(int shoppingCartId, [FromBody] int productVariantId)
        {
            try
            {
                var success = await _shoppingCartService.RemoveProductVariantAsync(shoppingCartId, productVariantId);
                if (success)
                {
                    return Ok(true);
                }
                return BadRequest(false);
            }
            catch (Exception ex) { 
                return BadRequest (ex.Message);
            }
        }
        [HttpGet("shoppingCartId/variants")]
        public async Task<ActionResult<IList<ProductVariant>>> GetProductVariants(int shoppingCartId)
        {
            try
            {
                var productVariants = await _shoppingCartService.GetProductVariantsAsync(shoppingCartId);
                if (productVariants == null)
                {
                    return NotFound();
                }
                return Ok(productVariants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
