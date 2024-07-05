using aqay_apis.Models;
namespace aqay_apis.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<PendingMerchant>> ListAllPendingMerchantsAsync();
        Task<string> AcceptMerchantAsync(int pendingMerchantId);
        Task<string> RejectMerchantAsync(int pendingMerchantId);
        Task<string> ToggleActivityAsync(string id);
        Task<IEnumerable<Consumer>> ListAllConsumersAsync();
        Task<IEnumerable<Merchant>> ListAllMerchantsAsync();
        Task<IEnumerable<Brand>> ListAllBrandsAsync();
        Task<string> GetEmailByUserIDAsync (string id);
    }
}
