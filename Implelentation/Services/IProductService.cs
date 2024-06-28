using aqay_apis.Dtos;

namespace aqay_apis.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(int pageSize, int pageNumber);
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(ProductDto productDto);
        Task<Product> UpdateAsync(int id, ProductDto productDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProductVariant>> GetProductSpecsAsync(int productId);
        Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId, int pageSize, int pageNumber);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId, int pageSize, int pageNumber);

    }
}
