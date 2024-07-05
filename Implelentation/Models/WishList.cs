using aqay_apis.Models;

namespace aqay_apis;

public class WishList
{
    public int Id { get; set;} 
    public ICollection<Product>? Products { get; set;}

    // Relationships
    public string ConsumerId { get; set; }
    public Consumer Consumer { get; set; }
}
