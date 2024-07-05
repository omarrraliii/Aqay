using aqay_apis.Models;

namespace aqay_apis;

public interface IWishListService
{
    Task<WishList> CreateWishListAsync (Consumer consumer);
    Task<WishList> AddProductToWishList(int id,int productId);
    Task<WishList> RemoveProductFromWishList(int id,int productId);
    Task<IEnumerable<Product>> GetWishListByIdAsync(int id);
    Task<bool> DeleteWishListAsync (int id);
}
