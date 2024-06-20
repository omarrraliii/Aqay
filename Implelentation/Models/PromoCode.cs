namespace aqay_apis;

public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEdit { get; set; }
    public DateTime ExpDate { get; set; }
    public double Percentage { get; set; }
    public double Amount { get; set; }
}
