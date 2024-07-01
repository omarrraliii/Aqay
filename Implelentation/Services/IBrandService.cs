using aqay_apis.Models;

namespace aqay_apis;

public interface IBrandService
{
    Task<Brand> CreateBrandAsync(BrandDto brandDto);
    Task<Brand> GetBrandByIdAsync(int id);
    Task<PaginatedResult<Brand>> GetAllBrandsAsync(int pageindex);
    Task<Brand> UpdateBrandAsync(int id, BrandDto brandDto);
    Task<bool> DeleteBrandAsync(int id);
}
