using aqay_apis.Context;
using aqay_apis.Dtos;
using Microsoft.EntityFrameworkCore;
namespace aqay_apis.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IAzureBlobService _azureBlobService;
        public ProductVariantService(ApplicationDbContext context, IAzureBlobService azureBlobService, IProductService productService)
        {
            _context = context;
            _productService = productService;
            _azureBlobService = azureBlobService;
        }
        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants.ToListAsync();
        }
        public async Task<ProductVariant> GetByIdAsync(int id)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                throw new Exception("Variant not found.");
            }
            return productVariant;
        }
        public async Task<bool> AddAsync(ProductVariantDto productVariantDto)
        {
            ValidateProductVariantDto(productVariantDto);

            var product = await _productService.GetByIdAsync(productVariantDto.ProductId);
            var imageUrl = productVariantDto.ImgFile != null ? await _azureBlobService.UploadAsync(productVariantDto.ImgFile) : null;

            var productVariant = new ProductVariant()
            {
                Size = productVariantDto.Size,
                Quantity = productVariantDto.Quantity,
                Color = productVariantDto.Color,
                ProductId = productVariantDto.ProductId,
                ImageUrl = imageUrl
            };

            await _context.ProductVariants.AddAsync(productVariant);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(int id, ProductVariantDto productVariantDto)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                throw new Exception("Variant not found.");
            }

            ValidateProductVariantDto(productVariantDto, false);

            if (!string.IsNullOrEmpty(productVariantDto.Size))
            {
                productVariant.Size = productVariantDto.Size;
            }
            if (!string.IsNullOrEmpty(productVariantDto.Color))
            {
                productVariant.Color = productVariantDto.Color;
            }
            if (productVariantDto.Quantity >= 0)
            {
                productVariant.Quantity = productVariantDto.Quantity;
            }
            if (productVariantDto.ProductId > 0)
            {
                productVariant.ProductId = productVariantDto.ProductId;
            }
            if (productVariantDto.ImgFile != null)
            {
                productVariant.ImageUrl = await _azureBlobService.UploadAsync(productVariantDto.ImgFile);
            }

            _context.ProductVariants.Update(productVariant);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                return false;
            }

            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> IncrementQuantityAsync(int id, int incrementBy)
        {
            var productVariant = await GetByIdAsync(id);
            productVariant.Quantity += incrementBy;
            await _context.SaveChangesAsync();
            return productVariant.Quantity;
        }
        public async Task<int> DecrementQuantityAsync(int id, int decrementBy)
        {
            var productVariant = await GetByIdAsync(id);
            if (productVariant.Quantity < decrementBy)
            {
                throw new Exception("Insufficient quantity.");
            }
            productVariant.Quantity -= decrementBy;
            await _context.SaveChangesAsync();
            return productVariant.Quantity;
        }
        private void ValidateProductVariantDto(ProductVariantDto productVariantDto, bool isNew = true)
        {
            if (isNew || (!isNew && !string.IsNullOrEmpty(productVariantDto.Size)))
            {
                if (string.IsNullOrEmpty(productVariantDto.Size))
                {
                    throw new ArgumentException("Size cannot be null or empty.");
                }
            }

            if (isNew || (!isNew && !string.IsNullOrEmpty(productVariantDto.Color)))
            {
                if (string.IsNullOrEmpty(productVariantDto.Color))
                {
                    throw new ArgumentException("Color cannot be null or empty.");
                }
            }

            if (isNew || (!isNew && productVariantDto.Quantity >= 0))
            {
                if (productVariantDto.Quantity < 0)
                {
                    throw new ArgumentException("Quantity cannot be negative.");
                }
            }

            if (isNew || (!isNew && productVariantDto.ProductId > 0))
            {
                if (productVariantDto.ProductId <= 0)
                {
                    throw new ArgumentException("Invalid ProductId.");
                }
            }
        }
    }
}
