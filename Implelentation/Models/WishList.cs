using aqay_apis.Models;

namespace aqay_apis;

public class WishList
{
    public int Id { get; set;} 
    public List<ProductVariant> ProductsVariants { get; set;} = new List<ProductVariant>();

    // Relationships
    public string ConsumerId { get; set; }
}
