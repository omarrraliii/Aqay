using System.Collections;
using aqay_apis.Models;

namespace aqay_apis;

public class ShoppingCart
{
    public int Id { get; set;}
    public double TotalPrice { get; set;}
    public string ConsumerId { get; set;}
    public Consumer Consumer{ get; set; } //should change it to one to many
    public Order Order { get; set; }

    public ICollection<Product> Products { get; set;}
}
