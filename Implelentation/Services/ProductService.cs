﻿using aqay_apis.Context;
using aqay_apis.Dtos;
using aqay_apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aqay_apis.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly ITagService _tagService;
        public ProductService(ApplicationDbContext context, ICategoryService categoryService, IAzureBlobService azureBlobService, ITagService tagService)
        {
            _context = context;
            _categoryService = categoryService;
            _azureBlobService = azureBlobService;
            _tagService = tagService;
        }
        public async Task<IEnumerable<object>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.ProductVariants)
                .OrderByDescending(p => p.LastEdit)
                .ToListAsync();
            var productsWithOneVariant = products.Select(p => new
            {
                Products = p,
                ProductVariants = p.ProductVariants.Take(1)
                //i want to return the first object in the ProductVariants
            }).ToList();
            return productsWithOneVariant;
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            return product;
        }
        public async Task<int> AddAsync(ProductDto productDto)
        {
            ValidateProductDto(productDto);
            var category = await _categoryService.getCategoryById(productDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                CreatedOn = DateTime.Now,
                LastEdit = DateTime.Now,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                ProductVariants = null,
                categoryName = productDto.categoryName,
                Tags = new List<Tag>()
            };
            if (productDto.TagName != null && productDto.TagName.Any())
            {
                foreach (var tag in productDto.TagName)
                {
                    var lowerCasetag = tag.ToLower();
                    var retrievedTag = await _tagService.GetTagByNameAsync(lowerCasetag);
                    if (retrievedTag == null)
                    {
                        var CreatedTag = await _tagService.CreateTagAsync(lowerCasetag);
                        product.Tags.Add(CreatedTag);
                    }
                    else
                    {
                        product.Tags.Add(retrievedTag);
                    }

                }
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<int> UpdateAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.Include(p => p.Tags)
                                                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            ValidateProductDto(productDto, false);
            product.Name = productDto.Name ?? product.Name;
            product.Price = productDto.Price >= 0 ? productDto.Price : product.Price;
            product.Description = productDto.Description ?? product.Description;
            product.LastEdit = DateTime.Now;
            product.CategoryId = productDto.CategoryId > 0 ? productDto.CategoryId : product.CategoryId;
            product.BrandId = productDto.BrandId > 0 ? productDto.BrandId : product.BrandId;
            if (productDto.TagName != null && productDto.TagName.Any())
            {
                var tagNamesFromDto = productDto.TagName.Select(t => t.ToLower()).ToHashSet();
                var tagsToRemove = product.Tags.Where(t => !tagNamesFromDto.Contains(t.Name.ToLower())).ToList();
                foreach (var tag in tagsToRemove)
                {
                    product.Tags.Remove(tag);
                }

                var existingTagNames = product.Tags.Select(t => t.Name).ToHashSet();
                foreach (var tag in productDto.TagName)
                {
                    var lowerCasetag = tag.ToLower();
                    if (!existingTagNames.Contains(lowerCasetag))
                    {
                        var retrievedTag = await _tagService.GetTagByNameAsync(lowerCasetag);
                        if (retrievedTag == null)
                        {
                            var CreatedTag = await _tagService.CreateTagAsync(lowerCasetag);
                            product.Tags.Add(CreatedTag);
                        }
                        else
                        {
                            product.Tags.Add(retrievedTag);
                        }
                    }

                }
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<ProductVariant>> GetProductSpecsAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            return product.ProductVariants;
        }
        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId)
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.BrandId == brandId)
                .OrderByDescending(p => p.LastEdit)
                .ToListAsync();
            return products.Select(p=> new Product
            {
                Id = p.Id,
                Name=p.Name,
                Price=p.Price,
                Description=p.Description,
                CategoryId=p.CategoryId,
                BrandId=p.BrandId,
            }).ToList();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Category.Name == categoryName)
                .OrderByDescending(p => p.LastEdit)
                .ToListAsync();
            return products.Select(p=> new Product
            {
                Id=p.Id,
                Name=p.Name,
                Price=p.Price,
                Description=p.Description,
                CategoryId=p.CategoryId,
                BrandId=p.BrandId,
            }).ToList();
        }
        private void ValidateProductDto(ProductDto productDto, bool isNew = true)
        {
            if (isNew || (!isNew && !string.IsNullOrEmpty(productDto.Name)))
            {
                if (string.IsNullOrEmpty(productDto.Name))
                {
                    throw new ArgumentException("Name cannot be null or empty.");
                }
            }
            if (isNew || (!isNew && productDto.Price >= 0))
            {
                if (productDto.Price < 0)
                {
                    throw new ArgumentException("Price cannot be negative.");
                }
            }
            if (isNew || (!isNew && !string.IsNullOrEmpty(productDto.Description)))
            {
                if (string.IsNullOrEmpty(productDto.Description))
                {
                    throw new ArgumentException("Description cannot be null or empty.");
                }
            }
            if (isNew || (!isNew && productDto.CategoryId > 0))
            {
                if (productDto.CategoryId <= 0)
                {
                    throw new ArgumentException("Invalid CategoryId.");
                }
            }
            if (isNew || (!isNew && productDto.BrandId > 0))
            {
                if (productDto.BrandId <= 0)
                {
                    throw new ArgumentException("Invalid BrandId.");
                }
            }
        }
        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var products = await _context.Products.Include(p => p.Brand)
                                                .Include(p => p.Category)
                                                .Include(p => p.ProductVariants)
                                                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                                                .ToListAsync();
            var productsCount =await _context.Products
                                       .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                                       .CountAsync();
            return products.Select(p=> new Product
            {
                Id=p.Id,
                Name=p.Name,
                Price=p.Price,
                Description=p.Description,
                CategoryId=p.CategoryId,
                BrandId=p.BrandId,
            }).ToList();        }
        public async Task<IEnumerable<Product>> GetProductsByTag(string tag)
        {
            var tagEntity = await _context.Tags
                .Include(t => t.Products)
                .FirstOrDefaultAsync(t => t.Name == tag);

            if (tagEntity == null)
            {
                return new List<Product>();
            }
            var products = tagEntity.Products
                .OrderByDescending(p => p.LastEdit)
                .ToList();
            return products.Select(p=> new Product
            {
                Id=p.Id,
                Name=p.Name,
                Price=p.Price,
                Description=p.Description,
                CategoryId=p.CategoryId,
                BrandId=p.BrandId,
            }).ToList();
        }

    }
}
