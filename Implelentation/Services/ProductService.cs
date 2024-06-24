using aqay_apis.Context;
using aqay_apis.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ICategoryService _categoryService;

        public ProductService(ApplicationDbContext context, IAzureBlobService azureBlobService, ICategoryService categoryService)
        {
            _context = context;
            _azureBlobService = azureBlobService;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .OrderByDescending(p => p.LastEdit)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            return product;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var uri = new Uri(product.ImageUrl);
                var blobName = uri.Segments.Last();
                await _azureBlobService.DeleteAsync(blobName);
            }

            // Upload new image
            var imageUrl = await _azureBlobService.UploadAsync(productUpdateDto.ImgFile);

            product.Name = productUpdateDto.Name;
            product.Price = productUpdateDto.Price;
            product.Size = productUpdateDto.Size;
            product.Description = productUpdateDto.Description;
            product.LastEdit = DateTime.UtcNow;
            product.RED = productUpdateDto.RED;
            product.BLUE = productUpdateDto.BLUE;
            product.GREEN = productUpdateDto.GREEN;
            product.Quantity = productUpdateDto.Quantity;
            product.ImageUrl = imageUrl;

            var category = await _categoryService.getCategoryByName(productUpdateDto.CategoryName);
            product.CategoryId = category.Id;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var imageUrl = await _azureBlobService.UploadAsync(productCreateDto.ImageFile);

            var category = await _categoryService.getCategoryByName(productCreateDto.CategoryName);
            var product = new Product
            {
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Size = productCreateDto.Size,
                Description = productCreateDto.Description,
                CreatedOn = DateTime.UtcNow,
                LastEdit = DateTime.UtcNow,
                RED = productCreateDto.RED,
                BLUE = productCreateDto.BLUE,
                GREEN = productCreateDto.GREEN,
                Quantity = productCreateDto.Quantity,
                CategoryId = category.Id,
                BrandId = productCreateDto.BrandId,
                ImageUrl = imageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            // Delete image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var uri = new Uri(product.ImageUrl);
                var blobName = uri.Segments.Last();
                await _azureBlobService.DeleteAsync(blobName);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }


        // New method to get all products within one category by name
        public async Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var category = await _categoryService.getCategoryByName(categoryName);
            return await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();
        }

        // New method to get all products within one brand by ID
        public async Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            return await _context.Products
                .Where(p => p.BrandId == brandId)
                .ToListAsync();
        }
    }
}
