using aqay_apis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace aqay_apis.Controllers
{
    //[Authorize(Roles = "Consumer,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private IWishListService _wishListService;
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProductToWishList(int id, int productId)
        {
            try
            {
                var wishList = await _wishListService.AddProductToWishList(id, productId);
                if (wishList == null)
                {
                    return NotFound();
                }
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("removeProduct")]
        public async Task<IActionResult> RemoveProductFromWishList(int id, int productId)
        {
            try
            {
                var wishList = await _wishListService.RemoveProductFromWishList(id, productId);
                if (wishList == null)
                {
                    return NotFound();
                }
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishListById(int id)
        {
            try
            {
                var wishList = await _wishListService.GetWishListByIdAsync(id);
                if (wishList == null)
                {
                    return NotFound($"WishList with ID {id} not found.");
                }
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


