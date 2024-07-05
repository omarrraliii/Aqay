namespace aqay_apis;
public class ProductVariant
{
    public int Id { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public ICollection<WishList> WishLists { get; set; }
    public string? ImageUrl { get; set; }
}
