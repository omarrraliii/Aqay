namespace aqay_apis.Dtos
{
    public class ProductVariantDto
    {
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public double? Price { get; set; }
        public IFormFile? ImgFile { get; set; }
        public string? ImgUrl { get; set; }
    }
}
