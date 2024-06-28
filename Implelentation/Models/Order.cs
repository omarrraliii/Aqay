namespace aqay_apis;

public class Order
{
    public int Id { get; set;}
    public int ShoppingCartId { get; set;}
    public ShoppingCart ShoppingCart { get; set;}
    public PAYMENTOPTIONS PAYMENTOPTIONS { get; set;}
    public ORDERSTATUSES ORDERSTATUSES{ get; set;}
    public double DeliveryFees { get; set;} 
    public double TotalPrice { get; set;}
    public DateTime CreatedOn { get; set;}
    public bool IsAccepted { get; set;}
}
