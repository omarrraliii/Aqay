using aqay_apis.Models;

namespace aqay_apis;

public class Review
{
    public int Id { get; set; }
    public string Describtion { get; set; }
    public double Rate { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEdit { get; set; }
    //One to One relationship with Consumer
    public string ConsumerId { get; set; }
    //One to One relationship with Product
    public int ProductId { get; set; }
    public Product Product{ get; set; }
}
