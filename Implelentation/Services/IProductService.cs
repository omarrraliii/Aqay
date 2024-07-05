using aqay_apis.Dtos;

namespace aqay_apis.Services
{
    public interface IProductService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> AddAsync(ProductDto productDto);
        Task<int> UpdateAsync(int id, ProductDto productDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByName (string name);
        Task<IEnumerable<Product>> GetProductsByTag(string tag);
        Task<IEnumerable<ProductVariant>> GetProductSpecsAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);

    }
}
