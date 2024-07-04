namespace aqay_apis.Dashboards
{
    public class MerchantDashboardStatistics
    {
        public double TotalRevenue { get; set; } = 0;
        public double TotalRevenueThisMonth { get; set; } = 0;
        public List<int> TopProductVariants { get; set; } = new List<int>();
        public int CompletedOrdersCount { get; set; } = 0;
        public int PendingOrdersCount { get; set; } = 0;
        public List<CategoryStatistics> CategoryStatistics { get; set; } = new List<CategoryStatistics>();
        public List<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();
        public int RefundCount { get; set; } = 0;
        public int ExchangeCount { get; set; } = 0;
        public double SixMonthRevenue { get; set; } = 0;
        public List<double> SixMonthRevenueList { get; set; } = new List<double>();
    }
}
