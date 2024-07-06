using aqay_apis.Dtos;
using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public interface IShoppingCartService
    {
        Task<int> CreateAsync(string ConsumerId);
        Task<bool> SetConsumerIdAsync(int shoppingCartId, string consumerId);
        Task<bool> DeleteAsync(int id);
        Task<ShoppingCart> ReadByIdAsync(int id);
        Task<ICollection<ShoppingCart>> ReadAllAsync();
        Task<bool> AddProductVariantAsync(int shoppingCartId, int productVariantId);
        Task<bool> RemoveProductVariantAsync(int shoppingCartId, int productVariantId);
        Task<IList<ProductVariantDto>> GetProductVariantsAsync(int shoppingCartId);

    }
}
