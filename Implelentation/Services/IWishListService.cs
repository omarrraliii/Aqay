using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis
{
    public interface IWishListService
    {
        Task<WishList> AddProductToWishList(int id, int productVariantId);
        Task<WishList> CreateWishListAsync(string consumerId);
        Task<WishList> RemoveProductFromWishList(int id, int productVariantId);
        Task<List<ProductVariant>> GetProductVariantsForWishList(int id);
        Task<int?> GetWishListIdByConsumerIdAsync(string consumerId); // New method
    }
}
