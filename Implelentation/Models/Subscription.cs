using aqay_apis.Models;

namespace aqay_apis;

public class Subscription
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Merchant? Merchant { get; set; }
    public int PlanId { get; set; }
    public Plan Plan { get; set; }

}
