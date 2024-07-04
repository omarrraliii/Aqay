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

        public async Task<List<double>> GetTotalRevenueForSixMonths(int brandId)
        {
            // Calculate the start date for 6 months ago from today
            DateTime startDate = DateTime.Today.AddMonths(-6).Date;

            // Initialize a list to store monthly revenues
            List<double> monthlyRevenues = new List<double>();

            // Calculate revenue for each month
            for (int i = 0; i < 6; i++)
            {
                DateTime monthStart = startDate.AddMonths(i);
                DateTime monthEnd = monthStart.AddMonths(1);

                double monthlyRevenue = await _context.Orders
                    .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED && o.CreatedOn >= monthStart && o.CreatedOn < monthEnd)
                    .SumAsync(o => o.TotalPrice);

                monthlyRevenues.Add(monthlyRevenue);
            }

            // Calculate total revenue for all six months
            double totalRevenue = monthlyRevenues.Sum();

            // Add the total revenue to the list
            monthlyRevenues.Add(totalRevenue);

            return monthlyRevenues;
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

            var orders = await GetOrdersByBrandAndStatus(brandId, ORDERSTATUSES.DELIVERED);
            double totalRevenue = orders.Sum(o => o.TotalPrice);

            var ordersForTheMonth = orders.Where(o => o.CreatedOn >= firstDayOfMonth).ToList();
            double totalRevenueThisMonth = ordersForTheMonth.Sum(o => o.TotalPrice);

            var topProductVariants = GetTopProductVariants(ordersForTheMonth);

            int completedOrdersCount = await GetOrderCountByStatus(brandId, ORDERSTATUSES.DELIVERED);
            int pendingOrdersCount = await GetOrderCountByStatus(brandId, ORDERSTATUSES.PENDING);

            var categoryStatistics = await GetCategoryStatistics(brandId, firstDayOfMonth);

            var productRatings = await GetProductRatings(brandId);

            int exchangeCount = await GetOrderCountByStatus(brandId, ORDERSTATUSES.EXCHANGE);
            int refundCount = await GetOrderCountByStatus(brandId, ORDERSTATUSES.REFUND);

            var sixMonthRevenueList = await GetTotalRevenueForSixMonths(brandId);
            double sixMonthRevenue = sixMonthRevenueList.Last();
            sixMonthRevenueList.RemoveAt(sixMonthRevenueList.Count - 1);

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
                SixMonthRevenue = sixMonthRevenue,
                SixMonthRevenueList = sixMonthRevenueList
            };
        }

        private async Task<List<Order>> GetOrdersByBrandAndStatus(int brandId, ORDERSTATUSES status)
        {
            return await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == status)
                .ToListAsync();
        }
        private List<int> GetTopProductVariants(List<Order> orders)
        {
            return orders
                .GroupBy(o => o.ProductVariantId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();
        }
        private async Task<int> GetOrderCountByStatus(int brandId, ORDERSTATUSES status)
        {
            return await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == status)
                .CountAsync();
        }
        private async Task<List<CategoryStatistics>> GetCategoryStatistics(int brandId, DateTime startDate)
        {
            var ordersCategories = await _context.Orders
                .Where(o => o.BrandId == brandId && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED && o.CreatedOn >= startDate)
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

            return ordersCategories
                .GroupBy(o => o.CategoryName)
                .Select(g => new CategoryStatistics
                {
                    Category = g.Key,
                    Female = g.Count(o => o.ConsumerGender),
                    Male = g.Count(o => !o.ConsumerGender),
                    TotalSales = g.Count()
                })
                .ToList();
        }
        private async Task<List<ProductRating>> GetProductRatings(int brandId)
        {
            var products = await _context.Products
                .Where(p => p.BrandId == brandId)
                .ToListAsync();

            return products.Select(product => new ProductRating
            {
                ProductId = product.Id,
                AverageRating = product.Rate
            }).ToList();
        }
    }
}
