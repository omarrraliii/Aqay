using System.Collections;
using System.Collections.ObjectModel;
using aqay_apis.Models;
using System.Collections.Generic;
namespace aqay_apis;

public class ShoppingCart
{
    public int Id { get; set;}
    public double TotalPrice { get; set;}
    public string? ConsumerId { get; set;}
    public double DeliveryFees { get; set; }
    public IList<int> ProductVariantIds { get; set; } = new List<int>();
}
