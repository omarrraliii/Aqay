using aqay_apis.Models;

namespace aqay_apis;

public interface IBrandService
{
    Task<Brand> CreateBrandAsync(string BrandOwnerId);
    Task<Brand> GetBrandByIdAsync(int id);
    Task<PaginatedResult<Brand>> GetAllBrandsAsync(int pageindex);
    Task<Brand> EditProfileAsync(int id, BrandDto brandDto);
    Task<bool> DeleteBrandAsync(int id);
}
