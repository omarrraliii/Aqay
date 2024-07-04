namespace aqay_apis.Dtos
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public ICollection<string>? TagName { get; set; }
    }
}
