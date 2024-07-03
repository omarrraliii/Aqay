namespace aqay_apis;

public class Order
{
    public int Id { get; set;}
    public ORDERSTATUSES ORDERSTATUSES{ get; set;}
    public double TotalPrice { get; set;}
    public DateTime CreatedOn { get; set;}
    public DateTime LastEdit { get; set; }
    public bool IsAccepted { get; set;} = false;
    public string ConsumerId { get; set;}
    public int ProductVariantId { get; set;}
    public string productName { get; set;}
    public string ConsumerName { get; set;}
    public string Address { get; set;}
    public int BrandId { get; set;}
}
