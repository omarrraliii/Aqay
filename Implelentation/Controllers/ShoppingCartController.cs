using aqay_apis.Services;
using aqay_apis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create()
        {
            var cartId = await _shoppingCartService.CreateAsync();
            return Ok(cartId);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ShoppingCart>> ReadById(int id)
        {
            var cart = await _shoppingCartService.ReadByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ShoppingCart>>> ReadAll()
        {
            var carts = await _shoppingCartService.ReadAllAsync();
            return Ok(carts);
        }

        [HttpPost("shoppingCartId/addProduct")]
        public async Task<ActionResult<bool>> AddProductVariant(int shoppingCartId, int productVariantId)
        {
            var success = await _shoppingCartService.AddProductVariantAsync(shoppingCartId, productVariantId);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }
        [HttpPost("shoppingCartId/removeProduct")]
        public async Task<ActionResult<bool>> RemoveProductVariant(int shoppingCartId, [FromBody] int productVariantId)
        {
            var success = await _shoppingCartService.RemoveProductVariantAsync(shoppingCartId, productVariantId);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpGet("shoppingCartId/variants")]
        public async Task<ActionResult<IList<ProductVariant>>> GetProductVariants(int shoppingCartId)
        {
            var productVariants = await _shoppingCartService.GetProductVariantsAsync(shoppingCartId);
            if (productVariants == null)
            {
                return NotFound();
            }
            return Ok(productVariants);
        }
    }
}
