using Microsoft.AspNetCore.Mvc;


namespace aqay_apis.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WishListController:ControllerBase
{
    private IWishListService _wishListService;
    public WishListController(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }
    [HttpPost("addProduct")]
    public async Task<IActionResult>  AddProductToWishList (int id,int productId)
    {
        var wishList=await _wishListService.AddProductToWishList(id, productId);
        if (wishList==null)
        {
            return NotFound();
        }
        return Ok(wishList);
    }
[HttpDelete]
    public async Task<IActionResult> RemoveProductFromWishList (int id,int productId)
    {
        var wishList = await _wishListService.RemoveProductFromWishList(id,productId);
        if (wishList==null)
        {
            return NotFound();
        }
        return Ok(wishList);
    }
[HttpGet]
    public async Task<IActionResult> GetWishListById (int id,int pageIndex=1)
    {
        var wishList= await _wishListService.GetWishListByIdAsync(id,pageIndex);
        return Ok(wishList);
    }
}
