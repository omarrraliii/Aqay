using aqay_apis.Dtos;

namespace aqay_apis.Services
{
    public interface IProductService
    {
        Task<PaginatedResult<object>> GetAllAsync(int pageIndex);
        Task<Product> GetByIdAsync(int id);
        Task<int> AddAsync(ProductDto productDto);
        Task<int> UpdateAsync(int id, ProductDto productDto);
        Task<bool> DeleteAsync(int id);
        Task<PaginatedResult<Product>> GetProductsByName (string name,int pageIndex);
        Task<PaginatedResult<Product>> GetProductsByTag(string tag,int pageIndex);
        Task<PaginatedResult<ProductVariant>> GetProductSpecsAsync(int productId,int pageIndex);
        Task<PaginatedResult<Product>> GetProductsByBrandAsync(int brandId, int pageIndex);
        Task<PaginatedResult<Product>> GetProductsByCategoryAsync(string categoryName, int pageIndex);

    }
}
