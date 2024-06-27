using aqay_apis.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId);
    }
}
