using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aqay_apis.Context;
using Microsoft.AspNetCore.Identity;

namespace aqay_apis
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalVariables _globalVariables;
        private readonly UserManager<User> _userManager;

        public BrandService(ApplicationDbContext context, UserManager<User> userManager,GlobalVariables globalVariables)
        {
            _context = context;
            _globalVariables = globalVariables;
            _userManager = userManager;
        }

        public async Task<Brand> CreateBrandAsync(BrandDto brandDto)
        {
            var merchant = _userManager.FindByIdAsync(brandDto.BrandOwnerId);
            if (merchant == null) {
                throw new Exception("Merchant not found");
            }
            var brand = new Brand
            {
                Name = brandDto.Name,
                Description = brandDto.Description,
                Tiktok = brandDto.Tiktok,
                Instagram = brandDto.Instagram,
                Facebook = brandDto.Facebook,
                WPNumber = brandDto.WPNumber,
                BrandOwnerId = brandDto.BrandOwnerId,
                About = brandDto.About,
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

        public async Task<Brand> UpdateBrandAsync(int id, BrandDto brandDto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return null;
            }

            brand.Name = brandDto.Name;
            brand.Description = brandDto.Description;
            brand.Tiktok = brandDto.Tiktok;
            brand.Instagram = brandDto.Instagram;
            brand.Facebook = brandDto.Facebook;
            brand.WPNumber = brandDto.WPNumber;
            brand.BrandOwnerId = brandDto.BrandOwnerId;
            brand.About = brandDto.About;
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
