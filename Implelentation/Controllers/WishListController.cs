using aqay_apis.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProductToWishList([FromQuery] int id, [FromQuery] int productVariantId)
        {
            try
            {
                var result = await _wishListService.AddProductToWishList(id, productVariantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWishList([FromQuery] string consumerId)
        {
            try
            {
                var result = await _wishListService.CreateWishListAsync(consumerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("removeProduct")]
        public async Task<IActionResult> RemoveProductFromWishList([FromQuery] int id, [FromQuery] int productVariantId)
        {
            try
            {
                var result = await _wishListService.RemoveProductFromWishList(id, productVariantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProductVariantsForWishList([FromQuery] int id)
        {
            try
            {
                var result = await _wishListService.GetProductVariantsForWishList(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getWishListId")]
        public async Task<IActionResult> GetWishListIdByConsumerId([FromQuery] string consumerId)
        {
            try
            {
                var result = await _wishListService.GetWishListIdByConsumerIdAsync(consumerId);
                if (result == null)
                {
                    return NotFound(new { message = $"Wishlist for consumer ID {consumerId} not found" });
                }
                return Ok(new
                {
                    wishlistId = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
