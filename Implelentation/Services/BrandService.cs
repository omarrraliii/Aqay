using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aqay_apis.Context;
using Microsoft.AspNetCore.Identity;
using aqay_apis.Services;

namespace aqay_apis
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalVariables _globalVariables;
        private readonly UserManager<User> _userManager;
        private readonly IAzureBlobService _azureBlobService;

        public BrandService(ApplicationDbContext context, UserManager<User> userManager,GlobalVariables globalVariables, IAzureBlobService azureBlobService)
        {
            _context = context;
            _globalVariables = globalVariables;
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
            return await _context.Brands.FindAsync(id);
        }
        public async Task<PaginatedResult<Brand>> GetAllBrandsAsync(int pageIndex)
        {
            var Brands=await _context.Brands.OrderByDescending(b=>b.CreatedOn)
                                            .Skip((pageIndex-1)*_globalVariables.PageSize)
                                            .Take(_globalVariables.PageSize)
                                            .ToListAsync();
            var brandCount= Brands.Count;
            var paginatedResult= new PaginatedResult<Brand>
            {
                Items=Brands,
                TotalCount=brandCount,
                HasMoreItems=(pageIndex*_globalVariables.PageSize)<brandCount
            };
            return paginatedResult;
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
                return false;
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
