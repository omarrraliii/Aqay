namespace aqay_apis.Services
{
    public interface ISubscriptionService
    {
        Task<ICollection<Plan>> GetAllPlansAsync();
        Task<Plan> GetPlanByIdAsync(int planId);
        Task SubscribeAsync(string userId, int planId);
        Task <Subscription> CreateAsync(int planId);
    }

}
