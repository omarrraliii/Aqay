namespace aqay_apis.Dtos
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Size { get; set; }
        public string? Description { get; set; }
        public int RED { get; set; }
        public int BLUE { get; set; }
        public int GREEN { get; set; }
        public int Quantity { get; set; }
        public string? CategoryName { get; set; }
        public IFormFile? ImgFile { get; set; }
    }
}
