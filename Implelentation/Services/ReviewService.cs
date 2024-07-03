using aqay_apis.Context;
using aqay_apis.Dtos;
using aqay_apis.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalVariables _globalVariables;
        private readonly UserManager<User> _userManager;
        public ReviewService(ApplicationDbContext applicationDbContext, GlobalVariables globalVariables, UserManager<User> userManager)
        {
            _context = applicationDbContext;
            _globalVariables = globalVariables;
            _userManager = userManager;
        }
        public async Task<PaginatedResult<Review>> GetAllReviewsAsync(int pageIndex, int productId)
        {
            int pageSize = _globalVariables.PageSize;

            // Get total count of reviews for the product
            int totalCount = await _context.Reviews
                                           .Where(r => r.ProductId == productId)
                                           .CountAsync();

            // Get the reviews for the specific page
            var reviews = await _context.Reviews
                                        .Where(r => r.ProductId == productId)
                                        .OrderByDescending(r => r.CreatedOn)
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            var paginatedResult = new PaginatedResult<Review>
            {
                Items = reviews,
                TotalCount = totalCount,
                HasMoreItems = (pageIndex * pageSize) < totalCount
            };

            return paginatedResult;
        }

        public async Task<Review> GetById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if(review == null)
            {
                throw new Exception("Review not found");
            }
            return review;

        }

        public async Task<Review> ReviewProductAsync(ReviewDto review)
        {
            var rate = review.Rate;
            if(rate> 5 || rate < 1)
            throw new Exception("Rate must be between 1 and 5");

            var order = await _context.Orders.FindAsync(review.orderId);
            if (order == null) throw new Exception("order not found");
            
            var productvar = await _context.ProductVariants.FindAsync(order.ProductVariantId);
            if (productvar == null) throw new Exception("variant not found");

            var product = await _context.Products.FindAsync(productvar.ProductId);
            if (product == null) throw new Exception("Product not found");

            var consumer = await _userManager.FindByIdAsync(order.ConsumerId);
            if (consumer == null) throw new Exception("Consumer not found");

            var brand = await _context.Brands.FindAsync(order.BrandId);
            if (brand == null) throw new Exception("Brand not found");

            var merchant = await _userManager.FindByIdAsync(brand.BrandOwnerId);
            if (merchant == null) throw new Exception("Merchant not found");

            if (!order.IsAccepted) throw new Exception("Order is not accepted");
            if (!consumer.IsActive) throw new Exception("Consumer inActive");
            if (!merchant.IsActive) throw new Exception("Merchant inActive");

            if(order.ORDERSTATUSES != ORDERSTATUSES.DELIVERED)
            throw new Exception("Can't review order before Successful delivery");

            if (order.IsReviewed) throw new Exception("Order already reviwed");

            var newReview = new Review()
            {
                Description = review.Description,
                Rate = review.Rate,
                ConsumerId = order.ConsumerId,
                Product = product,
                ProductId = product.Id,
                orderId = review.orderId,
                CreatedOn = DateTime.Now
            };
            product.ReviewCount++;
            
            _context.Reviews.Add(newReview);

            var avgRate = (product.Rate + rate) / 2;
            product.Rate = avgRate;

            _context.Products.Update(product);

            order.IsReviewed = true;
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

            return newReview;
            
        }



    }
}
