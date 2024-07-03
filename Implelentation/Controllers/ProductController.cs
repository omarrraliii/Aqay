using Microsoft.AspNetCore.Mvc;
using aqay_apis.Dtos;
using aqay_apis.Models;
using aqay_apis.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aqay_apis.Context;
using Microsoft.CodeAnalysis;
namespace aqay_apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ApplicationDbContext _context;
        public ProductsController(IProductService productService, IAzureBlobService azureBlobService , ApplicationDbContext context)
        {
            _productService = productService;
            _azureBlobService = azureBlobService;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int pageIndex = 1)
        {
            var products = await _productService.GetAllAsync(pageIndex);
            return Ok(products);
        }
        [HttpGet("id/")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var productId = await _productService.AddAsync(productDto);
                return Ok(productId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("product variant/")]
        public async Task<IActionResult> EditProductVariant(int variantId, [FromForm] ProductVariantDto variantDto)
        {
            try
            {
                var product = await _productService.GetByIdAsync(variantDto.ProductId);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                var variant = product.ProductVariants.FirstOrDefault(v => v.Id == variantId);
                if (variant == null)
                {
                    return NotFound(new { message = "Variant not found." });
                }
                variant.Size = variantDto.Size ?? variant.Size;
                variant.Color = variantDto.Color ?? variant.Color;
                variant.Quantity = variantDto.Quantity >= 0 ? variantDto.Quantity : variant.Quantity;
                if (variantDto.ImgFile != null)
                {
                    variant.ImageUrl = await _azureBlobService.UploadAsync(variantDto.ImgFile);
                }

                _context.ProductVariants.Update(variant);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("product variant/")]
        public async Task<IActionResult> AddProductVariant([FromForm] ProductVariantDto variantDto)
        {
            try
            {
                var product = await _productService.GetByIdAsync(variantDto.ProductId);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }
                var newVariant = new ProductVariant
                {
                    Size = variantDto.Size,
                    Color = variantDto.Color,
                    Quantity = variantDto.Quantity,
                    ProductId = variantDto.ProductId,
                    ImageUrl = variantDto.ImgFile != null ? await _azureBlobService.UploadAsync(variantDto.ImgFile) : "default-image-url"
                };
                product.ProductVariants.Add(newVariant);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            try
            {
                var productId = await _productService.UpdateAsync(id, productDto);
                return Ok(productId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        /// productttt
        [HttpGet("variants/")]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetProductVariants(int productId,int pageIndex=1)
        {
            try
            {
                var variants = await _productService.GetProductSpecsAsync(productId,pageIndex);
                return Ok(variants);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("brand/")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(int brandId, int pageIndex = 1)
        {
            var products = await _productService.GetProductsByBrandAsync(brandId, pageIndex);
            return Ok(products);
        }
        [HttpGet("category/")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId, int pageIndex = 1)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId, pageIndex);
            return Ok(products);
        }
        [HttpGet("name/")]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProductsByName(string name, int pageIndex = 1)
        {
            var result = await _productService.GetProductsByName(name, pageIndex);
            return Ok(result);
        }

        [HttpGet("tag/")]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProductsByTag(string tag, int pageIndex = 1)
        {
            var result = await _productService.GetProductsByTag(tag, pageIndex);
            return Ok(result);
        }
    }
}
