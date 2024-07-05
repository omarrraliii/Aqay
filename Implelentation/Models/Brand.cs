namespace aqay_apis.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEdit { get; set; }
        public string? Tiktok { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WPNumber { get; set; }
        public string? About { get; set; }
        public string? LogoUrl { get; set; }
        // Relationships
        public string BrandOwnerId { get; set; }
        public Merchant BrandOwner { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
