using System.Collections;

namespace aqay_apis.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEdit { get; set; }
        public string? Tiktok { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WPNumber { get; set; }
        // one-to-one relationship
        public string BrandOwnerId { get; set; }
        public Merchant BrandOwner { get; set; } //navigation property
        //one to many relationship with Products
        public ICollection<Product> Products { get; set; }
        //one to one relationship with About
        public int AboutId { get; set; }
        public About? About { get; set; }
        // list of id's of the ordered products
        // public ICollection<int> orderHistory { get; set; }

    }
}
