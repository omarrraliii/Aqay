using static aqay_apis.Helpers.Enums;

namespace aqay_apis.Models;

public class Subscription
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Merchant Merchant { get; set; }
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
    public SubscriptionStatus Status { get; set; }

}
