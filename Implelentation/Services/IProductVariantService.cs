using aqay_apis.Dtos;

namespace aqay_apis.Services
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariant>> GetAllAsync();
        Task<ProductVariant> GetByIdAsync(int id);
        Task<bool> AddAsync(ProductVariantDto productVariant);
        Task<bool> UpdateAsync(int id,ProductVariantDto productVariant);
        Task<bool> DeleteAsync(int id);
        Task<int> IncrementQuantityAsync(int id, int incrementBy);
        Task<int> DecrementQuantityAsync(int id, int decrementBy);
    }
}
