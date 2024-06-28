using aqay_apis.Dtos;

namespace aqay_apis.Services
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariant>> GetAllAsync();
        Task<ProductVariant> GetByIdAsync(int id);
        Task<ProductVariant> AddAsync(ProductVariantDto productVariant);
        Task<ProductVariant> UpdateAsync(int id,ProductVariantDto productVariant);
        Task<bool> DeleteAsync(int id);
        Task<ProductVariant> IncrementQuantityAsync(int id, int incrementBy);
        Task<ProductVariant> DecrementQuantityAsync(int id, int decrementBy);
    }
}
