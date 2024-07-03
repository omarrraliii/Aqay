using aqay_apis.Context;
using aqay_apis.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aqay_apis.Dashboards
{
    public class MerchantDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public MerchantDashboardService(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<double> GetTotalRevenueForSixMonths(int brandId)
        {
            // Calculate the start date for 6 months ago from today
            DateTime startDate = DateTime.Today.AddMonths(-6).Date;

            // Fetch total revenue for each month for the specified brand
            double totalRevenue = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED && o.CreatedOn >= startDate)
                .SumAsync(o => o.TotalPrice);

            return totalRevenue;
        }
        public async Task<MerchantDashboardStatistics> GetDashboardStatistics(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
            {
                throw new Exception("Brand not found");
            }

            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            var orders = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .ToListAsync();

            double totalRevenue = orders.Sum(o => o.TotalPrice);

            var ordersForTheMonth = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED && o.CreatedOn >= firstDayOfMonth)
                .ToListAsync();

            double totalRevenueThisMonth = ordersForTheMonth.Sum(o => o.TotalPrice);

            var topProductVariants = ordersForTheMonth
                .GroupBy(o => o.ProductVariantId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            int completedOrdersCount = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .CountAsync();

            int pendingOrdersCount = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.PENDING)
                .CountAsync();

            var ordersCategories = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED && o.CreatedOn >= firstDayOfMonth)
                .Select(o => new
                {
                    ProductVariantId = o.ProductVariantId,
                    ConsumerGender = o.ConsumerGender,
                    CategoryName = _context.ProductVariants
                        .Where(pv => pv.Id == o.ProductVariantId)
                        .Select(pv => pv.Product.Category.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var categoryStatistics = ordersCategories
                .GroupBy(o => o.CategoryName)
                .Select(g => new CategoryStatistics
                {
                    Category = g.Key,
                    Female = g.Count(o => o.ConsumerGender),
                    Male = g.Count(o => !o.ConsumerGender),
                    TotalSales = g.Count()
                })
                .ToList();

            var products = await _context.Products
                .Where(p => p.BrandId == brandId)
                .ToListAsync();

            List<ProductRating> productRatings = new List<ProductRating>();
            foreach (var product in products)
            {
                productRatings.Add(new ProductRating
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    AverageRating = product.Rate
                });
            }

            int exchangeCount = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.EXCHANGE)
                .CountAsync();

            int refundCount = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.REFUND)
                .CountAsync();

            double sixMonthRevenue = await GetTotalRevenueForSixMonths(brandId);

            return new MerchantDashboardStatistics()
            {
                TotalRevenue = totalRevenue,
                TotalRevenueThisMonth = totalRevenueThisMonth,
                TopProductVariants = topProductVariants,
                CompletedOrdersCount = completedOrdersCount,
                PendingOrdersCount = pendingOrdersCount,
                CategoryStatistics = categoryStatistics,
                ProductRatings = productRatings,
                RefundCount = refundCount,
                ExchangeCount = exchangeCount,
                SixMonthRevenue = sixMonthRevenue
            };
        }


    }
}
