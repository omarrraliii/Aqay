using Microsoft.EntityFrameworkCore;

namespace aqay_apis.Dashboards
{
    public class AdminDashboardStatistics
    {
        public double TotalRevenue { get; set; } = 0;
        public int SubscriptionRequestCount { get; set; } = 0;
        public int PendingMerchantCount { get; set; } = 0;
        public int MerchantCount { get; set; } = 0;
        public int ConsumerCount { get; set; } = 0;
        public int CompletedOrdersCount { get; set; } = 0;
        public List<TopProductVariant> TopProductVariants { get; set; } = new List<TopProductVariant>();
        public List<TopBrand> TopBrands { get; set; } = new List<TopBrand>();
        public List<TopCategory> TopCategories { get; set; } = new List<TopCategory>();
        public List<int> NewBrandsPerMonth { get; set; } = new List<int>();
        public List<CategoryStatistics> CategoryStatistics { get; set; } = new List<CategoryStatistics>();

    }
}
