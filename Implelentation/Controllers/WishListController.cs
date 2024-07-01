using aqay_apis.Models;
using Microsoft.AspNetCore.Mvc;


namespace aqay_apis.Controllers;
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
        var wishList = await _wishListService.AddProductToWishList(id, productId);
        if (wishList == null)
        {
            return NotFound();
        }
        return Ok(wishList);
    }
    [HttpDelete("removeProduct")]
    public async Task<IActionResult> RemoveProductFromWishList(int id, int productId)
    {
        var wishList = await _wishListService.RemoveProductFromWishList(id, productId);
        if (wishList == null)
        {
            return NotFound();
        }
        return Ok(wishList);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWishListById(int id, int pageIndex = 1)
    {
        var wishList = await _wishListService.GetWishListByIdAsync(id, pageIndex);
        if (wishList == null)
        {
            return NotFound($"WishList with ID {id} not found.");
        }
        return Ok(wishList);
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateWishList([FromBody] Consumer consumer)
    {
        if (consumer == null)
        {
            return BadRequest("Consumer data is null.");
        }

        var wishList = await _wishListService.CreateWishListAsync(consumer);
        return CreatedAtAction(nameof(GetWishListById), new { id = wishList.Id }, wishList);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWishList(int id)
    {
        var isDeleted = await _wishListService.DeleteWishListAsync(id);
        if (!isDeleted)
        {
            return NotFound($"WishList with ID {id} not found.");
        }
        return NoContent();
    }
}

