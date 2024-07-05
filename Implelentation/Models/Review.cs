using aqay_apis.Models;

namespace aqay_apis;
public class Review
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ConsumerId { get; set; }
    //One to One relationship with Product
    public int ProductId { get; set; }
    public int orderId { get; set; }
    public Product Product{ get; set; }
}
