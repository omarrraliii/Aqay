using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;
using aqay_apis.Context;
using Microsoft.AspNetCore.Identity;
using aqay_apis.Services;

namespace aqay_apis
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAzureBlobService _azureBlobService;
        public BrandService(ApplicationDbContext context, UserManager<User> userManager, IAzureBlobService azureBlobService)
        {
            _context = context;
            _userManager = userManager;
            _azureBlobService = azureBlobService;
        }
        public async Task<Brand> CreateBrandAsync(string BrandOwnerId)
        {
            var merchant = await _context.Merchants.FindAsync(BrandOwnerId);
            if (merchant == null) {
                throw new Exception("Merchant not found");
            }
            var brand = new Brand
            {
                Name = merchant.BrandName,
                Description = null,
                Tiktok = null,
                Instagram = null,
                Facebook =  null,
                WPNumber =  null,
                LogoUrl = null,
                BrandOwnerId = BrandOwnerId,
                About = null,
                CreatedOn = DateTime.Now,
                LastEdit = DateTime.Now
            };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) {
                throw new Exception("Brand not found");
            }
            return brand;
        }
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            var Brands=await _context.Brands.OrderByDescending(b=>b.CreatedOn)
                                            .ToListAsync();
            return Brands;
        }
        public async Task<Brand> EditProfileAsync(int id, BrandDto brandDto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }
            if (!string.IsNullOrEmpty(brandDto.Name))
            {
                brand.Name = brandDto.Name;
            }
            if (!string.IsNullOrEmpty(brandDto.Description))
            {
                brand.Description = brandDto.Description;
            }
            if (!string.IsNullOrEmpty(brandDto.Tiktok))
            {
                brand.Tiktok = brandDto.Tiktok;
            }
            if (!string.IsNullOrEmpty(brandDto.Instagram))
            {
                brand.Instagram = brandDto.Instagram;
            }
            if (!string.IsNullOrEmpty(brandDto.Facebook))
            {
                brand.Facebook = brandDto.Facebook;
            }
            if (!string.IsNullOrEmpty(brandDto.WPNumber))
            {
                brand.WPNumber = brandDto.WPNumber;
            }
            if (!string.IsNullOrEmpty(brandDto.About))
            {
                brand.About = brandDto.About;
            }
            if (brandDto.Logo != null)
            {
                brand.LogoUrl = await _azureBlobService.UploadAsync(brandDto.Logo);
            }
            brand.LastEdit = DateTime.Now;
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }
        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                throw new Exception("Brand not found!");
            }
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetBrandIdBy(string name)
        {
            var brand = await _context.Brands
                              .Where(b => b.Name == name)
                              .FirstOrDefaultAsync();

            if (brand == null)
            {
                throw new KeyNotFoundException($"Brand with name '{name}' not found.");
            }
            return brand.Id;
        }
    }
}
