using aqay_apis.Context;
using aqay_apis.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace aqay_apis.Dashboards
{
    public class AdminDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        public AdminDashboardService( ApplicationDbContext applicationDbContext, IOrderService orderService) {
            _context = applicationDbContext;
            _orderService = orderService;
        }
        public async Task<double> GetTotalRevenueForAllTime()
        {
            // Calculate the total revenue for all brands for all delivered orders
            double totalRevenue = await _context.Orders
                .Where(o => o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .SumAsync(o => o.TotalPrice);

            return totalRevenue;
        }
        public async Task<List<int>> GetNewBrandsPerMonthForLastSixMonths()
        {
            // Initialize a list to store counts for each month
            List<int> newBrandsCounts = new List<int>();

            // Get the current month and year
            DateTime now = DateTime.Now;

            // Iterate backwards for 6 months
            for (int i = 0; i < 6; i++)
            {
                // Calculate the start and end dates for the current month
                DateTime startDate = now.AddMonths(-i).Date;
                DateTime endDate = startDate.AddMonths(1).AddTicks(-1);

                // Query to count new brands created within the current month
                int newBrandsCount = await _context.Brands
                    .Where(b => b.CreatedOn >= startDate && b.CreatedOn <= endDate)
                    .CountAsync();

                // Add the count to the list
                newBrandsCounts.Add(newBrandsCount);
            }

            // Reverse the list to have counts in chronological order (from oldest to newest)
            newBrandsCounts.Reverse();

            return newBrandsCounts;
        }
        private async Task<List<CategoryStatistics>> GetCategoryStatistics()
        {
            var ordersCategories = await _context.Orders
                .Where(o => o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
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
        public async Task<AdminDashboardStatistics> GetAdminDashboardStatistics()
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            // Calculate total revenue for all time
            double totalRevenue = await GetTotalRevenueForAllTime();

            // Count number of subscription requests
            int subscriptionRequestCount = await _context.Subscriptions.CountAsync();

            // Count number of pending merchants
            int pendingMerchantCount = await _context.PendingMerchants.CountAsync();

            // Count number of merchants on the system
            int merchantCount = await _context.Merchants.CountAsync();


            // Count number of consumers on the system
            int consumerCount = await _context.Consumers.CountAsync();

            // Count number of completed orders in the whole system
            int completedOrdersCount = await _context.Orders
                .Where(o => o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .CountAsync();

            // Calculate top 5 best-selling product variants in the current month based on quantity sold

            var topProductVariants = await _context.Orders
                .Where(o => o.CreatedOn >= firstDayOfMonth && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .GroupBy(o => o.ProductVariantId)
                .OrderByDescending(g => g.Sum(o => 1)) // Counting orders, so sum by 1 for each order
                .Take(5)
                .Select(g => new TopProductVariant
                {
                    ProductVariantId = g.Key,
                    OrdersCount = g.Count() // Count the number of orders
                })
                .ToListAsync();
            // Calculate top 5 best-selling brands in the current month based on total sales
            var topBrands = await _context.Orders
                .Where(o => o.CreatedOn >= firstDayOfMonth && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .GroupBy(o => o.BrandId)
                .OrderByDescending(g => g.Sum(o => o.TotalPrice))
                .Take(5)
                .Select(g => new TopBrand
                {
                    BrandId = g.Key,
                    TotalSales = g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();

            // Calculate top 5 best-selling categories in the current month based on total sales
            var topCategories = await _context.Orders
                .Where(o => o.CreatedOn >= firstDayOfMonth && o.ORDERSTATUSES == ORDERSTATUSES.DELIVERED)
                .GroupBy(o => o.ProductVariantId) // Group by product variant ID
                .Select(g => new
                {
                    CategoryName = _context.ProductVariants
                        .Where(pv => pv.Id == g.Key)
                        .Select(pv => pv.Product.Category.Name)
                        .FirstOrDefault(),
                    TotalSales = g.Sum(o => o.TotalPrice) // Sum total price for each product variant
                })
                .OrderByDescending(g => g.TotalSales)
                .Take(5)
                .Select(g => new TopCategory
                {
                    CategoryName = g.CategoryName,
                    TotalSales = g.TotalSales
                })
                .ToListAsync();

            // Get new brands created per month for the last 6 months
            List<int> newBrandsPerMonth = await GetNewBrandsPerMonthForLastSixMonths();
            List<CategoryStatistics> categoryStatistics = await GetCategoryStatistics();

            return new AdminDashboardStatistics()
            {
                TotalRevenue = totalRevenue,
                SubscriptionRequestCount = subscriptionRequestCount,
                PendingMerchantCount = pendingMerchantCount,
                ConsumerCount = consumerCount,
                MerchantCount = merchantCount,
                TopProductVariants = topProductVariants,
                TopBrands = topBrands,
                TopCategories = topCategories,
                CompletedOrdersCount = completedOrdersCount,
                NewBrandsPerMonth = newBrandsPerMonth,
                CategoryStatistics = categoryStatistics
            };
        }

    }

}
