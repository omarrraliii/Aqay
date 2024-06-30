using aqay_apis.Models;

namespace aqay_apis;

public class Plan
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Describtion { get; set; }
    public double Price { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}
