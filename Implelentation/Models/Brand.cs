namespace aqay_apis.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // one-to-one relationship
        public string BrandOwnerId { get; set; }
        public Merchant BrandOwner { get; set; } //navigation property
    }
}
